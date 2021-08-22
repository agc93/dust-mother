using System;

namespace DustMother.Generators
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ViewModelPropertyAttribute : Attribute
    {
        public ViewModelPropertyAttribute(string propertyName)
        {
            
        }

        public string? SavePropertyName { get; set; }
        public string? PendingChanges { get; set; }

        public bool? NoDefault { get; set; } = false;
    }
}
