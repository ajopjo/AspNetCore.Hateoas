using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.HypermediaLinks.Tests.Integration
{
    [ApiController]
    [Route("api/fake")]
    public class FakeController : ControllerBase
    {
        [Route("helloworld")]
        public async Task<ActionResult> Get(string name)
        {
            return Ok($"Hello world {name}");
        }

        public ActionResult Get(string name, int age)
        {
            return Ok($"Hello world {name} and your age is {age}");
        }

        [Route("fakeModel")]
        public ActionResult Get([FromQuery] FakeRequest req)
        {
            return Ok(new FakeModel()
            {
                Id = req.Id,
                Name = req.Name
            });
        }
    }
}
