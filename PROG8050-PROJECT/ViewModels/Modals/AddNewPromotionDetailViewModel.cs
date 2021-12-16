using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using MvvmDialogs;
using PROG8050_PROJECT.Core.Services;
using PROG8050_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PROG8050_PROJECT.ViewModels.Modals
{
	class AddNewPromotionDetailViewModel : ObservableRecipient, IModalDialogViewModel
	{
		private bool? dialogResult;
		public bool? DialogResult
		{
			get => dialogResult;
			private set => SetProperty(ref dialogResult, value);
		}

		public ICommand Loaded { get; }
		public ICommand Close { get; }
		public ICommand AddNewPromotionDetail { get; }

		public ObservableCollection<Promotion> Promotions { get; set; } = new();
		private Promotion selectedPromotion;
		public Promotion SelectedPromotion
		{
			get => selectedPromotion;
			set
			{
				selectedPromotion = value;
				RetriveCategory();
			}
		}

		public ObservableCollection<Category> Categories { get; set; } = new();
		private Category selectedCategory;
		public Category SelectedCategory
		{
			get => selectedCategory;
			set
			{
				selectedCategory = value;
				RetriveProduct(SelectedCategory.Id);
			}
		}

		private bool isCategoryEnabled = false;
		public bool IsCategoryEnabled
		{
			get => isCategoryEnabled;
			set
			{
				isCategoryEnabled = value;
				this.OnPropertyChanged("IsCategoryEnabled");
			}
		}

		public ObservableCollection<Product> Products { get; set; } = new();
		private Product selectedProduct;
		public Product SelectedProduct
		{
			get => selectedProduct;
			set
			{
				selectedProduct = value;
			}
		}

		private bool isProductEnabled = false;
		public bool IsProductEnabled
		{
			get => isProductEnabled;
			set
			{
				isProductEnabled = value;
				this.OnPropertyChanged("IsProductEnabled");
			}
		}

		private double discount = 0;
		public double Discount
		{
			get => discount;
			set
			{
				discount = value;
				if (discount > 100.0)
				{
					discount = 100;
				}
			}
		}


		public AddNewPromotionDetailViewModel()
		{
			Loaded = new RelayCommand<object>(LoadedPage);
			Close = new RelayCommand<object>(CloseWindow);
			AddNewPromotionDetail = new RelayCommand<object>(AddNewPromotionDetailEvent);
		}

		private void LoadedPage(object sender)
		{
			ILoginService service = Ioc.Default.GetService<ILoginService>();
			if (!service.IsLogin)
			{
				Ioc.Default.GetService<INavigationService>().Navigate<LoginPageViewModel>();
			}

			// Load All Promotions
			RetrivePromotion();
		}

		private void CloseWindow(object sender)
		{
			this.DialogResult = false;
		}

		public void AddNewPromotionDetailEvent(object sender)
		{
			// Input Validations
			if (this.SelectedPromotion == null || this.SelectedProduct == null)
			{
				System.Windows.MessageBox.Show("Invalid request.", "Ooops!");
				return;
			}

			if (discount <= 0)
			{
				System.Windows.MessageBox.Show("Zero promotion discount.", "Ooops!");
				return;
			}

			if (discount > 100.0)
			{
				discount = 100.0;
			}

			// Insert Promotion
			IDBService database = Ioc.Default.GetService<IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			try
			{
				// First. Find Exist Promotion Detail
				DataTable exist = database.ExecuteReader($"SELECT * FROM PromotionDetail WHERE PromotionId = {SelectedPromotion.Id} and ProductId = {SelectedProduct.Id};");
				if (exist.Rows.Count > 0)
				{
					System.Windows.MessageBox.Show($"Promotion detail already added.", "Ooops!");
					return;
				}

				// Second. Insert after select.
				int row = database.ExecuteNonQuery(
					$"INSERT INTO PromotionDetail ('PromotionId', 'ProductId', 'Discount') VALUES ({SelectedPromotion.Id}, {SelectedProduct.Id}, {discount});");

				if (row <= 0)
				{
					System.Windows.MessageBox.Show("Fail to add new promotion detail.", "Ooops!");
					return;
				}

				this.DialogResult = true;
			}
			catch (Exception e)
			{
				System.Windows.MessageBox.Show("Database Error.", "Ooops!");
				Debug.WriteLine(e.Message);
			}
		}

		private void RetrivePromotion()
		{
			IDBService database = Ioc.Default.GetService<IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			try
			{
				List<Promotion> result = database.ExecuteReader<Promotion>("SELECT * FROM Promotion;");
				if (result.Count <= 0)
				{
					return;
				}

				// DataGrid Copy
				this.Promotions.Clear();
				result.ForEach(p => Promotions.Add(p));
			}
			catch (Exception e)
			{
				System.Windows.MessageBox.Show("Database Error.", "Ooops!");
				Debug.WriteLine(e.Message);
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

			try
			{
				List<Category> result = database.ExecuteReader<Category>("SELECT * FROM Category;");
				if (result.Count <= 0)
				{
					return;
				}

				// DataGrid Copy
				this.Categories.Clear();
				result.ForEach(p => Categories.Add(p));
				this.IsCategoryEnabled = true;
			}
			catch (Exception e)
			{
				System.Windows.MessageBox.Show("Database Error.", "Ooops!");
				Debug.WriteLine(e.Message);
			}
		}

		private void RetriveProduct(int categoryId)
		{
			IDBService database = Ioc.Default.GetService<IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			try
			{
				List<Product> result = database.ExecuteReader<Product>($"SELECT * FROM Product WHERE CategoryId = {categoryId};");
				if (result.Count <= 0)
				{
					return;
				}

				// DataGrid Copy
				this.Products.Clear();
				result.ForEach(p => Products.Add(p));
				this.IsProductEnabled = true;
			}
			catch (Exception e)
			{
				System.Windows.MessageBox.Show("Database Error.", "Ooops!");
				Debug.WriteLine(e.Message);
			}
		}
	}
}
