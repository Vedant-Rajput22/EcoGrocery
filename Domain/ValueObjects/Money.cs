namespace Domain.ValueObjects;

public record Money(decimal Amount, string Currency)
{
    public static Money Zero(string cur) => new(0, cur);

    public static Money operator +(Money a, Money b)
        => a.Currency == b.Currency
           ? new Money(a.Amount + b.Amount, a.Currency)
           : throw new InvalidOperationException("Currency mismatch");

    public static Money operator *(Money m, int q) => new(m.Amount * q, m.Currency);
}
