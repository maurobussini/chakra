using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ZenProgramming.Chakra.Core.WebApi.Helpers.ClientDetections
{
    /// <summary>
    /// Class for matxh expression
    /// </summary>
    internal class MatchExpression
    {
        /// <summary>
        /// List of regular expressions
        /// </summary>
        public List<Regex> Regexes { get; set; }

        /// <summary>
        /// Action to execute
        /// </summary>
        public Action<Match, object> Action { get; set; }
    }
}
