using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSharp.TestSuites
{
    public class LINQTestSuite : TestSuite, ITestSuite
    {
        public void Run()
        {
            WriteTestSuiteName();

            // Query Syntax
            LINQ_Basic();
            LINQ_Projection1();
            LINQ_ProjectionToAnonymousType();
            LINQ_Filtering();
            LINQ_Ordering();
            LINQ_Scalar();
            LINQ_ToList();
            LINQ_Let();
            LINQ_Into();
            LINQ_Join1();
            LINQ_Join2();
            LINQ_Join_Into();
            LINQ_Group();
            LINQ_Group_Ordered();
            LINQ_MultipleFromLetWhereClauses();
            LINQ_Scratch();

            // Method-based
            LINQ12();
            LINQ13();
            LINQ14();
            LINQ15();
            LINQ16();
            LINQ17();
            LINQ18();
            LINQ19();
            LINQ20();

            WriteTestSuiteName();
        }

        /// <summary>
        /// Uses query syntax to simply recreate the list of Customers
        /// </summary>
        private void LINQ_Basic()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Company company = new Company();

            /*IEnumerable<Customer>*/
            var customers =
            // From Clause: range variable.  The Type (Customer) is optional.  And collection (sequence)
            from /*Customer*/ c
            in company.Customers
                // Select Clause:  what to query
            select c;

            foreach (Customer c in customers) { Console.WriteLine(c.Name); }
        }

        /// <summary>
        /// Uses query syntax to create a list of names from the input customer sequence.  This is called Projection.
        /// </summary>
        private void LINQ_Projection1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Company company = new Company();

            /*IEnumerable<string>*/
            var customerNames =
            // From Clause: range variable and collection (sequence)
            from /*Customer*/ c
            in company.Customers
                // Select Clause: what to query
            select c.Name;

            foreach (string customerName in customerNames) { Console.WriteLine(customerName); }
        }

        /// <summary>
        /// Uses query syntax to create a list of CustomerViewModel's from the input customer sequence.  This is called Projection.
        /// </summary>
        private void LINQ_ProjectionToAnonymousType()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Company company = new Company();

            IEnumerable<CustomerViewModel> customerVMs =
                // From Clause range variable and collection (sequence)
                from /*Customer*/ c
                in company.Customers
                    // Select Clause what to query
                select new CustomerViewModel { Name = c.Name };

            foreach (CustomerViewModel customerVM in customerVMs) { Console.WriteLine(customerVM.Name); }
        }

        /// <summary>
        /// Use query syntax to  create a list of Customer's from the input customer sequence filtered by customer names.
        /// </summary>
        private void LINQ_Filtering()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Company company = new Company();

            IEnumerable<Customer> customers =
                // From Clause: range variable.  The Type (Customer) is optional.  And collection (sequence)
                from /*Customer*/ c
                in company.Customers
                    // Filter Clause
                where c.Name.Length > 4
                // Select Clause: what to query
                select c;

            foreach (Customer c in customers) { Console.WriteLine(c.Name); }
        }

        /// <summary>
        /// Use query syntax to  create a list of Customer's from the input customer sequence ordered by name descending
        /// </summary>
        private void LINQ_Ordering()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Company company = new Company();

            IEnumerable<Customer> customers =
                // From Clause range variable and collection (sequence)
                from /*Customer*/ c
                in company.Customers
                    // Order Clause
                orderby c.Name descending
                // Select Clause what to query
                select c;

            foreach (Customer c in customers) { Console.WriteLine(c.Name); }
        }

        /// <summary>
        /// Uses query syntax to count the number of Customers
        /// </summary>
        private void LINQ_Scalar()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Company company = new Company();

            int nbrCustomers = (
                // From Clause: range variable.  The Type (Customer) is optional.  And collection (sequence)
                from /*Customer*/ c
                in company.Customers
                    // Select Clause: what to query
                select c).Count();

            Console.WriteLine($"Nbr. Customers={nbrCustomers}");
        }

        /// <summary>
        /// Uses query syntax to create a list of CustomerViewModel's from the input customer sequence converted to a list
        /// </summary>
        private void LINQ_ToList()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Company company = new Company();

            List<Customer> customers = (
                // From Clause: range variable collection
                from c
                in company.Customers
                    // Select Clause: what to query
                select c).
                // convert to list
                ToList();

            customers.ForEach(c => Console.WriteLine(c.Name));
        }

        private class Ingredient
        {
            public string Name { get; set; }
            public int Calories { get; set; }
            public bool IsDairy { get; set; }

            public override string ToString()
            {
                return
                    "Name: " + Name + ", " +
                    "Calories: " + Calories + ", " +
                    "IsDairy: " + (IsDairy ? "true" : "false");
            }
        }

        class Kitchen
        {
            public Ingredient[] Ingredients { get; private set; }

            public Kitchen()
            {
                Ingredients = new Ingredient[]
                {
                    new Ingredient { Name = "Sugar", Calories = 500, IsDairy = false },
                    new Ingredient { Name = "Egg", Calories = 100, IsDairy = false },
                    new Ingredient { Name = "Milk", Calories = 150, IsDairy = true },
                    new Ingredient { Name = "Flour", Calories = 50, IsDairy = false },
                    new Ingredient { Name = "Butter", Calories = 200, IsDairy = true }
                };
            }
        }

        /// <summary>
        /// Uses syntax query with let clause
        /// </summary>
        private void LINQ_Let()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Kitchen kitchen = new Kitchen();

            IEnumerable<string> highCalorieIngredientNamesQuery =
                // range variable and collection
                from /*Ingredient*/ i
                in kitchen.Ingredients
                    // let clause
                let isBad = i.Calories >= 150 && i.IsDairy
                // condition
                where isBad
                // ordering
                orderby i.Name
                // what to query
                select i.Name;

            foreach (string ingredientName in highCalorieIngredientNamesQuery) { Console.WriteLine(ingredientName); }
        }

        /// <summary>
        /// Uses syntax query with into clause
        /// </summary>
        private void LINQ_Into()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Kitchen kitchen = new Kitchen();

            IEnumerable<Ingredient> highCalDairyQuery =
                // range variable and collection
                from /*Ingredient*/ i
                in kitchen.Ingredients
                    // Select Clause
                select new
                // anonymous type
                {
                    OriginalIngredient = i,
                    IsDairy = i.IsDairy,
                    IsHighCalorie = i.Calories >= 150
                }
                // new range variable, which is of the anonymous type created by the select above
                into temp
                // condition
                where temp.IsDairy && temp.IsHighCalorie
                // cannot write "select i;" as into hides the previous range variable i 
                select temp.OriginalIngredient;

            foreach (Ingredient ingredient in highCalDairyQuery) { Console.WriteLine(ingredient); }
        }

        /// <summary>
        /// Use query syntax to create an anonymous type from the input customer sequence joined with Order Descriptions on the customer ID and the order customer ID
        /// </summary>
        private void LINQ_Join1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Company company = new Company();

            // var is an anonymous type that is defined below in the new
            var customerOrders =
                // From Clause: range variable and collection (sequence)
                from /*Customer*/ c
                in company.Customers
                    // Join Clause
                join o in company.Orders on c.Id equals o.CustomerId
                // Select Clause
                // Anonymous Type
                select new
                {
                    Id = c.Id,
                    Name = c.Name,
                    Item = o.Description
                };

            foreach (var custOrd in customerOrders) { Console.WriteLine($"Customer ({custOrd.Id}): {custOrd.Name}, Item: {custOrd.Item}"); }
        }

        class Recipe
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Calories { get; set; }
        }

        class Review
        {
            public int Id { get; set; }
            public int RecipeId { get; set; }
            public string Text { get; set; }
        }

        class Cookbook
        {
            public Recipe[] Recipes { get; private set; }
            public Review[] Reviews { get; private set; }

            public Cookbook()
            {
                Recipes = new Recipe[]
                {
                    new Recipe { Id = 0, Name = "Crispy Duck", Calories = 500 },
                    new Recipe { Id = 1, Name = "Mashed Potato", Calories = 200 },
                    new Recipe { Id = 2, Name = "Sachertorte", Calories = 750 },
                    new Recipe { Id = 3, Name = "Panettone", Calories = 1000 }
                };
                Reviews = new Review[]
                {
                    new Review { Id = 0, RecipeId = 0, Text = "Tasty!" },
                    new Review { Id = 1, RecipeId = 0, Text = "Not nice :(" },
                    new Review { Id = 2, RecipeId = 0, Text = "Pretty good" },
                    new Review { Id = 3, RecipeId = 1, Text = "Too hard" },
                    new Review { Id = 4, RecipeId = 1, Text = "Loved it" },
                    new Review { Id = 5, RecipeId = 2, Text = "Love them goodies!" },
                    new Review { Id = 6, RecipeId = 3, Text = "Too sweet!" },
                    new Review { Id = 7, RecipeId = 3, Text = "Oh so good!" }
                };
            }
        }

        /// <summary>
        /// Uses syntax query with join clause
        /// </summary>
        private void LINQ_Join2()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Cookbook cookbook = new Cookbook();

            var query =
                // range variable and collection
                from recipe in cookbook.Recipes
                    // join
                join review in cookbook.Reviews on recipe.Id equals review.RecipeId
                // anonymous type
                select new
                {
                    RecipeName = recipe.Name,
                    ReviewText = review.Text
                };

            foreach (var recipeReview in query) { Console.WriteLine($"{recipeReview.RecipeName}-'{recipeReview.ReviewText}'"); }
        }

        /// <summary>
        /// Uses syntax query with outer join clause
        /// </summary>
        private void LINQ_Join_Into()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Cookbook cookbook = new Cookbook();

            var query =
                // range variable and Collection
                from Recipe recipe
                in cookbook.Recipes
                where recipe.Name.Length > 0 // bogus where clause
                // join
                join review in cookbook.Reviews on recipe.Id equals review.RecipeId
                into reviewGroup
                from review in reviewGroup.DefaultIfEmpty(new Review())
                select new
                {
                    RecipeName = recipe.Name,
                    ReviewText = review.Text
                };

            foreach (var recipeReview in query) { Console.WriteLine($"{recipeReview.RecipeName}-'{recipeReview.ReviewText}'"); }
        }

        /// <summary>
        /// Uses syntax query with outer group clause
        /// </summary>
        private void LINQ_Group()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Kitchen kitchen = new Kitchen();

            // Create a collection of groups of Ingredient's where Calories is the Key (same for each group)
            IEnumerable<IGrouping<int, Ingredient>> query =
                from /*Ingredient*/ i
                in kitchen.Ingredients
                group i by i.Calories;

            // public interface IGrouping<out TKey, out TElement> : IEnumerable<TElement>, IEnumerable
            foreach (IGrouping<int, Ingredient> ig in query)
            {
                Console.WriteLine($"Ingredients with {ig.Key} calories");
                foreach (Ingredient i in ig)
                {
                    Console.WriteLine($"\t{i.Name}");
                }
            }

        }

        /// <summary>
        /// Uses syntax query with outer group clause
        /// </summary>
        private void LINQ_Group_Ordered()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Kitchen kitchen = new Kitchen();

            // Same as above but ordered by calories
            IEnumerable<IGrouping<int, Ingredient>> query =
                from i in kitchen.Ingredients
                group i by i.Calories
                into calorieGroup
                orderby calorieGroup.Key
                select calorieGroup;

            foreach (IGrouping<int, Ingredient> ig in query)
            {
                Console.WriteLine($"Ingredients with {ig.Key} calories");
                foreach (Ingredient i in ig)
                {
                    Console.WriteLine($"\t{i.Name}");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LINQ_MultipleFromLetWhereClauses()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Cookbook cookbook = new Cookbook();

            foreach (var r in cookbook.Recipes) { Console.WriteLine($"Id={r.Id}, RecipeName={r.Name}, Calories={r.Calories}"); }
            foreach (var r in cookbook.Reviews) { Console.WriteLine($"Id={r.Id}, Id={r.RecipeId}, Text={r.Text}"); }

            // Selects from both sequences, and creates tuples of recipe-review.  This does not do a join
            var query =
                from r in cookbook.Recipes

                let name = r.Name
                where name.Length > 4
                orderby name

                let calories = r.Calories
                where calories > 500
                orderby calories descending

                from v in cookbook.Reviews
                let text = v.Text
                where text.Length > 6
                orderby text

                select new { RecipeName = name, Calories = calories, Text = text };

            foreach (var rr in query) { Console.WriteLine($"RecipeName={rr.RecipeName}, Calories={rr.Calories}, Text={rr.Text}"); }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LINQ_Scratch()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Company company = new Company();

            Console.WriteLine("Basic");
            IEnumerable<Customer> customers =
                from c in company.Customers
                select c;

            foreach (Customer c in customers) { Console.WriteLine(c.Name); }

            Console.WriteLine("Filtered");
            IEnumerable<Customer> customers2 =
                from c in company.Customers
                let name = c.Name
                where name.Length > 4
                select c;

            foreach (Customer c in customers) { Console.WriteLine(c.Name); }

            Console.WriteLine("Grouped");
            var customerGroups =
                from c in company.Customers
                group c by c.Name[0];

            foreach (IGrouping<char, Customer> cg in customerGroups)
            {
                Console.WriteLine(cg.Key);
                foreach (Customer c in cg) { Console.WriteLine(c.Name); }
            }
        }

        /// <summary>
        /// Uses method-based (fluent) syntax to union 2 customer sequences
        /// </summary>
        private void LINQ12()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Company company = new Company();

            List<Customer> additionalCustomers = new List<Customer> { new Customer { Id = 1, Name = "Gary" } };

            // Method-based query
            List<Customer> customers = company.Customers.Union(additionalCustomers).ToList();

            customers.ForEach(c => Console.WriteLine(c.Name));
        }

        /// <summary>
        /// Uses both query and method-based syntax to perform the same query
        /// </summary>
        private void LINQ13()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            string[] names = { "Burke", "Connor", "Frank", "Everett", "Albert", "George", "Harris", "David" };

            // In query (expression) syntax
            IEnumerable<string> query =
                // range variable and collection
                from s in names
                    // condition
                where s.Length == 5
                // ordering
                orderby s
                // what to query
                select s.ToUpper();

            foreach (string s in query) { Console.WriteLine(s); }

            // method-based query
            IEnumerable<string> query2 =
                names
                // Filter
                .Where(s => s.Length == 5)
                // Sorter
                .OrderBy(s => s)
                // Projector
                .Select(s => s.ToUpper());

            foreach (string s in query2) { Console.WriteLine(s); }
        }

        /// <summary>
        /// Creates a delegate instance using a lambda expression, and passes the instance to a query using method-based syntax
        /// </summary>
        private void LINQ14()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            string[] names = { "Abe", "Burke", "Connor", "Frank", "Everett", "Albert", "George", "Harris", "David", "Gus", "Burt" };

            // public delegate TResult Func<in T, out TResult>(T arg);
            Func<string, bool>
                // delegate instance
                condition =
                // parameters
                n
                // lambda operator
                =>
                // method
                n.Length > 4;

            IEnumerable<string> filteredNames = names.Where(condition);

            foreach (string s in filteredNames) { Console.WriteLine(s); }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LINQ15()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Cookbook cookbook = new Cookbook();

            var reviewsByRecipe = cookbook.Recipes.SelectMany(c => cookbook.Reviews.Where(v => c.Id == v.RecipeId).Select(r => new { RecipeName = c.Name, ReviewText = r.Text }));

            foreach (var review in reviewsByRecipe) { Console.WriteLine($"{review.RecipeName}-'{review.ReviewText}'"); }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LINQ16()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            List<string> array = new List<string>
            {
                "dot",
                "net",
                "perls"
            };

            // Convert each string in the string array to a character array.
            // ... Then combine those character arrays into one.
            var result = array.SelectMany(element => element.ToCharArray());

            // Display letters.
            foreach (char letter in result) { Console.WriteLine(letter); }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LINQ17()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            int[] numbers = { 2, 3, 8, 7, 9 };
            string[] strings = { "a", "ab", "abc", "abcd", "abcde", "abcdef", "abcdefg", "x", "xy", "xyz" };

            // For each 'n' in 'numbers', select the strings in 'strings' that are of length 'n'
            var query1 = numbers.SelectMany(n => strings.Where(s => s.Length == n));
            foreach (string s in query1) { Console.WriteLine(s); }

            // For each 'n' in 'numbers', select the strings in 'strings' that are of length 'n', and create an anonymous type with the length and string
            var query2 = numbers.SelectMany(n => strings.Where(s => s.Length == n).Select(s => new { Length = n, String = s }));
            foreach (var a in query2) { Console.WriteLine($"{a.Length}-'{a.String}'"); }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LINQ18()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Cookbook cookbook = new Cookbook();

            var reviewByRecipe = cookbook.Reviews.Join(
                cookbook.Recipes,
                review => review.RecipeId,
                recipe => recipe.Id,
                (review, recipe) => new { RecipeName = recipe.Name, ReviewText = review.Text });

            foreach (var recipeReview in reviewByRecipe) { Console.WriteLine($"{recipeReview.RecipeName}-'{recipeReview.ReviewText}'"); }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LINQ19()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Kitchen kitchen = new Kitchen();

            // Create a collection of groups of Ingredient's where Calories is the Key (same for each group)
            IEnumerable<IGrouping<int, Ingredient>> query = kitchen.Ingredients.GroupBy(i => i.Calories);

            // public interface IGrouping<out TKey, out TElement> : IEnumerable<TElement>, IEnumerable
            foreach (IGrouping<int, Ingredient> ingredientGroup in query)
            {
                Console.WriteLine($"Ingredients with {ingredientGroup.Key} calories");
                foreach (Ingredient ingredient in ingredientGroup)
                {
                    Console.WriteLine($" - {ingredient.Name}");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LINQ20()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            var rand = new Random((int) (DateTime.Now.Ticks % int.MaxValue));
            List<int> numbers = new List<int>();
            for (int n = 0; n < 20; n++)
            {
                numbers.Add(rand.Next() % 1000);
            }

            var orderedNumbers = numbers.OrderBy(p => p).Reverse().ToList();

            foreach (int n in orderedNumbers) { Console.WriteLine($"{n}"); }
        }
    }
}
