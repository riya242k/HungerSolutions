using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HungerSolutions.Restaurant.Models
{
    internal class RestaurantModel
    {
        public string restaurantName { get; set; }
        public int rating { get; set; }
        public string restaurantLocation { get; set; }
        public string phone { get; set; }
        public string imageName { get; set; }
        public int restaurantId { get; set; }
    }

    internal class ReestaurantHomePageModel
    {

        public IEnumerable<RestaurantModel>? restaurantDetails { get; set; }
        public string searchString { get; set; }
    }
}
