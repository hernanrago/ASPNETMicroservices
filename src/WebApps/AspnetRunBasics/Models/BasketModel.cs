namespace AspnetRunBasics.Models
{
        public class BasketModel
    {
        public string UserName { get; set; }

        public ICollection<BasketItemExtendedModel> Items { get; set; } = new List<BasketItemExtendedModel>();

        public decimal TotalPrice { get; set; }
    }
}