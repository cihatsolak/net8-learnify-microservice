namespace Learnify.Order.Domain.Entities;

public class Address : BaseEntity<int>
{
    public string Province { get; set; } 
    public string District { get; set; } 
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public string Line { get; set; }
}