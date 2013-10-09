using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ClearScript.V8;
using ScriptRunner.Core;

namespace ScriptRunner.Javascript
{
    class JavascriptRunner : IRunner, IDisposable
    {
        V8ScriptEngine engine;

        public JavascriptRunner()
        {
            this.ResetEngine();
        }

        private void ResetEngine()
        {
            this.engine = new V8ScriptEngine();
            var assembly = System.Reflection.Assembly.Load("MeltDown.Core");

            foreach (var type in assembly.GetTypes())
            {
                engine.AddHostType(type.Name, type);
            }

            // Used for anonymous functions
            engine.AddHostType("Action", typeof(Action));
            // JS doesn't support console.log (JS/V8/Chrome style)
            engine.AddHostType("Console", typeof(Console));
        }

        public T Execute<T>(string script, IDictionary<string, object> parameters)
        {

            foreach (var kvp in parameters)
            {
                // TODO: these are engine-scope. Call ResetEngine mmkay?
                engine.AddHostObject(kvp.Key, kvp.Value);
            }

            var toReturn = engine.Evaluate(script);

            if (toReturn is T)
            {
                return (T)toReturn;
            }
            else
            {
                throw new ArgumentException("Expected " + typeof(T).FullName + " but got " + toReturn.GetType().FullName);
            }
        }

        public void Dispose()
        {
            this.engine.Dispose();
        }
    }
}
