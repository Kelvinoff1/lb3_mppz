using StoreUseCases.Data;
using StoreUseCases.Models;

namespace StoreUseCases.UseCases;

public class PayOrderUseCase
{
    private readonly StoreRepository _repository;
    private readonly ApplyLoyaltyDiscountUseCase _applyDiscount;

    public PayOrderUseCase(StoreRepository repository, ApplyLoyaltyDiscountUseCase applyDiscount)
    {
        _repository = repository;
        _applyDiscount = applyDiscount;
    }

    public PaymentReceipt Execute(int orderId, PaymentMethod method, DateTime date)
    {
        var order = _repository.GetOrderById(orderId)
            ?? throw new InvalidOperationException($"Замовлення з Id={orderId} не знайдено.");

        if (order.Status == OrderStatus.Paid)
            throw new InvalidOperationException($"Замовлення #{orderId} вже оплачено.");
        if (order.Items.Count == 0)
            throw new InvalidOperationException($"Замовлення #{orderId} порожнє.");

        decimal discount = _applyDiscount.Execute(order);

        var payment = new Payment
        {
            Id = _repository.NextPaymentId(),
            OrderId = orderId,
            Amount = order.Total,
            PaymentDate = date,
            Method = method
        };
        _repository.AddPayment(payment);

        return new PaymentReceipt(payment, order.Subtotal, discount);
    }
}
