using System.Threading.Tasks;
using DiscordBot.Domain.Database.Config;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace DiscordBot.Domain.Database.Service.Impl
{
    public class DatabaseService : IDatabaseService
    {
        private DBConfiguration _configuration;
        private CosmosClient _client;

        public DatabaseService(IOptionsMonitor<DBConfiguration> configuration)
        {
            _configuration = configuration.CurrentValue;
            configuration.OnChange((newConfig) =>
            {
                _configuration = newConfig;
                // if config changes - invalidate the client so a new one
                // has to be built
                _client = null;
            });
        }

        public CosmosClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new CosmosClient(_configuration.DbEndpoint,
                        _configuration.DbKey);
                }

                return _client;
            }
        }

        public Microsoft.Azure.Cosmos.Database GetObjectDatabase()
        {
            var client = Client;
            return client.GetDatabase(_configuration.ObjectStoreDb);
        }

        public async Task<Container> GetObjectContainerAsync<T>() where T : DatabaseObject
        {
            return await GetObjectDatabase().GetObjectContainer<T>();
        }
    }
}