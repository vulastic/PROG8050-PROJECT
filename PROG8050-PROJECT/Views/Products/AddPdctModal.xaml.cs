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
    /// Interaction logic for AddPdctModal.xaml
    /// </summary>
    public partial class AddPdctModal : Window
    {
      
       

        public AddPdctModal()
        {
            InitializeComponent();
        }
      
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
           
            if (MessageBox.Show($"{Product_txt.Text} Product is succesfully inserted", "Success",
            MessageBoxButton.OK) == MessageBoxResult.OK)
            {
                Close();
            }

        }

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            CategoryAddModal addcat = new CategoryAddModal();
            addcat.ShowDialog();
        }
    }
}
