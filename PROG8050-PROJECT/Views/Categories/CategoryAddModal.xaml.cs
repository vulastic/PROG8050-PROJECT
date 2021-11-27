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
    /// Interaction logic for CategoryAddModal.xaml
    /// </summary>
    public partial class CategoryAddModal : Window
    {
        public CategoryAddModal()
        {
            InitializeComponent();
        }

        private void btnCatCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        } 
        private void btnAddCat_Click(object sender, RoutedEventArgs e)
        {
           
            if (MessageBox.Show($"{cat_name.Text} Category added successfully", "Success", MessageBoxButton.OK) == MessageBoxResult.OK)
            {
                Close();
            }
            
        }
    }
}
