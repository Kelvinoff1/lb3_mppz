namespace StoreUseCases.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.New;

    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    public List<OrderItem> Items { get; set; } = new();

    public Payment? Payment { get; set; }

    public decimal Subtotal => Items.Sum(i => i.Total);

    public decimal DiscountPercent => Customer?.LoyaltyCard?.DiscountPercent ?? 0m;

    public decimal Total => Math.Round(Subtotal * (1 - DiscountPercent / 100m), 2);
    
    public void RegisterPayment(Payment payment)
    {
        if (Payment != null)
            throw new InvalidOperationException($"Замовлення #{Id} вже оплачено.");

        Payment = payment;
        payment.Order = this;
        Status = OrderStatus.Paid;
    }
}
