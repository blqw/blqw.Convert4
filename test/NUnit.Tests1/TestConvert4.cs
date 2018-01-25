using blqw;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit.Tests1
{
    [TestFixture]
    public class TestConvert4
    {
        [Test]
        public void To1()
        {
            var i = "1".To(0);
            Assert.AreEqual(i, 1);
        }

        [Test]
        public void To2()
        {
            var i = "1".To<long>();
            Assert.AreEqual(i, 1);
        }

    }
}
