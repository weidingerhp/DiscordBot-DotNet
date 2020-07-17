using System;

namespace DiscordBot.Domain.Database.ObjectStorage.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Property, AllowMultiple = true)]
    public class PartitionKeyAttribute : Attribute
    {
    }
}