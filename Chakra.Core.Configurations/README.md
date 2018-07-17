Chakra.Core.Configurations
===

Configuration extensions for Chakra.Core
---

Please checkout README.md on "Chakra.Core" package, first...
Then:

1) Define your own application configuration implementation class like that:
```csharp
public class ApplicationConfiguration : IApplicationConfigurationRoot
{
    public string EnvironmentName { get; set; }

    public string ApiKey { get; set; }

    public IList<SampleServiceSetting> Services { get; set; }
}

public class SampleServiceSetting
{
    public string Name { get; set; }
    public string BaseUrl{ get; set; }
}
```

2) Insert your **appsettings.json** file on application root folder with flag 'Copy to output directory' set to '*Copy always*'. 
Note that configuration works also with ASP.NET **environments** with value retrived from '*ASPNETCORE_ENVIRONMENT*' environment 
variable (ex. 'appsettings.Development.json', 'appsettings.Production.json', 'appsettings.[Environment Name].json').

3) Get configuration value using strongly-typed class with singleton instance like that:
```csharp
var apiKey = ConfigurationFactory<ApplicationConfiguration>.Instance.ApiKey;
var enviromentName = ConfigurationFactory<ApplicationConfiguration>.Instance.EnvironmentName;
```