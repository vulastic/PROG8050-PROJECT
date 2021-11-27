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
	/// Interaction logic for UpdateCustomer.xaml
	/// </summary>
	public partial class UpdateCustomer : Page
	{
		public UpdateCustomer()
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

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
			MessageBoxResult result=MessageBox.Show($"Customer details updated successfully!");
            if (result==MessageBoxResult.OK)
            {
				this.NavigationService.Navigate(new Uri("./Views/Order.xaml", UriKind.RelativeOrAbsolute));
			}
		}
    }
}
