using System;
using System.Collections.Generic;

namespace DiscordBot.Domain.Database.Service.Helpers
{
    public static class SimpleObjectStorageHandler
    {
        private static readonly Dictionary<Type, ObjectStoreProperties> _knownTypes = new Dictionary<Type, ObjectStoreProperties>();
        
        public static ObjectStoreProperties GetProperties<T>() where T : class
        {
            if (!_knownTypes.ContainsKey(typeof(T)))
            {
                _knownTypes[typeof(T)] = ObjectStoreProperties.Create(typeof(T));
            }

            return _knownTypes[typeof(T)];
        }

    }
}