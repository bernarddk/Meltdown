using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Scripting.Hosting;
using ScriptRunner.Core;

namespace ScriptRunner.Ruby
{
    class RubyRunner : IRunner
    {
        private ScriptEngine engine = IronRuby.Ruby.CreateEngine();
        private string commonHeaderScript = "";

        public RubyRunner()
        {
            if (File.Exists(@"Ruby\Common.rb"))
            {
                this.commonHeaderScript = File.ReadAllText(@"Ruby\Common.rb");
            }
        }

        public T Execute<T>(string script, IDictionary<string, object> parameters)
        {
            var scope = engine.Runtime.CreateScope();

            foreach (var kvp in parameters)
            {
                scope.SetVariable(kvp.Key, kvp.Value);
            }

            var finalScript = string.Format("{0}\n{1}", this.commonHeaderScript, script);
            var toReturn = engine.Execute(finalScript, scope);

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
