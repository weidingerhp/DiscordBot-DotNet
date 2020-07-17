using DiscordBot.Domain.Database.ObjectStorage.Attributes;
using DiscordBot.Domain.Database.Service;

namespace DiscordBot.Database.Test.ObjectStorage {
    public class TestClasses {
        [ContainerName(Name = "TestContainer")]
        public class TestDBObject1 : DatabaseObject {
            [PrimaryKey]
            public string SimpleAttribute1 { get; set; }
            public string SimpleAttribute2 { get; set; }
        }

        public class TestDBObject2 : DatabaseObject {
            public string SimpleAttribute1 { get; set; }
            [PrimaryKey]
            public string SimpleAttribute2 { get; set; }
        }

        public class TestDBObject3 : DatabaseObject {
            [PrimaryKey]
            public string SimpleAttribute1 { get; set; }
            [PrimaryKey]
            public string SimpleAttribute2 { get; set; }
            public string SimpleAttribute3 { get; set; }
        }

        public class TestDBObject4 : DatabaseObject {
            [PartitionKey]
            public string SimpleAttribute1 { get; set; }
            [PrimaryKey]
            public string SimpleAttribute2 { get; set; }
        }

        [ContainerName(Name = "TestContainer")]
        public class TestDBObjectErr : DatabaseObject {
            public string SimpleAttribute1 { get; set; }
            public string SimpleAttribute2 { get; set; }
        }

        public class TestDBObjectErr2 : DatabaseObject {
            [PrimaryKey]
            public string SimpleAttribute1 { get; set; }
            [PrimaryKey]
            [PartitionKey]
            public string SimpleAttribute2 { get; set; }
            [PartitionKey]
            public string SimpleAttribute3 { get; set; }
        }
        
    }
}