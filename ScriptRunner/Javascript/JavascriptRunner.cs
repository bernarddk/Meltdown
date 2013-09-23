using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meltdown.Core;
using Meltdown.Core.Model;
using Microsoft.ClearScript.V8;
using ScriptRunner.Core;

namespace ScriptRunner.Javascript
{
    class JavascriptRunner : IRunner, IDisposable
    {
        V8ScriptEngine engine = new V8ScriptEngine();

        public JavascriptRunner()
        {
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
