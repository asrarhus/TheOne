using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeCostCalculator
{
    class Program
    {
        private static Dictionary<string, double> oDictionaryProduce;
        private static Dictionary<string, double> oDictionaryPoultry;
        private static Dictionary<string, double> oDictionaryPantry;

        static void Main(string[] args)
        {
            oDictionaryProduce = new Dictionary<string, double>();
            oDictionaryPoultry = new Dictionary<string, double>();
            oDictionaryPantry = new Dictionary<string, double>();

            Console.WriteLine("Please enter items for produce ingredients. Enter -1 if done with produce ingredients");
            InputIngredients(oDictionaryProduce);

            Console.WriteLine("Please enter items for poultry ingredients. Enter -1 if done with poultry ingredients");
            InputIngredients(oDictionaryPoultry);

            Console.WriteLine("Please enter items for Pantry ingredients. Enter -1 if done with Pantry ingredients");
            InputIngredients(oDictionaryPantry);


            int iRecipeNumber = 1;
            List<Recipe> recipes = new List<Recipe>();

            Console.WriteLine("Please select items for  recipe {0}. Enter -1 if recipe {0} is not needed", iRecipeNumber);
            string szOption = "First recipe";

            while (szOption != "-1" || string.IsNullOrEmpty(szOption))
            {

                double SalesTax;
                double TotalDiscount;
                double TotalCost;

                CalculateRecipePrice(oDictionaryProduce,
                    oDictionaryPoultry, oDictionaryPantry,
                    out SalesTax, out TotalDiscount, out TotalCost);

                Recipe oRecipe = new Recipe();
                oRecipe.Tax = SalesTax;
                oRecipe.Discount = TotalDiscount;
                oRecipe.Total = TotalCost;
                recipes.Add(oRecipe);

                iRecipeNumber++;
                Console.WriteLine("Please select items for  recipe {0}. Enter -1 if recipe {0} is not needed", iRecipeNumber);
                szOption = Console.ReadLine();
            }

            int iRecipeCounter = 1;
            foreach (Recipe recipe in recipes)
            {
                Console.WriteLine("Recipe {0} \n", iRecipeCounter);

                Console.WriteLine("Tax = {0} \n", recipe.Tax);
                Console.WriteLine("Discount = {0} \n", recipe.Discount);
                Console.WriteLine("Total =  {0} \n \n", recipe.Total);
            }
        }

        private static void InputIngredients(Dictionary<string, double> oDictionary)
        {
            string szOption = Console.ReadLine();

            while (szOption != "-1")
            {
                while (string.IsNullOrEmpty(szOption))
                {
                    Console.WriteLine("Please enter an item. Enter -1 if done");
                    szOption = Console.ReadLine();
                }

                if (szOption != "-1")
                {
                    string szItem = szOption;
                    double price;

                    Console.WriteLine("Please enter dollar price for the above item as a decimal number ");
                    string szPrice = Console.ReadLine();

                    while (!double.TryParse(szPrice, out price))
                    {
                        Console.WriteLine("Price entered not valid ");
                        Console.WriteLine("Please enter price for the above item");

                        szPrice = Console.ReadLine();
                    }

                    oDictionary.Add(szItem, price);

                    Console.WriteLine("Please enter next item.  Enter -1 if done ");
                    szOption = Console.ReadLine();
                }
            }
        }

        private static void CalculateRecipePrice(Dictionary<string, double> oDictionaryProduce,
            Dictionary<string, double> oDictionaryPoultry, Dictionary<string, double> oDictionaryPantry,
            out double SalesTax, out double TotalDiscount, out double TotalCost)
        {

            double ProduceDiscount;
            double ProduceTotal = GetCostForIngredientList(oDictionaryProduce, out ProduceDiscount);

            double PoultryDiscount;
            double PoultryTotal = GetCostForIngredientList(oDictionaryPoultry, out PoultryDiscount);

            double PantryDiscount;
            double PantryTotal = GetCostForIngredientList(oDictionaryPantry, out PantryDiscount);

            SalesTax = (0.086 * PoultryTotal) + (0.086 * PantryTotal);

            TotalDiscount = ProduceDiscount + PoultryDiscount + PantryDiscount;

            TotalCost = ProduceTotal + PoultryTotal + PantryTotal + SalesTax;

        }

        private static double GetCostForIngredientList(Dictionary<string, double> oDictionary, out double TotalDiscount)
        {
            double Total = 0;
            TotalDiscount = 0;

            foreach (KeyValuePair<string, double> item in oDictionary)
            {
                double quantity;

                Console.WriteLine("Please enter quantity to be used for {0}. Enter 0 if the item is not needed ", item.Key);
                string szQuantity = Console.ReadLine();

                while (!double.TryParse(szQuantity, out quantity))
                {
                    Console.WriteLine("Quantity entered not valid ");
                    Console.WriteLine("Please enter Quantity for the above item");

                    szQuantity = Console.ReadLine();
                }

                double discount = 0;

                if (item.Key.ToUpper().Contains("ORGANIC"))
                {
                    discount = Math.Round(item.Value, 1) * 0.05; //5% discount

                    TotalDiscount = TotalDiscount + discount;
                }

                double price = (quantity * item.Value) - discount;

                Total = Total + price;
            }

            return Total;
        }


        private class Recipe
        {
            private double tax;
            private double discount;
            private double total;

            public double Tax
            {
                get { return Math.Round(tax,2); }
                set { tax = value; }
            }

            public double Discount
            {
                get { return Math.Round(discount,2); }
                set { discount = value; }
            }

            public double Total
            {
                get { return Math.Round(total,2); }
                set { total = value; }
            }
        }
    }
}
