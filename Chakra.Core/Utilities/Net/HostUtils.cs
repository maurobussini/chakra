using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace ZenProgramming.Chakra.Core.Utilities.Net
{
    /// <summary>
    /// Utilities for manage host
    /// </summary>
    public static class HostUtils
    {
        /// <summary>
        /// Returns list IPv4 address on current host
        /// </summary>
        /// <returns>Returns a list of addresses</returns>
        public static IList<IPAddress> GetLocalIPv4Addresses()
        {
            //Predispongo una lista di elementi di uscita
            IList<IPAddress> addresses = new List<IPAddress>();

            //Utilizzando il nome della macchina locale, recupero l'oggetto host
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());

            //Scorro l'elenco degli indirizzi finchè non individuo
            //quello su IPv4 che mi serve (enumerazione : "AddressFamily.InterNetwork"
            foreach (IPAddress currentAddress in hostEntry.AddressList)
                if (currentAddress.AddressFamily == AddressFamily.InterNetwork)
                    addresses.Add(currentAddress);

            //Mando un uscita la lista di indirizzi
            return addresses;
        }

        /// <summary>
        /// Execute a "ping" operation on specified address
        /// and return a reply from the remote host
        /// </summary>
        /// <param name="targetIpAddress">Target address</param>
        /// <returns>Return a reply</returns>
        public static PingReply Ping(string targetIpAddress)
        {
            //Eseguo la validazione degli argomenti
            if (string.IsNullOrEmpty(targetIpAddress)) throw new ArgumentNullException(nameof(targetIpAddress));

            //Istanzio un nuovo oggetto ping e uno di opzioni
            using (Ping pingSender = new Ping())
            {
                //Imposto le informazioni opzionali nell'oggetto
                PingOptions options = new PingOptions { DontFragment = true };

                //Eseguo una chiamata Ping inviando un buffer di dati
                //di test (senza significato) solo per verificare la ricezione
                return pingSender.Send(targetIpAddress, 120,
                    Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"), options);
            }
        }

        /// <summary>
        /// Get host name of specific IP address
        /// </summary>
        /// <param name="ipAddress">IP address</param>
        /// <returns>Returns host name</returns>
        public static string GetHostName(string ipAddress)
        {
            //Eseguo la validazione degli argomenti
            if (string.IsNullOrEmpty(ipAddress)) throw new ArgumentNullException(nameof(ipAddress));

            //Eseguo il parsing dell'indirizzo IP
            IPAddress addr = IPAddress.Parse(ipAddress);

            //Recupero l'entry dell'host corrente
            IPHostEntry entry = Dns.GetHostEntry(addr);

            //Emetto il nome
            return entry.HostName;
        }
    }
}
