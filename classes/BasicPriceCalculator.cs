public class BasicPriceCalculator
{
    public void OnPriceCalculation(List<Product> products)
    {
        decimal total = products.Sum(p => p.Price);
        Console.WriteLine($"Total Price: {total}");
    }
}