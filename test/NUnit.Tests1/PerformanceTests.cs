using System.Security.AccessControl;
using blqw.ConvertServices;
using System.Collections;
using blqw;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;

namespace NUnit.Tests1
{
    [TestFixture]
    public class PerformanceTests
    {
        public PerformanceTests()
        {
            var types = new Type[] {
                typeof(string),
                typeof(int),
                typeof(DateTime),
                typeof(List<int>),
                typeof(long),
                typeof(float),
                typeof(double),
                typeof(char),
                typeof(Guid),
            };

            foreach (var type in types)
            {
                Console.WriteLine("".ChangeType(type, null, null));
            }
        }

        [Test]
        public void 系统方式转型性能1()
        {
            var s = int.MaxValue.ToString();
            for (var i = 0; i < 100000; i++)
            {
                int.TryParse(s, out var j);
            }
        }

        [Test]
        public void Convert4方式转型性能1()
        {
            var s = int.MaxValue.ToString();
            for (var i = 0; i < 100000; i++)
            {
                var j = s.To(0);
            }
        }


        [Test]
        public void 系统方式转型性能2()
        {
            var s = "1,2,3,4";
            for (var i = 0; i < 100000; i++)
            {
                var list = s.Split(',').Select(int.Parse).ToList();
            }
        }

        [Test]
        public void Convert4方式转型性能2()
        {
            var s = "1,2,3,4";
            for (var i = 0; i < 100000; i++)
            {
                var list = s.To<List<int>>();
            }
        }

        [Test]
        public void 系统方式转型性能3()
        {
            var s = "1;2;3;4;;;";
            for (var i = 0; i < 100000; i++)
            {
                var list = s.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            }
        }

        [Test]
        public void Convert4方式转型性能3()
        {
            var s = "1;2;3;4;;;";
            for (var i = 0; i < 100000; i++)
            {
                var list = s.To<List<int>>(new ConvertSettings()
                                                .AddStringSeparator(';')
                                                .AddStringSplitOptions(StringSplitOptions.RemoveEmptyEntries));
            }
        }

        [Test]
        public void 系统方式转型性能4()
        {
            var s = Guid.NewGuid().ToString();
            for (var i = 0; i < 100000; i++)
            {
                var g = Guid.Parse(s);
            }
        }

        [Test]
        public void Convert4方式转型性能4()
        {
            var s = Guid.NewGuid().ToString();
            for (var i = 0; i < 100000; i++)
            {
                var g = s.To<Guid>();
            }
        }

        [Test]
        public void 系统方式转型性能5()
        {
            var s = "2018-02-23 14:02:10";
            for (var i = 0; i < 100000; i++)
            {
                var d = DateTime.Parse(s);
            }
        }

        [Test]
        public void Convert4方式转型性能5()
        {
            var s = "2018-02-23 14:02:10";
            for (var i = 0; i < 100000; i++)
            {
                var d = s.To<DateTime>();
            }
        }
    }
}
