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

        class User
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public DateTime Birthday { get; set; }
            public bool Sex { get; set; }
        }
        class MyClass
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

        [Test]
        public void 一般转换测试()
        {
            Assert.AreEqual(1, "1".To<int>());
            Assert.AreEqual(null, "".To<int?>());

            var my = "{\"ID\":1,\"Name\":\"blqw\"}".To<MyClass>();
            Assert.IsNotNull(my);
            Assert.AreEqual(1, my.ID);
            Assert.AreEqual("blqw", my.Name);

            var arr = "[1,2,3,4,5,6]".To<int[]>();
            Assert.IsNotNull(arr);
            Assert.AreEqual(6, arr.Length);
            Assert.AreEqual(1, arr[0]);
            Assert.AreEqual(2, arr[1]);
            Assert.AreEqual(3, arr[2]);
            Assert.AreEqual(4, arr[3]);
            Assert.AreEqual(5, arr[4]);
            Assert.AreEqual(6, arr[5]);

            var user = new User()
            {
                ID = 1,
                Name = "blqw",
                Birthday = DateTime.Now,
                Sex = true
            };
            var my2 = user.To<MyClass>();
            Assert.IsNotNull(my2);
            Assert.AreEqual(1, my2.ID);
            Assert.AreEqual("blqw", my2.Name);
        }

        [Test]
        public void 泛型转换测试()
        {
            var list = "1,2,3,4".To<List<int>>();
            Assert.AreEqual(list?.Count, 4);
            Assert.AreEqual(list[0], 1);
            Assert.AreEqual(list[1], 2);
            Assert.AreEqual(list[2], 3);
            Assert.AreEqual(list[3], 4);
        }

    }
}
