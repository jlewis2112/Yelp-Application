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
using Npgsql;

namespace _451_mileston3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Business
        {
            //business ID
            public string bid { get; set; }

            //business name
            public string name { get; set; }

            //business state
            public string state { get; set; }

            //business city
            public string city { get; set; }

            //business zipcode
            public int zipcode { get; set; }

            //business zipcode
            public int numTips { get; set; }

            //business Address
            public string address { get; set; }

            //business Distance
            public double distance { get; set; }

            //business rating
            public double rating { get; set; }

            //Number of tips
            public int numCheckIns { get; set; }
        }

        public class Friend
        {
            public string name { get; set; }
            public double rating { get; set; }
            public string date { get; set; }

        }

        public class FriendTip
        {
            public string name { get; set; }
            public string business { get; set; }
            public string text { get; set; }
            public string date { get; set; }
            public string city { get; set; }
            public string likes { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            addState();
            addColumns2BusinessGrid();
            addColumns2FriendsGrid();
            addColumns2FriendsTipGrid();
            addSortComboItems();
        }

        // this is the connection string to the data base. please change the parameters to connect to the needed database
        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = ; password = ";
        }

        //adding all of the state values to the State Lists combo box
        private void addState()
        {
            //very similar sql command adjust if needed. Should be the same but different names.
            string sqlStr = "SELECT distinct state FROM business ORDER BY state";
            executeQuery(sqlStr, addState);
        }

        //add the sort by strings to the combo box
        public void addSortComboItems()
        {
            SortByComboBox.Items.Add("name default");
            SortByComboBox.Items.Add("highest rated");
            SortByComboBox.Items.Add("most number of tips");
            SortByComboBox.Items.Add("most Checkins");
            SortByComboBox.Items.Add("nearest");
            SortByComboBox.SelectedIndex = 0;
        }

        //adding columns to every grid
        public void addColumns2BusinessGrid()
        {
            
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "BusinessName";
            col1.Binding = new Binding("name");
            col1.Width = 150;
            BusinessGrid.Columns.Add(col1);

            
            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Address";
            col2.Binding = new Binding("address");
            col2.Width = 175;
            BusinessGrid.Columns.Add(col2);

            
            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            col3.Width = 100;
            BusinessGrid.Columns.Add(col3);

            
            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "State";
            col4.Binding = new Binding("state");
            col4.Width = 50;
            BusinessGrid.Columns.Add(col4);

            
            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "Distance(miles)";
            col5.Binding = new Binding("distance");
            col5.Width = 90;
            BusinessGrid.Columns.Add(col5);

            
            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Header = "Stars";
            col6.Binding = new Binding("rating");
            col6.Width = 50;
            BusinessGrid.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Header = "# of Tips";
            col7.Binding = new Binding("numTips");
            col7.Width = 75;
            BusinessGrid.Columns.Add(col7);

            DataGridTextColumn col8 = new DataGridTextColumn();
            col8.Header = "Total Checkins";
            col8.Binding = new Binding("numCheckIns");
            col8.Width = 90;
            BusinessGrid.Columns.Add(col8);
        }

        public void addColumns2FriendsGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Name";
            col1.Binding = new Binding("name");
            col1.Width = 150;
            FriendDataGrid.Columns.Add(col1);


            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Ave Stars";
            col2.Binding = new Binding("rating");
            col2.Width = 75;
            FriendDataGrid.Columns.Add(col2);


            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "Yelping Since";
            col3.Binding = new Binding("date");
            col3.Width = 200;
            FriendDataGrid.Columns.Add(col3);
        }

        public void addColumns2FriendsTipGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Name";
            col1.Binding = new Binding("name");
            col1.Width = 150;
            FriendTipDataGrid.Columns.Add(col1);


            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Business";
            col2.Binding = new Binding("business");
            col2.Width = 100;
            FriendTipDataGrid.Columns.Add(col2);


            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            col3.Width = 60;
            FriendTipDataGrid.Columns.Add(col3);


            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Text";
            col4.Binding = new Binding("text");
            col4.Width = 225;
            FriendTipDataGrid.Columns.Add(col4);


            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "Date";
            col5.Binding = new Binding("date");
            col5.Width = 90;
            FriendTipDataGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Header = "Likes";
            col6.Binding = new Binding("likes");
            col6.Width = 50;
            FriendTipDataGrid.Columns.Add(col6);
        }

        //excutes a sql command and then executes the needed function
        private void executeQuery(string sqlstr, Action<NpgsqlDataReader> myf)
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sqlstr;
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (myf != null)
                            {
                                myf(reader);
                            }
                        }
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        //adding a user ID to the list box
        private void addUserID(NpgsqlDataReader reader)
        {
            UserIDListBox.Items.Add(reader.GetString(0));
        }

        //This adds a states from the sql data reader to the combo box states
        private void addState(NpgsqlDataReader reader)
        {
            StateComboBox.Items.Add(reader.GetString(0));
        }

        //This adds cities from the sql data reader to the list box cities.
        private void addCity(NpgsqlDataReader reader)
        {
            CityListBox.Items.Add(reader.GetString(0));
        }

        //zip codes to the list box
        private void addZipcode(NpgsqlDataReader reader)
        {
            ZipCodeListBox.Items.Add(reader.GetInt32(0));
        }

        //This adds categories from the sql data reader to the categories list box
        private void addCategory(NpgsqlDataReader reader)
        {
            CategoryListBox.Items.Add(reader.GetString(0));
        }

        //populates the business once a selection is made
        private void addBusinessGridRow(NpgsqlDataReader reader)
        {
            //This will need to get changed due to indexing. find the needed information from the sql table index
            BusinessGrid.Items.Add(new Business() { name = reader.GetString(0), address = reader.GetString(1), city = reader.GetString(2), state = reader.GetString(3), numTips = reader.GetInt32(4), numCheckIns = reader.GetInt32(5), bid = reader.GetString(6), rating = reader.GetDouble(7), distance = reader.GetDouble(8)});
        }

        private void addSelectedCategoryRow(string category)
        {
            //This will need to get changed due to indexing. find the needed information from the sql table index
            StoredCategoryListBox.Items.Add(category);
        }

        private void addBusinessCategoryRow(NpgsqlDataReader reader)
        {
            string category;
            category = reader.GetString(0);
            BusinessCategoriesAttributes.Items.Add("    " + category);
        }

        private void addBusinessAttributeRow(NpgsqlDataReader reader)
        {
            string attribute, value;
            attribute = reader.GetString(0);
            value = reader.GetString(1);

            if (value.CompareTo("False") != 0)
            {

                if (value.CompareTo("True") != 0)
                {
                    BusinessCategoriesAttributes.Items.Add("    " + attribute + " : " + value);
                }

                else
                {
                    BusinessCategoriesAttributes.Items.Add("    " + attribute);
                }
            }
        }
        ////populates the freind list grid
        private void addFriendGridRow(NpgsqlDataReader reader)
        {
            //This will need to get changed due to indexing. find the needed information from the sql table index
            FriendDataGrid.Items.Add(new Friend() { name = reader.GetString(0), rating = reader.GetDouble(1), date = reader.GetDate(2).ToString()});
        }

        private void addFriendTipGridRow(NpgsqlDataReader reader)
        {
            //This will need to get changed due to indexing. find the needed information from the sql table index
            FriendTipDataGrid.Items.Add(new FriendTip() { name = reader.GetString(0), business = reader.GetString(1), city = reader.GetString(2), text = reader.GetString(3), date = reader.GetTimeStamp(4).ToString(), likes = reader.GetInt16(5).ToString() });
        }

        private void addUserInformation(NpgsqlDataReader reader)
        {
            NameTextBox.Text = reader.GetString(0);
            StarsTextBox.Text = reader.GetDouble(1).ToString();
            FansTextBox.Text = reader.GetInt32(2).ToString();
            YelpingSinceTextBox.Text = reader.GetDate(3).ToString();
            FunnyTextBox.Text = reader.GetInt32(4).ToString();
            CoolTextBox.Text = reader.GetInt32(5).ToString();
            HelpfullTextBox.Text = reader.GetInt32(6).ToString();
            TipCountTextBox.Text = reader.GetInt32(7).ToString();
            TotalTipLikesTextBox.Text = reader.GetInt32(8).ToString();
            LatTextBox.Text = reader.GetDouble(9).ToString();
            LongTextBox.Text = reader.GetDouble(10).ToString();
        }


        //event handlers////

        private void SearchUserbutton_Click(object sender, RoutedEventArgs e)
        {
            string sqlStr = "SELECT user_id FROM userlist WHERE name = '" + UserTextBox.Text + "' ORDER BY user_id ;";
            UserIDListBox.Items.Clear();
            executeQuery(sqlStr, addUserID);
        }

        private void UserIDListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LatTextBox.IsReadOnly = true;
            LongTextBox.IsReadOnly = true;
            FriendTipDataGrid.Items.Clear();
            FriendDataGrid.Items.Clear();
            //get friends
            string sqlStr = "SELECT name, average_stars, join_date FROM Userlist, friends_with " +
                "WHERE  friend1 = '" + UserIDListBox.SelectedItem.ToString() + "' AND friend2 = user_id " +
                "OR friend1 = user_id AND friend2 = '" + UserIDListBox.SelectedItem.ToString() + "' " +
                "ORDER BY name;";
            executeQuery(sqlStr, addFriendGridRow);

            //get userinformation
            sqlStr = "SELECT name, average_stars, num_fans, join_date, is_funny, is_cool, is_useful, num_tips, num_likes," +
                "latitude, longitude FROM UserList WHERE user_id = '" + UserIDListBox.SelectedItem.ToString() + "' ;";
            executeQuery(sqlStr, addUserInformation);

            //query friends latest tips
            sqlStr = "SELECT Userlist.name, Business.name, Business.city, Tip.tip_text, Tip.tip_date, Tip.num_likes " +
                "FROM Userlist, Business, Tip, friends_with " +
                "WHERE Userlist.user_id = friends_with.friend2 AND friends_with.friend1 = '" + UserIDListBox.SelectedItem.ToString() + "' AND friends_with.friend2 = Tip.user_id AND Business.business_id = Tip.business_id " +
                "OR Userlist.user_id = friends_with.friend1 AND friends_with.friend1 = Tip.user_id  AND friends_with.friend2 = '" + UserIDListBox.SelectedItem.ToString() + "' AND Business.business_id = Tip.business_id " +
                "ORDER BY tip_date ;";
            executeQuery(sqlStr, addFriendTipGridRow);
        }

        private void StateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //clear needed items
            CityListBox.Items.Clear();
            ZipCodeListBox.Items.Clear();
            CategoryListBox.Items.Clear();

            if (StateComboBox.SelectedIndex > -1)
            {
                //use the sql command to get the cities from a state.
                string sqlStr = "SELECT distinct city FROM business WHERE state = '" + StateComboBox.SelectedItem.ToString() + "' ORDER BY city";
                executeQuery(sqlStr, addCity);
            }
        }

        private void CityListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //clear needed items
            ZipCodeListBox.Items.Clear();
            CategoryListBox.Items.Clear();

            if (CityListBox.SelectedIndex > -1)
            {
                //sql command to get the zip codes may have to get changed
                string sqlStr = "SELECT distinct zipcode FROM business WHERE state = '" + StateComboBox.SelectedItem.ToString() + "' AND city = '" + CityListBox.SelectedItem.ToString() + "'ORDER BY zipcode";
                executeQuery(sqlStr, addZipcode);
            }
        }

        private void ZipCodeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BusinessGrid.Items.Clear();
            CategoryListBox.Items.Clear();

            if (ZipCodeListBox.SelectedIndex > -1)
            {
                //get each of the businesses values by zipcode might need to be changed
                string sqlStr = generateBusinessFilteredSearch();
                executeQuery(sqlStr, addBusinessGridRow);

                sqlStr = "SELECT DISTINCT category_name FROM HasCategories WHERE business_id IN (SELECT business_id " +
                            "FROM Business WHERE zipcode = '" + ZipCodeListBox.SelectedItem.ToString() + "') ORDER BY category_name;";
                executeQuery(sqlStr, addCategory);
            }
        }

        private void UpdateUserbutton_Click(object sender, RoutedEventArgs e)
        {
            string sqlStr;
            if (!LatTextBox.IsReadOnly && !LongTextBox.IsReadOnly)
            {
                sqlStr = "UPDATE UserList SET latitude = " + LatTextBox.Text + ", longitude = " + LongTextBox.Text + " WHERE user_id = '" + UserIDListBox.SelectedItem.ToString() + "'";
                executeQuery(sqlStr, null);
                LatTextBox.IsReadOnly = true;
                LongTextBox.IsReadOnly = true;
            }
        }

        private void EditUserbutton_Click(object sender, RoutedEventArgs e)
        {
            LatTextBox.IsReadOnly = false;
            LongTextBox.IsReadOnly = false;
        }

        private void TipButton_Click(object sender, RoutedEventArgs e)
        {
            Business selectedBusiness = BusinessGrid.SelectedItem as Business;


            string uid = UserIDListBox.SelectedItem.ToString();
            string uname = UserTextBox.Text;
            string bid = selectedBusiness.bid;
            string bname = selectedBusiness.name;
            TipWindow tipWindow = new TipWindow(uid, uname, bid, bname);
            tipWindow.Show();
        }

        private void CategoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            string sqlStr = "";
            BusinessGrid.Items.Clear();
            if (CategoryListBox.SelectedIndex > -1)
            {
                if(StoredCategoryListBox.Items.Contains(CategoryListBox.SelectedItem.ToString()) == false)
                {
                    addSelectedCategoryRow(CategoryListBox.SelectedItem.ToString());
                }

                sqlStr = generateBusinessFilteredSearch();
                executeQuery(sqlStr, addBusinessGridRow);
            }
        }

        private void RemoveCategory_Click(object sender, RoutedEventArgs e)
        {
            string sqlStr = "";
            if (StoredCategoryListBox.SelectedIndex > -1)
            {
                BusinessGrid.Items.Clear();
                StoredCategoryListBox.Items.RemoveAt(StoredCategoryListBox.SelectedIndex);

                sqlStr = generateBusinessFilteredSearch();
                executeQuery(sqlStr, addBusinessGridRow);
            }
        }


        private string generateBusinessFilteredSearch()
        {
            string sqlStr = "", category_str = "", price_str = "", sort_by_str = "";
            
            // Make sure returned businesses are part of ALL selected categories
            for (int i = 0; i < StoredCategoryListBox.Items.Count; i++)
            {
                string item = StoredCategoryListBox.Items[i] as string;
                category_str += " AND business_id IN (SELECT business_id FROM HasCategories WHERE category_name = '" + item + "')";
            }

            switch(SortByComboBox.SelectedIndex)
            {
                case 0: // name default
                    sort_by_str = "ORDER BY name";
                    break;
                case 1: // highest rated
                    sort_by_str = "ORDER BY stars desc";
                    break;
                case 2: // most number of tips
                    sort_by_str = "ORDER BY num_tips desc";
                    break;
                case 3: // most Checkins
                    sort_by_str = "ORDER BY num_checkins desc";
                    break;
                case 4: // nearest
                    sort_by_str = "ORDER BY distance";
                    break;
                default:
                    break;
            }
            
            if(LowPriceCheckBox.IsChecked == true)
            {
                price_str = "AND (business_id IN (SELECT business_id FROM HasAttribute WHERE attribute_name = 'RestaurantsPriceRange2' AND value = '1') ";
            }
            if (MidPriceCheckBox.IsChecked == true)
            {
                if(price_str.CompareTo("") == 0)
                {
                    price_str = "AND (";
                }
                else
                {
                    price_str += "OR ";
                }
                price_str += "business_id IN (SELECT business_id FROM HasAttribute WHERE attribute_name = 'RestaurantsPriceRange2' AND value = '2') ";
            }
            if (HighPriceCheckBox.IsChecked == true)
            {
                if (price_str.CompareTo("") == 0)
                {
                    price_str = "AND (";
                }
                else
                {
                    price_str += "OR ";
                }
                price_str += "business_id IN (SELECT business_id FROM HasAttribute WHERE attribute_name = 'RestaurantsPriceRange2' AND value = '3') ";
            }
            if (ExpensiveCheckBox.IsChecked == true)
            {
                if (price_str.CompareTo("") == 0)
                {
                    price_str = "AND (";
                }
                else
                {
                    price_str += "OR ";
                }
                price_str += "business_id IN (SELECT business_id FROM HasAttribute WHERE attribute_name = 'RestaurantsPriceRange2' AND value = '4') ";
            }

            if (price_str.CompareTo("") != 0)
            {
                price_str += ") ";
            }

            //select businesses based of the filtered category. command might need to be changed
            sqlStr = "SELECT distinct name, address, city, state, num_tips, num_checkins, business_id, stars, " +
                "myDistance(" + LatTextBox.Text.ToString() + ", " + LongTextBox.Text.ToString() + ", latitude , longitude) " +
                "FROM business WHERE zipcode = '" + ZipCodeListBox.SelectedItem.ToString() + "'" + category_str + " " + price_str + sort_by_str + ";";


            return sqlStr;
        }


        private void Business_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Business selectedBusiness;
            string sqlStr;
            if (BusinessGrid.SelectedIndex > -1)
            {
                BusinessCategoriesAttributes.Items.Clear();
                selectedBusiness = BusinessGrid.SelectedItem as Business;        
                BusinessNameTextBox.Text = selectedBusiness.name;
                AddressTextBox.Text = selectedBusiness.address;

                BusinessCategoriesAttributes.Items.Add("Categories:");

                sqlStr = "SELECT distinct category_name  " +
                "FROM HasCategories WHERE business_id = '" + selectedBusiness.bid.ToString() + "'" + ";";

                executeQuery(sqlStr, addBusinessCategoryRow);

                BusinessCategoriesAttributes.Items.Add("Attributes:");

                sqlStr = "SELECT distinct attribute_name, value  " +
                "FROM HasAttribute WHERE business_id = '" + selectedBusiness.bid.ToString() + "'" + ";";

                executeQuery(sqlStr, addBusinessAttributeRow);
            }
                
        }

        private void LowPriceCheckClicked(object sender, RoutedEventArgs e)
        {
            BusinessGrid.Items.Clear();
            string sqlStr = generateBusinessFilteredSearch();
            executeQuery(sqlStr, addBusinessGridRow);
        }

        private void MidPriceCheckClicked(object sender, RoutedEventArgs e)
        {
            BusinessGrid.Items.Clear();
            string sqlStr = generateBusinessFilteredSearch();
            executeQuery(sqlStr, addBusinessGridRow);
        }

        private void HighPriceCheckClicked(object sender, RoutedEventArgs e)
        {
            BusinessGrid.Items.Clear();
            string sqlStr = generateBusinessFilteredSearch();
            executeQuery(sqlStr, addBusinessGridRow);
        }

        private void ExpensivePriceCheckClicked(object sender, RoutedEventArgs e)
        {
            BusinessGrid.Items.Clear();
            string sqlStr = generateBusinessFilteredSearch();
            executeQuery(sqlStr, addBusinessGridRow);
        }
    }
}
