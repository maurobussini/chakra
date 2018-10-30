using CSDetermineOSAndBrowserASPNETCore.UserAgent;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;

namespace ZenProgramming.Chakra.WebApi.Helpers
{
    /// <summary>
    /// Contains utilities for API service
    /// </summary>
    public static class ApiUtils
    {
        /// <summary>
        /// Get client IP address based on request
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Returns IP</returns>
        public static string GetClientIp()
        {
            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
            var ip = heserver.AddressList[2].ToString();
            return ip;
        }

        /// <summary>
        /// Get current user agent
        /// </summary>
        /// <returns>Returns user agent</returns>
        public static string GetUserAgent(HttpRequest request)
        {
            //Argument validations
            if (request == null) throw new ArgumentNullException(nameof(request));

            //Check if header exists
            if (request.Headers == null || !request.Headers.ContainsKey("User-Agent"))
                return null;

            //Get value of headers
            return request.Headers["User-Agent"].ToString();
        }

        /// <summary>
        /// Get requesting browser capabilities
        /// </summary>
        /// <returns>Returns capabilities structure</returns>
        public static ClientCapabilities GetClientCapabilities(HttpRequest request)
        {
            //First, get user agent
            var userAgent = GetUserAgent(request);

            //If invalid, return
            if (string.IsNullOrEmpty(userAgent))
                return null;

            //Returns capabilities
            return new ClientCapabilities(userAgent);
        }
    }
}
