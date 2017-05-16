using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            TimeSpan span = new TimeSpan(-7, -7, -7);
            TimeSpan duration = span.Duration();
            Console.WriteLine(duration);
        }
}
