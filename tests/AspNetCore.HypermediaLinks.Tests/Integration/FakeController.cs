using Microsoft.AspNetCore.Mvc;
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

        [Route("fakeModel")]
        public ActionResult GetSimpleModel([FromQuery] FakeRequest req)
        {
            return Ok(new FakeModel()
            {
                Id = req.Id,
                Name = req.Name
            });
        }

        [Route("fakeArrayModel")]
        public ActionResult Get(string name, int id)
        {
            var result = new FakeArrayModel()
            {
                Name = name,
                Id = id,
                FakeModels = new FakeModel[] { new FakeModel() { Name = "test1", Id = 1 }, new FakeModel() { Name = "test2", Id = 2 } }

            };

            return Ok(result);
        }


    }
}
