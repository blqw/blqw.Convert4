using blqw.Kanai;
using Microsoft.Extensions.DependencyInjection;
using System;
using blqw.DI;
using System.Diagnostics;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            new ServiceCollection()
                .AddKanai()
                .ConfigureServices()
                .BuildServiceProvider()
                .Configure();

            //var x = "1".ChangeType<int>(null);
            var y = (
                "0xFF".ChangeType<double>(null),
                "0xFF".ChangeType<float>(null),
                "0b0101".ChangeType<double>(null),
                "0b0101".ChangeType<float>(null),
                "0xFF".ChangeType<decimal>(null),
                "0b0101".ChangeType<decimal>(null));
            Debugger.Break();
            Console.WriteLine("1".ChangeType<int>(null));




        }
    }
}
