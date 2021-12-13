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
			[typeof(LoginPageViewModel)]	= new System.Uri("./Views/LoginPage.xaml", UriKind.Relative),
			[typeof(HomePageViewModel)]		= new System.Uri("./Views/HomePage.xaml", UriKind.Relative),
			[typeof(ProductPageViewModel)]	= new System.Uri("./Views/HomePage.xaml", UriKind.Relative)
		};
		
		private readonly Frame frame;

		public NavigationService(Frame frame)
		{
			this.frame = frame;
		}

		public bool CanGoBack => this.frame.CanGoBack;

		public void GoBack() => this.frame.GoBack();

		public void Navigate<T>(object args)
		{
			this.frame.Navigate(this.viewMapping[typeof(T)], args);
		}
	}
}
