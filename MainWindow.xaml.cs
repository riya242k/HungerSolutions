using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using HungerSolutions.Database;
using HungerSolutions.Profile.Models;
using HungerSolutions.Restaurant.Views;
using HungerSolutions.SharedFiles;

namespace HungerSolutions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // If user clicks on login button
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            //get inputs
            string emailLogin = EmailAddressLogin.Text;
            string passwordLogin = PasswordLogin.Password;
            bool errorFlag = false;
            string errors ="";

            // -- validate inputs

            //check email
            if (!FieldValidators.IsMailValid(emailLogin))
            {
                errors = "Invalid Email";
                errorFlag = true;
            }

            //check password
            if (String.IsNullOrEmpty(passwordLogin))
            {
                errors += "\nEmpty Password";
                errorFlag = true;
            }

            // show errors on UI
            if (errorFlag)
            {
                MessageBox.Show(errors);
            }
            else
            {
                UserModel userLoginInput = new UserModel()
                {
                    emailAddress = emailLogin,
                    password = passwordLogin
                };

                DatabaseQueries dbQueries = new DatabaseQueries();
                bool validCredentials = dbQueries.ValidateLoginCredentials(userLoginInput);

                if (validCredentials)
                {
                    RestaurantIndexPage restaurantHomepage = new RestaurantIndexPage(userLoginInput.emailAddress);
                    this.Visibility = Visibility.Hidden;
                    restaurantHomepage.Show();
                }
                else
                {
                    MessageBox.Show("Invalid Credentials");
                }
            }
        }
    }
}
