using AdoNetStorageDemo.IActors;
using AdoNetStorageDemo.IActors.State;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Providers;
using System.Threading.Tasks;

namespace AdoNetStorageDemo.Actors
{


    [StorageProvider(ProviderName = "OrleansStorage")]
    public class CustomerGrain : Grain<CustomerState>, ICustomerGrain
    {
        private readonly ILogger logger;

        public CustomerGrain(ILogger<CustomerGrain> logger)
        {
            this.logger = logger;
        }

        public override Task OnActivateAsync()
        {
            State.Id = this.GetPrimaryKey(); ;
            return base.OnActivateAsync();
        }


        public Task<CustomerState> GetState()
        {
            return Task.FromResult(State);
        }

        public async Task SetState(CustomerState state)
        {
            State = state;
            State.Id = this.GetPrimaryKey(); ;
            await WriteStateAsync();

            var key = this.GetPrimaryKey();
            logger.LogInformation($"[Customer-{key}] Set name to:{State.Name}");
            return;
        }
    }
}
