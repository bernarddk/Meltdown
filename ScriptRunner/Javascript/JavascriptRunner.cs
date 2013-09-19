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
    class JavascriptRunner : IRunner
    {
        V8ScriptEngine engine = new V8ScriptEngine();

        public JavascriptRunner()
        {
            engine.AddHostType("CommandAction", typeof(Meltdown.Core.Command.CommandAction));
            engine.AddHostType("Command", typeof(Command));
            engine.AddHostType("InteractiveObject", typeof(InteractiveObject));
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
    }
}
