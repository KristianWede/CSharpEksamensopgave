public class Program
{
    // Method to create and return a list of dummy products for demonstration purposes.
    public List<Product> GetDummyProducts()
    {
        return new List<Product>
        {
            // Dairy Products
            new Product('A', "Milk", 10.00m, 1),
            new Product('B', "Cheese", 8.00m, 1),
            new Product('C', "Yogurt", 6.00m, 1),

            // Pantry Products
            new Product('D', "Bread", 12.50m, 2),
            // 'E' represents individual Pasta
            new Product('E', "Pasta", 5.00m, 2, multipackID: 'F', multipackQuantity: 6),
            // 'F' represents a multipack of 6 'E'
            new Product('F', "Pasta Multipack", 25.00m, 2),
            new Product('G', "Cereal", 7.00m, 2),
            new Product('H', "Rice", 9.00m, 2),

            // Beverage Products
            // 'I' has a promotional price if 3 are purchased
            new Product('I', "Juice", 15.00m, 3, promotionalPrice: 10.00m, promotionThreshold: 3),
            // 'J' has a promotional price if 2 are purchased
            new Product('J', "Water", 20.00m, 3, promotionalPrice: 18.00m, promotionThreshold: 2),
            new Product('K', "Coffee", 12.00m, 3),
            new Product('L', "Tea", 11.00m, 3),

            // Fresh Produce
            new Product('M', "Apples", 3.00m, 4),
            new Product('N', "Bananas", 2.50m, 4),
            new Product('O', "Carrots", 4.00m, 4),

            // Frozen Foods
            new Product('P', "Ice Cream", 10.00m, 5),
            new Product('Q', "Frozen Pizza", 12.00m, 5),
            new Product('R', "Frozen Vegetables", 5.50m, 5),

            // Snack Products
            new Product('S', "Chips", 2.50m, 6),
            new Product('T', "Chocolate", 2.00m, 6),
            new Product('U', "Cookies", 3.00m, 6),

            // Household Items
            new Product('V', "Laundry Detergent", 15.00m, 7),
            new Product('W', "Dish Soap", 3.50m, 7),
            new Product('X', "Paper Towels", 5.00m, 7),

            // Personal Care Products
            new Product('Y', "Shampoo", 7.50m, 8),
            new Product('Z', "Toothpaste", 4.50m, 8),
        };
    }

    // The main entry point of the program.
    public static void Main()
    {
        Program program = new Program();
        program.InitScanner();
    }

    // Method to initialize the scanning process.
    public void InitScanner()
    {
        var scanner = new Scanner(); // Create a new scanner object.
        var allProducts = GetDummyProducts(); // Retrieve the list of all dummy products.
        var basicCalculator = new BasicPriceCalculator(allProducts); // Initialize the basic price calculator.
        var advancedCalculator = new AdvancedPriceCalculator(allProducts); // Initialize the advanced price calculator.

        // Ask the user to choose between basic or advanced calculator.
        Console.WriteLine("Select calculator type: 1 for Basic, 2 for Advanced");
        var choice = Console.ReadLine();

        // Subscribe the chosen calculator to the scanner event.
        if (choice == "1")
        {
            scanner.PriceCalculationEvent += basicCalculator.OnPriceCalculation;
            Console.WriteLine("Using Basic Price Calculator.");
        }
        else if (choice == "2")
        {
            scanner.PriceCalculationEvent += advancedCalculator.OnPriceCalculation;
            Console.WriteLine("Using Advanced Price Calculator.");
        }
        else
        {
            Console.WriteLine("Invalid choice. Defaulting to Basic Price Calculator.");
            scanner.PriceCalculationEvent += basicCalculator.OnPriceCalculation;
        }

        // Loop to handle the scanning of products.
        while (true)
        {
            Console.WriteLine("Enter product code or 'exit' to quit:");
            var input = Console.ReadLine();

            if (input == "exit") break; // Exit the loop if the user enters 'exit'.

            try
            {
                var product = CreateProductFromCode(input); // Create a product from the input code.
                scanner.Scan(product); // Scan the product.
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message); // Handle invalid product codes.
            }
        }
    }

    // Method to create a product from its code.
    public Product CreateProductFromCode(string code)
    {
        if (string.IsNullOrEmpty(code) || code.Length != 1)
        {
            throw new ArgumentException("Product code must be a single character.");
        }

        List<Product> allProducts = GetDummyProducts(); // Get all dummy products.
        char productCode = code.ToUpper()[0]; // Convert the input code to uppercase.

        var product = allProducts.FirstOrDefault(p => p.ID == productCode); // Find the product with the given code.
        if (product != null)
        {
            return product; // Return the found product.
        }
        else
        {
            throw new ArgumentException($"Product with code '{productCode}' does not exist."); // Throw an exception if no product is found.
        }
    }
}
