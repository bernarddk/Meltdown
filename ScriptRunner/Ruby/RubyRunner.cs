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

        public T Execute<T>(string script, IDictionary<string, object> parameters)
        {
            var scope = engine.Runtime.CreateScope();

            foreach (var kvp in parameters)
            {
                scope.SetVariable(kvp.Key, kvp.Value);
            }

            var toReturn = engine.Execute(script, scope);

            if (toReturn is T)
            {
                return (T)toReturn;
            }
            else
            {
                throw new ArgumentException("Expected an instance of " + typeof(T).FullName + " but got " + toReturn.GetType().FullName + " instead.");
            }
        }
    }
}
