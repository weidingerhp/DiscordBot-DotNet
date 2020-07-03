using Newtonsoft.Json;

namespace DiscordBot.Domain.Database.Service
{
    public abstract class DatabaseObject
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("partkey")]
        public string PartKey { get; set; }
        
    }
}