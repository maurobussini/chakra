using System;
using System.Collections.Generic;
using System.Linq;
using ZenProgramming.Chakra.Core.Diagnostic;

namespace ZenProgramming.Chakra.Core.Bio
{
    /// <summary>
    /// Represents class that allow evolution of individuals
    /// </summary>
    public abstract class Evolution<TIndividual>
        where TIndividual : class, new()
    {
        /// <summary>
        /// Generatore di numeri casuali
        /// </summary>
        private readonly Random _Random = new Random();

        /// <summary>
        /// Population of individuals
        /// </summary>
        public IList<TIndividual> Population { get; private set; }

        /// <summary>
        /// Frequency (in generation) of mutation
        /// </summary>
        public int MutationFrequency { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="population">List of initial individuals</param>
        protected Evolution(IList<TIndividual> population)
        {
            //Eseguo la validazione degli argomenti
            if (population == null) throw new ArgumentNullException(nameof(population));
            
            //Temporaneamente imposto come fissa la mutazione ogni 100 generazioni
            MutationFrequency = 100;

            //La popolazione in ingresso deve essere almeno di 4 elementi per permettere
            //l'esecuzione della selezione e l'accoppiamento della metà migliore
            if (population.Count < 4) throw new InvalidOperationException("Unable to " +
                                                                          "execute evolution on population with less than 4 individuals. At least 4 elements are " +
                                                                          "required in order to execute selection of the best half of population and complete " +
                                                                          $"crossovers in order to generate new generation. Found '{population.Count}' individuals.");

            //Imposto i valori nelle proprietà
            Population = population;
        }

        /// <summary>
        /// Apply mutation to a single individual
        /// </summary>
        /// <param name="targetOfMutation">Target of mutation</param>
        protected abstract void Mutate(TIndividual targetOfMutation);

        /// <summary>
        /// Execute crossover between two individuals to create a new element
        /// </summary>
        /// <param name="father">Father</param>
        /// <param name="mother">Mother</param>
        /// <returns>Returns new individual</returns>
        protected abstract TIndividual Crossover(TIndividual father, TIndividual mother);

        /// <summary>
        /// Calculate fitness value on specified individual
        /// </summary>
        /// <param name="individual">Individual</param>
        /// <returns>Returns fitness value</returns>
        protected abstract double Fitness(TIndividual individual);

        /// <summary>
        /// Start evolution on popolution while best individual 
        /// reach acceptable fitness value
        /// </summary>
        /// <param name="acceptableFitness">Acceptable fitness value</param>
        /// <returns>Returns best individual generated</returns>
        public TIndividual Evolve(double acceptableFitness)
        {
            //Avvio l'evoluzione
            return Evolve((gen, evals) =>
            {                
                //Ordino per miglior fitness (il più basso)
                var ordered = evals.OrderBy(e => e.Item2).ToList();

                //Recupero l'individuo migliore
                var bestWithFitness = ordered.First();

                Tracer.Info("Best : {0} - Fitness : {1}", bestWithFitness.Item1, bestWithFitness.Item2);

                //Confermo una nuova iterazione se il fitness è più alto
                return bestWithFitness.Item2 > acceptableFitness;
            });
        }

        /// <summary>
        /// Start evolution on specified population
        /// </summary>
        /// <param name="generations">Number of generations</param>
        /// <returns>Returns best individual generated</returns>
        public TIndividual Evolve(int generations)
        {
            //Avvio l'evoluzione (arrestando quando raggiungo la generazione interessata)
            return Evolve((gen, evals) => gen <= generations);
        }

        private TIndividual Evolve(Func<int, IList<Tuple<TIndividual, double>>, bool> iterationFunction)
        {
            //Eseguo la validazione degli argomenti
            if (iterationFunction == null) throw new ArgumentNullException(nameof(iterationFunction));

            //1) Intialization of population
            //2) Evaluation -> Fitness
            //3) Selection of best individuals
            //4) Crossover between best half individuals
            //5) Mutation of random individuals
            //6) Return (2)

            //Variabile per il conteggio delle generazioni
            int generationCounter = 0;

            //Eseguo la valutazione di tutti gli individui tramite funzione di fitness
            var evaluations = CalculateFitness(Population);

            do
            {
                //Eseguo la selezione della migliore popolazione
                Population = ExecuteSelection(evaluations);

                //Calcolo i crossover della popolazione attuale
                var crossovers = GenerateCrossovers(Population);

                //Accodo tutti i crossovers nella nuova popolazione
                foreach (TIndividual currentCrossover in crossovers)
                    Population.Add(currentCrossover);

                //Eseguo la mutazione se necessario 
                if (generationCounter%MutationFrequency == 0)
                    PerformMutation(Population);

                //Eseguo la valutazione dopo la mutazione
                evaluations = CalculateFitness(Population);

                //Incremento il numero di generazione
                generationCounter++;
            }
            while (iterationFunction(generationCounter, evaluations));


            ////Continuo ad interare finchè la funzione di iterazione è vera
            //while (iterationFunction(generationCounter))
            //{
            //    //Eseguo la valutazione di tutti gli individui tramite funzione di fitness
            //    var evaluations = CalculateFitness(Population);

            //    //Eseguo la selezione della migliore popolazione
            //    Population = ExecuteSelection(evaluations);

            //    //Calcolo i crossover della popolazione attuale
            //    var crossovers = GenerateCrossovers(Population);

            //    //Accodo tutti i crossovers nella nuova popolazione
            //    foreach (TIndividual currentCrossover in crossovers)
            //        Population.Add(currentCrossover);

            //    //Eseguo la mutazione se necessario 
            //    if (generationCounter % MutationFrequency == 0)
            //        PerformMutation(Population);

            //    //Incremento il numero di generazione
            //    generationCounter++;
            //}

            //Calcolo nuovamente il fitness sull'attuale popolazione
            IList<Tuple<TIndividual, double>> results = CalculateFitness(Population);

            //Ordino per miglior fitness (il più basso)
            results = results.
                OrderBy(e => e.Item2).
                ToList();

            //Mando in uscita il primo elemento
            return results.First().Item1;
        }

        /// <summary>
        /// Apply mutation on a random individual
        /// </summary>
        /// <param name="population">Population</param>
        private void PerformMutation(IList<TIndividual> population)
        {
            //Eseguo la validazione degli argomenti
            if (population == null) throw new ArgumentNullException(nameof(population));

            //Eseguo la selezione random di un elemento della popolazione
            int randomPosition = _Random.Next(0, population.Count - 1);

            //Eseguo la mutazione di un individuo selezionato random
            Mutate(population[randomPosition]);
        }

        /// <summary>
        /// Execute crossovers of specified list of individuals
        /// </summary>
        /// <param name="selections">Source selection</param>
        /// <returns>Returns list of crossovers</returns>
        private IList<TIndividual> GenerateCrossovers(IList<TIndividual> selections)
        {
            //Eseguo la validazione degli argomenti
            if (selections == null) throw new ArgumentNullException(nameof(selections));

            //Se le selezioni per qualche motivo sono dispari, emetto eccezione
            if (selections.Count % 2 == 1)
                throw new InvalidProgramException("Individuals selected " +
                                                  $"must be even to execute crossover. Found '{selections.Count}' elements.");

            //Calcolo il numero di iterazioni necessarie al crossover
            int iterations = selections.Count / 2;

            //Predispongo in uscita la lista dei crossovers
            IList<TIndividual> crossovers = new List<TIndividual>();

            //Itero per il numero di iterazioni calcolate imponendo che il primo
            //elemento della lista di incroci con l'ultimo e il secondo il penultimo
            //e così via, al fine di permettere di mantenere omogeneità nella razza
            for (int i = 0; i < iterations; i++)
            {
                //Devo generare due crossover per ogni coppia, al fine di permettere che la generazione
                //successiva sia mantenuta in termine di numero di individui (più o meno, considerata
                //l'applicazione della correzione basata sul numero pari di genitori selezionati)
                TIndividual firstChild = Crossover(selections[i], selections[((iterations * 2) - 1) - i]);
                TIndividual secondChild = Crossover(selections[i], selections[((iterations * 2) - 1) - i]);

                //Aggiungo i nuovi figli alla lista di uscita
                crossovers.Add(firstChild);
                crossovers.Add(secondChild);
            }

            //Mando in uscita i crossovers
            return crossovers;
        }

        /// <summary>
        /// Select best individuals based on evalutaions
        /// </summary>
        /// <param name="evaluations">Evaluations</param>
        /// <returns>Returns list of best individuals</returns>
        private IList<TIndividual> ExecuteSelection(IList<Tuple<TIndividual, double>> evaluations)
        {
            //Eseguo la validazione degli argomenti
            if (evaluations == null) throw new ArgumentNullException(nameof(evaluations));

            //Ordino le informazioni di valutazione in ascendente
            evaluations = evaluations.
                OrderBy(e => e.Item2).
                ToList();

            //Calcolo la metà degli elementi; nel caso in cui il numero di elementi
            //sia dispari, il numero di elementi selezionati sarà la metà +1
            int halfCount = evaluations.Count % 2 == 0 ? 
                evaluations.Count / 2 : evaluations.Count / 2 + 1;

            //Se la metà è dispari, aggiungo ancora un elemento
            if (halfCount % 2 == 1) halfCount = halfCount + 1;

            //Predispongo in uscita la lista della nuova popolazione
            IList<TIndividual> selectedPopulation = new List<TIndividual>();

            //Inserisco la nuova popolazione
            for (int k = 0; k < halfCount; k++)
                selectedPopulation.Add(evaluations[k].Item1);

            //Mando in uscita la selezione
            return selectedPopulation;
        }

        /// <summary>
        /// Calculate fitness on every member of population
        /// </summary>
        /// <param name="population">Source population</param>
        /// <returns>Returns list of individuals and theirs evaluation</returns>
        private IList<Tuple<TIndividual, double>> CalculateFitness(IList<TIndividual> population)
        {
            //Eseguo la validazione degli argomenti
            if (population == null) throw new ArgumentNullException(nameof(population));

            //Inizializzo il dizionario di uscita
            //IList<IndividualEvaluation<TIndividual>> evaluations = new List<IndividualEvaluation<TIndividual>>();
            IList<Tuple<TIndividual, double>> evaluations = new List<Tuple<TIndividual, double>>();

            //Scorro tutti gli elementi della popolazione
            foreach (var individual in population)
            {
                //Calcolo il fitness per l'individuo
                double fitnessValue = Fitness(individual);

                //Creo un nuovo oggetto
                //IndividualEvaluation<TIndividual> evaluation = new IndividualEvaluation<TIndividual>();
                Tuple<TIndividual, double> evaluation = new Tuple<TIndividual, double>(individual, fitnessValue);

                //Aggiungo la valutazione alla lista
                evaluations.Add(evaluation);
            }

            //Mando in uscita i fitness
            return evaluations;
        }
    }
}