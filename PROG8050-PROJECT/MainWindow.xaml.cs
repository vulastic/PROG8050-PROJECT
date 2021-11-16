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

namespace PROG8050_PROJECT
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		public void ChangeViewNavigation(string viewPath)
		{
			PagesNavigation.Navigate(new System.Uri(viewPath, UriKind.RelativeOrAbsolute));
		}

		// Close Window
		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		// Maximize | Restore Window
		private void btnRestore_Click(object sender, RoutedEventArgs e)
		{
			if (WindowState == WindowState.Normal)
			{
				WindowState = WindowState.Maximized;
			}
			else
			{
				WindowState = WindowState.Normal;
			}
		}

		// Minimize Window
		private void btnMinimize_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}

		// Maximize Window | Drag Window Position
		private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount == 1)
			{
				this.DragMove();
				e.Handled = true;
			}
			else if (e.ClickCount == 2)
			{
				if (WindowState == WindowState.Normal)
				{
					WindowState = WindowState.Maximized;
				}
				else
				{
					WindowState = WindowState.Normal;
				}
			}
		}

		private void btnHome_Click(object sender, RoutedEventArgs e)
		{
			PagesNavigation.Navigate(new System.Uri("./Views/Home.xaml", UriKind.RelativeOrAbsolute));
		}

		private void btnProduct_Click(object sender, RoutedEventArgs e)
		{
			PagesNavigation.Navigate(new System.Uri("./Views/Product.xaml", UriKind.RelativeOrAbsolute));
		}

		private void btnCategory_Click(object sender, RoutedEventArgs e)
		{
			PagesNavigation.Navigate(new System.Uri("./Views/Category.xaml", UriKind.RelativeOrAbsolute));
		}

		private void btnPromotion_Click(object sender, RoutedEventArgs e)
		{
			PagesNavigation.Navigate(new System.Uri("./Views/Promotion.xaml", UriKind.RelativeOrAbsolute));
		}

		private void btnCustomer_Click(object sender, RoutedEventArgs e)
		{
			PagesNavigation.Navigate(new System.Uri("./Views/Customer.xaml", UriKind.RelativeOrAbsolute));
		}

		private void btnOrder_Click(object sender, RoutedEventArgs e)
		{
			PagesNavigation.Navigate(new System.Uri("./Views/Order.xaml", UriKind.RelativeOrAbsolute));
		}

		private void btnUser_Click(object sender, RoutedEventArgs e)
		{
			PagesNavigation.Navigate(new System.Uri("./Views/User.xaml", UriKind.RelativeOrAbsolute));
		}
	}
}
