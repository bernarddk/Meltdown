using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Scripting.Hosting;
using ScriptRunner.Core;

namespace ScriptRunner.Ruby
{
    class RubyRunner : IRunner
    {
        private ScriptEngine engine = IronRuby.Ruby.CreateEngine();

        public T Execute<T>(string scriptFullPath, IDictionary<string, object> parameters)
        {
            var scope = engine.Runtime.CreateScope();

            foreach (var kvp in parameters)
            {
                scope.SetVariable(kvp.Key, kvp.Value);
            }

            string contents = System.IO.File.ReadAllText(scriptFullPath);
            var toReturn = engine.Execute<T>(contents, scope);
            return toReturn;
        }
    }
}
