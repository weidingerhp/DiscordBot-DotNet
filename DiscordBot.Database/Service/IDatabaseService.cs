using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace DiscordBot.Domain.Database.Service
{
    public interface IDatabaseService
    {
        /// <summary>
        /// Get a simple, connected Cosmos-Client.
        /// </summary>
        public CosmosClient Client { get; }

        /// <summary>
        /// Get the Database for All Object Types
        /// </summary>
        /// <returns></returns>
        public Microsoft.Azure.Cosmos.Database GetObjectDatabase();

        /// <summary>
        ///  Prepares a Container for object Storage of the given Type
        /// </summary>
        /// <typeparam name="T">The Type you want to store</typeparam>
        /// <returns></returns>
        Task<Container> GetObjectContainerAsync<T>() where T : DatabaseObject;
    }
}