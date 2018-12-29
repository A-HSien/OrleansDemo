using AdoNetStorageDemo.IActors;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdoNetStorageDemo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IClusterClient client;

        public ValuesController(IClusterClient client)
        {
            this.client = client;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var id = Guid.Parse("c394edd6-9c8b-41df-b97d-09eb48f1de63");
            var customer = client.GetGrain<ICustomer>(id);
            var name = $"Customer-{id}";
            var newName = await customer.GetName();
            var result1 = (newName == name);


            var allCustomer = client.GetGrain<ICustomers>("AllCustomer");
            await allCustomer.AddCustomer(customer);
            var all = await allCustomer.GetAllCustomer();
            var result2 = all.Any(e => e.GetName().Result == name);

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
