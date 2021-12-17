using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using MvvmDialogs;
using PROG8050_PROJECT.Core.Services;
using PROG8050_PROJECT.Models;
using PROG8050_PROJECT.ViewModels.Modals;
using PROG8050_PROJECT.Views.Modals;

namespace PROG8050_PROJECT.ViewModels
{
	class OrderPageViewModel : ObservableRecipient
	{
		public ICommand SearchCustomer { get; }
		public ICommand AddNewCustomer { get; }
		public ICommand CreateOrder { get; }
		public ICommand DeleteSelectedOrder { get; }

		public ICommand DoubleClickCustomers { get; }
		public ICommand DoubleClickSelectQuantity { get; }
		public ICommand DoubleClickUpdateQuantity { get; }

		public ObservableCollection<Product> Products { get; set; } = new();

		public ObservableCollection<Customer> Customers { get; set; } = new();
		public ObservableCollection<Category> Categories { get; set; } = new();
		public ObservableCollection<CreateOrder> CreateOrders { get; set; } = new();

		private Customer selectedCustomer;
		public Customer SelectedCustomer
		{
			get => selectedCustomer;
			set
			{
				selectedCustomer = value;
				this.OnPropertyChanged("SelectedCustomer");
			}
		}

		private Category selectedCategory;
		public Category SelectedCategory
		{
			get => selectedCategory;
			set
			{
				selectedCategory = value;
				RetiveProduct();
				this.OnPropertyChanged("SelectedCategory");
			}
		}

		private Product selectedProduct;
		public Product SelectedProduct
		{
			get => selectedProduct;
			set
			{
				selectedProduct = value;
				this.OnPropertyChanged("SelectedProduct");
			}
		}

		// Update Customer
		public ICommand CancelUpdateCustomer { get; }
		public ICommand UpdateCustomer { get; }

		private bool isUpdateCustomerVisible = false;
		public bool IsUpdateCustomerVisible
		{
			get => isUpdateCustomerVisible;
			set
			{
				isUpdateCustomerVisible = value;
				this.OnPropertyChanged("IsUpdateCustomerVisible");
			}
		}


		// Select Quantity
		public ICommand CancelSelectQuantity { get; }
		public ICommand SelectQuantity { get; }

		private bool isSelectQuantityVisible = false;
		public bool IsSelectQuantityVisible
		{
			get => isSelectQuantityVisible;
			set
			{
				isSelectQuantityVisible = value;
				this.OnPropertyChanged("IsSelectQuantityVisible");
			}
		}

		private int selectedQuantity = 0;
		public int SelectedQuantity
		{
			get => selectedQuantity;
			set
			{
				selectedQuantity = value;
				this.OnPropertyChanged("SelectedQuantity");
			}
		}

		public List<int> SelectedQuantities { get; set; } = new();

		// Update Quantity
		public ICommand CancelUpdateQuantity { get; }
		public ICommand UpdateQuantity { get; }

		private bool isUpdateQuantityVisible = false;
		public bool IsUpdateQuantityVisible
		{
			get => isUpdateQuantityVisible;
			set
			{
				isUpdateQuantityVisible = value;
				this.OnPropertyChanged("IsUpdateQuantityVisible");
			}
		}

		public CreateOrder SelectedOrder { get; set; }

		// Print Order
		public ICommand CancelPrintOrder { get; }
		public ICommand PrintOrder { get; }

		private bool isPrintOrderVisible = false;
		public bool IsPrintOrderVisible
		{
			get => isPrintOrderVisible;
			set
			{
				isPrintOrderVisible = value;
				this.OnPropertyChanged("IsPrintOrderVisible");
			}
		}

		public OrderPageViewModel()
		{
			SearchCustomer = new RelayCommand<object>(SearchCustomerEvent);
			AddNewCustomer = new RelayCommand<object>(AddNewCustomerEvent);
			CreateOrder = new RelayCommand<object>(CreateOrderEvent);
			DeleteSelectedOrder = new RelayCommand<object>(DeleteSelectedOrderEvent);

			DoubleClickCustomers = new RelayCommand<object>(DoubleClickCustomersEvent);
			DoubleClickSelectQuantity = new RelayCommand<object>(DoubleClickSelectQuantityEvent);
			DoubleClickUpdateQuantity = new RelayCommand<object>(DoubleClickUpdateQuantityEvent);

			// Update Customer
			CancelUpdateCustomer = new RelayCommand<object>(CancelUpdateCustomerEvent);
			UpdateCustomer = new RelayCommand<object>(UpdateCustomerEvent);

			// Select Quantity
			CancelSelectQuantity = new RelayCommand<object>(CancelSelectQuantityEvent);
			SelectQuantity = new RelayCommand<object>(SelectQuantityEvent);

			// Update Quantitiy
			CancelUpdateQuantity = new RelayCommand<object>(CancelUpdateQuantityEvent);
			UpdateQuantity = new RelayCommand<object>(UpdateQuantityEvent);

			// Print Order
			CancelPrintOrder = new RelayCommand<object>(CancelPrintOrderEvent);
			PrintOrder = new RelayCommand<object>(PrintOrderEvent);

			RetriveCategory();
			RetriveCustomer();
		}

