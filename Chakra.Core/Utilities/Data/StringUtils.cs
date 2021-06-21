using System;
using System.Linq;
using System.Text;

namespace ZenProgramming.Chakra.Core.Utilities.Data
{
    /// <summary>
    /// Contains utilities for manipulate strings 
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Convert decimal value to base-N string
        /// </summary>
        /// <param name="decimalValue">Decimal value</param>
        /// <param name="baseN">Base-N</param>
        /// <returns>Returns converted value</returns>
        public static string ConvertToBaseN(int decimalValue, int baseN)
        {
            //Validazione argomenti
            if (decimalValue < 0) throw new ArgumentOutOfRangeException(
                nameof(decimalValue), "Value base-10 must be greater then zero.");
            if (baseN < 0 || baseN > 36) throw new ArgumentOutOfRangeException(
                 nameof(baseN), "Base-N value must be greater then zero and lower or equals t0 36.");

            //Definisco i caratteri utilizzati
            const string AllowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            //Imposto una stringa di uscita e i valori resto
            string outString = "";
            int divideResult = decimalValue;

            //Eseguo un ciclo sull'elemento valore da convertire in "base N"
            //eseguendo una divisione con resto ad ogni ciclo
            do
            {
                //Eseguo la divisione tra il risultato della precedente divisione
                //e il valore di base, recuperando il resto e impostando il nuovo risultato
                int divideRemaining;
                divideResult = Math.DivRem(divideResult, baseN, out divideRemaining);

                //Eseguo la conversione del resto nel carattere di base 36
                //quindi lo accodo a sinistra della stringa di uscita
                outString = AllowedChars.ToArray()[divideRemaining] + outString;
            }
            //Eseguo il ciclo finchè ho un risultato maggiore di zero
            while (divideResult > 0);

            //Ritorno la stringa convertita
            return outString;
        }

        /// <summary>
        /// Convert data to decimal base to specified base
        /// </summary>
        /// <param name="sourceValue">Source base-N string</param>
        /// <param name="baseN">Base-N</param>
        /// <returns></returns>
        public static int ConvertFromBaseN(string sourceValue, int baseN)
        {
            //Validazione argomenti
            if (string.IsNullOrEmpty(sourceValue)) throw new ArgumentNullException(nameof(sourceValue));
            if (baseN <= 0) throw new ArgumentOutOfRangeException(nameof(baseN));

            //Definisco i caratteri utilizzati
            const string AllowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            //Verifico che la base specificata sia valida
            if (baseN > AllowedChars.Length) throw new InvalidProgramException(
                $"Base-N value must be greater then zero and lower or equals to {AllowedChars.Length}.");

            //Eseguo l'uppercase della stringa
            string elaboratedSource = sourceValue.ToUpper();            
            int decimalValue = 0;

            //Recupero solo i caratteri contemplati nella base definita
            //quindi separo in caratteri singoli la stringa sorgente
            string baseChars = AllowedChars.Substring(0, baseN);
            char[] allChars = elaboratedSource.ToArray();

            //Scorro tutti i caratteri (partendo dall'ultimo)            
            for (int i = 0; i < allChars.Length; i++)
            {
                //Recupero il carattere da elaborare
                var currentChar = allChars[i];

                //Calcolo il peso del carattere all'interno dei caratteri contemplati
                var weight = baseChars.IndexOf(currentChar);

                //Calcolo il moltiplicatore
                var multiplier = (allChars.Length - 1) - i;

                //Calcolo il valore decimale del carattere corrente come
                //posizione dello stesso nella stringa dei caratteri permessi
                //moltiplicato per la base, elevata alla posizione del carattere
                //all'interno della stringa sorgente (es. "100" in binario equivale
                //a 2(base)^2(posizione in stringa) * 1(posizione di "1" in "01")
                double currentDecimalValue = Math.Pow(baseN, multiplier) * weight;

                //Sommo il valore al precedente
                decimalValue = decimalValue + (int)Math.Round(currentDecimalValue, 0);
            }

            //Emetto il decimale
            return decimalValue;
        }

        /// <summary>
        /// Execute conversion of specified string in Camel-Case
        /// </summary>
        /// <param name="source">String to convert</param>
        /// <returns>Returns converted string</returns>
        public static string ConvertToCamelCase(string source)
        {
            //Se la stringa è vuota o nulla, la ritorno
            if (string.IsNullOrWhiteSpace(source))
                return source;

            //Inizializzo un builder per la stringa
            StringBuilder builder = new StringBuilder();

            //Verifico se ci sono oppure no minuscole
            bool noLowerCase = true;
            foreach (char ch in source)
                if (char.IsLower(ch))
                    noLowerCase = false;

            //Se il primo carattere è una lettera oppure un numero, lo inserisco
            if (char.IsLetterOrDigit(source[0]))
                builder.Append(char.ToUpper(source[0]));

            //Scorro tutti gli elementi partendo dal secondo
            for (int i = 1; i < source.Length; i++)
            {
                //Se il carattere corrente è una lettera/numero
                if (char.IsLetterOrDigit(source[i]))
                {
                    //Se quello precedente era una lettera/numero
                    if (!char.IsLetterOrDigit(source[i - 1]))
                    {
                        //Lo aggiungo come maiuscolo
                        builder.Append(char.ToUpper(source[i]));
                    }
                    //Se non ci sono minuscole
                    else if (noLowerCase)
                    {
                        //Lo aggiungo come minuscolo
                        builder.Append(char.ToLower(source[i]));
                    }
                    else
                    {
                        //Altrimenti semplicemente lo accodo
                        builder.Append(source[i]);
                    }
                }
            }

            //Mando in uscita il builder
            return builder.ToString();
        }

        /// <summary>
        /// Converts the string to camel case.
        /// </summary>
        /// <param name="source">Source string</param>
        /// <returns>Returns converted string</returns>
        public static string ToCamelCase(string source)
        {
            //Se ho solo un carattere o nessuno, ritorno la stringa
            if (source == null || source.Length < 2)
                return source;

            //Separlo la stringa in parole
            string[] words = source.Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            //Combino le parole
            string result = words[0].ToLower();

            //Scorro tutte le parole
            for (int i = 1; i < words.Length; i++)
            {
                //Aggiungo la parola con la prima maiuscola
                result += 
                    words[i].Substring(0, 1).ToUpper() +
                    words[i].Substring(1);
            }

            //Ritorno in uscita
            return result;
        }
    }
}
