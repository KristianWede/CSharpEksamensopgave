public class Product
{
    public char ID { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Group { get; set; }
    public int? MultipackQuantity { get; set; } = null; // Null if not a multipack
    public char? MultipackID { get; set; } = null; // ID of the multipack counterpart, null if none
    public decimal? PromotionalPrice { get; set; } = null; // Null if no promotion is applicable
    public int? PromotionThreshold { get; set; } = null; // Quantity threshold for promotion
    public decimal? Pant { get; set; } = null; // Pant amount, null if no deposit is required

    // Constructor
    public Product(char id, string name, decimal price, int group, int? multipackQuantity = null, char? multipackID = null, decimal? promotionalPrice = null, int? promotionThreshold = null, decimal? pant = null)
    {
        ID = id;
        Name = name;
        Price = price;
        Group = group;
        MultipackQuantity = multipackQuantity;
        MultipackID = multipackID;
        PromotionalPrice = promotionalPrice;
        PromotionThreshold = promotionThreshold;
        Pant = pant; // Assign the Pant value
    }


    public bool IsPartOfMultipack()
    {
        return MultipackID.HasValue;
    }

    public bool IsEligibleForPromotion(int quantity)
    {
        return PromotionalPrice.HasValue && quantity >= PromotionThreshold;
    }

    public decimal GetEffectivePrice(int quantity)
    {
        if (IsEligibleForPromotion(quantity))
        {
            return PromotionalPrice.Value;
        }
        return Price;
    }
}


// Products and Groups Definition:
//
// Group 1: Dairy
//   - 'A': Milk, Price: 10.00
//   - 'B': Cheese, Price: 8.00
//   - 'C': Yogurt, Price: 6.00
//
// Group 2: Pantry
//   - 'D': Bread, Price: 12.50
//   - 'E': Pasta, Price: 5.00 (Available in multipack, represented by 'F')
//   - 'F': Pasta Multipack, Price: 25.00
//   - 'G': Cereal, Price: 7.00
//   - 'H': Rice, Price: 9.00
//
// Group 3: Beverages
//   - 'I': Juice, Price: 15.00 (Promotional Price: 10.00)
//   - 'J': Water, Price: 20.00 (Available in multipack, Promotional Price: 18.00)
//   - 'K': Coffee, Price: 12.00
//   - 'L': Tea, Price: 11.00
//
// Group 4: Fresh Produce
//   - 'M': Apples, Price: 3.00
//   - 'N': Bananas, Price: 2.50
//   - 'O': Carrots, Price: 4.00
//
// Group 5: Frozen Foods
//   - 'P': Ice Cream, Price: 10.00
//   - 'Q': Frozen Pizza, Price: 12.00
//   - 'R': Frozen Vegetables, Price: 5.50
//
// Group 6: Snacks
//   - 'S': Chips, Price: 2.50
//   - 'T': Chocolate, Price: 2.00
//   - 'U': Cookies, Price: 3.00
//
// Group 7: Household Items
//   - 'V': Laundry Detergent, Price: 15.00
//   - 'W': Dish Soap, Price: 3.50
//   - 'X': Paper Towels, Price: 5.00
//
// Group 8: Personal Care
//   - 'Y': Shampoo, Price: 7.50
//   - 'Z': Toothpaste, Price: 4.50
//
// Note: Some products have promotional prices or are sold in multipacks.
//       Multipack and promotional details are described with each product.
