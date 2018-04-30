Chakra.Core.EntityFramework
===

Provider for Microsoft EntityFramework Core in Chakra.Core framework
---

Please checkout README.md on "Chakra.Core" package, first...
Then:

1) Define your own DbContext implementation like that:
```csharp
public class ApplicationDbContext: DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //TODO...Insert here configuration
    }
}
```

2) Implement a concrete class for a storage provider on Entity Framework Core
```csharp
[Repository]
public class EntityFrameworkProductRepository: EntityFrameworkRepositoryBase<Product, ApplicationDbContext>, IProductRepository
{
    public EntityFrameworkProductRepository(IDataSession dataSession) 
        : base(dataSession, dbcontext => dbcontext.Products) { }

    public IList<Product> FetchAvailableProductsInPeriod(DateTime from, DateTime to) 
    {
        //TODO...Insert here the method implementation
		//ex: => DataSession.Context.Products.Where(p => ...
    }
}
```

3) Register default data session for application (ex. fake data provider)
```csharp
SessionFactory.RegisterDefaultSession<EntityFrameworkDataSession<ApplicationDbContext>>();
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
