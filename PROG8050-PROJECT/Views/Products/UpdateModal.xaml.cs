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
    /// Interaction logic for UpdateModal.xaml
    /// </summary>
    public partial class UpdateModal : Window
    {
        public UpdateModal()
        {
            InitializeComponent();
        }

        private void btnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"{updateProduct_txt.Text} Product is succesfully Updated", "Success",
            MessageBoxButton.OK) == MessageBoxResult.OK)
            {
                Close();
            }
        }

        private void btnUpdateAddCat_Click(object sender, RoutedEventArgs e)
        {
            CategoryAddModal addcat = new CategoryAddModal();
            addcat.ShowDialog();
        }
    }
}
