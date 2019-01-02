using AdoNetStorageDemo.IActors;
using AdoNetStorageDemo.IActors.State;
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
    public class CustomersController : ControllerBase
    {
        private readonly IClusterClient client;

        public CustomersController(IClusterClient client)
        {
            this.client = client;
        }


        [HttpGet]
        public async Task<IEnumerable<CustomerState>> GetAll()
        {
            var customersGrain = client.GetGrain<ICustomersGrain>("AllCustomer");
            var allGrains = await customersGrain.GetAllCustomers();
            var allStates = allGrains.Select(g =>
           {
               var state = g.GetState().Result;
               return state;
           }).ToList();

            return allStates;
        }


        [HttpGet("{id}")]
        public async Task<CustomerState> Get(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("id can't be null or empty!");

            var customer = client.GetGrain<ICustomerGrain>(id);
            return await customer.GetState();
        }


        [HttpPost("CreateCustomer")]
        public async Task CreateCustomer(CustomerState state)
        {
            var customer = client.GetGrain<ICustomerGrain>(Guid.NewGuid());
            await customer.SetState(state);

            var allCustomersGrain = client.GetGrain<ICustomersGrain>("AllCustomer");
            await allCustomersGrain.AddCustomer(customer);
        }
    }
}
