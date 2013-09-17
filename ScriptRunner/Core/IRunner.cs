using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptRunner.Core
{
    // A runnable script engine
    interface IRunner
    {
        T Execute<T>(string scriptName, IDictionary<string, object> parameters);
    }
}
