using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZenProgramming.Chakra.Core.Parallel.MapReduce.Events;

namespace ZenProgramming.Chakra.Core.Parallel.MapReduce
{
    /// <summary>
    /// Abstract class used to create a processor base on map/reduce
    /// algorithm, that split input data and execute processing using
    /// different processors. Data are processed asyncronously over
    /// different threads and will be merged by specific function
    /// </summary>
    /// <typeparam name="TInput">Type of input</typeparam>
    /// <typeparam name="TOutput">Type of output data</typeparam>
    public abstract class MapReduceProcessorBase<TInput, TOutput>
    {
        #region Public events
        /// <summary>
        /// Raised when entire process in completed
        /// </summary>
        public event MapReduceProcessCompletedEventHandler<TOutput> ProcessCompleted;

        /// <summary>
        /// Raised when partial result is obtained
        /// </summary>
        public event MapReducePartialResultProcessedEventHandler<TOutput> PartialResulProcessed;
        #endregion

        #region Public properties
        /// <summary>
        /// Get list of input elements
        /// </summary>
        public IList<TInput> Input { get; private set; }

        /// <summary>
        /// Get number of parallel processor used
        /// </summary>
        public int NumberOfProcessors { get; private set; }

        /// <summary>
        /// Get partial results of partial processes
        /// </summary>
        public TOutput[] PartialResults { get; private set; }
        #endregion

        protected MapReduceProcessorBase(IEnumerable<TInput> input, int numberOfProcessors) 
        { 
            //Eseguo la validazione degli argomenti
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (numberOfProcessors < 1) throw new ArgumentOutOfRangeException(nameof(numberOfProcessors));

            //Enumero gli elementi in ingresso
            Input = input as List<TInput> ?? input.ToList();

            //Conto il numero di elementi in ingresso
            int count = Input.Count();

            //Se il numero di elementi è minore del numero di processori, non ha senso eseguire
            if (count < numberOfProcessors)
                throw new InvalidOperationException("Number or " +
                                                    $"processors ({numberOfProcessors}) is greater that input elements ('{count}').");

            //Inserisco i valori nelle proprietà
            NumberOfProcessors = numberOfProcessors;
        }

        /// <summary>
        /// Execute process of partial input data for specified processor
        /// </summary>
        /// <param name="partialInput">Input data</param>
        /// <param name="processorId">Executing processor identified</param>
        /// <returns>Return partial result of partial inputs</returns>
        protected abstract TOutput Process(IEnumerable<TInput> partialInput, int processorId);

        /// <summary>
        /// Executes aggregation of partial results and emit
        /// a global result for entire operation
        /// </summary>
        /// <param name="partialResults">Partial results</param>
        /// <returns>Returns global result</returns>
        protected abstract TOutput Reduce(TOutput[] partialResults);

        /// <summary>
        /// Execute map operation on multiple processors and emit
        /// results of execution after aggregation of partials
        /// </summary>
        /// <returns>Returns aggregate result</returns>
        public TOutput Execute()
        {
            //Determino quanti elementi sono dedicati a ciascun processore
            int packetCount = Input.Count() / NumberOfProcessors;
            if (Input.Count() % NumberOfProcessors != 0)
                packetCount = packetCount + 1;

            //Predispongo un array di task che saranno mandate in esecuzione
            //in parallelo; una task per ogni processore definito
            Task[] tasks = new Task[NumberOfProcessors];
            Task[] continueTasks = new Task[NumberOfProcessors];

            //Predispongo l'array con i risultati da emettere
            //per ciascun processore. Ogni task inserirà la sua
            //elaborazione nello "slot" assegnato il numero di processore
            PartialResults = new TOutput[NumberOfProcessors];

            //Itero l'operazione di preparazione della task sui processori
            for (int i = 0; i < NumberOfProcessors; i++)
            {
                //Determino l'identificativo del processore
                int processorId = i;

                //Taglio gli elementi del pacchetto
                IList<TInput> packet = Input.
                    Skip(i * packetCount).
                    Take(packetCount).
                    ToList();

                //Creo un oggetto di persistenza dello stato per l'esecuzione passando
                //il pacchetto appena tagliato e il numero del processore corrente
                //MapReduceState<TInput> state = new MapReduceState<TInput>(packet, processorId);

                //Definisco la funzione deve essere data in input alla task
                Func<TOutput> taskFunction = () => Process(packet, processorId);

                //Creo la task di esecuzione specificando la funzione
                Task<TOutput> currentProcessor = new Task<TOutput>(taskFunction);

                //Imposto la continuazioen della task con il risultato di
                //esecuzione che viene archiviato nell'array dei risultati
                //alla posizione identificata dal numero di processore
                continueTasks[processorId] = currentProcessor.ContinueWith(task =>
                {
                    //Inserisco il risultato dell'elaborazione
                    PartialResults[processorId] = task.Result;

                    //Se l'evento del risultato parziale è gestito, lo sollevo
                    if (PartialResulProcessed != null)
                        PartialResulProcessed(this, new MapReducePartialResultProcessedEventArgs<TOutput>(task.Result, processorId));
                });

                //Inserisco la task corrente nella lista
                tasks[processorId] = currentProcessor;
            }

            //Riscorro tutte le task e le avvio
            foreach (Task currentTask in tasks)
                currentTask.Start();

            //Attendo che tutte le task abbiamo terminato l'esecuzione
            Task.WaitAll(continueTasks);

            //Chiamo la funzione di aggregazione
            TOutput result = Reduce(PartialResults);

            //Se l'evento è gestito, lo sollevo
            if (ProcessCompleted != null)
                ProcessCompleted(this, new MapReduceProcessCompletedEventArgs<TOutput>(result));

            //Mando in uscita i risultati dell'esecuzione
            return result;
        }
    }
}
