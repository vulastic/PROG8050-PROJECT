using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using PROG8050_PROJECT.Core.Services;
using PROG8050_PROJECT.Models;

namespace PROG8050_PROJECT.ViewModels
{
	class OrderPageViewModel : ObservableRecipient
	{
		public ICommand DoubleClickCustomers { get; }
		public ICommand DoubleClickSelectQuantity { get; }

		public ObservableCollection<Product> Products { get; set; } = new();

		public ObservableCollection<Customer> Customers { get; set; } = new();
		public ObservableCollection<CreateOrder> CreateOrders { get; set; } = new();
		public ObservableCollection<Category> Categories { get; set; } = new();

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

		public int SelectedQuantity { get; set; } = new();
		public List<int> SelectedQuantities { get; set; } = new();

		private bool isUpdateQuantityVisible = false;
		public bool IsUpdateQuantityVisible
		{
			get => isUpdateQuantityVisible;
			set
			{
				isUpdateQuantityVisible = value;
				this.OnPropertyChanged("IsUpdateProductVisible");
			}
		}

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

		List<Category> categories;

		public OrderPageViewModel()
		{
			DoubleClickCustomers = new RelayCommand<object>(DoubleClickCustomersEvent);
			DoubleClickSelectQuantity = new RelayCommand<object>(DoubleClickSelectQuantityEvent);

			// Update Customer
			CancelUpdateCustomer = new RelayCommand<object>(CancelUpdateCustomerEvent);
			UpdateCustomer = new RelayCommand<object>(UpdateCustomerEvent);

			// Select Quantity
			CancelSelectQuantity = new RelayCommand<object>(CancelSelectQuantityEvent);
			SelectQuantity = new RelayCommand<object>(SelectQuantityEvent);

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
			IsSelectQuantityVisible = true;
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
	}
}
