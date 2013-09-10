using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleX;

namespace Meltdown.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            MainConsole console = MainConsole.Instance;
            console.Color = Color.Red;
            console.Write(0, 0, "RED");
            console.Color = Color.Yellow;
            console.Write(3, 0, "?!");
            console.Refresh();
            Console.ReadKey();

        }
    }
}
