using System;

namespace ZenProgramming.Chakra.Core.Reflection.Attributes
{
    /// <summary>
    /// Attrubute for mark exportable elements
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class ExportableAttribute: Attribute
    {
    }
}
