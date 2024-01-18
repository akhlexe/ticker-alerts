namespace TickerAlert.Domain.Entities
{
    public class PriceMeasure
    {
        public FinancialAsset Asset { get; set; }
        public decimal Price { get; set; }
        public DateTime MeasuredOn { get; set; }
    }
}
