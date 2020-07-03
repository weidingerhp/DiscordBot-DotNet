# Database Module for Coderdojo Discord-Bot

This module should simplify the way on how to access the CosmosDB that will be provided for the
Bot. For Testing it is also always possible to use the [CosmosDB-Emulator](https://aka.ms/cosmosdb-emulator).
See more info [here](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator).

## Configuration
For normal operation you need the following settings in your appsettings - file:

```json
  "Database": {
    "db_endpoint": "<URI Endpoint to cosmosdb>",
    "db_key": "<key to cosmosdb>",
    "db_objectstorename": "<name of the database to hold objects>"
  }

```
db_objectstorename is not mandatory - but you will have to specify the name in code if
you plan to use the objectstore-capabilities.

## Usage
The DB-Access will be Injected to your Constructor in case you need it.

```c#
public class MyModule {
    private IDatabaseService _database;

    public MyModule(IDatabaseService database) {
        _database = database
    }

}
```

### Preparing the Objects to Store
Any Simple POCO (Plain Old Csharp Object) can be stored. You just need to derive it from
`DatabaseObject` and you will have to specify at least one PrimaryKey which has to be a
Property.

```c#
public class MYStoredClass : DatabaseObject {
    [PrimaryKey]
    String ObjectsUniqueName {get; set;}
}
```

Primary Keys can be specified on multiple fields. You can also specify the Container-Name
as a Class Attribute

```c#
[ContainerName(Name = "MyContainer")]
public class MYStoredClass : DatabaseObject {
    [PrimaryKey]
    String ObjectsName {get; set;}
    
    [PrimaryKey]
    String ObjectsGuild {get; set;}

    object AnyOtherData {get; set;}
}
```

You could also specify a single string, double or bool-property as PartitionKey if needed.

### Querying Data
Can be done by using the builtin Query Routines in the CosmosDB - Driver

```c#
...
    public void QueryData() {
        var container = _database.GetContainer<MyStoredClass>();

        var datafeed = container.GetItemQueryIterator<MyStoredClass>("select * from c");

        var datavalues = await datafeed.ReadNextAsync();
        foreach (var value in datavalues)
        {
            string name = value.ObjectsName;
        }
    }
...
``` 

### Manipulating Data
There are also a few extension functions that should make manipulating the data a little
more easey

- container.StoreObjectAsync(obj) - stores the object data in the DB,
  throws an error if already exists
- container.UpSertObject(obj) - Updates object if present - otherwise a new object will
  be inserted
- container.DeleteObject(obj) - Removes the object from the container. Only the primary
  keys and (if present) the partitionkey has to be set
  

