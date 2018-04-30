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
                throw new InvalidOperationException(string.Format("Unable to use validator of type '{0}' without " +
                    "specify a custom item with key '{1}' and instance of '{2}' in validation context.", GetType().FullName,
                    DataSessionKey, typeof(IDataSession).FullName));

            //Recupero il data session di riferimento
            IDataSession dataSession = validationContext.Items[DataSessionKey] as IDataSession;

            //Se non ho un session holder, emetto eccezione
            if (dataSession == null)
                throw new InvalidOperationException(string.Format("Unable to find a valid '{0}' on " +
                    "item with key '{1}' in validation context.", typeof(IDataSession).FullName, DataSessionKey));

            //Ritorno il valore
            return dataSession;
        }
    }
}
