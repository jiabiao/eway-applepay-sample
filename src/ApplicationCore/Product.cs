namespace eWAY.Samples.MonkeyStore.Models
{
    public class Product: BaseEntity
    {
        public string Text { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }
    }
}
