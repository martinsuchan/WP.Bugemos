using System;
using System.Collections.Generic;

namespace UtilityBelt
{
    /// <summary>
    /// Simple dependency injector context.
    /// </summary>
    public class Context
    {
        public static T Get<T>()
            where T : class
        {
            Type t = typeof (T);
            if (!dict.ContainsKey(t))
            {
                throw new InvalidOperationException(string.Format("Type not found {0}", t));
            }

            return dict[t] as T;
        }

        public static void Set<T>(T obj)
            where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            dict[typeof (T)] = obj;
        }

        private static readonly Dictionary<Type, object> dict = new Dictionary<Type, object>();
        //private static readonly Dictionary<Type, Dictionary<string, object>> namedDict = new Dictionary<Type, Dictionary<string, object>>();
    }
}
