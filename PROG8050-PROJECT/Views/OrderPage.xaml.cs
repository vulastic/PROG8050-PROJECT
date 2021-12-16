using Microsoft.Toolkit.Mvvm.DependencyInjection;
using PROG8050_PROJECT.Core.Services;
using PROG8050_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
	/// Interaction logic for OrderPage.xaml
	/// </summary>
	public partial class OrderPage : Page
	{
		public static long Id { get; set; }

		private List<CreateOrder> createOrders = new List<CreateOrder>();
		private List<Customer> customers = new List<Customer>();
		private List<Category> categories = new List<Category>();

		public OrderPage()
		{
			InitializeComponent();
			//FillData();

			//tblCustomerDetails.ItemsSource = customers;
			//tblCProductDetails.ItemsSource = createOrders;
		}

		private void BtnSearch_Click(object sender, RoutedEventArgs e)
		{
			IDBService database = Ioc.Default.GetService<IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			try
			{
				//MessageBox.Show($"Customer: {textBox_CustomerSearchbar.Text} details loaded!");
				string search_txt = textBox_CustomerSearchbar.Text;
				if (String.IsNullOrEmpty(search_txt) || String.IsNullOrWhiteSpace(search_txt))
				{
					List<Customer> customers = database.ExecuteReader<Customer>($"select Id, FirstName, LastName, PhoneNo from Customer;");
					//tblCustomerDetails.ItemsSource = customers;
				}
				else
				{
					List<Customer> customers = database.ExecuteReader<Customer>($"select Id, FirstName, LastName, PhoneNo from Customer where FirstName = '{search_txt}';");
					//tblCustomerDetails.ItemsSource = customers;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message);
			}
			
		}

		private void BtnAddNew_Click(object sender, RoutedEventArgs e)
		{
			this.NavigationService.Navigate(new Uri("./Views/NewCustomer.xaml", UriKind.RelativeOrAbsolute));
		}

		private void BtnShowOrders_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show($"All Order will be populated in the order table..");
		}

		private void BtnDelete_Click(object sender, RoutedEventArgs e)
		{
			//CreateOrder dataRowView = (CreateOrder)tblCProductDetails.SelectedItem;
			//if (dataRowView == null)
			/*
			{
				MessageBox.Show($"Select a product to remove from the cart");
				return;
			}

			foreach (CreateOrder createOrder in createOrders)
			{
				/*
				if (createOrder.Id == dataRowView.Id)
				{
					createOrders.Remove(createOrder);
					ShowOrders();
					MessageBox.Show($" {dataRowView.Quantity} {dataRowView.Name} removed from the order list.");
					return;
				}
				*/
			//}
		}

		private void BtnSelectAll_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show($"Orders are selected from the order table!");
		}

		private void btnCreateOrder_Click(object sender, RoutedEventArgs e)
		{
			//string time = DateTime.Now.ToString().Remove(DateTime.Now.ToString().Length-3);
			if (createOrders.Count == 0)
			{
				MessageBox.Show($"Cart is empty! Add products to complete your order");
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
				int result = database.ExecuteNonQuery($"INSERT INTO 'Order' (CustomerId) VALUES ({Id};)");
				if (result <= 0)
				{
					System.Windows.MessageBox.Show("Fail to insert order.", "Ooops!");
					return;
				}

				DataTable temp = database.ExecuteReader("Select LAST_INSERT_ROWID();");
				if (temp.Rows.Count <= 0)
				{
					System.Windows.MessageBox.Show("Fail to create order.", "Ooops!");
					return;
				}

				/*
				int orderId = Convert.ToInt32(temp.Rows[0][0]);
				foreach (CreateOrder order in createOrders)
				{
					List<Product> products = database.ExecuteReader<Product>($"Select Id, Quantity from Product where Id = {order.Id};");
					if (products.Count <= 0)
					{
						System.Windows.MessageBox.Show("Cannot find the product.", "Ooops!");
						return;
					}

					Product product = products[0];
					product.Quantity -= order.Quantity;

					result = database.ExecuteNonQuery($"UPDATE Product set Quantity = {product.Quantity} WHERE Id = {order.Id}");
					if (result <= 0)
					{
						System.Windows.MessageBox.Show("Update quantity error.", "Ooops!");
						return;
					}

					result = database.ExecuteNonQuery($"INSERT INTO OrderDetail (OrderId,ProductId,Quantity) VALUES({orderId}, {order.Id}, {order.Quantity})");
					if (result <= 0)
					{
						System.Windows.MessageBox.Show("Insert order detail errir.", "Ooops!");
						return;
					}
				}

				MessageBox.Show($"Order {orderId} is placed successfully!", "Success", MessageBoxButton.OK);
				createOrders.Clear();
				ShowOrders();
				*/
				PrintOrder.Visibility = Visibility.Visible;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message);
			}
		}


		private void Row_CustomerDetail_Click(object sender, MouseButtonEventArgs e)
		{
			/*
			if (e.ClickCount == 2)
			{
				UpdateCustomerView.Visibility = System.Windows.Visibility.Visible;
				DataRowView dataRowView = (DataRowView)tblCustomerDetails.SelectedItem;
				textBox_CustId.Text = dataRowView.Row[0].ToString();
				textBox_FirstName.Text = dataRowView.Row[1].ToString();
				textBox_LastName.Text = dataRowView.Row[2].ToString();
				textBox_PhoneNo.Text = dataRowView.Row[3].ToString();
			}
			else
			{
				Row_CustomerforOrder(sender, e);

			}
			*/
		}

		private void Row_ProductDetail_Click(object sender, MouseButtonEventArgs e)
		{
			//quantityProductOrder.Visibility = System.Windows.Visibility.Visible;
			//DataRowView dataRowView = (DataRowView)tblProductDetails.SelectedItem;
			//Product_Name.Text = dataRowView.Row[1].ToString();
			//Product_Id.Text = dataRowView.Row[0].ToString();
			//Product_Price.Text = dataRowView.Row[3].ToString();

			//int quantity = Convert.ToInt32(dataRowView[2].ToString());
			//for (int i = 1; i <= quantity; i++)
			{
			//	comboboxProductQuantity.Items.Add(i);
			}
			//comboboxProductQuantity.SelectedIndex = 0;
		}

		private void FillData()
		{
			//load customer details
			IDBService database = Ioc.Default.GetService<IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			try
			{
				List<Customer> customers = database.ExecuteReader<Customer>("select Id, FirstName, LastName,PhoneNo from Customer");
				this.tblCustomerDetails.ItemsSource = customers;

			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message);
			}
		}

		private void OnSelection(object sender, EventArgs e)
		{
			IDBService database = Ioc.Default.GetService<IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			if (Id == 0)
			{
				System.Windows.MessageBox.Show($"Select a customer to proceed further");
				return;
			}

			//load product table
			int categoryId = categories[comboboxCategoryList.SelectedIndex].Id;

			List<Product> products = database.ExecuteReader<Product>($"select Id, Name, Quantity, Price, Image from Product where CategoryId = {categoryId};");
			//tblProductDetails.ItemsSource = products;
		}

		private void btnCUCancel_Click(object sender, RoutedEventArgs e)
		{
			//UpdateCustomerView.Visibility = Visibility.Collapsed;
		}

		private void btnCUUpdate_Click(object sender, RoutedEventArgs e)
		{
			IDBService database = Ioc.Default.GetService<IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			Dictionary<string, object> param = new Dictionary<string, object>();
			//param.Add("@id", textBox_CustId.Text);
			//param.Add("@firstname", textBox_FirstName.Text);
			//param.Add("@lastname", textBox_LastName.Text);
			//param.Add("@phoneNo", textBox_PhoneNo.Text);

			int result = database.ExecuteNonQuery("UPDATE Customer set FirstName = @firstname, LastName = @lastname, PhoneNo = @phoneNo WHERE Id = @id;", param);
			if (result == 1)
			{
				System.Windows.MessageBox.Show($"Customer details updated!");
				//UpdateCustomerView.Visibility = Visibility.Collapsed;
				FillData();
			}
			else
			{
				System.Windows.MessageBox.Show($"Customer details update failed!");
			}

		}

		private void btnUQCancel_Click(object sender, RoutedEventArgs e)
		{
			//quantityProductOrder.Visibility = Visibility.Collapsed;

		}

		private void btnUQOk_Click(object sender, RoutedEventArgs e)
		{
			/*
			CreateOrder orders = new CreateOrder();
			orders.Id = Convert.ToInt64(Product_Id.Text);
			orders.Name = Product_Name.Text;
			orders.Price = Convert.ToInt64(Product_Price.Text);
			orders.Quantity = Convert.ToInt64(comboboxProductQuantity.SelectedValue);
			orders.TotalPrice = orders.Price * orders.Quantity;
			CreateOrder.createOrders.Add(orders);
			quantityProductOrder.Visibility = Visibility.Collapsed;
			ShowOrders();
			*/
		}
		private void ShowOrders()
		{
			/*
			tblCProductDetails.Items.Clear();
			long totalPrice = 0L;
			foreach (CreateOrder createOrder in CreateOrder.createOrders)
			{
				tblCProductDetails.Items.Add(createOrder);
				totalPrice += createOrder.TotalPrice;
			}

			Total_Price.Text = totalPrice.ToString();
			*/
		}

		private void Row_ProductDetailEdit_Click(object sender, MouseButtonEventArgs e)
		{
			/*
			CreateOrder order = (CreateOrder)tblCProductDetails.SelectedItem;
			UProduct_Name.Text = order.Name;
			UProduct_Id.Text = order.Id.ToString();
			UProduct_Price.Text = order.Price.ToString();
			var temp = dbManager.ExecuteReader($"Select * from Product where Id={UProduct_Id.Text}");
			int quantity = 0;
			while (temp.Read())
			{
				quantity = Convert.ToInt32(temp[5].ToString());
			}

			for (int i = 1; i <= quantity; i++)
			{
				comboboxUProductQuantity.Items.Add(i);
			}
			comboboxUProductQuantity.SelectedIndex = Convert.ToInt32(order.Quantity) - 1;

			quantityUpdateProductOrder.Visibility = Visibility.Visible;
			*/
		}

		private void btnUOQCancel_Click(object sender, RoutedEventArgs e)
		{
			quantityUpdateProductOrder.Visibility = Visibility.Collapsed;

		}

		private void btnUOQUpdate_Click(object sender, RoutedEventArgs e)
		{
			/*
			CreateOrder order = new CreateOrder();
			foreach (CreateOrder createOrders in CreateOrder.createOrders)
			{
				if (createOrders.Id.Equals(Convert.ToInt64(UProduct_Id.Text)))
				{
					createOrders.Quantity = Convert.ToInt64(comboboxUProductQuantity.SelectedValue.ToString());
					createOrders.TotalPrice = createOrders.Price * createOrders.Quantity;
				}
			}
			quantityUpdateProductOrder.Visibility = Visibility.Collapsed;
			System.Windows.MessageBox.Show($"Order details updated!");
			ShowOrders();
			*/
		}

		private void Row_CustomerforOrder(object sender, MouseButtonEventArgs e)
		{
			/*
			if (e.ClickCount == 2)
			{
				Row_CustomerDetail_Click(sender, e);
			}
			else
			{
				DataRowView dataRowView = (DataRowView)tblCustomerDetails.SelectedItem;
				Id = Convert.ToInt64(dataRowView.Row[0].ToString());
			}
			*/
		}

		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			PrintOrder.Visibility = Visibility.Collapsed;

		}

		private void btnPrintOrder_Click(object sender, RoutedEventArgs e)
		{
			PrintDialog printDialog = new PrintDialog();
			printDialog.ShowDialog();
		}
	}
}
