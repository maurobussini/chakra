namespace ZenProgramming.Chakra.WebApi.Filters.Models
{
    /// <summary>
    /// Represents authentication model
    /// </summary>
    public class AuthenticationTrace
    {
        /// <summary>
        /// Flag for authenticated requests
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// Authentication type
        /// </summary>
        public string AuthenticationType { get; set; }

        /// <summary>
        /// User identity name
        /// </summary>
        public string IdentityName { get; set; }
    }
}
