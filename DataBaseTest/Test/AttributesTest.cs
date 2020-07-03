using DiscordBot.Domain.Database.ObjectStorage.Attributes;

namespace DiscordBot.Domain.Database.Test {
    [ContainerName(Name = "HalloWelt")]
    public class AttributesTest {
        [PrimaryKey]
        public string Name { get; set; }

        public static void Main(string[] args) {
            
        }
    }
}