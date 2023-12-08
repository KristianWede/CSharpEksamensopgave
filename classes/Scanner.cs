public class Scanner
{
    public delegate void PriceCalculationDelegate(List<Product> products);
    public event PriceCalculationDelegate? PriceCalculationEvent;

    private List<Product> scannedProducts;

    public Scanner()
    {
        scannedProducts = new List<Product>();
    }

    public void Scan(Product product)
    {
        // Simulate scanning delay
        Thread.Sleep(500);

        // Add the scanned product to the list
        scannedProducts.Add(product);

        // Trigger the event with the current list of scanned products
        PriceCalculationEvent?.Invoke(scannedProducts);
    }
}
