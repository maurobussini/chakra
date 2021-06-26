using System;
using System.Threading.Tasks;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Mocks.Data;

namespace ZenProgramming.Chakra.Core.Mocks.Async.Data
{
    /// <summary>
    /// Data transaction implementation for mockup
    /// </summary>
    public class MockDataTransactionAsync : MockDataTransaction, IDataTransactionAsync
    {
        #region Private fields
        private IMockDataSessionAsync _DataSession;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public MockDataTransactionAsync(IMockDataSessionAsync dataSession): base(dataSession)
        {
            //Imposto la data session
            _DataSession = dataSession ?? throw new ArgumentNullException(nameof(dataSession));

            //Imposto lo stato iniziale
            IsActive = true;
            IsTransactionOwner = true;
            WasCommitted = false;
            WasRolledBack = false;
            
            //Se già esiste una transanzione sull'holder, esco
            if (_DataSession.TransactionAsync != null)
                return;

            //Imposto l'istanza corrente
            _DataSession.SetActiveTransactionAsync(this);
        }

        /// <summary>
        /// Executes commit of transaction
        /// </summary>
        public Task CommitAsync()
        {
            //Se l'istanza è la proprietaria della transazione
            if (IsTransactionOwner)
            {
                //Imposto i flag per commit
                IsActive = false;
                WasCommitted = true;
                WasRolledBack = false;

                //Rimuovo il riferimento alla transazione
                _DataSession.SetActiveTransactionAsync(this);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Executes rollback of transaction
        /// </summary>
        public Task RollBackAsync()
        {
            //Se l'istanza è la proprietaria della transazione
            if (IsTransactionOwner)
            {
                //Imposto i flag per rollbak
                IsActive = false;
                WasCommitted = false;
                WasRolledBack = true;

                //Rimuovo il riferimento alla transazione
                _DataSession.SetActiveTransactionAsync(this);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Finalizes that ensures the object is correctly disposed of.
		/// </summary>
        ~MockDataTransactionAsync()
        {
            //Implicit dispose
            Dispose(false);
        }

    }
}
