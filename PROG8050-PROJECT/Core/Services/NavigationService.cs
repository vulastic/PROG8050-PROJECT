using System;
using System.Collections.Generic;
using System.Windows.Controls;
using PROG8050_PROJECT.Views;
using PROG8050_PROJECT.ViewModels;

namespace PROG8050_PROJECT.Core.Services
{
	class NavigationService : INavigationService
	{
		private readonly Dictionary<Type, Type> viewMapping = new()
		{
			[typeof(LoginPageViewModel)] = typeof(LoginPage),
			[typeof(HomePageViewModel)] = typeof(HomePage),
			[typeof(ProductPageViewModel)] = typeof(ProductPage)
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
