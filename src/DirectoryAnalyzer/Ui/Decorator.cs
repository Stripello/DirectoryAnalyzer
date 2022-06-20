using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryAnalyzer.Ui
{
    internal static class Decorator
    {
        internal static void IndicateWork()
        {
            Console.Write(' ');
            while (true)
            {
                Console.Write("\b|");
                Thread.Sleep(300);
                Console.Write("\b/");
                Thread.Sleep(300);
                Console.Write("\b-");
                Thread.Sleep(300);
                Console.Write("\b\\");
                Thread.Sleep(300);
            }
        }
    }
}
