using System;
using ZenProgramming.Chakra.Core.Reflection.Attributes;

namespace ZenProgramming.Chakra.Core.Data.Repositories.Attributes
{
    /// <summary>
    /// Attribute for mark repository class implementation
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RepositoryAttribute: ExportableAttribute
    {
    }
}
