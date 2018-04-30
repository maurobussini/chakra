using System.Configuration;

namespace ZenProgramming.Chakra.Configurations
{
    /// <summary>
    /// Represents configuration element with key
    /// </summary>
    public abstract class KeyConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// Key of the current value
        /// </summary>
        public abstract string Key { get; set; }
    }
}
