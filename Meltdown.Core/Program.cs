using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            new InteractiveFictionGame("Content.json").Start();
        }
    }
}
