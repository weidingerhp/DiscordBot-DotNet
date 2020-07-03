using System;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DiscordBot.Domain.Database.Service.Helpers;
using Microsoft.Azure.Cosmos;

namespace DiscordBot.Domain.Database.Service
{
    public static class ObjectStoreExtensions
    {

        public async static Task<Container> GetContainer<T>(this Microsoft.Azure.Cosmos.Database database) where T : DatabaseObject
        {
            var props = SimpleObjectStorageHandler.GetProperties<T>();
            string partitionKey = $"/{props.PartitionKey?.Name ?? "partkey"}";
            return await database.CreateContainerIfNotExistsAsync(props.ContainerName, partitionKey);
        }
        
        public async static Task<ItemResponse<T>> StoreObjectAsync<T>(this Container container, T obj) where T : DatabaseObject
        {
            var props = SimpleObjectStorageHandler.GetProperties<T>();

            CalculateStorageKeys(props, obj);

            return await container.CreateItemAsync(obj);
        }

        public async static Task<ItemResponse<T>> UpSertObject<T>(this Container container, T obj) where T : DatabaseObject
        {
            var props = SimpleObjectStorageHandler.GetProperties<T>();

            CalculateStorageKeys(props, obj);
            PartitionKey partKey = CalculatePartitionKey(props, obj);

            return await container.UpsertItemAsync(obj, partKey);
        }

        public async static Task<ItemResponse<T>> DeleteObject<T>(this Container container, T obj)
            where T : DatabaseObject
        {
            var props = SimpleObjectStorageHandler.GetProperties<T>();
            CalculateStorageKeys(props, obj);
            PartitionKey partKey = CalculatePartitionKey(props, obj);
            return await container.DeleteItemAsync<T>(obj.Id, partKey);
        }

        private static PartitionKey CalculatePartitionKey<T>(ObjectStoreProperties props, T databaseObject) where T : DatabaseObject
        {
            if (props.PartitionKey == null)
            {
                return new PartitionKey(databaseObject.PartKey);
            }

            object partKeyValue = props.PartitionKey.GetMethod.Invoke(databaseObject, null);
            if (props.PartitionKey.PropertyType == typeof(string))
                return new PartitionKey((string) partKeyValue);
            if (props.PartitionKey.PropertyType == typeof(bool))
                return new PartitionKey((bool) partKeyValue);
            if (props.PartitionKey.PropertyType == typeof(double))
                return new PartitionKey((double) partKeyValue);

            return new PartitionKey(partKeyValue?.ToString());
        }

        private static void CalculateStorageKeys<T>(ObjectStoreProperties props, T storeObject) where T : DatabaseObject
        {
            if (storeObject.Id != null) return; // id is already set - nothing to do
            
            byte[] primaryKeyHash = null;
            if (props.PrimaryKeys.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var key in props.PrimaryKeys)
                {
                    builder.Append(key.GetMethod.Invoke(storeObject, null)?.ToString() ?? "null");
                }

                primaryKeyHash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(builder.ToString()));
            }
            else
            {
                throw new InvalidOperationException("Cannot Store objects without at least one primary Key");
            }

            if (props.PartitionKey != null)
            {
                storeObject.Id = Convert.ToBase64String(primaryKeyHash);
            }
            else
            {
                storeObject.PartKey = Convert.ToBase64String(primaryKeyHash, 0, 5);
                storeObject.Id = Convert.ToBase64String(primaryKeyHash, 5, primaryKeyHash.Length - 5);
            }
        }
    }
}