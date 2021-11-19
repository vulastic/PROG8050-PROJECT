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

namespace PROG8050_PROJECT.ViewModels
{
	class MainWindowModel : Notifier
	{
		private ICommand closeWindow;
		public ICommand CloseWindow => (this.closeWindow) ??= new RelayCommand(parameter =>
		{
			(parameter as Window).Close();
		});

		private ICommand restoreWindow;
		public ICommand RestoreWindow => (this.restoreWindow) ??= new RelayCommand(parameter =>
		{
			Window window = parameter as Window;
			if (window.WindowState == WindowState.Normal)
			{
				window.WindowState = WindowState.Maximized;
			}
			else
			{
				window.WindowState = WindowState.Normal;
			}
		});

		private ICommand minimizeWindow;
		public ICommand MinimizeWindow => (this.minimizeWindow) ??= new RelayCommand(paramater =>
		{
			(paramater as Window).WindowState = WindowState.Minimized;
		});

		private ICommand maximizeWindow;
		public ICommand MaximizeWindow => (this.maximizeWindow) ??= new RelayCommand(parameter =>
		{
			MouseButtonEventArgs args = parameter as MouseButtonEventArgs;

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
		});

		private ICommand showHomeView;
		public ICommand ShowHomeView =>(this.showHomeView) ??= new RelayCommand(parameter =>
		{
			Frame navi = parameter as Frame;
			navi.Navigate(new System.Uri("./Views/Home.xaml", UriKind.RelativeOrAbsolute));
		});
	}

	public class NavigationConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
