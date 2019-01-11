using AspNetCore.HypermediaLinks;
using AspNetCore.HypermediaLinks.Tests.Integration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Collections;

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
        [Fact]
        public void ControllerMetaDataTest()
        {
            var res = GetMetaDataForAction<FakeController>(c => nameof(c.Get));
            Assert.Equal("Fake", res.Item1);
            Assert.Equal("Get", res.Item2);
        }


        [Fact]
        public void ControllerActionMetaDataTestWithParams()
        {
            var res = GetMetaDataForActionwithParams<FakeController>(c => c.Get("test", 10));
            Assert.Equal("Get", res.Item1);
            var routeVals = res.Item2;
            Assert.Equal(2, res.Item2.Count());
        }

        [Theory]
        [ClassData(typeof(FakeRequestTestData))]
        public void ControllerTestWithComplexParams(FakeRequest req)
        {
            var result = GetMetaDataForActionwithParams<FakeController>(c => c.GetSimpleModel(req));
            Assert.Equal("GetSimpleModel", result.Item1);
            var routeVals = result.Item2;
            Assert.Equal(1, result.Item2.Count());
            Assert.Equal(req, result.Item2.FirstOrDefault().Value);
        }

        private Tuple<string, string> GetMetaDataForAction<T>(Expression<Func<T, string>> action) where T : ControllerBase
        {
            var controllerName = typeof(T).GetControllerName<T>();
            if (!(action.Body is ConstantExpression constExp))
                throw new ArgumentException("Action name must be a constant expression");

            return new Tuple<string, string>(controllerName, constExp.Value.ToString());
        }

        private Tuple<string, Dictionary<string, object>> GetMetaDataForActionwithParams<T>(Expression<Func<T, object>> action) where T : ControllerBase
        {
            var routeValues = action.GetRouteValues();
            return new Tuple<string, Dictionary<string, object>>(action.GetActionName(), routeValues);
        }

    }

    public class FakeRequestTestData : TheoryData<FakeRequest>
    {
        public FakeRequestTestData()
        {
            Add(new FakeRequest() { Id = 1, Name = "test1" });
            Add(new FakeRequest() { Id = 2, Name = "test2" });
        }
    }

    class TestClass : HyperMediaSupportModel
    {

        public IEnumerable<TestClass> Tests { get; set; }
        public IList<TestClass> TestLists { get; set; }
        public List<TestClass> TestClassLists { get; set; }
        public ICollection<TestClass> TestClassCollection { get; set; }
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
