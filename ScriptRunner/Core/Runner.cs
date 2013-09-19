using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptRunner.Javascript;
using ScriptRunner.Ruby;

namespace ScriptRunner.Core
{    
    public class Runner
    {
        public static Runner Instance { get { return instance; } }

        private static Runner instance = new Runner();
        
        private readonly IDictionary<string, IRunner> SupportedEngines = new Dictionary<string, IRunner>() {
            { "rb", new RubyRunner() },
            { "js", new JavascriptRunner() }
        };

        private IDictionary<string, object> parameters = new Dictionary<string, object>();
        
        public Runner BindParameter(string key, object value)
        {
            this.parameters[key] = value;
            return this;
        }

        public T Execute<T>(string scriptName)
        {
            if (!scriptName.Contains('.'))
            {
                throw new ArgumentOutOfRangeException("Script name (" + scriptName + ") must have an extension.");
            }
            else
            {
                string extension = scriptName.Substring(scriptName.LastIndexOf('.') + 1).ToLower();
                if (!SupportedEngines.ContainsKey(extension))
                {
                    throw new ArgumentOutOfRangeException("ScriptRunner can't run ." + extension + " files. Valid extensions are: " + string.Join(",", SupportedEngines));
                }
                else
                {
                    string script = System.IO.File.ReadAllText(scriptName);
                    return this.SupportedEngines[extension].Execute<T>(script, parameters);
                }
            }
        }
    }
}
