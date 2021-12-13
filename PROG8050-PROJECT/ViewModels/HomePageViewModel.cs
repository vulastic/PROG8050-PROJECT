using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using PROG8050_PROJECT.Core;

namespace PROG8050_PROJECT.ViewModels
{
	class HomePageViewModel : ObservableObject
	{
		public ICommand Loaded { get; }

		public HomePageViewModel()
		{
			Loaded = new RelayCommand<object>(LoadedPage);
		}
		

		private void LoadedPage(object sender)
		{
			
		}
	}
}
