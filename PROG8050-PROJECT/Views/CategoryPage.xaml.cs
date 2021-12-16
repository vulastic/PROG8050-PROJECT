using Microsoft.Toolkit.Mvvm.DependencyInjection;
using PROG8050_PROJECT.Core.Services;
using PROG8050_PROJECT.Models;
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
	/// Interaction logic for CategoryPage.xaml
	/// </summary>
	public partial class CategoryPage : Page
	{
		public CategoryPage()
		{
			InitializeComponent();
			FillDataGrid();
		}

		private void Button_Add_CategoryElement_Click(object sender, RoutedEventArgs e)
		{
			AddCategoryElementInputBox.Visibility = System.Windows.Visibility.Visible;
		}

		private void Button_Edit_CategoryElement_Click(object sender, RoutedEventArgs e)
		{
			if (this.categoryDataGrid.SelectedItem == null)
			{
				return;
			}

			Category selected = this.categoryDataGrid.SelectedItem as Category;

			InputEditCategoryIdBox.Text = selected.Id.ToString();
			InputEditCategoryBox.Text = selected.Name;
			EditCategoryElementInputBox.Visibility = System.Windows.Visibility.Visible;
		}

		private void Button_Close_CategoryElement_Click(object sender, RoutedEventArgs e)
		{
			AddCategoryElementInputBox.Visibility = System.Windows.Visibility.Hidden;
		}

		private void Button_Submit_CategoryElement_Click(object sender, RoutedEventArgs e)
		{
			IDBService database = Ioc.Default.GetService<IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			if (string.IsNullOrEmpty(InputCategoryBox.Text))
			{
				MessageBox.Show(" Field Cannot be empty!");
				return;
			}

			try
			{
				int row = database.ExecuteNonQuery($"INSERT INTO Category ('Name') VALUES ('{this.InputCategoryBox.Text.ToString()}');");

				FillDataGrid();
				
				AddCategoryElementInputBox.Visibility = System.Windows.Visibility.Hidden;
				MessageBox.Show($"{InputCategoryBox.Text} Category is succesfully inserted", "Success", MessageBoxButton.OK);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message);
			}
		}

		private void EditCategoryUpdateButton_Click(object sender, RoutedEventArgs e)
		{
			IDBService database = Ioc.Default.GetService<IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			if (string.IsNullOrEmpty(InputEditCategoryIdBox.Text))
			{
				MessageBox.Show("Field Cannot be empty!");
				return;
			}

			int id;
			if (!int.TryParse(this.InputEditCategoryIdBox.Text, out id))
			{
				MessageBox.Show("Id should be number!");
				return;
			}

			try
			{
				int row = database.ExecuteNonQuery($"update Category SET Name = '{this.InputEditCategoryBox.Text.ToString()}' where Id = {id};");
				
				FillDataGrid();
				EditCategoryElementInputBox.Visibility = System.Windows.Visibility.Hidden;

				MessageBox.Show($"{InputEditCategoryBox.Text} Product is succesfully Updated", "Success", MessageBoxButton.OK);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message);
			}
		}

		private void EditCategoryCancelButton_Click(object sender, RoutedEventArgs e)
		{
			EditCategoryElementInputBox.Visibility = System.Windows.Visibility.Hidden;

		}

		private void FillDataGrid()
		{
			IDBService database = Ioc.Default.GetService<IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			try
			{
				List<Category> categories = database.ExecuteReader<Category>("select * from Category order by id;");
				categoryDataGrid.ItemsSource = categories;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message);
			}
		}

		private void btnSearch_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(textBox_Search.Text))
			{
				FillDataGrid();
				return;
			}

			IDBService database = Ioc.Default.GetService<IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			try
			{
				string query;
				int categoryId;
				if (int.TryParse(textBox_Search.Text, out categoryId))
				{
					query = $"select * from Category where Id = {categoryId};";
				}
				else
				{
					query = $"select * from Category where Name = '{textBox_Search.Text}';";
				}

				List<Category> categories = database.ExecuteReader<Category>(query);
				categoryDataGrid.ItemsSource = categories;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message);
			}
		}

		public void Button_Delete_CategoryElement_Click(object sender, RoutedEventArgs e)
		{
			if (this.categoryDataGrid.SelectedItem == null)
			{
				MessageBox.Show("Please select the category.");
				return;
			}

			Category category = this.categoryDataGrid.SelectedItem as Category;

			var Result = MessageBox.Show($"Do you want to delete {category.Name} ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (Result == MessageBoxResult.Yes)
			{
				IDBService database = Ioc.Default.GetService<IDBService>();
				if (!database.IsOpen)
				{
					System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
					return;
				}

				try
				{
					database.ExecuteNonQuery($"DELETE FROM Category WHERE Id = {category.Id}");
					FillDataGrid();
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}
		}
	}
}
