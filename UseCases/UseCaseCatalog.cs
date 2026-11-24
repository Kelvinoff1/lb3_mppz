using StoreUseCases.Data;

namespace StoreUseCases.UseCases;

public class UseCaseCatalog
{
    public ViewProductsByCategoryUseCase ViewProductsByCategory { get; }   
    public ViewCustomerOrdersUseCase ViewCustomerOrders { get; }           
    public ViewCustomerProfileUseCase ViewCustomerProfile { get; }         
    public ViewProductSuppliersUseCase ViewProductSuppliers { get; }       
    public CreateOrderUseCase CreateOrder { get; }                         
    public AddProductToOrderUseCase AddProductToOrder { get; }             
    public PayOrderUseCase PayOrder { get; }                               
    public ApplyLoyaltyDiscountUseCase ApplyLoyaltyDiscount { get; }       
    public CheckProductStockUseCase CheckProductStock { get; }             

    public UseCaseCatalog(StoreRepository repository)
    {
        CheckProductStock = new CheckProductStockUseCase(repository);
        AddProductToOrder = new AddProductToOrderUseCase(repository, CheckProductStock);
        CreateOrder = new CreateOrderUseCase(repository, CheckProductStock, AddProductToOrder);
        ApplyLoyaltyDiscount = new ApplyLoyaltyDiscountUseCase();
        PayOrder = new PayOrderUseCase(repository, ApplyLoyaltyDiscount);
        ViewProductsByCategory = new ViewProductsByCategoryUseCase(repository);
        ViewCustomerOrders = new ViewCustomerOrdersUseCase(repository);
        ViewCustomerProfile = new ViewCustomerProfileUseCase(repository);
        ViewProductSuppliers = new ViewProductSuppliersUseCase(repository);
    }
}
