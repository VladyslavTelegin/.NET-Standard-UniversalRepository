# .NET-Standard-UniversalRepository

## Usage explanation:

### Step 1. Define Dto and Domain models.

``` C#
...

public class SampleDomain : IUniversalDomainObject
{
    public int Id { get; set; }
}

[Dapper.Contrib.Extensions.Table("dbo.Samples")]
public class SampleDto : IUniversalDataTransferObject<SampleDomain>
{
    public int Id { get; set; }
}

```

### Step 2. Add UniversalRepository to your project like Middleware extension.

``` C#

...

public void ConfigureServices(IServiceCollection services)
{
    ...
    
    // You can use ConnectionConfig model to specify DB connection params:
    services.AddUniversalRepository(
        dataTransferObjectsContainerAssembly: Assembly.GetAssembly(typeof(SampleDto)), 
        connectionConfig: new ConnectionConfig
        {
            DataSource = ".",
            InitialCatalog = "SampleService",
            User = "John Doe",
            Password = "JohnDoe2019#",
            IntegratedSecurity = true,
            PersistSecurityInfo = false,
        }
    );
   
    // Or You can use simple ConnectionString:
    services.AddUniversalRepository(
        dataTransferObjectsContainerAssembly: Assembly.GetAssembly(typeof(SampleDto)),
        connectionString: "Data Source=.;Initial Catalog=SampleService;Integrated Security=True;" + 
                          "User=JohnDoe;Password=JohnDoe2019#;Persist Security Info=False;"
    );
}
```

### Step 3. Resolve required repository and use it.

``` C#
...

public class SamplesController : Controller
{
    private readonly IUniversalDataService<SampleDomain> _samplesRepository;

    public SamplesController(IUniversalDataService<SampleDomain> samplesRepository)
    {
        _samplesRepository = samplesRepository;
    }
    
     [HttpGet]
     public async Task<IEnumerable<SampleDomain>> Get()
     {
         var samples = await _samplesRepository.GetAllAsync();
         return samples.Result;
     }
}

```
