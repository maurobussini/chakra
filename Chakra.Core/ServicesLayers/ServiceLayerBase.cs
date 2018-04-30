using System;

namespace ZenProgramming.Chakra.Core.ServicesLayers
{
    /// <summary>
    /// Represents abstract class for create base service layers
    /// </summary>
    public class ServiceLayerBase : IServiceLayer
    {
        #region Private fields
        private bool _IsDisposed;
        #endregion

        /// <summary>
		/// Finalizer that ensures the object is correctly disposed of.
		/// </summary>
        ~ServiceLayerBase()
		{
            //Richiamo i dispose implicito
			Dispose(false);
		}

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //Eseguo una dispose esplicita
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
            //Se l'oggetto è già rilasciato, esco
            if (_IsDisposed)
                return;

            //Se è richiesto il rilascio esplicito
            if (isDisposing)
            {
                //RIlascio della logica non finalizzabile
            }

            //Marco il dispose e invoco il GC
            _IsDisposed = true;            
        }
    }
}
