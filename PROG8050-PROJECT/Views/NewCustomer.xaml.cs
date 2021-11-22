using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace PROG8050_PROJECT.View
{
	/// <summary>
	/// Interaction logic for NewCustomer.xaml
	/// </summary>
	public partial class NewCustomer : Page
	{
		public NewCustomer()
		{
			InitializeComponent();
		}

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
			MessageBoxResult result=MessageBox.Show("Do you want to close this window?", "Confirmation", MessageBoxButton.YesNo);
			if (result==MessageBoxResult.Yes)
			{
				this.NavigationService.Navigate(new Uri("./Views/Order.xaml", UriKind.RelativeOrAbsolute));
			} 
			
		}

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
			MessageBoxResult result=MessageBox.Show($"{textBox_Email.Text} customer is Successfully Added!");
            if (result==MessageBoxResult.OK)
            {
				this.NavigationService.Navigate(new Uri("./Views/Order.xaml", UriKind.RelativeOrAbsolute));
			}
		}
    }
}
