using AdoNetStorageDemo.IActors.State;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AdoNetStorageDemo.Web.Test
{
    public class CustomersControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;

        public CustomersControllerTest(WebApplicationFactory<Startup> factory)
        {
            client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllTest()
        {
            // Arrange
            try
            {
                var response = await client.GetAsync("/api/Customers");

                response.EnsureSuccessStatusCode(); // Status Code 200-299
                Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

                var content = await response.Content.ReadAsStringAsync();
                var customers = JsonConvert.DeserializeObject<IEnumerable<CustomerState>>(content);
            }
            catch (Exception ex)
            {
                throw ex;
            };

        }

        [Fact]
        public async Task GetOneTest()
        {
            // Arrange
            try
            {
                var response = await client.GetAsync("/api/Customers");
                var content = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<IEnumerable<CustomerState>>(content).FirstOrDefault();

                response = await client.GetAsync($"/api/Customers/{customer.Id}");
                content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CustomerState>(content);

                Assert.Equal(customer.Name, result.Name);

            }
            catch (Exception ex)
            {
                throw ex;
            };

        }

        [Fact]
        public async Task CreateCustomerTest()
        {
            // Arrange
            try
            {
                var response = await client.PostAsJsonAsync<CustomerState>("/api/Customers/CreateCustomer", new CustomerState { Name = $"New-{DateTime.Now.ToShortTimeString()}" });

                response.EnsureSuccessStatusCode(); // Status Code 200-299
            }
            catch (Exception ex)
            {
                throw ex;
            };

        }
    }
}