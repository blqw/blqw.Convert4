using blqw.Kanai;
using Microsoft.Extensions.DependencyInjection;
using System;
using blqw.DI;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {

            var interfaceTypes = typeof(Dictionary<string, object>).GetInterfaces().ToList();

            interfaceTypes.Sort((a, b) => a.IsAssignableFrom(b) ? 1 : b.IsAssignableFrom(a) ? -1 : 0);

            Console.WriteLine(interfaceTypes);
            new ServiceCollection()
                .AddKanai()
                .ConfigureServices()
                .BuildServiceProvider()
                .Configure();

            Console.WriteLine("1".Cast<int>(null));




        }
    }
}
