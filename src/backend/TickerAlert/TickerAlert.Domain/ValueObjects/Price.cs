namespace TickerAlert.Domain.ValueObjects
{
    public class Price
    {
        public decimal Value { get; }

        public Price(decimal value)
        {
            Value = value < 0 ? 0 : value;
        }
    }
}
