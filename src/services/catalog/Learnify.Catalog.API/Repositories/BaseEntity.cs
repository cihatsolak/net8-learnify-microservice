namespace Learnify.Catalog.API.Repositories;

public abstract class BaseEntity
{
    //snowflake algoritm
    //indexlemeyi kolaylaştırır.
    [BsonElement("_id")]
    public Guid Id { get; set; }
}
