using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCore.HypermediaLinks.Tests.Integration
{
    [ApiController]
    [Route("api/fake")]
    public class FakeController : ControllerBase
    {
        [Route("helloworld")]
        public async Task<ActionResult> Get(string name, int age)
        {
            return Ok($"Hello world {name} {age}");
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
        public ActionResult GetCollectionModel(string name, int id)
        {
            var result = new FakeArrayModel()
            {
                Name = name,
                Id = id,
                FakeModelsArrays = new FakeModel[] { new FakeModel() { Name = "test1", Id = 1 }, new FakeModel() { Name = "test2", Id = 2 } },
                FakeModelList = new List<FakeModel>() { new FakeModel() { Name = "test3", Id = 3 }, new FakeModel() { Name = "test4", Id = 4 } },
                FakeModels = new List<FakeModel>() { new FakeModel() { Name = "test5", Id = 5 }, new FakeModel() { Name = "test6", Id = 6 } }

            };

            return Ok(result);
        }


    }
}
