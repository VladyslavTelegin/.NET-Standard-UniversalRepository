# .NET Standard UniversalRepository

## Usage explanation:

### Step 1. Define DTO and Domain models.

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

### Step 2. Add UniversalRepository to your project like extension.

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

#### Full method's signature:

``` C#
public static void AddUniversalRepository(this IServiceCollection serviceCollection,
                                          Assembly dataTransferObjectsContainerAssembly,
                                          string connectionString,
                                          IEnumerable<Profile> mappingProfilesList = null,
                                          bool isTransientScoped = false,
                                          bool isCachingEnabled = false,
                                          IOptions<MemoryCacheOptions> memoryCacheOptions = null)

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
        return samples.IsSuccess ? samples.Result : null;
    }
}

```
