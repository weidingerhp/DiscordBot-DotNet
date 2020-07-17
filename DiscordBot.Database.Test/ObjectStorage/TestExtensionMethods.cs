using System.Threading;
using System.Threading.Tasks;
using DiscordBot.Domain.Database.Service;
using Microsoft.Azure.Cosmos;
using Moq;
using NUnit.Framework;
using static Moq.It;

namespace DiscordBot.Database.Test.ObjectStorage {
    public class TestExtensionMethods {
        [Test]
        public void TestGetContainerExtension() {
            var databaseMock = new Mock<Microsoft.Azure.Cosmos.Database>();
            
            databaseMock.Object.GetObjectContainer<TestClasses.TestDBObject1>();
            
            Assert.AreEqual(1, databaseMock.Invocations.Count);
            Assert.AreEqual("CreateContainerIfNotExistsAsync", databaseMock.Invocations[0].Method.Name);
            Assert.AreEqual("TestContainer", databaseMock.Invocations[0].Arguments[0]);
            Assert.AreEqual("/partkey", databaseMock.Invocations[0].Arguments[1]);
        }
        
        [Test]
        public void TestUpSertObject() {
            var databaseMock = new Mock<Container>();
            var testObj = new TestClasses.TestDBObject1() {
                SimpleAttribute1 = "Simple",
                SimpleAttribute2 = "Attribute"
            };
            
            databaseMock.Object.UpSertObject(testObj);
            
            Assert.AreEqual(1, databaseMock.Invocations.Count);
            Assert.AreEqual("UpsertItemAsync", databaseMock.Invocations[0].Method.Name);
            Assert.AreEqual(testObj, databaseMock.Invocations[0].Arguments[0]);
            Assert.AreEqual(new PartitionKey("H7seOUM="), databaseMock.Invocations[0].Arguments[1]);
            
            // Check if ID has been calculated
            Assert.AreEqual("wsbFYCR6yPkol4A=", testObj.Id);
        }

        [Test]
        public void TestStoreObject() {
            var databaseMock = new Mock<Container>();
            var testObj = new TestClasses.TestDBObject1() {
                SimpleAttribute1 = "Simple",
                SimpleAttribute2 = "Attribute"
            };

            databaseMock.Object.StoreObjectAsync(testObj);

            Assert.AreEqual(1, databaseMock.Invocations.Count);
            Assert.AreEqual("CreateItemAsync", databaseMock.Invocations[0].Method.Name);
            Assert.AreEqual(testObj, databaseMock.Invocations[0].Arguments[0]);
            
            // Check if ID has been calculated
            Assert.AreEqual("wsbFYCR6yPkol4A=", testObj.Id);
            Assert.AreEqual("H7seOUM=", testObj.PartKey);
        }

        [Test]
        public void TestDeleteObject() {
            var databaseMock = new Mock<Container>();
            var testObj = new TestClasses.TestDBObject1() {
                SimpleAttribute1 = "Simple",
                SimpleAttribute2 = "Attribute"
            };
            
            databaseMock.Object.DeleteObject(testObj);
            
            Assert.AreEqual(1, databaseMock.Invocations.Count);
            Assert.AreEqual("DeleteItemAsync", databaseMock.Invocations[0].Method.Name);
            Assert.AreEqual("wsbFYCR6yPkol4A=", databaseMock.Invocations[0].Arguments[0]);
            Assert.AreEqual(new PartitionKey("H7seOUM="), databaseMock.Invocations[0].Arguments[1]);
            
            // Check if ID has been calculated
            Assert.AreEqual("wsbFYCR6yPkol4A=", testObj.Id);
        }
        
    }
}