using System;

namespace ZenProgramming.Chakra.Configurations.Attributes
{
    /// <summary>
    /// Attribute used to mark configuration section
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigurationSectionAttribute : Attribute
    {
        /// <summary>
        /// Configuration tag name
        /// </summary>
        public string TagName { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tagName">Configuration tag name</param>
        public ConfigurationSectionAttribute(string tagName)
        {
            //Eseguo la validazione degli argomenti
            if (string.IsNullOrEmpty(tagName)) throw new ArgumentNullException(nameof(tagName));

            //Imposto il valore della proprietà
            TagName = tagName;
        }
    }
}
