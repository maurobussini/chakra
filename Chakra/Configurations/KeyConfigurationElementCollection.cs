using System.Linq;

namespace ZenProgramming.Chakra.Configurations
{
    /// <summary>
    /// Represents collection for multiple elements identified by key
    /// </summary>
    /// <typeparam name="TKeyConfigurationElement">Type of key configuration element</typeparam>
    public class KeyConfigurationElementCollection<TKeyConfigurationElement> : GenericConfigurationElementCollection<TKeyConfigurationElement>
        where TKeyConfigurationElement : KeyConfigurationElement, new()
    {
        #region Public properties
        /// <summary>
        /// Returns element using key
        /// </summary>
        /// <param name="key">Key of element</param>
        /// <returns></returns>
        public new TKeyConfigurationElement this[string key]
        {
            get { return Elements.SingleOrDefault(e => e.Key == key); }
        }
        #endregion
    }
}
