using blqw.Kanai;
using NUnit.Framework;

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
