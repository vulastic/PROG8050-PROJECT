using System;
using System.Collections.Generic;
using System.Windows.Controls;
using PROG8050_PROJECT.ViewModels;

namespace PROG8050_PROJECT.Core.Services
{
	class NavigationService : INavigationService
	{
		private readonly Dictionary<Type, Uri> viewMapping = new()
		{
			[typeof(LoginPageViewModel)] = new System.Uri("./Views/LoginPage.xaml", UriKind.Relative),
			[typeof(PromotionPageViewModel)] = new System.Uri("./Views/PromotionPage.xaml", UriKind.Relative),
			[typeof(OrderPageViewModel)] = new System.Uri("./Views/OrderPage.xaml", UriKind.Relative),
			[typeof(CustomerPageViewModel)] = new System.Uri("./Views/CustomerPage.xaml", UriKind.Relative),
			[typeof(AdminPageViewModel)] = new System.Uri("./Views/AdminPage.xaml", UriKind.Relative)
		};

		private readonly Dictionary<string, Uri> stringViewMapping = new()
		{
			["LoginPage".ToLower()] = new System.Uri("./Views/LoginPage.xaml", UriKind.Relative),
			["HomePage".ToLower()] = new System.Uri("./Views/HomePage.xaml", UriKind.Relative),
			["ProductPage".ToLower()] = new System.Uri("./Views/ProductPage.xaml", UriKind.Relative),
			["CategoryPage".ToLower()] = new System.Uri("./Views/CategoryPage.xaml", UriKind.Relative),
			["PromotionPage".ToLower()] = new System.Uri("./Views/PromotionPage.xaml", UriKind.Relative),
			["CustomerPage".ToLower()] = new System.Uri("./Views/CustomerPage.xaml", UriKind.Relative),
			["OrderPage".ToLower()] = new System.Uri("./Views/OrderPage.xaml", UriKind.Relative),
			["AdminPage".ToLower()] = new System.Uri("./Views/AdminPage.xaml", UriKind.Relative)
		};

		private readonly Frame frame;

		public NavigationService(Frame frame)
		{
			this.frame = frame;
		}

		public string Next { get; set; }

		public bool CanGoBack => this.frame.CanGoBack;

		public void GoBack() => this.frame.GoBack();

		public void Navigate<T>(object args)
		{
			this.frame.Navigate(this.viewMapping[typeof(T)], args);
		}

		public void Navigate(string name)
		{
			if (name != null && stringViewMapping.ContainsKey(name.ToLower()))
			{
				this.frame.Navigate(this.stringViewMapping[name.ToLower()]);
			}
		}

		public void Navigate(Uri uri)
		{
			this.frame.Navigate(uri);
		}
	}
}
