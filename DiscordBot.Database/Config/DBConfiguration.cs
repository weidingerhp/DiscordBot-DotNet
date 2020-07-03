using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Domain.Database.Config {
	public class DBConfiguration {
		[JsonProperty("db_endpoint")]
		public string DbEndpoint { get; set; }

		[JsonProperty("db_key")]
		public string DbKey { get; set; }
		
		[JsonProperty("db_objectstorename")]
		public string ObjectStoreDb { get; set; }
	}
}
