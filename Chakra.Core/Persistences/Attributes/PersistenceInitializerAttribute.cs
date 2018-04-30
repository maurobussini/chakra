using System;
using ZenProgramming.Chakra.Core.Reflection.Attributes;

namespace ZenProgramming.Chakra.Core.Persistences.Attributes
{
    /// <summary>
    /// Attribute for persistence initializer
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PersistenceInitializerAttribute: ExportableAttribute
    {
    }
}
