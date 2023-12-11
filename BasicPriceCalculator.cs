public class BasicPriceCalculator
{
    private List<Product> allProducts;

    // Constructor: Initializes the calculator with a list of all available products.
    public BasicPriceCalculator(List<Product> products)
    {
        allProducts = products;
    }

    // Method called to perform price calculation based on scanned products.
    public void OnPriceCalculation(List<Product> scannedProducts)
    {
        // Group the scanned products by their ID, then by their group, and order them.
        var groupedProducts = scannedProducts.GroupBy(p => p.ID)
            .Select(g => new { Product = g.First(), Quantity = g.Count() })
            .GroupBy(p => p.Product.Group)
            .OrderBy(g => g.Key);

        decimal grandTotal = 0; // Initialize the grand total of all products.

        // Iterate through each group (e.g., Dairy, Pantry) of products.
        foreach (var group in groupedProducts)
        {
            decimal groupSubtotal = 0; // Initialize subtotal for each group.

            // Iterate through each unique product in the group.
            foreach (var item in group)
            {
                decimal itemTotalPrice = 0; // Initialize total price for the current product.
                int quantity = item.Quantity; // Determine the quantity of the current product.
                Product product = item.Product; // Get the product details.

                // Calculate the total price based on multipack or promotional pricing, if applicable.
                if (product.MultipackID.HasValue && quantity >= product.MultipackQuantity)
                {
                    Product multipack = allProducts.First(p => p.ID == product.MultipackID.Value);
                    int multipacks = quantity / product.MultipackQuantity.Value;
                    int remainder = quantity % product.MultipackQuantity.Value;
                    itemTotalPrice = multipacks * multipack.Price + remainder * product.Price;
                }
                else if (product.PromotionalPrice.HasValue)
                {
                    if (quantity == product.PromotionThreshold.Value)
                    {
                        itemTotalPrice = quantity * product.PromotionalPrice.Value;
                    }
                    else if (quantity > product.PromotionThreshold.Value)
                    {
                        itemTotalPrice = product.PromotionThreshold.Value * product.PromotionalPrice.Value;
                        itemTotalPrice += (quantity - product.PromotionThreshold.Value) * product.Price;
                    }
                    else
                    {
                        itemTotalPrice = quantity * product.Price;
                    }
                }
                else
                {
                    itemTotalPrice = quantity * product.Price;
                }

                groupSubtotal += itemTotalPrice; // Add the item's total price to the group's subtotal.
            }

            grandTotal += groupSubtotal; // Add the group's subtotal to the grand total.
        }

        // Retrieve and display the last scanned product.
        var lastScannedProduct = scannedProducts.LastOrDefault();
        if (lastScannedProduct != null)
        {
            Console.WriteLine($"Last Scanned: {lastScannedProduct.Name} @ {lastScannedProduct.Price:C}");
        }

        // Display the grand total price of all scanned products.
        Console.WriteLine($"Grand Total: {grandTotal:C}");
    }

    // Utility method to translate product group numbers into group names.
    private static string GroupNameTranslator(int groupNumber)
    {
        switch (groupNumber)
        {
            case 1: return "Dairy";
            case 2: return "Pantry";
            case 3: return "Beverages";
            case 4: return "Fresh Produce";
            case 5: return "Frozen Foods";
            case 6: return "Snacks";
            case 7: return "Household Items";
            case 8: return "Personal Care";
            case 9: return "Miscellaneous";
            default: return "Unknown Group";
        }
    }
}
