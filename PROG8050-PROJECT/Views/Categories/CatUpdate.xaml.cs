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
using System.Windows.Shapes;

namespace PROG8050_PROJECT
{
    /// <summary>
    /// Interaction logic for CatUpdate.xaml
    /// </summary>
    public partial class CatUpdate : Window
    {
        public CatUpdate()
        {
            InitializeComponent();
        }

        private void catbtnUpdate_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show($"{cat_name.Text} Category is succesfully Updated", "Success",
            MessageBoxButton.OK) == MessageBoxResult.OK)
            {
                Close();
            }
        }

        private void btnUpdateCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
