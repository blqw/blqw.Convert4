using blqw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            CodeTimer.Initialize();
            var count = 100000;
            {
                var s = int.MaxValue.ToString();
                CodeTimer.Time("系统方式转型性能1", count, () => int.TryParse(s, out var j));
                CodeTimer.Time("Convert4方式转型性能1", count, () => s.To(0));
            }

            {
                var s = "1,2,3,4";
                CodeTimer.Time("系统方式转型性能2", count, () => s.Split(',').Select(int.Parse).ToList());
                CodeTimer.Time("Convert4方式转型性能2", count, () => s.To<List<int>>());
            }

            {
                var s = "1;2;3;4;;;";
                CodeTimer.Time("系统方式转型性能3", count, () => s.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList());
                CodeTimer.Time("Convert4方式转型性能3", count, () => s.Convert<List<int>>(new ConvertSettings()
                                                                    .AddStringSeparator(';')
                                                                    .AddStringSplitOptions(StringSplitOptions.RemoveEmptyEntries)));
            }

            {
                var s = Guid.NewGuid().ToString();
                CodeTimer.Time("系统方式转型性能4", count, () => Guid.Parse(s));
                CodeTimer.Time("Convert4方式转型性能4", count, () => s.To<Guid>());
            }

            {
                var s = "2018-02-23 14:02:10";
                CodeTimer.Time("系统方式转型性能5", count, () => DateTime.Parse(s));
                CodeTimer.Time("Convert4方式转型性能5", count, () => s.To<DateTime>());
            }

            {
                var s = int.MaxValue.ToString();
                CodeTimer.Time("系统方式转型性能1", count, () => int.TryParse(s, out var j));
                CodeTimer.Time("Convert4方式转型性能1", count, () => s.To(0));
            }

            {
                var s = "1,2,3,4";
                CodeTimer.Time("系统方式转型性能2", count, () => s.Split(',').Select(int.Parse).ToList());
                CodeTimer.Time("Convert4方式转型性能2", count, () => s.To<List<int>>());
            }

            {
                var s = "1;2;3;4;;;";
                CodeTimer.Time("系统方式转型性能3", count, () => s.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList());
                CodeTimer.Time("Convert4方式转型性能3", count, () => s.Convert<List<int>>(new ConvertSettings()
                                                                    .AddStringSeparator(';')
                                                                    .AddStringSplitOptions(StringSplitOptions.RemoveEmptyEntries)));
            }

            {
                var s = Guid.NewGuid().ToString();
                CodeTimer.Time("系统方式转型性能4", count, () => Guid.Parse(s));
                CodeTimer.Time("Convert4方式转型性能4", count, () => s.To<Guid>());
            }

            {
                var s = "2018-02-23 14:02:10";
                CodeTimer.Time("系统方式转型性能5", count, () => DateTime.Parse(s));
                CodeTimer.Time("Convert4方式转型性能5", count, () => s.To<DateTime>());
            }

            Console.ReadLine();
        }
    }
}
