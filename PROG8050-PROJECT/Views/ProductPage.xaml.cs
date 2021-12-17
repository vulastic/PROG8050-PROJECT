using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Win32;
using PROG8050_PROJECT.Core.Services;
using PROG8050_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
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
	/// Interaction logic for ProductPage.xaml
	/// </summary>
	public partial class ProductPage : Page
	{
		List<int> cateogryIds = new();

		public ProductPage()
		{
			InitializeComponent();
			fill_category();
			FillDataGrid();
		}

		private void Button_Add_ItemElement_Click(object sender, RoutedEventArgs e)
		{
			AddItemElementInputBox.Visibility = System.Windows.Visibility.Visible;

			InputProductNameBox.Text = String.Empty;
			InputProductDescriptionBox.Text = String.Empty;
			InputProductPriceBox.Text = String.Empty;
			InputProducQuantityBox.Text = String.Empty;
			InputCategoryIdBox.SelectedIndex = 0;
			imgProduct.Source = null;
		}

		private void Button_Edit_ItemElement_Click(object sender, RoutedEventArgs e)
		{
			if (this.productDataGrid.SelectedItem == null)
			{
				return;
			}

			try
			{
				Product product = this.productDataGrid.SelectedItem as Product;

				InputEditId.Text = product.Id.ToString();
				InputEditCategoryIdBox.SelectedItem = product.CategoryName;
				InputEditProductNameBox.Text = product.Name;
				InputEditProductDescriptionBox.Text = product.Description;
				InputEditProductPriceBox.Text = product.Price.ToString();
				InputEditProductQuantityBox.Text = product.Quantity.ToString();

				if (product.Image != null)
				{
					MemoryStream stream = new MemoryStream(product.Image);
					BitmapImage bitmap = new BitmapImage();
					bitmap.BeginInit();
					bitmap.StreamSource = stream;
					bitmap.EndInit();
					imgEditProduct.Source = bitmap as ImageSource;
				}

				EditItemElementInputBox.Visibility = System.Windows.Visibility.Visible;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error:" + ex.Message);
			}
		}

		private void ProductCancelButton_Click(object sender, RoutedEventArgs e)
		{
			AddItemElementInputBox.Visibility = System.Windows.Visibility.Hidden;
		}

		private void fill_category()
		{
			IDBService database = Ioc.Default.GetService<IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			try
			{
				List<Category> categories = database.ExecuteReader<Category>("SELECT * FROM Category order by Id");
				foreach (Category category in categories)
				{
					cateogryIds.Add(category.Id);
					InputCategoryIdBox.Items.Add(category.Name);
					InputEditCategoryIdBox.Items.Add(category.Name);
				}

			}
			catch (Exception ex)
			{
				// write exception info to log or anything else
				MessageBox.Show("Error: " + ex.Message);
			}
		}

		private void ProductSubmitButton_Click(object sender, RoutedEventArgs e)
		{
			int price = 0, quantity = 0;
			if (!int.TryParse(InputProductPriceBox.Text, out price) || !int.TryParse(InputProducQuantityBox.Text, out quantity))
			{
				System.Windows.MessageBox.Show("Invalid price or quantity format.", "Ooops!");
				return;
			}

			if (InputProductNameBox.Text != "" || InputProductDescriptionBox.Text != "")
			{
				IDBService database = Ioc.Default.GetService<IDBService>();
				if (!database.IsOpen)
				{
					System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
					return;
				}

				try
				{
					Dictionary<string, object> parameter = new();
					parameter.Add("@name", InputProductNameBox.Text);
					parameter.Add("@cid", cateogryIds[InputCategoryIdBox.SelectedIndex]);
					parameter.Add("@description", InputProductDescriptionBox.Text);
					parameter.Add("@price", price);
					parameter.Add("@quantity", quantity);

					if (imgProduct.Source != null)
					{
						BitmapImage bitmap = imgProduct.Source as BitmapImage;
						
						JpegBitmapEncoder encoder = new JpegBitmapEncoder();
						encoder.Frames.Add(BitmapFrame.Create(bitmap));
						using (MemoryStream stream = new MemoryStream())
						{
							encoder.Save(stream);
							parameter.Add("@img", stream.ToArray());
						}
					}
					else
					{
						parameter.Add("@img", null);
					}

					int row = database.ExecuteNonQuery(@"Insert into Product(Name, CategoryId, Description, Price, Quantity, Image) VALUES (@name, @cid, @description, @price, @quantity, @img)", parameter);
					
					FillDataGrid();

					AddItemElementInputBox.Visibility = System.Windows.Visibility.Hidden;

					MessageBox.Show($"{InputProductNameBox.Text} Product is succesfully inserted", "Success", MessageBoxButton.OK);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}

			}
			else
			{
				MessageBox.Show(" Field Cannot be empty!");
			}
		}

		private void Button_DeleteProductClick(object sender, RoutedEventArgs e)
		{
			Product product = this.productDataGrid.SelectedItem as Product;
			if (product == null)
			{
				MessageBox.Show("Please select the product.");
				return;
			}

			var Result = MessageBox.Show($"Do you want to delete {product.Name} ? ", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (Result == MessageBoxResult.Yes)
			{
				try
				{
					IDBService database = Ioc.Default.GetService<IDBService>();
					if (!database.IsOpen)
					{
						System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
						return;
					}

					int row = database.ExecuteNonQuery($"delete from Product where Id = {product.Id}");
					
					FillDataGrid();

					if (row > 0)
					{
						MessageBox.Show("Succesfully Deleted");
					}

				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}
		}

		private void btnSearchProduct_Click(object sender, RoutedEventArgs e)
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
				int productId;
				if (int.TryParse(textBox_Search.Text, out productId))
				{
					query = "SELECT Product.Id, Product.Name, Category.Id as 'CategoryId', Category.Name as 'CategoryName', Product.Description, Product.Price," +
							$"Product.Quantity, Product.Image FROM Product Left outer JOIN Category ON Product.CategoryId = Category.Id WHERE Product.Id = {productId};";
				}
				else
				{
					query = "SELECT Product.Id, Product.Name, Category.Id as 'CategoryId', Category.Name as 'CategoryName', Product.Description, Product.Price," +
							$"Product.Quantity, Product.Image FROM Product Left outer JOIN Category ON Product.CategoryId = Category.Id WHERE Product.Name = '{textBox_Search.Text}';";
				}
				
				List<Product> products = database.ExecuteReader<Product>(query);
				productDataGrid.ItemsSource = products;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message);
			}
		}

		private void EditItemUpdateButton_Click(object sender, RoutedEventArgs e)
		{
			int price = 0, quantity = 0;
			if (!int.TryParse(InputEditProductPriceBox.Text, out price) || !int.TryParse(InputEditProductQuantityBox.Text, out quantity))
			{
				System.Windows.MessageBox.Show("Invalid price or quantity format.", "Ooops!");
				return;
			}

			IDBService database = Ioc.Default.GetService<IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			if (InputEditProductDescriptionBox.Text != "" || InputEditProductNameBox.Text != "")
			{
				try
				{
					Dictionary<string, object> parameter = new();
					parameter.Add(":pid", Convert.ToInt32(InputEditId.Text));
					parameter.Add(":name", InputEditProductNameBox.Text);
					parameter.Add(":cid", InputEditCategoryIdBox.SelectedIndex);
					parameter.Add(":description", InputEditProductDescriptionBox.Text);
					parameter.Add(":price", price);
					parameter.Add(":quantity", quantity);

					if (imgEditProduct.Source != null)
					{
						BitmapImage bitmap = imgEditProduct.Source as BitmapImage;

						JpegBitmapEncoder encoder = new JpegBitmapEncoder();
						encoder.Frames.Add(BitmapFrame.Create(bitmap));
						using (MemoryStream stream = new MemoryStream())
						{
							encoder.Save(stream);
							parameter.Add(":img", stream.ToArray());
						}
					}
					else
					{
						parameter.Add(":img", null);
					}

					int row = database.ExecuteNonQuery("update Product set Name=:name,CategoryId=:cid,Description=:description,Price=:price,Quantity=:quantity,Image=:img where Id =:pid", parameter);

					FillDataGrid();
					EditItemElementInputBox.Visibility = System.Windows.Visibility.Hidden;
					MessageBox.Show($"{InputEditProductNameBox.Text} Product is succesfully Updated", "Success", MessageBoxButton.OK);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}
			else
			{
				MessageBox.Show(" Field Cannot be empty!");
			}
		}

		private void BtnLoadFromFile_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog op = new OpenFileDialog();
			op.Title = "Select a picture";
			op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
			  "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
			  "Portable Network Graphic (*.png)|*.png";
			if (op.ShowDialog() == true)
			{
				imgProduct.Source = new BitmapImage(new Uri(op.FileName));
			}
		}

		private void BtnEditFromFile_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog op = new OpenFileDialog();
			op.Title = "Select a picture";
			op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
			  "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
			  "Portable Network Graphic (*.png)|*.png";
			if (op.ShowDialog() == true)
			{
				imgEditProduct.Source = new BitmapImage(new Uri(op.FileName));
			}
		}

		private void EditItemCancelButton_Click(object sender, RoutedEventArgs e)
		{
			EditItemElementInputBox.Visibility = System.Windows.Visibility.Hidden;
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
				List<Product> list = database.ExecuteReader<Product>(
					"SELECT Product.Id, Product.Name, Category.Id as 'CategoryId', Category.Name as 'CategoryName', Product.Description, Product.Price," +
					"Product.Quantity, Product.Image FROM Product Left outer JOIN Category ON Product.CategoryId = Category.Id;");
				productDataGrid.ItemsSource = list;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message);
			}

		}
	}
}
