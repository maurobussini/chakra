using System;
using System.IO;
using ZenProgramming.Chakra.Core.WebApi.Filters.Helpers;
using ZenProgramming.Chakra.Core.WebApi.Filters.Models;
using ZenProgramming.Chakra.Core.WebApi.Filters;

namespace ZenProgramming.Chakra.Core.WebApi.Filters
{
    /// <summary>
    /// Represents an WebAPI filter to trace on separate, self contained, files
    /// unhandled exception occurred during request/response
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class TraceOnErrorFileAttribute : TraceAttribute
    {
        #region Public properties
        /// <summary>
        /// Path (absolute or relative) of target folder
        /// </summary>
        public string TargetFolder { get; set; }
        #endregion

        /// <summary>
        /// Contructor
        /// </summary>
        public TraceOnErrorFileAttribute()
        {
            //Imposto le opzioni base
            EnableRequestTrace = false;
            EnableResponseTrace = true;
            TargetFolder = Path.Combine("App_Data", "traced-errors");
        }

        /// <summary>
        /// Trace request of action
        /// </summary>
        /// <param name="request">Request trace</param>
        protected override void TraceRequest(RequestTrace request)
        {
            //Non eseguo nessun tracciamento della request; eseguendo
            //l'override non viene tracciato nulla nemmeno nella classe base
        }

        /// <summary>
        /// Trace response of action
        /// </summary>
        /// <param name="response">Response trace</param>
        protected override void TraceResponse(ResponseTrace response)
        {
            //Validazione argomenti
            if (response == null) throw new ArgumentNullException(nameof(response));

            //Eseguo la generazione del file di log dell'eventuale errore
            TraceUtils.WriteErrorLogFile(response, TargetFolder);
        }
    }
}
