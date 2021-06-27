using System;

namespace ZenProgramming.Chakra.Core.WebApi.Helpers.ClientDetections
{
    /// <summary>
    /// Class for detect client capabilities
    /// </summary>
    public class ClientCapabilities
    {
        /// <summary>
        /// User agent
        /// </summary>
        public string UserAgent { get; private set; }

        /// <summary>
        /// Client browser
        /// </summary>
        public ClientBrowser ClientBrowser { get; private set; }

        /// <summary>
        /// Client operative system
        /// </summary>
        public ClientOs ClientOs { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userAgent">User agent string</param>
        public ClientCapabilities(string userAgent)
        {
            //Arguments validation
            if (string.IsNullOrEmpty(userAgent)) throw new ArgumentNullException(nameof(userAgent));

            //Set values
            UserAgent = userAgent;

            //Build client browser and os
            ClientBrowser = new ClientBrowser(userAgent);
            ClientOs = new ClientOs(userAgent);
        }        
    }
}
