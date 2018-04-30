Chakra.Core
===

Framework and guidelines for build .NET 4.x and .NET Core based enterprise applications.
---

1) Given a defined set of entities and model for your application workflow...
```csharp
public class Product: IEntity
{
    public int? Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
```

2) Define a repository interface for retrieve your domain entities
```csharp
public interface IProductRepository: IRepository<Product>
{
    IList<Product> FetchAvailableProductsInPeriod(DateTime from, DateTime to);
}
```

3) Define one or more fake scenarios with a common interface for test (ad unit test) your application layers
```csharp
public class IApplicationScenario: IScenario
{
    IList<Product> Product{ get; set; }
}

public class SimpleScenario: IApplicationScenario
{
    public IList<Product> Product{ get; set; }

    public void InitializeEntities()
    {
        Products.Add(new Product
        {
            Id = Product.Count + 1, 
            Code = "ABC", 
            Name = "Product ABC", 
            Description = "Non important..."
        });
    }
}
```

4) Implement a concrete class for a storage provider (ex: fake provider)
```csharp
[Repository]
public class MockupProductRepository: MockupRepositoryBase<Product, IApplicationScenario>, IProductRepository
{
    public MockupProductRepository(IDataSession dataSession) 
        : base(dataSession, scenario => scenario.Products) { }

    public IList<Product> FetchAvailableProductsInPeriod(DateTime from, DateTime to) 
    {
        //TODO...Insert here the method implementation
		//ex. => Scenario.Products.Where(p => p...
    }
}
```

5) Register default data session for application (ex. fake data provider, specifying default scenario)
```csharp
ScenarioFactory.Initialize(new SimpleScenario());
SessionFactory.RegisterDefaultSession<MockupDataSession>();
```

6) Open session on storage (ex. database) using default configured provider, resolve repository interface 
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
