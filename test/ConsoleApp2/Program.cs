using blqw.Kanai;
using Microsoft.Extensions.DependencyInjection;
using System;
using blqw.DI;

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

            Console.WriteLine("1".Cast<int>(null));
        }
    }
}
