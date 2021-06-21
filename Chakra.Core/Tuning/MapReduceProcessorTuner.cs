using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ZenProgramming.Chakra.Core.Parallel.MapReduce;
using ZenProgramming.Chakra.Core.Parallel.MapReduce.Events;
using ZenProgramming.Chakra.Core.Utilities.Server;

namespace ZenProgramming.Chakra.Core.Tuning
{
    /// <summary>
    /// Tuner utility used to iterate map/reduce processor over different
    /// number or processor and various cicles in order to find correct 
    /// balance of processors related with input data
    /// </summary>
    /// <typeparam name="TMapReduceProcessor">Type of map/reduce processor</typeparam>
    /// <typeparam name="TInput">Type of input data</typeparam>
    /// <typeparam name="TOutput">Type of output data</typeparam>
    public class MapReduceProcessorTuner<TMapReduceProcessor, TInput, TOutput>
        where TMapReduceProcessor : MapReduceProcessorBase<TInput, TOutput>
    {
        #region Public events
        /// <summary>
        /// Raised when entire process in completed
        /// </summary>
        public event MapReduceProcessCompletedEventHandler<TOutput> ProcessCompleted;

        /// <summary>
        /// Raised when partial result is obtained
        /// </summary>
        public event MapReducePartialResultProcessedEventHandler<TOutput> PartialResultProcessed;
        #endregion

        #region Public properties
        /// <summary>
        /// Get and set maximum processors used by the tuner
        /// </summary>
        public int MaxProcessors { get; set; }

        /// <summary>
        /// Get list of source elements
        /// </summary>
        public IList<TInput> Input { get; private set; }

        /// <summary>
        /// Get and set interval between iterations
        /// </summary>
        public TimeSpan DelayBetweenIterations { get; set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="input">Input list</param>
        /// <param name="maxProcessors">Maximum number of processors</param>
        public MapReduceProcessorTuner(IEnumerable<TInput> input, int? maxProcessors = null)
        {
            //Eseguo la validazione degli argomenti
            if (input == null) throw new ArgumentNullException(nameof(input));

            //Enumero gli elementi in ingresso
            Input = input as List<TInput> ?? input.ToList();

            //Se non ho specificato i processori, imposto il numero di elementi della lista
            if (maxProcessors == null) maxProcessors = Input.Count();
            if (maxProcessors <= 0) throw new ArgumentOutOfRangeException(nameof(maxProcessors));

            //Imposto le mirror interne            
            MaxProcessors = maxProcessors.Value;

            //Imposto l'intervallo tra iterazioni a 10 secondi
            DelayBetweenIterations = new TimeSpan(0, 0, 10);
        }

        /// <summary>
        /// Execute iteration of procedures N times
        /// </summary>
        /// <param name="iterations">Number or iterations</param>
        public void Execute(int iterations)
        {
            //Definisco il tipo del processore e la funzione di istanziamento
            Type processorType = typeof(TMapReduceProcessor);
            Func<int, TMapReduceProcessor> instanceMethod = processorCount =>
            {
                //Creo l'istanza del processore di elaborazione passando i dati in 
                //ingresso e il numero di processori da applicare per l'esecuzione corrente
                TMapReduceProcessor processorInstance = Activator.CreateInstance(processorType,
                    new object[] { Input, processorCount }) as TMapReduceProcessor;

                //Se non è stato possibile istanziare, emetto eccezione
                if (processorInstance == null)
                    throw new InvalidProgramException("Unable to create " +
                                                      $"instance of processor of type '{processorType}'.");

                //Ritorno in uscita l'istanza
                return processorInstance;
            };

            //Eseguo l'iterazione il numero di volte richiesto
            for (int iteration = 1; iteration <= iterations; iteration++)
            {
                //Visualizzo un messaggio di feedback per l'utente
                ConsoleUtils.WriteColorLine(ConsoleColor.Yellow, "[{0}]", DateTime.Now);
                ConsoleUtils.WriteColorLine(ConsoleColor.Yellow, "Starting iteration {0}/{1} on " + 
                    "processor '{2}'...", iteration, iterations, processorType.Name);

                //Eseguo delle verifiche con il numero di processori che
                //aumenta da 1 al numero di elementi presenti nella lista sorgente
                for (int p = 1; p <= MaxProcessors; p++)
                {
                    //Memorizzo l'orario di avvio della procedura
                    DateTime start = DateTime.Now;

                    //Istanzio il processore passando il numero di processori
                    TMapReduceProcessor processor = instanceMethod(p);

                    //Aggancio gli eventi di eventi del processore
                    processor.ProcessCompleted += ProcessCompleted;
                    processor.PartialResulProcessed += PartialResultProcessed;

                    //Mando in esecuzione ed ottengo il risultato finale
                    processor.Execute();

                    //Sgancio gli eventi di eventi del processore
                    processor.ProcessCompleted -= ProcessCompleted;
                    processor.PartialResulProcessed -= PartialResultProcessed;

                    //Emetto l'esito dell'operazione a console
                    ConsoleUtils.WriteColorLine(ConsoleColor.Green, "Execution completed in {0} milliseconds " + 
                        "with {1} parallel processors.", DateTime.Now.Subtract(start).TotalMilliseconds, p);
                }

                //Visualizzo un messaggio di feedback per l'utente
                ConsoleUtils.WriteColorLine(ConsoleColor.DarkGreen, "Iteration {0}/{1} completed. " + 
                    "Sleeping for {2}...", iteration, iterations, DelayBetweenIterations);

                //Imposto l'itervallo tra le iterazioni
                Thread.Sleep(DelayBetweenIterations);
            }
        }
    }
}
