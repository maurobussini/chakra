using System;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Diagnostic;

namespace ZenProgramming.Chakra.Core.Mocks.Data
{
    /// <summary>
    /// Data transaction implementation for mockup
    /// </summary>
    public class MockDataTransaction : IDataTransaction
    {
        #region Private fields
        private bool _IsDisposed;
        private IMockDataSession _DataSession;
        #endregion

        #region Public properties

        /// <summary>
        /// Transaction is active
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Transaction was rolled back
        /// </summary>
        public bool WasRolledBack { get; private set; }

        /// <summary>
        /// Transaction was committed
        /// </summary>
        public bool WasCommitted { get; private set; }

        /// <summary>
        /// Specify that current instance is transaction owner
        /// </summary>
        public bool IsTransactionOwner { get; protected set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public MockDataTransaction(IMockDataSession dataSession)
        {
            //Validazione argomenti
            if (dataSession == null) throw new ArgumentNullException(nameof(dataSession));

            //Imposto lo stato iniziale
            IsActive = true;
            IsTransactionOwner = true;
            WasCommitted = false;
            WasRolledBack = false;

            //Imposto la data session
            _DataSession = dataSession;

            //Se già esiste una transanzione sull'holder, esco
            if (_DataSession.Transaction != null)
                return;

            //Imposto l'istanza corrente
            _DataSession.SetActiveTransaction(this);
        }

        /// <summary>
        /// Executes commit of transaction
        /// </summary>
        public void Commit()
        {
            //Se l'istanza è la proprietaria della transazione
            if (IsTransactionOwner)
            {
                //Imposto i flag per commit
                IsActive = false;
                WasCommitted = true;
                WasRolledBack = false;

                //Rimuovo il riferimento alla transazione
                _DataSession.SetActiveTransaction(this);
            }
        }

        /// <summary>
        /// Executes rollback of transaction
        /// </summary>
        public void Rollback()
        {
            //Se l'istanza è la proprietaria della transazione
            if (IsTransactionOwner)
            {
                //Imposto i flag per rollbak
                IsActive = false;
                WasCommitted = false;
                WasRolledBack = true;

                //Rimuovo il riferimento alla transazione
                _DataSession.SetActiveTransaction(this);
            }
        }

        /// <summary>
        /// Finalizes that ensures the object is correctly disposed of.
		/// </summary>
        ~MockDataTransaction()
        {
            //Implicit dispose
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //Explicit dispose
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="isDisposing">Explicit dispose</param>
        protected virtual void Dispose(bool isDisposing)
        {
            //If already disposed, exit
            if (_IsDisposed)
                return;

            //If explicit dispose
            if (isDisposing)
            {
                //If owns instance but was not closed, trace error
                if (IsTransactionOwner && IsActive)
                    Tracer.Error("Transaction was opened but never commited or rolled back.");
            }

            //Set as disposed
            _IsDisposed = true;
        }
    }
}
