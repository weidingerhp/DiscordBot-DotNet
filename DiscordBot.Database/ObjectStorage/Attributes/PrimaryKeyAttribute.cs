using System;

namespace DiscordBot.Domain.Database.ObjectStorage.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {
    }
}