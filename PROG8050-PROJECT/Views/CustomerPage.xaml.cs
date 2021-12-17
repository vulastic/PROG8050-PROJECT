using Microsoft.Toolkit.Mvvm.DependencyInjection;
using PROG8050_PROJECT.Core.Services;
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
	/// Interaction logic for CustomerPage.xaml
	/// </summary>
	public partial class CustomerPage : Page
	{
		public CustomerPage()
		{
			InitializeComponent();
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			MainWindow AddCustomer = new MainWindow();
			//AddCustomer.Show();
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			MainWindow UpdateCustomer = new MainWindow();
			//UpdateCustomer.Show();

		}

		private void Datagriduser_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			//string connectionPath = @"data source = C:\Users\Admin\Documents\GitHub\PROG8050 - PROJECT\PROG8050 - PROJECT\database.sqlite";
			//using (SQLiteConnection connection = new SQLiteConnection(connectionPath))
			//{
			//SQLiteCommand command = connection.CreateCommand();
			//connection.Open();
			//string query = "select Email as 'Email', Name as 'Name', Gender as 'Gender', Phone as 'Phone Number' from user";
			//command.CommandText = query;
			//command.ExecuteNonQuery();
			//SQLiteDataAdapter da = new SQLiteDataAdapter(command);
			//DataSet ds = new DataSet();
			//da.Fill(ds, "user");
			//int c = ds.Tables["user"].Rows.count;
			//tblCustomerDetails.DataSource = ds.Tables["user"];
			//tblCustomerDetails.Sort(tblCustomerDetails.Columns["Email"], ListSortDirection.Ascending);
			//tblCustomerDetails.Readonly = true;
			//connection.Close();
			//}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			IDBService database = Ioc.Default.GetService<IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			//SQLiteCommand comm = new SQLiteCommand("select from customer set Email=@email,password=@password,Name=@name,Gender=@gender,Phone=@phno where Name=@name");
			//comm.ExecuteNonQuery();
			MessageBox.Show("Successfully updated");

		}
	}
}
