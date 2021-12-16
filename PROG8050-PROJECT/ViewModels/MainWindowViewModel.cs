using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PROG8050_PROJECT.Core.Services;
using MvvmDialogs;

namespace PROG8050_PROJECT.ViewModels
{
	class MainWindowViewModel : ObservableRecipient
	{
		public ICommand Loaded { get; }
		public ICommand Close { get; }
		public ICommand Restore { get; }
		public ICommand Minimize { get; }
		public ICommand Maximize { get; }

		public ICommand NavigateHome { get; }
		public ICommand NavigateProduct { get; }
		public ICommand NavigateCategory { get; }
		public ICommand NavigatePromotion { get; }
		public ICommand NavigateCustomer { get; }
		public ICommand NavigateOrder { get; }
		public ICommand NavigateAdmin { get; }

		public MainWindowViewModel()
		{
			Loaded = new RelayCommand<object>(LoadWindow);
			Close = new RelayCommand<object>(CloseWindow);
			Restore = new RelayCommand<object>(RestoreWindow);
			Minimize = new RelayCommand<object>(MinimizeWindow);
			Maximize = new RelayCommand<object>(MaximizeWindow);

			NavigateHome = new RelayCommand(NavigateHomePage);
			NavigateProduct = new RelayCommand(NavigateProductPage);
			NavigateCategory = new RelayCommand(NavigateCategoryPage);
			NavigatePromotion = new RelayCommand(NavigatePromotionPage);
			NavigateCustomer = new RelayCommand(NavigateCustomerPage);
			NavigateOrder = new RelayCommand(NavigateOrderPage);
			NavigateAdmin = new RelayCommand(NavigateAdminPage);
		}

		#region RelayCommands
		private void LoadWindow(object sender)
		{
			MainWindow window = sender as MainWindow;

			string connectionString = "Data Source=./../../../database.sqlite";

			// Service Injection
			Ioc.Default.ConfigureServices(
				new ServiceCollection()
				.AddSingleton<INavigationService>(new NavigationService(window.serviceFrame))
				.AddSingleton<IDBService>(new SQLiteDBService(connectionString))
				.AddSingleton<IDialogService, DialogService>() 
				.AddSingleton<ILoginService, LoginService>()
				.BuildServiceProvider());

			Ioc.Default.GetService<INavigationService>().Navigate<HomePageViewModel>();
		}

		private void CloseWindow(object sender)
		{
			(sender as Window).Close();
		}

		private void RestoreWindow(object sender)
		{
			Window window = sender as Window;
			if (window.WindowState == WindowState.Normal)
			{
				window.WindowState = WindowState.Maximized;
			}
			else
			{
				window.WindowState = WindowState.Normal;
			}
		}
		
		private void MinimizeWindow(object sender)
		{
			(sender as Window).WindowState = WindowState.Minimized;
		}

		private void MaximizeWindow(object sender)
		{
			MouseButtonEventArgs args = sender as MouseButtonEventArgs;
			Window window = Window.GetWindow((DependencyObject)args.Source);
			if (args.ClickCount == 1)
			{
				window.DragMove();
				args.Handled = true;
			}
			else if (args.ClickCount == 2)
			{
				if (window.WindowState == WindowState.Normal)
				{
					window.WindowState = WindowState.Maximized;
				}
				else
				{
					window.WindowState = WindowState.Normal;
				}
			}
		}

		private void NavigateHomePage()
		{
			Ioc.Default.GetRequiredService<INavigationService>().Navigate<HomePageViewModel>();
		}

		private void NavigateProductPage()
		{
			ILoginService loginService = Ioc.Default.GetRequiredService<ILoginService>();
			if (!loginService.IsLogin)
			{
				// Set Next to Promotion
				INavigationService navigationService = Ioc.Default.GetRequiredService<INavigationService>();
				navigationService.Next = "ProductPage";

				navigationService.Navigate<LoginPageViewModel>();
				return;
			}

			Ioc.Default.GetRequiredService<INavigationService>().Navigate("ProductPage");
		}

		private void NavigateCategoryPage()
		{
			ILoginService loginService = Ioc.Default.GetRequiredService<ILoginService>();
			if (!loginService.IsLogin)
			{
				// Set Next to Promotion
				INavigationService navigationService = Ioc.Default.GetRequiredService<INavigationService>();
				navigationService.Next = "CategoryPage";

				navigationService.Navigate<LoginPageViewModel>();
				return;
			}

			Ioc.Default.GetRequiredService<INavigationService>().Navigate("CategoryPage");
		}

		private void NavigatePromotionPage()
		{
			ILoginService loginService = Ioc.Default.GetRequiredService<ILoginService>();
			if (!loginService.IsLogin)
			{
				// Set Next to Promotion
				INavigationService navigationService = Ioc.Default.GetRequiredService<INavigationService>();
				navigationService.Next = "PromotionPage";

				navigationService.Navigate<LoginPageViewModel>();
				return;
			}

			Ioc.Default.GetRequiredService<INavigationService>().Navigate<PromotionPageViewModel>();
		}

		private void NavigateCustomerPage()
		{
			Ioc.Default.GetRequiredService<INavigationService>().Navigate<CustomerPageViewModel>();
		}

		private void NavigateOrderPage()
		{
			Ioc.Default.GetRequiredService<INavigationService>().Navigate<OrderPageViewModel>();
		}

		private void NavigateAdminPage()
		{
			Ioc.Default.GetRequiredService<INavigationService>().Navigate<AdminPageViewModel>();
		}
		#endregion
	}
}
