using System;
using System.Collections.Generic;

namespace AspNetCore.HypermediaLinks.Tests.Integration
{
    public class FakeModel : HyperMediaSupportModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public IEnumerable<FakeAddressModel> Addresses { get; set; }

        public override void AddHypermediaLinks(HypermediaBuilder builder)
        {
            Add(builder.FromController<FakeController>(c => nameof(c.Get), new { Id = Guid.NewGuid(), Name = "Ajo" }).Build().AddSelfRel());
        }
    }

    public class FakeAddressModel
    {
        public Guid Id { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
    }

    public class FakeRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
