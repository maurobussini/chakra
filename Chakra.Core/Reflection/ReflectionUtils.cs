using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ZenProgramming.Chakra.Core.Reflection
{
    /// <summary>
    /// Class with utilities for reflection operations
    /// </summary>
    public static class ReflectionUtils
    {
        /// <summary>
        /// Executes verticalization of properties of object 
        /// </summary>
        /// <param name="data">Object data</param>
        /// <returns>Returns dictionary</returns>
        public static IDictionary<string, object> VerticalizeProperties(object data)
        {
            //Eseguo la validazione degli argomenti
            if (data == null) throw new ArgumentNullException(nameof(data));

            //Recupero il tipo dell'elemento passato
            Type dataType = data.GetType();

            //Inizializzo il dizionario di uscita
            IDictionary<string, object> dictionary = new Dictionary<string, object>();

            //Recupero tutte le proprietà pubbliche
            PropertyInfo[] propertyInfos = dataType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            //Scorro tutte le proprietà e genero i valori
            foreach (var propertyInfo in propertyInfos)
            {
                //Tento il recupero dell'attributo display sulla proprietà
                DisplayAttribute displayAttribute = GetSingleAttribute<DisplayAttribute>(propertyInfo, false);

                //Se ho l'attributo impostato, prendo il valore, altrimenti il nome della proprietà
                string name = displayAttribute == null ? propertyInfo.Name : displayAttribute.Name;

                //Eseguo l'estrazione del valore della proprietà
                object value = propertyInfo.GetValue(data, null);

                //Compongo l'oggetto da accodare
                dictionary.Add(name, value);
            }

            //Mando in uscita
            return dictionary;
        }

        /// <summary>
        /// Generate type using its full type name
        /// </summary>
        /// <param name="typeFullName">Full type name</param>
        /// <returns>Returns instance of type or null value</returns>
        public static Type GenerateType(string typeFullName)
        {
            //Eseguo la validazione degli argomenti
            if (string.IsNullOrEmpty(typeFullName)) throw new ArgumentNullException(nameof(typeFullName));

            //Tento la generazione del tipo già presente nel dominio, evitando di 
            //emettere eccezione nel caso in cui non sia possibile procedere alla creazione
            Type generatedType = Type.GetType(typeFullName, false);

            //Se il type è stato generato, esco dalla funzione
            if (generatedType != null)
                return generatedType;

            //Recupero l'elenco degli assembly dell'applicazione
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            //Scorro l'elenco di tutti gli assembly contenuti
            //nel dominio applicativo e cerco di eseguire la creazione
            //del tipo specificato per ciascun assembly in elenco
            foreach (Assembly currentAssembly in assemblies)
            {
                //Eseguo la generazione del tipo specificato dal nome
                generatedType = currentAssembly.GetType(typeFullName, false, true);

                //Se il tipo è stato generato, esco dala funzione
                if (generatedType != null) return generatedType;
            }

            //Se arrivo a questo punto, ritorno null
            return null;
        }

        /// <summary>
        /// Get single attribute of specified type
        /// </summary>
        /// <typeparam name="TAttribute">Type of attribute</typeparam>
        /// <param name="sourceType">Source type</param>
        /// <param name="inherit">Must search in inheritance tree</param>
        /// <returns>Returns single attribute</returns>
        public static TAttribute GetSingleAttribute<TAttribute>(Type sourceType, bool inherit)
            where TAttribute : Attribute
        {
            //Eseguo la validazione degli argomenti
            if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));

            //Eseguo il recupero degli attributi custom (specificando il tipo)
            object[] attributes = sourceType.GetCustomAttributes(typeof(TAttribute), inherit);

            //Se l'attributo non è stato trovato, esco tornando null
            if (attributes.Length == 0)
                return null;

            //Se ho trovato più di un attributo, emetto eccezione
            if (attributes.Length > 1)
                throw new InvalidProgramException(
                    $"Found {attributes.Length} attributes of type '{typeof(TAttribute).FullName}' on source type '{sourceType.FullName}': " +
                    "a single attribute must be provided.");

            //Recupero l'attributo di relational per l'oggetto corrente
            return attributes[0] as TAttribute;
        }

        /// <summary>
        /// Get single attribute of specified property
        /// </summary>
        /// <typeparam name="TAttribute">Type of attribute to search</typeparam>
        /// <param name="property">Property info instance</param>
        /// <param name="inherit">Search attribute in subclasses</param>
        /// <returns>Return istance of attribute</returns>
        public static TAttribute GetSingleAttribute<TAttribute>(PropertyInfo property, bool inherit)
            where TAttribute : Attribute
        {
            //Eseguo la validazione degli argomenti
            if (property == null) throw new ArgumentNullException(nameof(property));

            //Eseguo il recupero degli attributi custom (specificando il tipo)
            object[] attributes = property.GetCustomAttributes(typeof(TAttribute), inherit);

            //Se l'attributo non è stato trovato, esco tornando null
            if (attributes.Length == 0)
                return null;

            //Se ho trovato più di un elemento, emetto eccezione
            if (attributes.Length > 1)
                throw new InvalidOperationException("Unable to recover a single attribute " +
                                                    $"of type '{typeof(TAttribute)}' on property '{property.Name}'. Found {attributes.Length} elements.");

            //Recupero l'attributo di relational per l'oggetto corrente
            return attributes[0] as TAttribute;
        }

        /// <summary>
        /// Get instance of custom attribute on specific enum value
        /// </summary>
        /// <typeparam name="TEnum">Type of enum</typeparam>
        /// <typeparam name="TAttribute">Type of attribute</typeparam>
        /// <param name="enumValue">Enumeration value</param>
        /// <returns>Returns attribute instance or null</returns>
        public static TAttribute GetSingleAttribute<TEnum, TAttribute>(TEnum enumValue)
            where TAttribute : Attribute
            where TEnum : struct
        {
            //Recupero il type dell'enumerazione
            Type enumElementType = typeof(TEnum);

            //Recupero il membro dell'enumerazione
            var memberInfo = enumElementType.GetMember(enumValue.ToString());

            //Se non ho trovato il membro cercato, emetto eccezione
            if (memberInfo.Length == 0) throw new NullReferenceException("Unable to " +
                                                                         $"find element '{enumValue}' on type '{typeof(TEnum).FullName}'.");

            //Recupero l'istanza dell'attributo
            TAttribute attributeInstance = memberInfo[0].GetCustomAttribute<TAttribute>(false);
            return attributeInstance;
        }

        /// <summary>
        /// Fetch list of specified attribute on provided type
        /// </summary>
        /// <typeparam name="TAttribute">Type of attribute</typeparam>
        /// <param name="sourceType">Source type</param>
        /// <param name="inherit">Must search in inheritance tree</param>
        /// <returns>Returns single attribute</returns>
        public static IList<TAttribute> FetchMultipleAttributes<TAttribute>(Type sourceType, bool inherit)
            where TAttribute : Attribute
        {
            //Eseguo la validazione degli argomenti
            if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));

            //Inizializzo la lista di uscita
            IList<TAttribute> outValues = new List<TAttribute>();

            //Eseguo il recupero degli attributi custom (specificando il tipo)
            object[] attributes = sourceType.GetCustomAttributes(typeof(TAttribute), inherit);

            //Scorro tutti gli elementi, li converto e li aggiungo
            foreach (var attribute in attributes)
                outValues.Add(attribute as TAttribute);
            
            //Ritorno la lista di elementi
            return outValues;
        }        
    }
}
