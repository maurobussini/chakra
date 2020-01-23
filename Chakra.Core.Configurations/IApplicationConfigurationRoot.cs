namespace ZenProgramming.Chakra.Core.Configurations
{
    /// <summary>
    /// Interface for application configuration root
    /// </summary>
    public interface IApplicationConfigurationRoot
    {
        /// <summary>
        /// Name of current environment
        /// </summary>
        string EnvironmentName { get; set; }
    }
}
