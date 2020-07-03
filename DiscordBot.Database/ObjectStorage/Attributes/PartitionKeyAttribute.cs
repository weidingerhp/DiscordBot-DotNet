using System;

namespace DiscordBot.Domain.Database.ObjectStorage.Attributes {
    [AttributeUsage(validOn: AttributeTargets.Method, AllowMultiple = true)]
    public class PartitionKeyAttribute : Attribute {
    }
}