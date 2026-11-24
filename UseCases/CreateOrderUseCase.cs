using StoreUseCases.Data;
using StoreUseCases.Models;

namespace StoreUseCases.UseCases;

public class CreateOrderUseCase
{
    private readonly StoreRepository _repository;
    private readonly CheckProductStockUseCase _checkStock;
    private readonly AddProductToOrderUseCase _addProduct;

    public CreateOrderUseCase(StoreRepository repository,
        CheckProductStockUseCase checkStock, AddProductToOrderUseCase addProduct)
    {
        _repository = repository;
        _checkStock = checkStock;
        _addProduct = addProduct;
    }

    public Order Execute(int customerId, int employeeId, DateTime date,
        IReadOnlyCollection<(int ProductId, int Quantity)> items)
    {
        if (items.Count == 0)
            throw new InvalidOperationException("Замовлення має містити хоча б один товар.");
        
        foreach (var group in items.GroupBy(i => i.ProductId))
            _checkStock.Execute(group.Key, group.Sum(i => i.Quantity));

        var order = new Order
        {
            Id = _repository.NextOrderId(),
            OrderDate = date,
            CustomerId = customerId,
            EmployeeId = employeeId
        };
        _repository.AddOrder(order);

        foreach (var (productId, quantity) in items)
            _addProduct.Execute(order.Id, productId, quantity); 

        return order;
    }
}
