using HungerSolutions.Database;
using HungerSolutions.Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HungerSolutions.Restaurant.Views
{
    /// <summary>
    /// Interaction logic for RestaurantIndexPage.xaml
    /// </summary>
    public partial class RestaurantIndexPage : Window
    {
        private readonly ReestaurantHomePageModel restaurantHomePageModel;
        private string userEmailId;

        public RestaurantIndexPage(string userEmailId)
        {
            Trace.WriteLine("On restuarant page 1 ---- "+userEmailId);
            InitializeComponent();
            this.restaurantHomePageModel = new ReestaurantHomePageModel();
            WindowOnLoad();
            this.DataContext = this.restaurantHomePageModel;
            this.userEmailId = userEmailId;
        }

        private async void WindowOnLoad()
        {
            Trace.WriteLine("In on load 2 ---- ");
            DatabaseQueries dbQueries = new DatabaseQueries();

            List<RestaurantModel> onLoadSearchResult = await dbQueries.GetRestaurants_All_Or_BySearch("Generic");

            Trace.WriteLine("Back after query, count ---- 3 "+ onLoadSearchResult.Count);
            Trace.WriteLine("Back after query, response ---- 4 " + onLoadSearchResult);

            if (onLoadSearchResult == null || onLoadSearchResult.Count == 0)
            {
                Trace.WriteLine("In if ---- 5 ");
                MessageBox.Show("No results found for restaurants", "No results found for restaurants", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Trace.WriteLine("In else ---- 6 ");
                this.restaurantHomePageModel.restaurantDetails = onLoadSearchResult;
                this.DataContext = null;
                this.DataContext = this.restaurantHomePageModel;
            }
        }
    }
}