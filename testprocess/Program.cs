using System;
using System.Linq;

namespace testprocess
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Console.Out.WriteLine("This is written in stdout");
            Console.Out.WriteLine($"Arguments:\n [{String.Join(", ",args)}]");

            Console.Error.WriteLine("This is written in stderr");
            
            //Console.ReadLine();
            return 0;
        }
    }
}