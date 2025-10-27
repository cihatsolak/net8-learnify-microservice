namespace Learnify.Web.Options;

public class MicroserviceOption
{
    public required MicroserviceOptionItem Catalog { get; set; }
}

public class MicroserviceOptionItem
{
    public required string BaseAddress { get; set; }
}