using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace PROG8050_PROJECT.ViewModels
{
	class PromotionPageViewModel : ObservableRecipient
	{
		public ICommand Loaded { get; }

		public ICommand SearchPromotion { get; }
		public ICommand SelectAllPromotion { get; }
		public ICommand AddPromotion { get; }
		public ICommand DeletePromotion { get; }

		public ICommand SearchPromotionDetail { get; }
		public ICommand SelectAllPromotionDetail { get; }

		public ICommand AddPromotionDetail { get; }
		public ICommand DeletePromotionDetail { get; }

		private ObservableCollection<Promotion> promotionTable = new ObservableCollection<Promotion>();
		public ObservableCollection<Promotion> PromotionTable
		{
			get => promotionTable;
			set
			{
				promotionTable = value;
			}
		}

		private Promotion selectedPromotion;
		public Promotion SelectedPromotion
		{
			get => selectedPromotion;
			set
			{
				selectedPromotion = value;

				if (selectedPromotion == null)
				{
					return;
				}

				if (promotionDetails == null || promotionDetails.Count == 0)
				{
					RetrivePromotionDetail(selectedPromotion.Id);
				}
				else
				{
					promotionDetailTable.Clear();
					promotionDetails.ForEach((x) =>
					{
						if (x.PromotionId == selectedPromotion.Id)
						{
							promotionDetailTable.Add(x);
						}
					});
					PromotionDetailTable = promotionDetailTable;
				}
			}
		}

		bool isSelectAllPromotion = false;
		private List<Promotion> promotions;
		

		private ObservableCollection<PromotionDetail> promotionDetailTable = new ObservableCollection<PromotionDetail>();
		public ObservableCollection<PromotionDetail> PromotionDetailTable
		{
			get => promotionDetailTable;
			set
			{
				promotionDetailTable = value;
			}
		}

		bool isSelectAllPromotionDetail = false;
		private List<PromotionDetail> promotionDetails;


		private IDialogService dialogService;

		public PromotionPageViewModel()
		{
			this.dialogService = Ioc.Default.GetService<IDialogService>();

			Loaded = new RelayCommand<object>(LoadedPage);

			SearchPromotion = new RelayCommand<object>(SearchPromotionEvent);
			SelectAllPromotion = new RelayCommand<object>(SelectAllPromotionEvent);

			AddPromotion = new RelayCommand<object>(AddPromotionEvent);
			DeletePromotion = new RelayCommand<object>(DeletePromotionEvent);

			SearchPromotionDetail = new RelayCommand<object>(SearchPromotionDetailEvent);
			SelectAllPromotionDetail = new RelayCommand<object>(SelectAllPromotionDetailEvent);

			AddPromotionDetail = new RelayCommand<object>(AddPromotionDetailEvent);
			DeletePromotionDetail = new RelayCommand<object>(DeletePromotionDetailEvent);
		}

		private void LoadedPage(object sender)
		{
			ILoginService service = Ioc.Default.GetService<ILoginService>();
			if (!service.IsLogin)
			{
				Ioc.Default.GetService<INavigationService>().Navigate<LoginPageViewModel>();
			}

			// Load All Promotions
			RetrivePromptions();
		}

		private void SearchPromotionEvent(object sender)
		{
			if (promotions == null || promotionDetails == null)
			{
				return;
			}

			if (string.IsNullOrEmpty(sender as string))
			{
				promotionTable.Clear();
				promotions.ForEach((x) =>
				{
					x.IsSelected = false;
					promotionTable.Add(x);
				});
				return;
			}

			int inputId;
			if (Int32.TryParse(sender as string, out inputId))
			{
				promotionTable.Clear();
				foreach(Promotion promotion in promotions)
				{
					if (promotion.Id == inputId)
					{
						promotion.IsSelected = false;
						promotionTable.Add(promotion);
					}
				}
			}
			else
			{
				string inputName = sender as string;
				promotionTable.Clear();
				foreach(Promotion promotion in promotions)
				{
					if (promotion.Name.ToLower().Contains(inputName.ToLower()))
					{
						promotion.IsSelected = false;
						promotionTable.Add(promotion);
					}
				}
			}

			PromotionDetailTable.Clear();
		}

		private void SelectAllPromotionEvent(object sender)
		{
			isSelectAllPromotion = !isSelectAllPromotion;
			foreach (Promotion promotion in promotionTable)
			{
				promotion.IsSelected = isSelectAllPromotion;
			}
		}

		private void AddPromotionEvent(object sender)
		{
			AddNewPromotionViewModel modalView = new AddNewPromotionViewModel();

			bool? success = dialogService.ShowDialog(this, modalView);
			if (success == true)
			{
				RetrivePromptions();

				int promotionId = modalView.PromotionId;
				RetrivePromotionDetail(promotionId);
			}
		}

		private void DeletePromotionEvent(object sender)
		{
			// Insert DB
			IDBService database = Ioc.Default.GetService<IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			try
			{
				StringBuilder builder = new StringBuilder();
				foreach (Promotion promotion in PromotionTable)
				{
					if (promotion.IsSelected)
					{
						if (builder.Length != 0)
						{
							builder.Append(",");
						}
						builder.Append(promotion.Id);
					}
				}

				// Nothing selected
				if (builder.Length <= 0)
				{
					return;
				}

				// First, delete all promotion details.
				int affectedRow = database.ExecuteNonQuery(
					$"DELETE FROM PromotionDetail WHERE PromotionId IN ({builder.ToString()});" +
					$"DELETE FROM Promotion WHERE Id IN ({builder.ToString()});");

				if (affectedRow < 0)
				{
					return;
				}

				RefreshAll();

				// Get Promotion Again
				RetrivePromptions();
			}
			catch (Exception e)
			{
				System.Windows.MessageBox.Show("Database Error.", "Ooops!");
				Debug.WriteLine(e.Message);
			}
		}

		private void SearchPromotionDetailEvent(object sender)
		{
			if (selectedPromotion == null)
			{
				promotionDetailTable.Clear();
				return;
			}

			if (promotionDetails == null)
			{
				return;
			}

			if (string.IsNullOrEmpty(sender as string))
			{
				promotionDetailTable.Clear();
				promotionDetails.ForEach((x) =>
				{
					if (selectedPromotion.Id == x.PromotionId)
					{
						x.IsSelected = false;
						promotionDetailTable.Add(x);
					}
				});
				return;
			}

			int inputId;
			if (Int32.TryParse(sender as string, out inputId))
			{
				promotionDetailTable.Clear();
				foreach (PromotionDetail promotionDetail in promotionDetails)
				{
					if (selectedPromotion.Id != promotionDetail.PromotionId)
					{
						continue;
					}

					if (promotionDetail.ProductId == inputId)
					{
						promotionDetail.IsSelected = false;
						promotionDetailTable.Add(promotionDetail);
					}
				}
			}
			else
			{
				string inputName = sender as string;
				promotionDetailTable.Clear();
				foreach (PromotionDetail promotionDetail in promotionDetails)
				{
					if (selectedPromotion.Id != promotionDetail.PromotionId)
					{
						continue;
					}

					if (promotionDetail.Name.ToLower().Contains(inputName.ToLower()))
					{
						promotionDetail.IsSelected = false;
						promotionDetailTable.Add(promotionDetail);
					}
				}
			}
		}

		private void SelectAllPromotionDetailEvent(object sender)
		{
			isSelectAllPromotionDetail = !isSelectAllPromotionDetail;
			foreach (PromotionDetail promotionDetail in promotionDetailTable)
			{
				promotionDetail.IsSelected = isSelectAllPromotionDetail;
			}
		}

		private void AddPromotionDetailEvent(object sender)
		{
			AddNewPromotionDetailViewModel modalView = new AddNewPromotionDetailViewModel();

			bool? success = dialogService.ShowDialog(this, modalView);
			if (success == true)
			{
				int promotionId = modalView.SelectedPromotion.Id;
				RetrivePromotionDetail(promotionId);
			}
		}

		private void DeletePromotionDetailEvent(object sender)
		{
			// Insert DB
			IDBService database = Ioc.Default.GetService<IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			try
			{
				StringBuilder builder = new StringBuilder();
				foreach (PromotionDetail promotionDetail in PromotionDetailTable)
				{
					if (promotionDetail.IsSelected)
					{
						if (builder.Length != 0)
						{
							builder.Append(",");
						}
						builder.Append(promotionDetail.Id);
					}
				}

				// Nothing selected
				if (builder.Length <= 0)
				{
					return;
				}

				// First, delete all promotion details.
				int affectedRow = database.ExecuteNonQuery($"DELETE FROM PromotionDetail WHERE Id IN ({builder.ToString()});");

				if (affectedRow < 0)
				{
					return;
				}

				// Get Promotion Again
				if (selectedPromotion != null)
				{
					RetrivePromotionDetail(selectedPromotion.Id);
					isSelectAllPromotionDetail = false;
				}
			}
			catch (Exception e)
			{
				System.Windows.MessageBox.Show("Database Error.", "Ooops!");
				Debug.WriteLine(e.Message);
			}
		}

		private void RetrivePromptions()
		{
			// Insert DB
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
				this.promotionTable.Clear();
				result.ForEach(p => promotionTable.Add(p));
				PromotionTable = promotionTable;

				// Backup Copy
				promotions = result;
			}
			catch (Exception e)
			{
				System.Windows.MessageBox.Show("Database Error.", "Ooops!");
				Debug.WriteLine(e.Message);
			}
		}

		private void RetrivePromotionDetail(int? promotionId = null)
		{
			IDBService database = Ioc.Default.GetService<IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			try
			{
				List<PromotionDetail> result = database.ExecuteReader<PromotionDetail>(
					"SELECT PromotionDetail.Id as 'Id', PromotionDetail.PromotionId as 'PromotionId', PromotionDetail.ProductId as 'ProductId', Product.CategoryId as 'Category', " +
					"Product.Name as 'Name', Product.Price as 'Price', Product.Quantity as 'Quantity', PromotionDetail.Discount as 'Discount' " +
					"FROM PromotionDetail INNER JOIN Product ON PromotionDetail.ProductId = Product.Id;");
				
				this.promotionDetailTable.Clear();
				result.ForEach((p) =>
				{
					if (promotionId == null || promotionId == p.PromotionId)
					{
						promotionDetailTable.Add(p);
					}
					PromotionDetailTable = promotionDetailTable;
				});

				// Bug fix
				if (selectedPromotion == null)
				{
					foreach (Promotion promotion in promotions)
					{
						if (promotion.Id == promotionId)
						{
							selectedPromotion = promotion;
							break;
						}
					}
				}

				promotionDetails = result;
			}
			catch (Exception e)
			{
				System.Windows.MessageBox.Show("Database Error.", "Ooops!");
				Debug.WriteLine(e.Message);
			}
		}

		private void RefreshAll()
		{
			// Promotion
			if (promotions != null)
			{
				promotions.Clear();
			}

			if (promotionTable != null)
			{
				promotionTable.Clear();
			}

			PromotionTable.Clear();
			isSelectAllPromotion = false;

			SelectedPromotion = null;

			// Promotion Detail
			if (promotionDetails != null)
			{
				promotionDetails.Clear();
			}

			if (promotionDetailTable != null)
			{
				promotionDetailTable.Clear();
			}

			PromotionDetailTable.Clear();
			isSelectAllPromotionDetail = false;
		}
	}
}
