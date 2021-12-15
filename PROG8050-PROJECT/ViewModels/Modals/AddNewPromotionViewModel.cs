using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using MvvmDialogs;
using PROG8050_PROJECT.Core.Services;
using PROG8050_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
	class AddNewPromotionViewModel : ObservableRecipient, IModalDialogViewModel
	{
		private bool? dialogResult;
		public bool? DialogResult
		{
			get => dialogResult;
			private set => SetProperty(ref dialogResult, value);
		}

		private ObservableCollection<Category> categories = new ObservableCollection<Category>();
		public ObservableCollection<Category> Categories
		{
			get => categories;
			set
			{
				categories = value;
			}
		}

		private Category selectedCateogy;
		public Category SelectedCategory
		{
			get => selectedCateogy;
			set
			{
				selectedCateogy = value;
				this.OnPropertyChanged("SelectedCategory");
			}
		}

		public string Name { get; set; }
		public string Description { get; set; }
		public int PromotionId { get; set; }

		private double discount = 0;
		public string Discount
		{
			get => discount.ToString();
			set
			{
				string temp = Regex.Replace(value, @"[^\d]", "");
				discount = Convert.ToInt32(temp);

				if (discount > 100.0)
				{
					discount = 100.0;
				}
				this.OnPropertyChanged("Discount");
			}
		}

		public DateTime StartDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
		public DateTime EndDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

		public ICommand Loaded { get; }
		public ICommand Close { get; }
		public ICommand AddNewPromotion { get; }

		public AddNewPromotionViewModel()
		{
			Loaded = new RelayCommand<object>(LoadedPage);
			Close = new RelayCommand<object>(CloseWindow);
			AddNewPromotion = new RelayCommand<object>(AddNewPromotionEvent);
		}

		private void LoadedPage(object sender)
		{
			ILoginService service = Ioc.Default.GetService<ILoginService>();
			if (!service.IsLogin)
			{
				Ioc.Default.GetService<INavigationService>().Navigate<LoginPageViewModel>();
			}

			// Load All Promotions
			RetriveCategories();
		}

		private void CloseWindow(object sender)
		{
			this.dialogResult = false;
			(sender as Window).Close();
		}

		private void RetriveCategories()
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
				this.categories.Clear();
				result.ForEach(p => categories.Add(p));

				this.SelectedCategory = categories.First();
			}
			catch (Exception e)
			{
				System.Windows.MessageBox.Show("Database Error.", "Ooops!");
				Debug.WriteLine(e.Message);
			}
		}

		public void AddNewPromotionEvent(object sender)
		{
			// Input Validations
			if (this.Name == null || String.IsNullOrEmpty(this.Name))
			{
				System.Windows.MessageBox.Show("Name is empty.", "Ooops!");
				return;
			}

			if (selectedCateogy == null)
			{
				System.Windows.MessageBox.Show("Please select the category.", "Ooops!");
				return;
			}

			if (discount > 100)
			{
				discount = 100;
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
				// First. Find Exist Promotion
				DataTable exist = database.ExecuteReader($"SELECT Id FROM Promotion WHERE Name = {this.Name}");
				if (exist.Rows.Count > 0)
				{
					System.Windows.MessageBox.Show($"{this.Name} already added.", "Ooops!");
					return;
				}

				// Second. Insert after select.
				Dictionary<string, object> param = new Dictionary<string, object>();
				param.Add("@name", this.Name);
				param.Add("@desc", this.Description);
				param.Add("@start", (StartDate.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
				param.Add("@end", (EndDate.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);

				DataTable result = database.ExecuteReader("INSERT INTO Promotion ('Name', 'Description', 'StartDatetime', 'EndDatetime') VALUES (@name, @desc, @start, @end);" +
					"SELECT Id FROM Promotion WHERE Name = @name", param);

				if (result.Rows.Count <= 0)
				{
					System.Windows.MessageBox.Show("Fail to add new promotion.", "Ooops!");
					return;
				}

				// Third. Add Promotion Detail
				int promotionId = Convert.ToInt32(result.Rows[0][0]);
				int row = database.ExecuteNonQuery($"INSERT INTO PromotionDetail(PromotionId, ProductId, Discount) SELECT {promotionId}, Product.Id, {this.discount} FROM Product WHERE Product.CategoryId = {this.SelectedCategory.Id};");
				if (row == 0)
				{
					System.Windows.MessageBox.Show("Cannot add promotion details.", "Ooops!");
					return;
				}

				System.Windows.MessageBox.Show($"{row} promotion details are created.", "Ooops!");

				PromotionId = promotionId;
				DialogResult = true;
			}
			catch (Exception e)
			{
				System.Windows.MessageBox.Show("Database Error.", "Ooops!");
				Debug.WriteLine(e.Message);
			}
		}

	}
}
