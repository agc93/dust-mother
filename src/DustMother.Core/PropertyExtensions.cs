using System;
using System.Collections.Generic;
using System.Linq;
using UnSave.Types;

namespace DustMother.Core
{
    public static class PropertyExtensions {
        public static T FindProperty<T>(this IEnumerable<IUnrealProperty> properties, Func<IUnrealProperty, bool> predicate) where T : class {
            return properties.FirstOrDefault(predicate) as T;
        }
    }
}