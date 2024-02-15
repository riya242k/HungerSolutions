using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;
using HungerSolutions.MenuItem.Models;
using System.Diagnostics;
using HungerSolutions.Restaurant.Models;
using HungerSolutions.Profile.Models;
using System.Data;
using System.Numerics;

namespace HungerSolutions.Database
{
    internal class DatabaseQueries : DbConnection
    {
        SqlConnection connection;

        public DatabaseQueries()
        {
            connection = new SqlConnection(dbConnectionString);
        }

        // checks userAccount Table for matching email and password
        public bool ValidateLoginCredentials(UserModel pUserModel)
        {
            int matchCount = 0; //init
            string selectQuery = $"Select Count(*) FROM UserAccount ac WHERE ac.email = '{pUserModel.emailAddress}' AND ac.passphrase = '{pUserModel.password}'";

            connection.Open();
            SqlCommand cmd = new SqlCommand(selectQuery, connection);
            matchCount = (int)cmd.ExecuteScalar();
            connection.Close();

            return (matchCount == 0 ? false : true);


        }

        // get complete user details based on email id
        public UserModel GetUserAccountDetails(string pEmailId)
        {
            UserModel userAccount = new UserModel();
            try
            {
                string selectQuery = $"Select * FROM UserAccount ac WHERE ac.email = '{pEmailId}'";

                connection.Open();
                SqlCommand cmd = new SqlCommand(selectQuery, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    userAccount.userAccountID = reader.GetInt32(reader.GetOrdinal("UserAccountID"));
                    userAccount.username = reader.GetString(reader.GetOrdinal("userName"));
                    userAccount.emailAddress = reader.GetString(reader.GetOrdinal("email"));
                    userAccount.phone = reader.GetString(reader.GetOrdinal("phone"));
                    userAccount.password = reader.GetString(reader.GetOrdinal("passphrase"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return userAccount;
        }

        // update user's email, name and phone number as requested
        public async Task<bool> UpdateUserNameAndContactDetails(UserModel pUserModel, string pEmailID)
        {
            try
            {
                connection.Open();

                string updateQuery = $"UPDATE UserAccount SET userName='{pUserModel.username}', email='{pUserModel.emailAddress}',phone='{pUserModel.phone}' where email='{pEmailID}'";

                SqlCommand cmd = new SqlCommand(updateQuery, connection);

                int rowAffected = (int)await cmd.ExecuteNonQueryAsync();
                connection.Close();

                ////Updated
                return (rowAffected > 0);
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        // get complete detail of all restaurants in DB
        public async Task<List<RestaurantModel>> GetRestaurants_All_Or_BySearch(string pSearchString)
        {

            List<RestaurantModel> restaurants = new List<RestaurantModel>();
            SqlCommand cmd;

            if (String.IsNullOrEmpty(pSearchString) || pSearchString == "Generic")
            {
                string searchStringGeneric = $"SELECT * from Restaurant";
                Debug.WriteLine("SQL SEARCH STRING:" + searchStringGeneric);

                cmd = new SqlCommand(searchStringGeneric, connection);
                // Debug.WriteLine("SQL SEARCH STRING IF:" + searchStringGeneric);
            }
            else
            {
                string selectQueryBasedOnSearchString = $"SELECT * from Restaurant r where r.restaurantName LIKE '%{pSearchString}%'";
                cmd = new SqlCommand(selectQueryBasedOnSearchString, connection);
                 Debug.WriteLine("SQL SEARCH STRING ELSE:" + selectQueryBasedOnSearchString);
            }
            try
            {
                await connection.OpenAsync();
                cmd.CommandTimeout = 100000;

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader == null)
                    {
                        throw new Exception("Something went wrong while performing search. Contact your administrator.");
                    }
                    else
                    {
                        while (await reader.ReadAsync())
                        {
                            RestaurantModel restaurant = new RestaurantModel();
                            restaurant.restaurantName = reader.GetString(reader.GetOrdinal("restaurantName"));
                            restaurant.rating = reader.GetInt32(reader.GetOrdinal("rating"));
                            restaurant.restaurantLocation = reader.GetString(reader.GetOrdinal("restaurantLocation"));
                            restaurant.phone = reader.GetString(reader.GetOrdinal("phone"));
                            restaurant.imageName = "/SharedFiles/Images/Restaurant_Images/"+reader.GetString(reader.GetOrdinal("imageName"));
                            restaurant.restaurantId = reader.GetInt32(reader.GetOrdinal("RestaurantID"));

                            Trace.WriteLine("Inside the query method, image ---- 1.1 "+ restaurant.imageName);
                            Trace.WriteLine("Inside the query method, name ---- 1.1 " + restaurant.restaurantName);
                            Trace.WriteLine("Inside the query method, id ---- 1.1 " + restaurant.restaurantId);
                            Trace.WriteLine("Inside the query method, rating ---- 1.1 " + restaurant.rating);
                            restaurants.Add(restaurant);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return restaurants;
        }

        // get complete detail of all menu items in DB
        public async Task<List<MenuItemModel>> GetAllmenuItems()
        {

            List<MenuItemModel> menuItems = new List<MenuItemModel>();

            try
            {
                string selectQuery = $"SELECT * from MenuItem";
                Debug.WriteLine("SQL SEARCH STRING:" + selectQuery);

                await connection.OpenAsync();

                SqlCommand cmd = new SqlCommand(selectQuery, connection);

                cmd.CommandTimeout = 100000;

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader == null)
                    {
                        throw new Exception("Something went wrong while performing search. Contact your administrator.");
                    }
                    else
                    {
                        while (await reader.ReadAsync())
                        {
                            MenuItemModel menuItem = new MenuItemModel();
                            menuItem.menuItemName = reader.GetString(reader.GetOrdinal("itemName"));
                            menuItem.description = reader.GetString(reader.GetOrdinal("smallDescription"));
                            menuItem.category = reader.GetString(reader.GetOrdinal("category"));
                            menuItem.price = reader.GetDecimal(reader.GetOrdinal("price"));
                            menuItem.imageName = reader.GetString(reader.GetOrdinal("imageName"));
                            menuItem.restaurantId = reader.GetInt32(reader.GetOrdinal("restaurantID"));

                            menuItems.Add(menuItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return menuItems;
        }

        // get complete detail of all menu items belonging to given restaurant
        public async Task<List<MenuItemModel>> GetMenuItemsFromRestaurantId(int pRestaurantID)
        {

            List<MenuItemModel> menuItems = new List<MenuItemModel>();

            try
            {
                string selectQuery = $"SELECT * from MenuItem MI where MI.restaurantID = {pRestaurantID}";
                Debug.WriteLine("SQL SEARCH STRING:" + selectQuery);

                await connection.OpenAsync();

                SqlCommand cmd = new SqlCommand(selectQuery, connection);

                cmd.CommandTimeout = 100000;

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader == null)
                    {
                        throw new Exception("Something went wrong while performing search. Contact your administrator.");
                    }
                    else
                    {
                        while (await reader.ReadAsync())
                        {
                            MenuItemModel menuItem = new MenuItemModel();
                            menuItem.menuItemName = reader.GetString(reader.GetOrdinal("itemName"));
                            menuItem.description = reader.GetString(reader.GetOrdinal("smallDescription"));
                            menuItem.category = reader.GetString(reader.GetOrdinal("category"));
                            menuItem.price = reader.GetDecimal(reader.GetOrdinal("price"));
                            menuItem.imageName = reader.GetString(reader.GetOrdinal("imageName"));
                            menuItem.restaurantId = reader.GetInt32(reader.GetOrdinal("restaurantID"));

                            menuItems.Add(menuItem);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return menuItems;
        }
    }
}
