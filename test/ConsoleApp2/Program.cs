using blqw.Kanai;
using Microsoft.Extensions.DependencyInjection;
using System;
using blqw.DI;
using System.Collections.Generic;
using System.Linq;
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
            var y = "a".ChangeType<int>(null);
            Debugger.Break();
            Console.WriteLine("1".ChangeType<int>(null));




        }
    }
}
