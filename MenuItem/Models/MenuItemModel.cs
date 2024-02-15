using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HungerSolutions.MenuItem.Models
{
    internal class MenuItemModel
    {
        public string menuItemName { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public decimal price { get; set; }
        public string imageName { get; set; }
        public int restaurantId { get; set; }
    }
}
