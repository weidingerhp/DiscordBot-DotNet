using System.Collections.Generic;
using System.Reflection;
using DiscordBot.Domain.Database.Service.Helpers;
using NUnit.Framework;

namespace DiscordBot.Database.Test.ObjectStorage
{
    public class ObjectStorePropertiesTest
    {
        [Test]
        public void TestParseObject() {
            var props = ObjectStoreProperties.Create(typeof(TestClasses.TestDBObject1));
            Assert.AreEqual("TestContainer", props.ContainerName);
            Assert.AreEqual(props.PartitionKey, null);
            Assert.AreEqual(props.PrimaryKeys,
                new List<PropertyInfo>() {typeof(TestClasses.TestDBObject1).GetProperty("SimpleAttribute1")});

            props = ObjectStoreProperties.Create(typeof(TestClasses.TestDBObject2));
            Assert.AreEqual("TestDBObject2", props.ContainerName);
            Assert.AreEqual(props.PartitionKey, null);
            Assert.AreEqual(props.PrimaryKeys,
                new List<PropertyInfo>() {typeof(TestClasses.TestDBObject2).GetProperty("SimpleAttribute2")});

            props = ObjectStoreProperties.Create(typeof(TestClasses.TestDBObject3));
            Assert.AreEqual("TestDBObject3", props.ContainerName);
            Assert.AreEqual(props.PartitionKey, null);
            Assert.AreEqual(props.PrimaryKeys,
                new List<PropertyInfo>() {
                    typeof(TestClasses.TestDBObject3).GetProperty("SimpleAttribute1"),
                    typeof(TestClasses.TestDBObject3).GetProperty("SimpleAttribute2")
                });
            
        }

        [Test]
        public void TestParseWithPartitionKey() {
            var props = ObjectStoreProperties.Create(typeof(TestClasses.TestDBObject4));
            Assert.AreEqual("TestDBObject4", props.ContainerName);
            Assert.AreEqual(props.PartitionKey, typeof(TestClasses.TestDBObject4).GetProperty("SimpleAttribute1"));
            Assert.AreEqual(props.PrimaryKeys,
                new List<PropertyInfo>() {typeof(TestClasses.TestDBObject4).GetProperty("SimpleAttribute2")});
        }

        [Test]
        public void TestErrorObject() {
            Assert.Catch(() => {
                var props = ObjectStoreProperties.Create(typeof(TestClasses.TestDBObjectErr));
            });
            Assert.Catch(() => {
                var props = ObjectStoreProperties.Create(typeof(TestClasses.TestDBObjectErr2));
            });
        }

    }
}