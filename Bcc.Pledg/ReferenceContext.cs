using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Bcc.Pledg
{
    public class ReferenceContext
    {
        private readonly ILogger<ReferenceContext> _logger;
        public ReferenceContext(ILogger<ReferenceContext> logger) {
            _logger = logger;
        }

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
        public static object PostReference(string typeName, object sectors)
        {
            try
            {
                // if (References.TryAdd(Type.GetType($"Bcc.Pledg.Models.{typeName}", true, true), sectors)) {
                References.GetOrAdd(Type.GetType($"Bcc.Pledg.Models.{typeName}", true, true), sectors);
                if (References.TryGetValue(Type.GetType($"Bcc.Pledg.Models.{typeName}", true, true), out object value))
                    {
                    
                        return value;
                    }
               // }
            }
            catch (Exception e) {
                
                Console.WriteLine(e.Message);
            }

            

           
            return null;
        }
    }
}
