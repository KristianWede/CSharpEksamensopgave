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
            // Add Pant to item total price if applicable
            if (product.Pant.HasValue)
            {
                decimal pantTotal = product.Pant.Value * quantity;
                itemTotalPrice += pantTotal; // Include Pant in the item total price
                // Optionally, you can display the Pant information if needed
                //Console.WriteLine($"Pant: {product.Pant.Value:C} x {quantity}"); 
            }

            groupSubtotal += itemTotalPrice; // Add the item's total price to the group's subtotal.
        }

        grandTotal += groupSubtotal; // Add the group's subtotal to the grand total.
    }

    // Retrieve and display the last scanned product and its standard price.
    var lastScannedProduct = scannedProducts.LastOrDefault();
    if (lastScannedProduct != null)
    {
        // Display the basic information of the last scanned product
        Console.Write($"Scanned product : {lastScannedProduct.Name} @ {lastScannedProduct.Price:C}");

        // If the last scanned product includes Pant, display the Pant information
        if (lastScannedProduct.Pant.HasValue)
        {
            Console.WriteLine($", Pant: {lastScannedProduct.Pant.Value:C}");
        }
        else
        {
            Console.WriteLine(); // Just end the line if there's no Pant.
        }
    }


    // Display the grand total price of all scanned products, including Pant.
    Console.WriteLine($"Grand Total: {grandTotal:C}");
}
}
