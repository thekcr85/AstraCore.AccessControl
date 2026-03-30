namespace AstraCore.AccessControl.Domain.ValueObjects;

public sealed record CardNumber
{
    public string Value { get; }

    private CardNumber(string value)
    {
        Value = value;
    }

    public static CardNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Card number cannot be empty", nameof(value));

        if (value.Length != 16)
			throw new ArgumentException($"Card number must be exactly 16 characters, got {value.Length}.", nameof(value));

		if (!value.All(char.IsLetterOrDigit))
            throw new ArgumentException("Card number must contain only letters and digits", nameof(value));

        return new CardNumber(value.ToUpperInvariant());
    }

    public static implicit operator string(CardNumber cardNumber) => cardNumber.Value;

	public override string ToString() => Value;
}
