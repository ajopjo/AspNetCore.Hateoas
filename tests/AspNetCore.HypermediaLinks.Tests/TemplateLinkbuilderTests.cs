using AspNetCore.HypermediaLinks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AspNetCore.HypermediaLinks.Tests
{
    public class TemplateLinkbuilderTests
    {
        [Fact]
        public void ModelLinkTest()
        {
            var model = new MoqTestModel() { Id = 1 };
            model.AddHyperMediaSupportLinks(new HypermediaBuilder());
            Assert.Equal(1, model.Links.Count());
            Assert.Equal(model.Links.FirstOrDefault().Rel, "self");
            Assert.Equal(model.Links.FirstOrDefault().Href, new Uri("https://templatetest.com/moq/1/items"));
        }

        [Fact]
        public void ModelArrayLinkTest()
        {
            var model = new MoqArrayTestModel()
            {
                TestModels = new MoqTestModel[] {
                    new MoqTestModel(){ Id=1},
                    new MoqTestModel(){Id=2 }
                }
            };
            model.AddHyperMediaSupportLinks(new HypermediaBuilder());
            Assert.Equal(1, model.Links.Count());
            Assert.Equal(model.Links.FirstOrDefault().Rel, "self");
            Assert.Equal(model.Links.FirstOrDefault().Href, new Uri("https://templatetest.com/moqarray"));
            int count = 0;
            foreach (var item in model.TestModels)
            {
                ++count;
                Assert.Equal(1, item.Links.Count());
                Assert.Equal(item.Links.FirstOrDefault().Rel, "self");
                Assert.Equal(item.Links.FirstOrDefault().Href, new Uri($"https://templatetest.com/moq/{count}/items"));
            }
        }

        [Fact]
        public void ModelGenericsLinkTest()
        {
            var model = new MoqGenericListTestModel()
            {
                TestModels = new MoqTestModel[] {
                    new MoqTestModel(){ Id=1},
                    new MoqTestModel(){Id=2 }
                },
                TestModelList = new List<MoqTestModel>()
                {
                    new MoqTestModel(){ Id=1},
                    new MoqTestModel(){Id=2 }
                }
            };
            model.AddHyperMediaSupportLinks(new HypermediaBuilder());
            Assert.Equal(1, model.Links.Count());
            Assert.Equal(model.Links.FirstOrDefault().Rel, "self");
            Assert.Equal(model.Links.FirstOrDefault().Href, new Uri("https://templatetest.com/genericlist"));
            int count = 0;
            foreach (var item in model.TestModels)
            {
                ++count;
                Assert.Equal(1, item.Links.Count());
                Assert.Equal(item.Links.FirstOrDefault().Rel, "self");
                Assert.Equal(item.Links.FirstOrDefault().Href, new Uri($"https://templatetest.com/moq/{count}/items"));
            }
            count = 0;
            foreach (var item in model.TestModelList)
            {
                ++count;
                Assert.Equal(1, item.Links.Count());
                Assert.Equal(item.Links.FirstOrDefault().Rel, "self");
                Assert.Equal(item.Links.FirstOrDefault().Href, new Uri($"https://templatetest.com/moq/{count}/items"));
            }
        }
    }

    class MoqTestModel : HyperMediaSupportModel
    {
        public int Id { get; set; }
        public override void AddHypermediaLinks(HypermediaBuilder builder)
        {
            Add(builder.Fromtemplate("/moq/{id}/items", new { id = Id }, new Uri("https://templatetest.com")).Build().AddSelfRel());
        }
    }

    class MoqArrayTestModel : HyperMediaSupportModel
    {
        public MoqTestModel[] TestModels { get; set; }
        public override void AddHypermediaLinks(HypermediaBuilder builder)
        {
            Add(builder.Fromtemplate(template: "/moqarray", uri: new Uri("https://templatetest.com")).Build().AddSelfRel());
        }
    }

    class MoqGenericListTestModel : HyperMediaSupportModel
    {
        public IEnumerable<MoqTestModel> TestModels { get; set; }
        public IEnumerable<MoqTestModel> TestModelList { get; set; }

        public override void AddHypermediaLinks(HypermediaBuilder builder)
        {
            Add(builder.Fromtemplate(template: "/genericlist", uri: new Uri("https://templatetest.com")).Build().AddSelfRel());
        }
    }
}
