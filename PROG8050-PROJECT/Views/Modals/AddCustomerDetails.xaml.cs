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
using System.Data.SQLite;
namespace PROG8050_PROJECT
{
    /// <summary>
    /// Interaction logic for AddCustomerDetails.xaml
    /// </summary>
    public partial class AddCustomerDetails : Page
    {
        public AddCustomerDetails()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection connection = new SQLiteConnection("database.sqlite");
            connection.Open();
            string query = string.Format("INSERT INTO 'user'('Email','Name','Gender','Phone') VALUES ('" + txtemail + "','" + txtname + "','" + gender + "','" + txtphno + "')");
            MessageBox.Show("Successfully Added");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            txtemail.Clear();
            txtname.Clear();
            txtphno.Clear();
        }
    }
}
