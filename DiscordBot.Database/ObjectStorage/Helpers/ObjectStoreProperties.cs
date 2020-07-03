using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DiscordBot.Domain.Database.ObjectStorage.Attributes;

namespace DiscordBot.Domain.Database.Service.Helpers
{
    public class ObjectStoreProperties
    {
        public List<PropertyInfo> PrimaryKeys { get; private set; }
        public PropertyInfo PartitionKey { get; private set; }

        public string ContainerName { get; private set; }

        internal static ObjectStoreProperties Create(Type t)
        {
            var primaryKeys = new List<PropertyInfo>();
            PropertyInfo partitionKey = null;

            string containerName =
                t.GetCustomAttributes(
                        typeof(ContainerNameAttribute), true)
                    .Cast<ContainerNameAttribute>()
                    .Select(x => x.Name).FirstOrDefault() ?? t.Name;


            Console.Out.WriteLine($"Container : {containerName}");

            foreach (var m in t.GetProperties())
            {
                if (!m.CanRead) continue;

                foreach (Attribute a in m.GetCustomAttributes())
                {
                    switch (a)
                    {
                        case PrimaryKeyAttribute _:
                            primaryKeys.Add(m);
                            break;
                        case PartitionKeyAttribute _ when partitionKey != null:
                            throw new InvalidOperationException("Only one PartitionKey Attribute is allowed!");
                        case PartitionKeyAttribute _:
                            partitionKey = m;
                            break;
                    }
                }
            }

            if (primaryKeys == null)
            {
                throw new InvalidOperationException("Object must at least have on Primary Key Property");
            }

            return new ObjectStoreProperties
            {
                PartitionKey = partitionKey,
                PrimaryKeys = primaryKeys,
                ContainerName = containerName
            };
        }
    }
}