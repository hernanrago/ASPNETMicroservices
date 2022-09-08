namespace Shopping.Aggregator.Models
{
    public class BasketModel
    {
        public string UserName { get; set; }

        public IEnumerable<BasketItemExtendedModel> Items { get; set; } = new List<BasketItemExtendedModel>();

        public decimal TotalPrice { get; set; }
    }
}