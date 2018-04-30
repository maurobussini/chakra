using System;
using System.Security.Cryptography;
using System.Text;

namespace ZenProgramming.Chakra.Core.Utilities.Security
{
    /// <summary>
    /// Represents SHA string encoder/decoder processor
    /// </summary>
    public static class ShaProcessor
    {
        /// <summary>
        /// Encodes string using SHA-1 algotithm
        /// </summary>
        /// <param name="input">String to encode</param>
        /// <returns>Returns enconded string in SHA-1</returns>
        public static string Sha1Encrypt(string input)
        {
            //Eseguo la codifica con l'algoritmo selezionato
            return ShaEncrypt<SHA1CryptoServiceProvider>(input);

            #region OLD VERSION
            ////Utilizzo l'encoder e il provider di codifica
            //UTF8Encoding encoder = new UTF8Encoding();
            //using (SHA1CryptoServiceProvider sha1Hasher = new SHA1CryptoServiceProvider())
            //{
            //    //Eseguo la computazione, quindi la conversione in stringa da array
            //    byte[] hashedDataBytes = sha1Hasher.ComputeHash(encoder.GetBytes(input));
            //    return ByteArrayToString(hashedDataBytes);
            //}
            #endregion
        }

        /// <summary>
        /// Encodes string using SHA-256 algotithm
        /// </summary>
        /// <param name="input">String to encode</param>
        /// <returns>Returns enconded string in SHA-256</returns>
        public static string Sha256Encrypt(string input)
        {
            //Eseguo la codifica con l'algoritmo selezionato
            return ShaEncrypt<SHA256CryptoServiceProvider>(input);
        }

        /// <summary>
        /// Encodes string using provided hash algoritm
        /// </summary>
        /// <param name="input">String to encode</param>
        /// <returns>Returns encoded string</returns>
        public static string ShaEncrypt<THashAlgoritm>(string input)
            where THashAlgoritm : HashAlgorithm
        {
            //Utilizzo l'encoder e il provider di codifica
            UTF8Encoding encoder = new UTF8Encoding();
            using (HashAlgorithm cryptoTransform = Activator.CreateInstance<THashAlgoritm>())
            {
                //Eseguo la computazione, quindi la conversione in stringa da array
                byte[] hashedDataBytes = cryptoTransform.ComputeHash(encoder.GetBytes(input));
                return ByteArrayToString(hashedDataBytes);
            }
        }

        /// <summary>
        /// Converts a byte array in string
        /// </summary>
        /// <param name="inputArray">Input array</param>
        /// <returns>String version</returns>
        private static string ByteArrayToString(byte[] inputArray)
        {
            //Mediante stringbuikder decodifico un valore esadecimale
            StringBuilder output = new StringBuilder("");
            for (int i = 0; i < inputArray.Length; i++)
                output.Append(inputArray[i].ToString("X2"));
            return output.ToString();
        }
    }
}
