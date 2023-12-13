public class AdvancedPriceCalculator
{
    private List<Product> allProducts;
    public AdvancedPriceCalculator(List<Product> products)
    {
        allProducts = products;
    }

public void OnPriceCalculation(List<Product> scannedProducts)
{
    // Group the scanned products by their unique ID and count them, then group by category for display.
    var groupedProducts = scannedProducts.GroupBy(p => p.ID)
        .Select(g => new { Product = g.First(), Quantity = g.Count() })
        .GroupBy(p => p.Product.Group)
        .OrderBy(g => g.Key); // Sort the groups for consistent display order.

    decimal grandTotal = 0; // Initialize the grand total, which will sum up the subtotals of all groups.

    foreach (var group in groupedProducts) // Iterate through each group of products.
    {
        Console.WriteLine($"Group {GroupNameTranslator(group.Key)}:"); // Display the group name.
        decimal groupSubtotal = 0; // Initialize subtotal for the current group.

        foreach (var item in group) // Iterate through each item in the current group.
        {
            decimal itemTotalPrice = 0; // Initialize total price for the current item.
            int quantity = item.Quantity; // Get the quantity of the current item.
            Product product = item.Product; // Get the product details of the current item.

            // If the product is part of a multipack and the quantity is enough to form at least one multipack, apply multipack pricing.
            if (product.MultipackID.HasValue && quantity >= product.MultipackQuantity)
            {
                Product multipack = allProducts.First(p => p.ID == product.MultipackID.Value); // Find the corresponding multipack product.
                int multipacks = quantity / product.MultipackQuantity.Value; // Calculate how many complete multipacks are present.
                int remainder = quantity % product.MultipackQuantity.Value; // Calculate any remaining items after forming multipacks.
                itemTotalPrice = multipacks * multipack.Price + remainder * product.Price; // Total price considers both multipack and regular items.
                Console.WriteLine($" - {product.Name} x{quantity} (Multipack applied for {multipacks * product.MultipackQuantity.Value}) @ {itemTotalPrice:C}");
            }
            // If the product has a promotional price, apply it up to the threshold amount, then apply the regular price for any additional items.
            else if (product.PromotionalPrice.HasValue)
            {
                if (quantity == product.PromotionThreshold.Value)
                {
                    // If the quantity is exactly equal to the threshold, apply the promotional price
                    itemTotalPrice = quantity * product.PromotionalPrice.Value;
                    Console.WriteLine($" - {product.Name} x{quantity} @ {product.PromotionalPrice.Value:C} each (Promotion applied)");
                }
                else if (quantity > product.PromotionThreshold.Value)
                {
                    // If the quantity is greater than the threshold, apply the promotional price up to the threshold,
                    // and the standard price for any additional items
                    itemTotalPrice = product.PromotionThreshold.Value * product.PromotionalPrice.Value;
                    itemTotalPrice += (quantity - product.PromotionThreshold.Value) * product.Price;
                    Console.WriteLine($" - {product.Name} x{product.PromotionThreshold.Value} @ {product.PromotionalPrice.Value:C} each (Promotion applied)");
                    Console.WriteLine($" - {product.Name} x{quantity - product.PromotionThreshold.Value} @ {product.Price:C} each (Standard price)");
                }
                else
                {
                    // If the quantity is less than the threshold, apply the standard price
                    itemTotalPrice = quantity * product.Price;
                    Console.WriteLine($" - {product.Name} x{quantity} @ {product.Price:C} each (Standard price)");
                }
            } 
        
            else
            {
                // Apply standard pricing
                itemTotalPrice = quantity * product.Price;
                Console.WriteLine($" - {product.Name} x{quantity} @ {product.Price:C} each (Standard price)");
            }

            // Add Pant to item total price if applicable
            if (product.Pant.HasValue)
            {
                decimal pantTotal = product.Pant.Value * quantity;
                itemTotalPrice += pantTotal; // Include Pant in the item total price
                Console.WriteLine($"Pant: {product.Pant.Value:C} x {quantity} (Total Pant: {pantTotal:C})");
            }

            groupSubtotal += itemTotalPrice;
        }

        Console.WriteLine($"   Subtotal for Group {GroupNameTranslator(group.Key)}: {groupSubtotal:C}");
        grandTotal += groupSubtotal;
    }

    Console.WriteLine($"Grand Total: {grandTotal:C}");
}


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
            case 10: return "Pant";
            default: return "Unknown Group";
        }
    }
}
