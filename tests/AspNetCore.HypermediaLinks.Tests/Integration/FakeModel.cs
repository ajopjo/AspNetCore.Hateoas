using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.HypermediaLinks.Tests.Integration
{
    public class FakeModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public IEnumerable<FakeAddressModel> Addresses { get; set; }
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
