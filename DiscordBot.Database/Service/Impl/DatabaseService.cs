using System.Threading.Tasks;
using DiscordBot.Domain.Database.Config;
using DiscordBot.Domain.Database.Service.Helpers;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace DiscordBot.Domain.Database.Service.Impl
{
    public class DatabaseService : IDatabaseService
    {
        private IOptionsMonitor<DBConfiguration> _configuration;
        private CosmosClient _client;

        public DatabaseService(IOptionsMonitor<DBConfiguration> configuration)
        {
            _configuration = configuration;
        }

        public CosmosClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new CosmosClient(_configuration.CurrentValue.DbEndpoint,
                        _configuration.CurrentValue.DbKey);
                }

                return _client;
            }
        }

        public Microsoft.Azure.Cosmos.Database GetObjectDatabase()
        {
            var client = Client;
            return client.GetDatabase(_configuration.CurrentValue.ObjectStoreDb);
        }

        public async Task<Container> GetObjectContainerAsync<T>() where T : DatabaseObject
        {
            return await GetObjectDatabase().GetContainer<T>();
        }
    }
}