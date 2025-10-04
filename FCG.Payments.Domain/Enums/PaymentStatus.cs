namespace FCG.Payments.Domain.Enums
{
    public enum PaymentStatus
    {
        Paid,
        Failed_InsufficientFunds,
        Failed_Timeout,
        Failed_InvalidCard,
        Failed_NetworkError
    }
}
