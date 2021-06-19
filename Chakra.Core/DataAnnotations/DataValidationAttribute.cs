using System;
using System.ComponentModel.DataAnnotations;
using ZenProgramming.Chakra.Core.Data;

namespace ZenProgramming.Chakra.Core.DataAnnotations
{
    /// <summary>
    /// Represents base class for a data validation attribute
    /// </summary>
    public abstract class DataValidationAttributeBase : ValidationAttribute
    {
        #region Static fields
        /// <summary>
        /// Data session key
        /// </summary>
        public static readonly string DataSessionKey = "DataSession";
        #endregion
        
        /// <summary>
        /// Constructor
        /// </summary>
        protected DataValidationAttributeBase() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        protected DataValidationAttributeBase(string errorMessage) 
            : base(errorMessage) { }

        /// <summary>
        /// COnstructor
        /// </summary>
        /// <param name="errorMessageAccessor">Error message delegate</param>
        protected DataValidationAttributeBase(Func<string> errorMessageAccessor) 
            : base(errorMessageAccessor) { }

        /// <summary>
        /// Get active data session using validation context
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Returns instance of session holder</returns>
        protected IDataSession GetActiveDataSession(ValidationContext validationContext)
        {
            //Tento il recupero della chiave dal dizionario
            if (!validationContext.Items.ContainsKey(DataSessionKey))
                throw new InvalidOperationException($"Unable to use validator of type '{GetType().FullName}' without " +
                                                    $"specify a custom item with key '{DataSessionKey}' and instance of '{typeof(IDataSession).FullName}' in validation context.");

            //Recupero il data session di riferimento
            IDataSession dataSession = validationContext.Items[DataSessionKey] as IDataSession;

            //Se non ho un session holder, emetto eccezione
            if (dataSession == null)
                throw new InvalidOperationException($"Unable to find a valid '{typeof(IDataSession).FullName}' on " +
                                                    $"item with key '{DataSessionKey}' in validation context.");

            //Ritorno il valore
            return dataSession;
        }
    }
}
