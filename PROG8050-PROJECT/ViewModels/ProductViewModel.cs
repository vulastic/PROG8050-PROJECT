using PROG8050_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;


namespace PROG8050_PROJECT.ViewModels
{
	class ProductViewModel : Notifier
	{
		private Product product = new Product();
		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		public Product Product

		{
			get => product;
			set
			{
				product = value;
				OnPropertyChanged("Customer");
			}
		}
		private ICommand addProduct;
		public ICommand AddProduct => (this.addProduct) ??= new RelayCommand(parameter =>
		{
			if (String.IsNullOrEmpty(product.CategoryId.ToString()))
			{
				System.Windows.MessageBox.Show("Please Select the Category");
				return;
			}
			if (String.IsNullOrEmpty(product.Name))
			{
				System.Windows.MessageBox.Show("Please Enter the Name");
				return;
			}
			if (String.IsNullOrEmpty(product.Description))
			{
				System.Windows.MessageBox.Show("Please Enter Description");
				return;
			}
			if (String.IsNullOrEmpty(product.Price.ToString()))
			{
				System.Windows.MessageBox.Show("Please Enter the Price");
				return;
			}
			if (String.IsNullOrEmpty(product.Quantity.ToString()))
			{
				System.Windows.MessageBox.Show("Please Enter the Quantity");
				return;
			}
			if (String.IsNullOrEmpty(product.Image.ToString()))
			{
				System.Windows.MessageBox.Show("Please Add the Image");
				return;
			}


			SQLiteDBManager dbManager = SQLiteDBManager.Instance;

			if (!dbManager.IsOpen())
			{
				System.Windows.MessageBox.Show("Database is not connected");
			}
			Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("@id", product.Id);
			param.Add("@categoryId", product.CategoryId);
			param.Add("@name", product.Name);
			param.Add("@description", product.Description);
			param.Add("@price", product.Price);
			param.Add("@quantity", product.Quantity);
			param.Add("@image", product.Image);


			List<Product> tempUser = dbManager.ExecuteReader<Product>("select * from Product where Id = @id", param);

			int result = dbManager.ExecuteNonQuery("insert into Product (CategoryId, Name, Description, Price, Quantity,Image) values(@categoryId,@name,@description,@price,@quantity,@image)", param);
			if (result == 1)
			{
				System.Windows.MessageBox.Show($"Product added successfully");
			}
			else
			{
				System.Windows.MessageBox.Show($"Product addition failed");
				return;
			}

		});

	}
}
