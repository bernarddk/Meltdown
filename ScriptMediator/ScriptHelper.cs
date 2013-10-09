using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptMediator
{
    public static class ScriptHelper
    {
        public static List<T> ToList<T>(dynamic source)
        {
            var found = new List<T>();
            ScriptType type = DetectScriptType(source);
            switch (type)
            {
                case ScriptType.Ruby:
                    for (int i = 0; i < source.Count; i++)
                    {
                        object next = source[i];
                        found.Add(GetAsType<T>(next));
                    }
                    break;
                case ScriptType.Javascript:
                    for (int i = 0; i < source.length; i++)
                    {
                        object next = source[i];
                        found.Add(GetAsType<T>(next));
                    }
                    break;
                default:
                    throw new ArgumentException("Not sure how to check if " + source + " is an array.");
            }

            return found;
        }

        private static T GetAsType<T>(dynamic next)
        {                        
            if (next is T)
            {
                return (T)next;
            }
            else
            {
                if (typeof(T) == typeof(string))
                {
                    return next.ToString();
                }
                else
                {
                    throw new ArgumentException("Expected instances of " + typeof(T).FullName + ", but found " + next.GetType().FullName + " (" + next + ")");
                }
            }
        }

        public static bool IsArray(dynamic source) {
            // A poor man's language independence
            ScriptType type = DetectScriptType(source);
            switch (type) {
                case ScriptType.Ruby:
                    return source.GetType().Name == "RubyArray";
                case ScriptType.Javascript:
                    return source.constructor.name == "Array";            
                default:
                    throw new ArgumentException("Not sure how to check if " + source + " is an array.");
            }
        }

        public static ScriptType DetectScriptType(dynamic source)
        {
            Type type = source.GetType();
            if (type.FullName.StartsWith("IronRuby"))
            {
                return ScriptType.Ruby;
            }
            else if (type.FullName.StartsWith("Microsoft.ClearScript"))
            {
                return ScriptType.Javascript;
            }
            else
            {
                throw new ArgumentException("Can't figure out the script type for " + source);
            }
        }
    }
}
