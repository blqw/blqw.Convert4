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
    public class BugTests
    {
        [Test]
        public void NullToObject()
        {
            object a = null;
            var b = a.To<object>();
            Assert.AreEqual(null, b);
        }
    }
}
