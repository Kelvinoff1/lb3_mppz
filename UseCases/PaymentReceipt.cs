using StoreUseCases.Models;

namespace StoreUseCases.UseCases;

public record PaymentReceipt(Payment Payment, decimal Subtotal, decimal Discount);
