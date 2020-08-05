using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Bcc.Pledg
{
    public class ReferenceContext
    {
        public static Dictionary<Type, string> Resources = new Dictionary<Type, string>();
        public static ConcurrentDictionary<Type, object> References = new ConcurrentDictionary<Type, object>();
        public static T[] GetReference<T>()
        {
            if (References.TryGetValue(typeof(T), out object value))
            {
                return value as T[];
            }

            return new T[0];
        }

        public static object GetReference(string typeName)
        {
            if (References.TryGetValue(Type.GetType($"Bcc.Pledg.Models.{typeName}", true, true), out object value))
            {
                return value;
            }

            return null;
        }
    }
}
