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

namespace PROG8050_PROJECT.Views
{
	/// <summary>
	/// Interaction logic for AdminPage.xaml
	/// </summary>
	public partial class AdminPage : Page
	{
		public AdminPage()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			MainWindow AddUser = new MainWindow();
			//AddUser.Show();
		}


		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			MainWindow UpdateUser = new MainWindow();
			//UpdateUser.Show();
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			/*
			try
			{
				IDBService database = Ioc.Default.GetService<IDBService>();
				if (!database.IsOpen)
				{
					System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
					return;
				}

				SQLiteConnection.open();
				string query = "delete from user where email='" + this.email.txt.Text + "'";
				SQLiteCommand createCommand = new SQLiteCommand(query, SQLiteConnection);
				createcommand.ExecuteNonQuery();

				if (MessageBox.Show("Do you want to delet {Name} user?", "Remove User", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				{
					MessageBoxResult result = MessageBox.Show("{Name} user is deleted", "Sucess");
				}
				else
				{
					MessageBoxResult result = MessageBox.Show("{Name} user is not deleted");
				}
				SQLiteConnection.close();
			}
			*/
		}

		private void Search_Click(object sender, RoutedEventArgs e)
		{
			this.NavigationService.Navigate(new Uri("./Views/Modals/AddUser.xaml", UriKind.RelativeOrAbsolute));
		}
	}
}
