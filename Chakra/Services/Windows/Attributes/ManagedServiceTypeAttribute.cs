using System;

namespace ZenProgramming.Chakra.Services.Windows.Attributes
{
    /// <summary>
    /// Represents attribute for mark managed service installer
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ManagedServiceTypeAttribute : Attribute
    {
        /// <summary>
        /// Managed windows service class type
        /// </summary>
        public Type ManagedServiceType { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="managedServiceType">Managed windows service type</param>
        public ManagedServiceTypeAttribute(Type managedServiceType) 
        {
            //Imposto le informazioni nelle proprietà
            ManagedServiceType = managedServiceType;
        }
    }
}
