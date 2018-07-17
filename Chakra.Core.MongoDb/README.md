Chakra.Core.MongoDb
===

Provider for MongoDB in Chakra.Core framework
---

Please checkout README.md on "Chakra.Core" package, first...
Then:

1) Define your own MongoDbOptions implementation like that:
```csharp
public class ApplicationMongoDbOptions: IMongoDbOptions
{
    public string ConnectionString { get; set; }

    public ApplicationMongoDbOptions()
    {        
        //Set connection string from application configuration, xml files, hardcoded, etc.
        ConnectionString = "mongodb://dbusername:dbpassword@servername/databasename?authSource=authenticationdatabase";
    }
}
```

2) Implement a concrete class for a storage provider on MongoDb Core
```csharp
[Repository]
public class MongoDbProductRepository: MongoDbRepositoryBase<Product, ApplicationMongoDbOptions>, IProductRepository
{
    public MongoDbProductRepository(IDataSession dataSession) 
        : base(dataSession) { }

    public IList<Product> FetchAvailableProductsInPeriod(DateTime from, DateTime to) 
    {
        //TODO...Insert here the method implementation
		//ex: => DataSession.Database.GetCollection<Product>().Where(p => ...
    }
}
```

3) Register default data session for application
```csharp
SessionFactory.RegisterDefaultSession<MongoDbDataSession<ApplicationMongoDbOptions>>();
```

4) Open session on storage (ex. database) using default configured provider, resolve repository interface 
using registered storage provider and obtain concrete repository implementation
```csharp
using (IDataSession dataSession = SessionFactory.OpenSession())
{
    var productRepository = dataSession.ResolveRepository<IProductRepository>();

    var entities = productRepository.FetchAvailableProductsInPeriod(
        new DateTime(2013, 8, 1), DateTime.Now);
}
```

You have the opportunity to switch a provider (ex. from fake to Entity Framework, from NHibernate 
to MongoDb) changing a single line of code on your application that can be really **database agnostic** 
and every single part can be tested in its own isolated context.
