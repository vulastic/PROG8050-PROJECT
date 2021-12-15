using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using PROG8050_PROJECT.Core.Services;

namespace PROG8050_PROJECT.ViewModels
{
	class PromotionPageViewModel : ObservableRecipient
	{
		public ICommand Loaded { get; }
		public ICommand AddCategoryPromotion { get; }
		public ICommand SearchCategoryPromotion { get; }
		public ICommand AddProductPromotion { get; }
		public ICommand SearchProductPromotion { get; }
		public ICommand DeleteProductPromotion { get; }

		public PromotionPageViewModel()
		{
			Loaded = new RelayCommand<object>(LoadedPage);
		}

		private void LoadedPage(object sender)
		{
			ILoginService service = Ioc.Default.GetService<ILoginService>();
			if (!service.IsLogin)
			{
				Ioc.Default.GetService<INavigationService>().Navigate<LoginPageViewModel>();
			}
		}
	}
}
