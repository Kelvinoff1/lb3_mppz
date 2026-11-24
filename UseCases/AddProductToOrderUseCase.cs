using StoreUseCases.Data;
using StoreUseCases.Models;

namespace StoreUseCases.UseCases;

public class AddProductToOrderUseCase
{
    private readonly StoreRepository _repository;
    private readonly CheckProductStockUseCase _checkStock;

    public AddProductToOrderUseCase(StoreRepository repository, CheckProductStockUseCase checkStock)
    {
        _repository = repository;
        _checkStock = checkStock;
    }

    public OrderItem Execute(int orderId, int productId, int quantity)
    {
        var order = _repository.GetOrderById(orderId)
            ?? throw new InvalidOperationException($"Замовлення з Id={orderId} не знайдено.");

        if (order.Status != OrderStatus.New)
            throw new InvalidOperationException($"Замовлення #{orderId} вже оплачено, змінювати його не можна.");

        _checkStock.Execute(productId, quantity);

        var product = _repository.GetProductById(productId)!;
        product.DecreaseStock(quantity);

        var existing = order.Items.FirstOrDefault(i => i.ProductId == productId);
        if (existing != null)
        {
            existing.Quantity += quantity;
            return existing;
        }

        var item = new OrderItem
        {
            Id = _repository.NextOrderItemId(),
            OrderId = orderId,
            ProductId = productId,
            Quantity = quantity
        };
        _repository.AddOrderItem(item);
        return item;
    }
}
