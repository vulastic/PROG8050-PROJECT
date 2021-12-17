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
    /// Interaction logic for UpdateUser.xaml
    /// </summary>
    public partial class UpdateUser : Page
    {
        public UpdateUser()
        {
            InitializeComponent();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection connection = new SQLiteConnection("database.sqlite");
            connection.Open();
            SQLiteCommand comm = new SQLiteCommand("update customer set Email=@email,Name=@name,Gender=@gender,Phone=@phno where id=uid");
            comm.ExecuteNonQuery();
            MessageBox.Show("Successfully Updated");

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
