using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Npgsql;

namespace _451_mileston3
{
    /// <summary>
    /// Interaction logic for TipWindow.xaml
    /// </summary>
    public partial class TipWindow : Window
    {
        public TipWindow(string uid, string uname, string bid, string bname)
        {
            InitializeComponent();
            User_ID.Text = "Current User: " + uname;
            Business_Name.Text = "Current Business: " + bname;
            this.uid = uid;
            this.bid = bid;
            AddColumns2TipGrid();
            BuildTipGrid();
        }

        public void AddColumns2TipGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Name";
            col1.Binding = new Binding("name");
            col1.Width = 150;
            TipDataGrid.Columns.Add(col1);


            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Business";
            col2.Binding = new Binding("business");
            col2.Width = 100;
            TipDataGrid.Columns.Add(col2);


            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            col3.Width = 60;
            TipDataGrid.Columns.Add(col3);


            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Text";
            col4.Binding = new Binding("text");
            col4.Width = 225;
            TipDataGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "Date";
            col5.Binding = new Binding("date");
            col5.Width = 90;
            TipDataGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Header = "Likes";
            col6.Binding = new Binding("likes");
            col6.Width = 50;
            TipDataGrid.Columns.Add(col6);
        }

        private readonly string uid;
        private readonly string bid;

        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = milestone1db; password = Gr33nj0s001";
        }

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

        private void AddTipGridRow(NpgsqlDataReader reader)
        {
            TipDataGrid.Items.Add(new MainWindow.FriendTip() { name = reader.GetString(0), business = reader.GetString(1), city = reader.GetString(2), text = reader.GetString(3), date = reader.GetTimeStamp(4).ToString() });
        }

        private void AddTipButton_Click(object sender, RoutedEventArgs e)
        {
            string sqlStr = "INSERT INTO tip (business_id, user_id, tip_text, tip_date) VALUES ('" + bid + "', '" + uid + "', '" + TipTextBox.Text + "', '" + DateTime.Now.ToString() + "') ;";
            TipDataGrid.Items.Clear();
            executeQuery(sqlStr, null);
            BuildTipGrid();
        }

        private void BuildTipGrid()
        {
            string sqlStr = "SELECT Userlist.name, Business.name, Business.city, Tip.tip_text, Tip.tip_date, Tip.num_likes " +
                "FROM Userlist, Business, Tip " +
                "WHERE Business.business_id = Tip.business_id AND Business.business_id = '" + bid + "' AND Tip.user_id = Userlist.user_id " +
                "ORDER BY Tip.tip_date ;";
            executeQuery(sqlStr, AddTipGridRow);
        }
    }
}
