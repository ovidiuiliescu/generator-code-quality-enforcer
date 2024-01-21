using System;

namespace Common
{
    /// <summary>
    /// Simple demo attribute for validation/code quality enforcement.
    /// In practice, this can be extended with more metadata to allow for better control.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SimpleAttribute : Attribute
    {
    }
}
