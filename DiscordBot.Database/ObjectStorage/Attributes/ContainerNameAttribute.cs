using System;

namespace DiscordBot.Domain.Database.ObjectStorage.Attributes {
    [AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = false)]
    public class ContainerNameAttribute : Attribute {
        
        public string Name { get; set; }
    }
}