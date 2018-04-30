using System.Collections.Generic;
using System.Configuration;

namespace ZenProgramming.Chakra.Configurations
{
    /// <summary>
    /// Represents generic collection for multiple elements
    /// </summary>
    /// <typeparam name="TConfigurationElement">Type of configuration element</typeparam>
    public class GenericConfigurationElementCollection<TConfigurationElement> : ConfigurationElementCollection, IEnumerable<TConfigurationElement>
        where TConfigurationElement : ConfigurationElement, new()
    {
        #region Public properties
        /// <summary>
        /// Elements of collection
        /// </summary>
        protected List<TConfigurationElement> Elements { get; private set; }
        /// <summary>
        /// Return element using index
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns></returns>
        public TConfigurationElement this[int index]
        {
            get { return Elements[index]; }
        }        
        #endregion

        protected GenericConfigurationElementCollection()
        {
            //Inizializzo la collezione di elementi
            Elements = new List<TConfigurationElement>();
        }

        /// <summary>
        /// Creates a new configuration element
        /// </summary>
        /// <returns>Returns new configuration element</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            TConfigurationElement newElement = new TConfigurationElement();
            Elements.Add(newElement);
            return newElement;
        }

        /// <summary>
        /// Gets the element key for a specified configuration element        
        /// </summary>
        /// <param name="element">The onfiguration element to return the key for.</param>
        /// <returns>Returns an object that acts as the key for the specified configuration element.</returns>        
        protected override object GetElementKey(ConfigurationElement element)
        {
            //Recupero dalla lista l'elemento specificato
            return Elements.Find(e => e.Equals(element));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>Returns enumerator that can be used to iterate through the collection.</returns>
        public new IEnumerator<TConfigurationElement> GetEnumerator()
        {
            //Ritorno l'enumeratore
            return Elements.GetEnumerator();
        }
    }
}
