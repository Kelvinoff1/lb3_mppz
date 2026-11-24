using StoreUseCases.Data;
using StoreUseCases.Models;
using StoreUseCases.Services;
using StoreUseCases.UseCases;

namespace StoreUseCases.UI;

public static class DemoScenario
{
    public static void Run(StoreRepository repository, StoreInfoService info, UseCaseCatalog uc)
    {
        StoreInfoService.PrintHeader("UC-01. Переглянути товари за категорією (категорія: Кава)");
        ConsoleMenu.PrintProducts(uc.ViewProductsByCategory.Execute(1));

        StoreInfoService.PrintHeader("UC-03. Переглянути профіль покупця (Id = 1)");
        ConsoleMenu.PrintProfile(uc.ViewCustomerProfile.Execute(1));

        StoreInfoService.PrintHeader("UC-02. Переглянути замовлення покупця (Id = 1)");
        foreach (var order in uc.ViewCustomerOrders.Execute(1))
            info.PrintOrder(order);

        StoreInfoService.PrintHeader("UC-04. Переглянути постачальників товару (Чорний чай)");
        foreach (var s in uc.ViewProductSuppliers.Execute(2))
            Console.WriteLine($"  - {s.Name}, {s.Phone}");

        StoreInfoService.PrintHeader("UC-05, UC-06, UC-09. Створити замовлення (Раф x3, Салямі x1)");
        var newOrder = uc.CreateOrder.Execute(customerId: 2, employeeId: 1, date: new DateTime(2026, 11, 16),
            items: new List<(int, int)> { (1, 3), (3, 1) });
        info.PrintOrder(newOrder);
        Console.WriteLine($"Залишок товару \"Раф\" на складі: {repository.GetProductById(1)!.StockQuantity}");

        StoreInfoService.PrintHeader("UC-09. Перевірка залишку (спроба замовити 100 курок)");
        try
        {
            uc.CheckProductStock.Execute(productId: 4, quantity: 100);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }

        StoreInfoService.PrintHeader("UC-07, UC-08. Оплатити замовлення (зі знижкою за карткою лояльності)");
        var receipt = uc.PayOrder.Execute(newOrder.Id, PaymentMethod.Card, new DateTime(2026, 11, 16));
        ConsoleMenu.PrintReceipt(receipt);
        info.PrintOrder(newOrder);
    }
}
