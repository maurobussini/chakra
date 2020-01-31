Charka.Core.Mocks
===

Storage provider and engine for Mock data, usable to Unit Tests, Behavioral Tests, Integration Tests 
and...for simulate a fully-working application without a real database.

## Usage

Given an application codename "Chakra", just create a simple and logical scheme of your data 
model. Be sure that this structure implements interface 'IScenario'

```csharp
public interface IChakraScenario: IScenario
{
	public IList<Person> Persons { get; set; }
	public IList<Departments> Departments { get; set; }
}
```


Just register a default data session with Mock engine...

```csharp
SessionFactory.RegisterDefaultDataSession<MockDataSession<SimpleScenario>>();
```