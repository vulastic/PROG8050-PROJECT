using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PROG8050_PROJECT.Views
{
    /// <summary>
    /// Interaction logic for AddUser.xaml
    /// </summary>
    public partial class AddUser : Page
    {
        public AddUser()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
			/*
            SQLiteConnection connection = new SQLiteConnection("database.sqlite");
            connection.Open();
            string query = string.Format("INSERT INTO 'user'('Email','Password','Name','Gender','Phone') VALUES ('" + txtemail + "','" + txtpassword + "','" + txtname + "','" + gender + "','" + txtphno + "')");
            MessageBox.Show("Successfully Added");
            */
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            txtemail.Clear();
            txtname.Clear();
            txtpassword.Clear();
            txtconfirm.Clear();
            txtphno.Clear();
        }
    }
    
}
