using AspNetCore.HypermediaLinks;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace AspNetCore.HypermediaLinks.Tests
{
    public class ExtensionsTest
    {
        [Fact]
        public void IsgenerictypeSupportLinkTest()
        {
            var model = new TestClass();
            var props = model.GetType().GetProperties(BindingFlags.DeclaredOnly |
                                    BindingFlags.Public |
                                    BindingFlags.Instance);

            foreach (var prop in props)
            {
                Assert.Equal(true, prop.IsLinkSupportGenericType());
            }
        }

        [Fact]
        public void IsArraytypeSupportLinkTest()
        {
            var model = new ArrayTest();
            var props = model.GetType().GetProperties(BindingFlags.DeclaredOnly |
                                    BindingFlags.Public |
                                    BindingFlags.Instance);

            foreach (var prop in props)
            {
                Assert.Equal(true, prop.IsLinkSupportArray());
            }
        }

        [Fact]
        public void IsModelSupportLinkTest()
        {
            var model = new ModelTest();

            var props = model.GetType().GetProperties(BindingFlags.DeclaredOnly |
                                    BindingFlags.Public |
                                    BindingFlags.Instance);

            foreach (var prop in props)
            {
                Assert.Equal(true, prop.IsLinkSupportModel());
            }

        }

    }

    class TestClass : HyperMediaSupportModel
    {

        public IEnumerable<TestClass> Tests { get; set; }
        public IList<TestClass> TestLists { get; set; }
        public override void AddHypermediaLinks(HypermediaBuilder builder)
        {
            throw new NotImplementedException();
        }
    }

    class ArrayTest
    {
        public TestClass[] Tests { get; set; }
    }

    class ModelTest
    {
        public TestClass TestModels { get; set; }
    }
}
