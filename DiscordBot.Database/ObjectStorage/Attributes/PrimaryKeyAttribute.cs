using System;

namespace DiscordBot.Domain.Database.ObjectStorage.Attributes {
    [AttributeUsage(validOn: AttributeTargets.Property, AllowMultiple = false)]
    public class PrimaryKeyAttribute : Attribute {
        
    }
}