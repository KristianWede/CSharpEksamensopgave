public class AdvancedPriceCalculator
{
    public void OnPriceCalculation(List<Product> products)
    {
        var groupedProducts = products.GroupBy(p => p.Group)
                                     .Select(group => new { Group = group.Key, Products = group.ToList() });

        foreach (var group in groupedProducts)
        {
            Console.WriteLine($"Group {group.Group}:");
            foreach (var product in group.Products)
            {
                Console.WriteLine($"Product {product.ID} - Price: {product.Price}");
            }
        }
    }
}
