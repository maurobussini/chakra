﻿using System;
using ZenProgramming.Chakra.Core.Data;

namespace ZenProgramming.Chakra.Core.Mocks.Async.Data.Extensions
{
    /// <summary>
    /// Extensions for data session
    /// </summary>
    public static class DataSessionExtensions
    {
        /// <summary>
        /// Convert provided data session in "IMockDataSession"
        /// </summary>
        /// <param name="instance">Instance</param>
        /// <returns>Returns converted data session</returns>
        public static IMockDataSessionAsync AsMockDataSessionAsync(this IDataSessionAsync instance) 
        {
            //Arguments validation
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            //Tento il cast della sessione generica a MockDataSession
            if (instance is IMockDataSessionAsync mockSession)
                return mockSession;

            throw new InvalidCastException($"Specified data session of type '{instance.GetType().FullName}' " + 
                $"cannot be converted to type '{typeof(IMockDataSessionAsync).FullName}'.");

            //Returns instance
        }
    }
}