		private void RetriveCustomer()
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
				List<Customer> result = database.ExecuteReader<Customer>("select * from Customer;");
				if (result.Count <= 0)
				{
					return;
				}

				this.Customers.Clear();
				result.ForEach(x => Customers.Add(x));

			}
			catch (Exception ex)
			{
				System.Windows.MessageBox.Show("Error: " + ex.Message);
			}
		}

		private void RetriveCategory()
		{
			IDBService database = Ioc.Default.GetService<IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			List<Category> result = database.ExecuteReader<Category>("SELECT * FROM Category;");
			if (result.Count <= 0)
			{
				return;
			}

			result.ForEach(x => Categories.Add(x));
		}

		private void DoubleClickCustomersEvent(object sender)
		{
			IsUpdateCustomerVisible = true;
		}

		private void CancelUpdateCustomerEvent(object sender)
		{
			IsUpdateCustomerVisible = false;
		}

		private void UpdateCustomerEvent(object sender)
		{
			IDBService database = Ioc.Default.GetService<IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database.", "Ooops!");
				return;
			}

			if (selectedCustomer == null)
			{
				System.Windows.MessageBox.Show("Cannot find the customer.", "Ooops!");
				return;
			}

			Dictionary<string, object> param = new Dictionary<string, object>();
			param.Add("@id", selectedCustomer.Id);
			param.Add("@firstname", selectedCustomer.FirstName);
			param.Add("@lastname", selectedCustomer.LastName);
			param.Add("@phoneNo", selectedCustomer.PhoneNo);

			int result = database.ExecuteNonQuery("UPDATE Customer set FirstName = @firstname, LastName = @lastname, PhoneNo = @phoneNo WHERE Id = @id;", param);
			if (result == 1)
			{
				System.Windows.MessageBox.Show($"Customer details updated!");
				IsUpdateCustomerVisible = false;
				RetriveCustomer();
			}
			else
			{
				System.Windows.MessageBox.Show($"Customer details update failed!");
			}
		}

		private void DoubleClickSelectQuantityEvent(object sender)
		{
			SelectedQuantities.Clear();
			int quantity = SelectedProduct.Quantity;
			for (int i = 0; i <= quantity; ++i)
			{
				SelectedQuantities.Add(i);
			}
			SelectedQuantity = SelectedQuantities.First();
			IsSelectQuantityVisible = true;
		}

		private void DoubleClickUpdateQuantityEvent(object sender)
		{
			if (SelectedOrder == null)
			{
				return;
			}

			int quantity = 0;
			foreach (Product product in Products)
			{
				if (product.Id == SelectedOrder.ProductId)
				{
					quantity = product.Quantity;
				}
			}
			
			SelectedQuantities.Clear();
			for (int i = 0; i <= quantity; ++i)
			{
				SelectedQuantities.Add(i);
			}
			SelectedQuantity = SelectedQuantities.First();
			IsUpdateQuantityVisible = true;
		}

		private void CancelSelectQuantityEvent(object sender)
		{
			IsSelectQuantityVisible = false;
		}

		private void SelectQuantityEvent(object sender)
		{
			CreateOrder order = new CreateOrder()
			{
				ProductId = SelectedProduct.Id,
				Name = SelectedProduct.Name,
				Price = (int)SelectedProduct.Price,
				Quantity = SelectedQuantity,
				TotalPrice = (int)SelectedProduct.Price * SelectedQuantity
			};

			CreateOrders.Add(order);
			IsSelectQuantityVisible = false;
		}

		private void RetiveProduct()
		{
			if (selectedCategory == null)
			{
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
				List<Product> products = database.ExecuteReader<Product>($"select Id, Name, Quantity, Price, Image from Product where CategoryId = {selectedCategory.Id};");
				if (products.Count <= 0)
				{
					return;
				}

				Products.Clear();
				products.ForEach(x => Products.Add(x));
			}
			catch (Exception ex)
			{
				System.Windows.MessageBox.Show("Error: " + ex.Message, "Ooops!");
			}
		}

		private void SearchCustomerEvent(object sender)
		{
			string searchText = sender as string;
			if (string.IsNullOrEmpty(searchText))
			{
				RetriveCustomer();
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
				List<Customer> customers = database.ExecuteReader<Customer>($"select Id, FirstName, LastName, PhoneNo from Customer where FirstName = '{searchText}';");

				this.Customers.Clear();
				customers.ForEach(x => this.Customers.Add(x));
			}
			catch (Exception ex)
			{
				System.Windows.MessageBox.Show("Error: " + ex.Message);
			}
		}

		private void AddNewCustomerEvent(object sender)
		{
			IDialogService dialogService = Ioc.Default.GetService<IDialogService>();

			AddNewCustomerViewModel modalView = new AddNewCustomerViewModel();

			bool? success = dialogService.ShowDialog(this, modalView);
			if (success == true)
			{
				RetriveCustomer();
			}
		}

		private void CancelUpdateQuantityEvent(object sender)
		{
			IsUpdateQuantityVisible = false;
		}

		private void UpdateQuantityEvent(object sender)
		{
			if (SelectedOrder == null)
			{
				return;
			}

			CreateOrder order = new CreateOrder()
			{
				ProductId = SelectedOrder.ProductId,
				Name = SelectedOrder.Name,
				Price = SelectedOrder.Price,
				Quantity = SelectedQuantity,
				TotalPrice = SelectedOrder.TotalPrice
			};

			CreateOrders.Remove(SelectedOrder);
			CreateOrders.Add(order);

			SelectedOrder = order;
			
			IsUpdateQuantityVisible = false;
		}

		private void CreateOrderEvent(object sender)
		{
			if (SelectedCustomer == null)
			{
				System.Windows.MessageBox.Show($"Please select the customer first.");
				return;
			}

			if (CreateOrders.Count == 0)
			{
				System.Windows.MessageBox.Show($"Cart is empty! Add products to complete your order");
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
				int result = database.ExecuteNonQuery($"INSERT INTO 'Order' (CustomerId) VALUES ({SelectedCustomer.Id});");
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

				int orderId = Convert.ToInt32(temp.Rows[0][0]);
				foreach (CreateOrder order in CreateOrders)
				{
					List<Product> products = database.ExecuteReader<Product>($"Select Id, Quantity from Product where Id = {order.ProductId};");
					if (products.Count <= 0)
					{
						System.Windows.MessageBox.Show("Cannot find the product.", "Ooops!");
						return;
					}

					Product product = products[0];
					product.Quantity -= order.Quantity;

					result = database.ExecuteNonQuery($"UPDATE Product set Quantity = {product.Quantity} WHERE Id = {order.ProductId}");
					if (result <= 0)
					{
						System.Windows.MessageBox.Show("Update quantity error.", "Ooops!");
						return;
					}

					result = database.ExecuteNonQuery($"INSERT INTO OrderDetail (OrderId, ProductId, Quantity) VALUES({orderId}, {order.ProductId}, {order.Quantity})");
					if (result <= 0)
					{
						System.Windows.MessageBox.Show("Insert order detail errir.", "Ooops!");
						return;
					}
				}

				System.Windows.MessageBox.Show($"Order {orderId} is placed successfully!", "Success");
				CreateOrders.Clear();

				IsPrintOrderVisible = true;
			}
			catch (Exception ex)
			{
				System.Windows.MessageBox.Show("Error: " + ex.Message);
			}
		}

		private void PrintOrderEvent(object sender)
		{
			System.Windows.MessageBox.Show("The feature is developing.", "Ooops!");
			IsPrintOrderVisible = false;
		}

		private void CancelPrintOrderEvent(object sender)
		{
			IsPrintOrderVisible = false;
		}

		private void DeleteSelectedOrderEvent(object sender)
		{
			List<CreateOrder> remove = new();
			foreach(CreateOrder order in CreateOrders)
			{
				if (order.IsSelected)
				{
					remove.Add(order);
				}
			}

			remove.ForEach((x) =>
			{
				CreateOrders.Remove(x);
			});
		}
	}
}
