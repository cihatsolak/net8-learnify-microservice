namespace Learnify.Discount.API.Options;

public sealed class MongoDbOption
{
    [Required]
    public string DatabaseName { get; set; }

    [Required]
    public string ConnectionString { get; set; }
}
