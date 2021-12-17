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
    /// Interaction logic for UpdateCustomer.xaml
    /// </summary>
    public partial class UpdateCustomer : Page
    {
        public UpdateCustomer()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection connection = new SQLiteConnection("database.sqlite");
            connection.Open();
            SQLiteCommand comm = new SQLiteCommand("update user set Email=@email,password=@password,Name=@name,Gender=@gender,Phone=@phno where id=uid");
            comm.ExecuteNonQuery();
            MessageBox.Show("Successfully updated");
        }
    }
}
