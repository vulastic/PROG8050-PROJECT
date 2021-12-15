using Microsoft.Toolkit.Mvvm.ComponentModel;
using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		public ICommand Close { get; }



		private void CloseWindow(object sender)
		{
			(sender as Window).Close();
		}
	}
}
