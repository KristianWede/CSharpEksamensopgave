public class Program
{
    public static void Main()
    {
        var scanner = new Scanner();
        var basicCalculator = new BasicPriceCalculator();
        var advancedCalculator = new AdvancedPriceCalculator();

        scanner.PriceCalculationEvent += basicCalculator.OnPriceCalculation;
        scanner.PriceCalculationEvent += advancedCalculator.OnPriceCalculation;

        while (true)
        {
            Console.WriteLine("Enter product code or 'exit' to quit:");
            var input = Console.ReadLine();

            if(input == "exit") break;

            // You might want to add error handling for invalid inputs
            var product = CreateProductFromCode(input); // Implement this method
            scanner.Scan(product);
        }
    }


    private static Product CreateProductFromCode(string code)
{
    if (string.IsNullOrEmpty(code) || code.Length != 1)
    {
        throw new ArgumentException("Invalid product code.");
    }

    char productCode = code[0];
    switch (productCode)
    {
        case 'A':
            return new Product('A', 10.00m, 1, "Product A Description");
        case 'B':
            return new Product('B', 15.50m, 1, "Product B Description", 14.00m); // Example with promotional price
        case 'C':
            return new Product('C', 7.75m, 2, "Product C Description");
        // ... and so on for other product codes
        default:
            throw new ArgumentException("Unknown product code.");
    }
}


}
