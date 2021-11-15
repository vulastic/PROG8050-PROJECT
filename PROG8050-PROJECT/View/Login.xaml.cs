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
	/// Interaction logic for Login.xaml
	/// </summary>
	public partial class Login : Page
	{
		public Login()
		{
			InitializeComponent();
		}

		private void button_Create_Click(object sender, RoutedEventArgs e)
		{
			//NavigationService.Navigate(new Uri("./View/CreateAccount.xaml"), UriKind.Relative);
			CreateAccount create = new CreateAccount();
			NavigationService.Navigate(create);
		}


		private void btnLogin_Click(object sender, RoutedEventArgs e)
		{
			string id = textBox_ID.Text;
			if (id.ToLower() == "admin")
			{
				MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
				mainWindow.ChangeViewNavigation("./View/Admin.xaml");
			}
			else
			{
				MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
				mainWindow.ChangeViewNavigation("./View/User.xaml");
			}
		}

		private void btnNewUser_Click(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine($"Click: {sender}");

			MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
			mainWindow.ChangeViewNavigation("./View/CreateAccount.xaml");
		}

		private void btnForgot_Click(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine($"Click: {sender}");
		}
	}
}
