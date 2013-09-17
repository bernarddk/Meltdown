using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptRunner.Ruby;

namespace ScriptRunner.Core
{    
    public class ScriptRunner
    {
        public static ScriptRunner Instance { get { return instance; } }

        private static ScriptRunner instance = new ScriptRunner();
        
        private readonly IDictionary<string, IRunner> SupportedEngines = new Dictionary<string, IRunner>() {
            { "rb", new RubyRunner() }
        };

        private IDictionary<string, object> parameters = new Dictionary<string, object>();

        private ScriptRunner()
        {
        }

        public ScriptRunner BindParameter(string key, object value)
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
                    return this.SupportedEngines[extension].Execute<T>(scriptName, parameters);
                }
            }
        }
    }
}
