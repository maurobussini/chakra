using System;
using System.Collections.Generic;
using System.Linq;

namespace ZenProgramming.Chakra.Core.Utilities.Data
{
    /// <summary>
    /// Contains utilities for randomization
    /// </summary>
    public static class RandomizationUtils
    {
        /// <summary>
        /// Get a random element from source list
        /// </summary>
        /// <typeparam name="TElement">Type of element</typeparam>
        /// <param name="elements">Elements list</param>
        /// <param name="random">Random generator instance</param>
        /// <returns>Returns a random element</returns>
        public static TElement GetRandomElement<TElement>(IList<TElement> elements, Random random = null)
        {
            //Eseguo la validazione degli argomenti
            if (elements == null) throw new ArgumentNullException(nameof(elements));

            //Se non ho elementi nella lista, ritorno null
            if (elements.Count == 0) return default(TElement);
            
            //Creo un oggetto random da inizializzare
            random = random ?? new Random();

            //Genero un numero random tra 0 e il numero di elementi massimi
            int position = random.Next(0, elements.Count());

            //Ritorno un elemento di posizione random indicata
            return elements[position];
        }

        /// <summary>
        /// Execute generation of random code
        /// </summary>
        /// <param name="digits">Number of digits</param>
        /// <param name="random">Random generator instance</param>
        /// <returns>Returns random code</returns>
        public static string GenerateRandomString(int digits, Random random = null)
        {
            //Validazione argomenti
            if (digits <= 0) throw new ArgumentOutOfRangeException(nameof(digits));
            
            //Creo un oggetto random da inizializzare
            random = random ?? new Random();

            //Genero un buffer di appoggio e randomizzo
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);

            //Eseguo la conversione del valore in esadecimale
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());

            //Se è del numero di cifre giusto, ritorno
            if (digits % 2 == 0)
                return result;

            //Se non è del numero di cifre giusto, aggiungo "0"
            return result + random.Next(16).ToString("X");
        }

        /// <summary>
        /// Generates a random email address
        /// </summary>
        /// <param name="random">Random generator instance</param>
        /// <returns>Random email</returns>
        public static string GenerateRandomEmail(Random random = null)
        {
            //Genero due stringa random per
            string username = GenerateRandomString(6, random);
            string secondLevel = GenerateRandomString(6, random);
            string firstLevel = GetRandomElement(new[] {"com", "it", "net"}, random);

            //Ritorno l'indirizzo composto
            return $"{username}@{secondLevel}.{firstLevel}";
        }

        /// <summary>
        /// Generates a random date/time
        /// </summary>
        /// <param name="from">From date</param>
        /// <param name="to">To date</param>
        /// <param name="random">Random generator instance</param>
        /// <returns>Random date/time component</returns>
        public static DateTime GetRandomDate(DateTime? from = null, DateTime? to = null, Random random = null)
        {
            //Se non ho impostato i limiti minimi o massimi, li imposto d'ufficio
            if (from == null) from = new DateTime(1900, 1, 1);
            if (to == null) to = new DateTime(2100, 1, 1);

            //Se la data di inizio è maggiore di quella di fine, errore
            if (from > to)
                throw new InvalidOperationException($"From date '{@from}' cannot be greater than to date '{to}'.");

            //Calcolo la differenza in giorni tra le date
            int differenceDays = (int)Math.Round(to.Value.Subtract(from.Value).TotalDays, 0);

            //Creo un oggetto random da inizializzare
            random = random ?? new Random();

            //Randomizzo un numero tra 0 e il numero di giorni
            int randomDays = random.Next(0, differenceDays);

            //Calcolo il numero di secondi in 24 ore
            int differenceSeconds = (int) Math.Round(DateTime.Now.Date.AddDays(1)
                .Subtract(DateTime.Now.Date).TotalSeconds);

            //Randomizzo un secondo
            int randomSeconds = random.Next(0, differenceSeconds);

            //Sommo i giorni e i secondi alla data di inizio
            return from.Value.AddDays(randomDays).AddSeconds(randomSeconds);
        }
    }
}
