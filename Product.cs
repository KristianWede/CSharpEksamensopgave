public class Product
{
    public char ID { get; set; }
    public decimal Price { get; set; }
    public int Group { get; set; }
    public string Description { get; set; }
    public decimal? PromotionalPrice { get; set; }

    public Product(char id, decimal price, int group, string description, decimal? promotionalPrice = null)
    {
        ID = id;
        Price = price;
        Group = group;
        Description = description;
        PromotionalPrice = promotionalPrice;
    }
}
