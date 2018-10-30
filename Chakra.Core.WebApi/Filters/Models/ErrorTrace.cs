namespace ZenProgramming.Chakra.WebApi.Filters.Models
{
    /// <summary>
    /// Represents error trace
    /// </summary>
    public class ErrorTrace
    {
        /// <summary>
        /// Flag that specify if has error
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// Message of error
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Full stack trace
        /// </summary>
        public string StackTrace { get; set; }
    }
}
