using System;
using System.Collections.Generic;

namespace AspNetCore.HypermediaLinks.Tests.Integration
{
    public class FakeModel : HyperMediaSupportModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public override void AddHypermediaLinks(HypermediaBuilder builder)
        {
            Add(builder.FromController<FakeController>(c => nameof(c.GetSimpleModel), new { id = Id, name = Name }).Build().AddSelfRel());
        }
    }

    public class FakeRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class FakeArrayModel : HyperMediaSupportModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FakeModel[] FakeModelsArrays { get; set; }
        public IEnumerable<FakeModel> FakeModels { get; set; }
        public List<FakeModel> FakeModelList { get; set; }

        public override void AddHypermediaLinks(HypermediaBuilder builder)
        {
            Add(builder.FromController<FakeController>(c => nameof(c.GetCollectionModel), values: new { id = Id, name = Name }).Build().AddSelfRel());
        }
    }


}
