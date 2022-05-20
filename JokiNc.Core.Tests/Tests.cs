using System;
using System.Linq;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace JokiNc.Core.Tests
{
    [TestFixture]
    public class Tests
    {
        public static LineElement[] Elements = LineElement.FromStringArray(new[]
        {
            "TRANS", "G0", "X1", "Y2", "Z13", "G54"
        });

        [Test]
        public void TestFindMany()
        {
            var value = Elements[0].FindMany(10);
            var arr = value.ToArray();
            Assert.IsTrue(arr.Length == 5 && arr.All(x => x != null));
        }

        [Test]
        public void TestFindByName()
        {
            var value = Elements[0].FindByName("Z");
            Assert.NotNull(value);
        }
        
        [Test]
        public void TestCreateLine()
        {
            Line line = new Line("G0 X1 Y3.2   Z-7.3");
            Assert.IsTrue(line != null && line.Elements.Count == 4);
        }

        [Test]
        public void TestCreateLine2()
        {
            var line = LineElement.FromString("G0 X1 Y3.2   Z-7.3");
            Assert.IsTrue(line != null && line.Length == 4);
        }

        [Test]
        public void TestMergeModular()
        {
            var line = LineElement.FromStringArray(new string[]
            {
                 "X11.3"
            });
            var modal = line.FirstOrDefault().MergeIntoFunction();
            Assert.IsTrue(modal != null && modal.Count() == 2);
        }
        
        [Test]
        public void TestMergeToString()
        {
            var line = LineElement.FromStringArray(new string[]
            {
                "X11.3", "Z19.2122", "Y-9.326"
            });
            var modal = line.FirstOrDefault().MergeIntoFunction();
            var str = modal.MergeIntoString();
            Assert.NotNull(str);
        } 
        
        [Test]
        public void TestIsPreserved()
        {
            var line = LineElement.FromStringArray(new string[]
            {
                "X11.3", "Z19.2122", "Y-9.326"
            });
            var modal = line.FirstOrDefault().MergeIntoFunction();
            var first = modal.First();
            Assert.IsTrue(first.Context != null && first.Context.Elements.Count == 4);
        } 
        
        [Test]
        public void TestFindAll()
        {
            var first = Elements[0];
            var found = first.FindAll().ToArray();
            Assert.IsTrue(found != null && found.Length == 6);
        }
    }
}