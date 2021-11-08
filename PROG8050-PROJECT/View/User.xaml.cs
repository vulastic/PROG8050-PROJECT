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

namespace PROG8050_PROJECT.View
{
	/// <summary>
	/// Interaction logic for User.xaml
	/// </summary>
	public partial class User : Page
	{
		public User()
		{
			InitializeComponent();
		}

		private void button_Purchase_Click(object sender, RoutedEventArgs e)
		{
			User_Purchase page = new User_Purchase();
			NavigationService.Navigate(page);
		}

		private void button_Order_Click(object sender, RoutedEventArgs e)
		{
			User_ManageOrder page = new User_ManageOrder();
			NavigationService.Navigate(page);
		}

		private void button_Profile_Click(object sender, RoutedEventArgs e)
		{
			User_ManageProfile page = new User_ManageProfile();
			NavigationService.Navigate(page);
		}
	}
}
