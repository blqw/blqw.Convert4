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

        [Test]
        public void 接口转换测试()
        {
            var list1 = "1,2,3,4".To<IList<int>>();
            Assert.AreEqual(list1?.Count, 4);
            Assert.AreEqual(list1[0], 1);
            Assert.AreEqual(list1[1], 2);
            Assert.AreEqual(list1[2], 3);
            Assert.AreEqual(list1[3], 4);

            var list3 = "1,2,3,4".To<IEnumerable>().Cast<object>().ToList();
            Assert.AreEqual(list3?.Count, 4);
            Assert.AreEqual(list3[0], "1");
            Assert.AreEqual(list3[1], "2");
            Assert.AreEqual(list3[2], "3");
            Assert.AreEqual(list3[3], "4");



            var list2 = "1,2,3,4".To<IList>();
            Assert.AreEqual(list2?.Count, 4);
            Assert.AreEqual(list2[0], "1");
            Assert.AreEqual(list2[1], "2");
            Assert.AreEqual(list2[2], "3");
            Assert.AreEqual(list2[3], "4");
        }

        [Test]
        public void 自定义转换参数()
        {
            var list = "1;2;3;;4".To<List<int>>(new ConvertSettings().AddStringSeparator(";"));
            Assert.AreEqual(list?.Count, 5);
            Assert.AreEqual(list[0], 1);
            Assert.AreEqual(list[1], 2);
            Assert.AreEqual(list[2], 3);
            Assert.AreEqual(list[3], 0);
            Assert.AreEqual(list[4], 4);
            list = "1;2;3;;4".To<List<int>>(new ConvertSettings()
                                                .AddStringSeparator(";")
                                                .AddStringSplitOptions(StringSplitOptions.RemoveEmptyEntries));
            Assert.AreEqual(list?.Count, 4);
            Assert.AreEqual(list[0], 1);
            Assert.AreEqual(list[1], 2);
            Assert.AreEqual(list[2], 3);
            Assert.AreEqual(list[3], 4);
        }

        [Test]
        public void 自定义格式化参数()
        {
            var format = "yyyyMMddHHmmssffffff";
            var enUS = CultureInfo.CreateSpecificCulture("en-US");
            var urPK = CultureInfo.CreateSpecificCulture("ur-PK");
            var time = DateTime.Now;

            var formatResult = time.ToString(format);
            var enUSResult = time.ToString(enUS);
            var urPKResult = time.ToString(urPK);

            var formatTest = time.To<string>(new ConvertSettings().AddFormat<DateTime>(format));
            var enUSTest = time.To<string>(new ConvertSettings().AddService(enUS));
            var urPKTest = time.To<string>(new ConvertSettings().AddForType<DateTime>(urPK));
            var urPKTest2 = time.To<string>(new ConvertSettings().AddForType<int>(urPK));

            Assert.AreEqual(formatTest, formatResult);
            Assert.AreEqual(enUSTest, enUSResult);
            Assert.AreEqual(urPKTest, urPKResult);
            Assert.AreNotEqual(urPKTest2, urPKResult);
        }


        [Test]
        public void 测试友好类名()
        {
            Assert.AreEqual(typeof(int).To<string>(), "int");
            Assert.AreEqual(typeof(char?).To<string>(), "char?");
            Assert.AreEqual(typeof(List<ICollection>).To<string>(), "List<ICollection>");
            Assert.AreEqual(typeof((int, long)).To<string>(), "(int, long)");
            Assert.AreEqual(typeof(List<>).To<string>(), "List<>");
            Assert.AreEqual(typeof(void).To<string>(), "void");
            Assert.AreEqual(typeof(Dictionary<List<long?>, Tuple<string, DateTime, Guid, (TimeSpan, int)>>).To<string>(),
                "Dictionary<List<long?>, Tuple<string, DateTime, Guid, (TimeSpan, int)>>");
            Assert.AreEqual(typeof(int*).To<string>(), "int*");
            Assert.AreEqual(typeof(int).MakeByRefType().To<string>(), "int&");
            Assert.AreEqual(typeof(int[]).To<string>(), "int[]");
        }



    }
}
