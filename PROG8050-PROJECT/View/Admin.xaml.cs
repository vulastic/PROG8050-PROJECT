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
	/// Interaction logic for Admin.xaml
	/// </summary>
	public partial class Admin : Page
	{
		public Admin()
		{
			InitializeComponent();
		}

		private void button_Product_Click(object sender, RoutedEventArgs e)
		{
			Admin_ManageProduct page = new Admin_ManageProduct();
			NavigationService.Navigate(page);
		}

		private void button_Customer_Click(object sender, RoutedEventArgs e)
		{
			Admin_ManageCustomer page = new Admin_ManageCustomer();
			NavigationService.Navigate(page);
		}

		private void button_Order_Click(object sender, RoutedEventArgs e)
		{
			Admin_ManageOrder page = new Admin_ManageOrder();
			NavigationService.Navigate(page);
		}

		private void button_Promotion_Click(object sender, RoutedEventArgs e)
		{
			Admin_ManagePromotion page = new Admin_ManagePromotion();
			NavigationService.Navigate(page);
		}
	}
}
