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

		private void btnLogin_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show($"Hello, {textBox_ID.Text}");
		}

		private void btnNewUser_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show($"Do you want to create new account?");
		}

		private void btnForgot_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show($"Are you sure foget your password?");
		}
	}
}
