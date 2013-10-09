using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptRunner.Core
{
    public static class ScriptHelper
    {
        public static List<string> ToStringList(dynamic source)
        {
            var found = new List<string>();
            ScriptType type = DetectScriptType(source);
            switch (type)
            {
                case ScriptType.Ruby:
                    for (int i = 0; i < source.Count; i++)
                    {
                        found.Add(source[i].ToString());
                    }
                    break;
                case ScriptType.Javascript:
                    for (int i = 0; i < source.length; i++)
                    {
                        found.Add(source[i]);
                    }
                    break;
                default:
                    throw new ArgumentException("Not sure how to check if " + source + " is an array.");
            }

            return found;
        }

        public static Boolean IsArray(dynamic source) {
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
                throw new ArgumentException("Can't figure out the script type.");
            }
        }

        public enum ScriptType { Ruby, Javascript }
    }
}
