namespace Learnify.Catalog.API.Options;

public sealed class MongoDbOption
{
    [Required]
    public string DatabaseName { get; set; }

    [Required]
    public string ConnectionString { get; set; }
}
