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
using System.Collections.Specialized;
using System.Data;


namespace NUnit.Tests1
{
    [TestFixture]
    class _20180614
    {
        [Test]
        public void 动态类型转换bug()
        {
            var a1 = Guid.NewGuid().ToString();
            var b = a1.ToDynamic();
            var a2 = Convert4.To<string>(b, null);
            Assert.AreEqual(a1, a2);


            var c1 = "1";
            var d = c1.ToDynamic();
            var c2 = Convert4.To<string>(d, null);
            Assert.AreEqual(c1, c2);
            var c3 = Convert4.To<int>(d, 0);
            Assert.AreEqual(int.Parse(c1), c3);
        }
    }
}
