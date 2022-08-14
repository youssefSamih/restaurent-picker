using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Restaurant
{
    public class RestaurantPick
    {
        #region You_should_not_modify_this_region


        private class Restaurant
        {
            public int RestaurantId { get; set; }
            public Dictionary<string, decimal> Menu { get; set; }
        }

        private readonly List<Restaurant> _restaurants = new List<Restaurant>();

        /// <summary>
        /// Reads the file specified at the path and populates the restaurants list
        /// </summary>
        /// <param name="filePath">Path to the comma separated restuarant menu data</param>
        public void ReadRestaurantData(string filePath)
        {
            try
            {
                var records = File.ReadLines(filePath);

                foreach (var record in records)
                {
                    var data = record.Split(',');
                    var restaurantId = int.Parse(data[0].Trim());
                    var restaurant = _restaurants.Find(r => r.RestaurantId == restaurantId);

                    if (restaurant == null)
                    {
                        restaurant = new Restaurant { Menu = new Dictionary<string, decimal>() };
                        _restaurants.Add(restaurant);
                    }

                    restaurant.RestaurantId = restaurantId;
                    restaurant.Menu.Add(data.Skip(2).Select(s => s.Trim()).Aggregate((a, b) => a.Trim() + "," + b.Trim()), decimal.Parse(data[1].Trim()));
                }

            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        static void Main(string[] args)
        {
            var restaurantPicker = new RestaurantPick();
            
            restaurantPicker.ReadRestaurantData(
                Path.GetFullPath(
                    Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, @"../../../../restaurant_data.csv")
                    )
                );

            // Item is found in restaurant 2 at price 6.50
            var bestRestaurant = restaurantPicker.PickBestRestaurant("gac");

            Console.WriteLine(bestRestaurant.Item1 + ", " + bestRestaurant.Item2);

            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        #endregion



        #region You_can_modify_this_region
        /// <summary>
        /// Takes in items you would like to eat and returns the best restaurant that serves them.
        /// </summary>
        /// <param name="items">Items you would like to eat (seperated by ',')</param>
        /// <returns>Restaurant Id and price tuple</returns>
        public Tuple<int, decimal> PickBestRestaurant(string items)
        {   /*
             *
             * Put your solution here
             *
             *
             */

            var restaurentItems = items.Split(',');

            var itemsMatches = new Dictionary<int, decimal>();

            foreach (var restaurant in _restaurants) {
                decimal actualPrice = 0.0M;

                Console.WriteLine(restaurant.RestaurantId);

                var buyedElements = new List<string>();

                foreach (var item in restaurentItems) {
                    Console.WriteLine("Finding price for: " + item);

                    if (buyedElements.Contains(item)) continue;

                    var choosedMeals = new Dictionary<string, decimal>();

                    foreach (var desiredMeal in restaurant.Menu) {
                        if (desiredMeal.Key.Contains(item)) {
                            choosedMeals.Add(desiredMeal.Key, desiredMeal.Value);
                        }
                    }

                    if (choosedMeals.Count > 0) {
                        var goodPrice = choosedMeals.Aggregate((l, r) => l.Value < r.Value ? l : r);

                        Console.WriteLine(goodPrice.Key + ": " + goodPrice.Value);

                        buyedElements.AddRange(goodPrice.Key.Split(","));

                        actualPrice += goodPrice.Value;

                    } else {
                        actualPrice = -1;

                        break;
                    }
                }

                if (actualPrice > -1)  {
                    Console.WriteLine("Price: " + actualPrice);

                    itemsMatches.Add(restaurant.RestaurantId, actualPrice);
                }
            }

            if (itemsMatches.Count() > 0)
            {
                var best_match = itemsMatches.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;

                return new Tuple<int, decimal>(best_match, itemsMatches[best_match]);
            }
            else
            {
                Console.WriteLine("No matches found");
                return null;
            }
        }

        #endregion
    }
}
