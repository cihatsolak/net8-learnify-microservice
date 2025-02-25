namespace Learnify.Order.Domain.Entities;

public class OrderItem : BaseEntity<int>
{

    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; }

    public void SetItem(Guid productId, string productName, decimal unitPrice)
    {
        ArgumentException.ThrowIfNullOrEmpty(productName);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(unitPrice);

        this.ProductId = productId;
        this.ProductName = productName;
        this.UnitPrice = unitPrice;
    }

    public void UpdatePrice(decimal newPrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(newPrice);

        this.UnitPrice = newPrice;
    }

    public void ApplyDiscount(float discountPercentage)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(discountPercentage, 100);
        ArgumentOutOfRangeException.ThrowIfNegative(discountPercentage);

        this.UnitPrice -= (this.UnitPrice * (decimal)discountPercentage / 100);
    }

    public bool IsSameItem(OrderItem otherItem)
    {

        return this.ProductId == otherItem.ProductId;
    }
}