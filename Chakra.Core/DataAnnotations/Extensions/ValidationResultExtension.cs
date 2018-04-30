using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZenProgramming.Chakra.Core.DataAnnotations.Extensions
{
    /// <summary>
    /// Represents validation result extensions
    /// </summary>
    public static class ValidationResultExtensions
    {
        /// <summary>
        /// Generate a validation summary using all validation
        /// messages detected inside specified list of validation results
        /// </summary>
        /// <param name="instance">Validation resuls</param>
        /// <returns>Returns validation summary</returns>
        public static string ToValidationSummary(this IEnumerable<ValidationResult> instance)
        {
            //Se non è stato passato un elemento valido, emetto eccezione
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            //Predispongo la stringa di uscita
            string outSummary = String.Empty;

            //Scorro tutti gli elementi presenti nella lista
            foreach (ValidationResult currentResult in instance)
                outSummary = (outSummary.Length == 0) ?
                    currentResult.ErrorMessage + "\n" :
                    outSummary + currentResult.ErrorMessage + "\n";

            //Mando in uscita il messaggio
            return outSummary;
        }

		/// <summary>
		/// Add new message as validation to provided list of validation results
		/// </summary>
		/// <param name="instance">Instance of list</param>
		/// <param name="message">Message</param>
		/// <param name="arguments">Arguments for format message</param>
	    public static void AddMessage(this IList<ValidationResult> instance, string message, params object[] arguments)
	    {
			//Se non è stato passato un elemento valido, emetto eccezione
			if (instance == null) throw new ArgumentNullException(nameof(instance));

			//Eseguo la formattazione del messaggio se ci sono argomenti
		    var formattedMessage = arguments != null && arguments.Length > 0
			    ? string.Format(message, arguments)
			    : message;

			//Accodo il messaggio
			instance.Add(new ValidationResult(formattedMessage));
		}
    }
}
