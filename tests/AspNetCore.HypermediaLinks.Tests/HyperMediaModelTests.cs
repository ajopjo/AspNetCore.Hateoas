using AspNetCore.HypermediaLinks;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AspNetCore.Hateoas.Tests
{
    public class HyperMediaModelTests
    {
        [Fact]
        public void AddDuplicateKeyTest()
        {

            var builder = new HypermediaBuilder();
            var model = new DuplicateKeyModel();
            Action act = () => model.AddHypermediaLinks(builder);
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void AddModelLinkTest()
        {
            var builder = new HypermediaBuilder();
            var model = new LinkTestModel()
            {
                MoqModel = new MoqHyperlinkModel()
            };
            model.AddHyperMediaSupportLinks(builder);
            Assert.Equal(model.Links.Count(), 1);

            Assert.Equal(model.MoqModel.Links.Count(), 1);
        }

        [Fact]
        public void AddArrayLinkTest()
        {
            var model = new LinkTestArrayModel()
            {
                LinkModels = new LinkTestModel[] { new LinkTestModel(), new LinkTestModel() }
            };
            model.AddHyperMediaSupportLinks(new HypermediaBuilder());
            Assert.Equal(model.Links.Count(), 1);
            foreach (var linkModel in model.LinkModels)
            {
                Assert.Equal(linkModel.Links.Count(), 1);
            }
        }

        [Fact]
        public void AddGenericModelLinkTest()
        {

            var testModels = new List<LinkTestModel>
            {
                new LinkTestModel()
                {
                    MoqModel = new MoqHyperlinkModel()
                },
                new LinkTestModel()
                {
                    MoqModel = new MoqHyperlinkModel()
                }
            };
            var model = new GenericsLinkTestModel()
            {
                MoqLinkModels = new MoqHyperlinkModel[] {
                    new MoqHyperlinkModel(), new MoqHyperlinkModel()
                },
                LinkTestModels = testModels
            };
            model.AddHyperMediaSupportLinks(new HypermediaBuilder());

            Assert.Equal(model.Links.Count(), 1);

            foreach (var moqModel in model.MoqLinkModels)
            {
                Assert.Equal(moqModel.Links.Count(), 1);
            }

            foreach (var testModel in model.LinkTestModels)
            {
                Assert.Equal(testModel.Links.Count(), 1);

                Assert.Equal(testModel.MoqModel.Links.Count(), 1);
            }
        }

    }
    class DuplicateKeyModel : HyperMediaSupportModel
    {
        public override void AddHypermediaLinks(HypermediaBuilder builder)
        {
            Add(new Link() { Rel = "self", Href = new Uri("https://test.com") });
            Add(new Link() { Rel = "self", Href = new Uri("https://test.com") });
        }
    }

    class MoqHyperlinkModel : HyperMediaSupportModel
    {
        public override void AddHypermediaLinks(HypermediaBuilder builder)
        {
            Add(new Link() { Rel = "self", Href = new Uri("https://moqmodel.test.com") });
        }
    }
    class LinkTestModel : HyperMediaSupportModel
    {
        public MoqHyperlinkModel MoqModel { get; set; }
        public override void AddHypermediaLinks(HypermediaBuilder builder)
        {
            Add(new Link() { Rel = "self", Href = new Uri("https://testmodel.test.com") });
        }
    }

    class LinkTestArrayModel : HyperMediaSupportModel
    {

        public LinkTestModel[] LinkModels { get; set; }

        public override void AddHypermediaLinks(HypermediaBuilder builder)
        {
            Add(new Link() { Rel = "self", Href = new Uri("https://testarraymodel.test.com") });
        }
    }

    class GenericsLinkTestModel : HyperMediaSupportModel
    {
        public IEnumerable<MoqHyperlinkModel> MoqLinkModels { get; set; }
        public IList<LinkTestModel> LinkTestModels { get; set; }
        public override void AddHypermediaLinks(HypermediaBuilder builder)
        {
            Add(new Link() { Rel = "self", Href = new Uri("https://testgenericlinkmodel.test.com") });
        }
    }
}
