using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using MvvmDialogs;
using PROG8050_PROJECT.Core.Services;

namespace PROG8050_PROJECT.ViewModels.Modals
{
	class ForgotPasswordViewModel : ObservableRecipient, IModalDialogViewModel
	{
		private enum EnableCondition
		{
			Email	  = 1 << 0,							// 0b00000001
			FirstName = 1 << 1,							// 0b00000010
			LastName  = 1 << 2,							// 0b00000100
			Complete  = Email | FirstName | LastName	// 0b00000111

		}

		private bool? dialogResult;
		public bool? DialogResult
		{
			get => dialogResult;
			private set => SetProperty(ref dialogResult, value);
		}

		private int isEnabled = 0;
		public bool IsEnabled
		{
			get => isEnabled == (int)EnableCondition.Complete;
			set
			{
				this.OnPropertyChanged("IsEnabled");
			}
		}

		private string email;
		public string Email
		{
			get => email;
			set
			{
				email = value.Replace(" ", String.Empty);

				Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$");
				Match match = regex.Match(email);
				if (match.Success)
				{
					isEnabled |= (sbyte)EnableCondition.Email;
					IsEnabled = true;
				}
				else
				{
					isEnabled &= ~(sbyte)EnableCondition.Email;
					IsEnabled = false;
				}
			}
		}

		private string firstname;
		public string FirstName
		{
			get => firstname;
			set
			{
				firstname = value;
				if (firstname.Length > 0)
				{
					isEnabled |= (sbyte)EnableCondition.FirstName;
					IsEnabled = true;
				}
				else
				{
					isEnabled &= ~(sbyte)EnableCondition.FirstName;
					IsEnabled = false;
				}
				this.OnPropertyChanged("FirstName");
			}
		}

		private string lastname;
		public string LastName
		{
			get => lastname;
			set
			{
				lastname = value;
				if (lastname.Length > 0)
				{
					isEnabled |= (sbyte)EnableCondition.LastName;
					IsEnabled = true;
				}
				else
				{
					isEnabled &= ~(sbyte)EnableCondition.LastName;
					IsEnabled = false;
				}
				this.OnPropertyChanged("LastName");
			}
		}

		public ICommand Close { get; }
		public ICommand FindPassword { get; }

		public ForgotPasswordViewModel()
		{
			Close = new RelayCommand<object>(CloseWindow);
			FindPassword = new RelayCommand<object>(FindPasswordEvent);
		}

		private void CloseWindow(object sender)
		{
			(sender as Window).Close();
		}

		private void FindPasswordEvent(object sender)
		{
			// Input Validations
			if (email == null || String.IsNullOrEmpty(email))
			{
				System.Windows.MessageBox.Show("E-mail is empty.", "Ooops!");
				return;
			}

			if (firstname == null || String.IsNullOrEmpty(firstname))
			{
				System.Windows.MessageBox.Show("First name is emtpy.", "Ooops!");
				return;
			}

			if (lastname == null || String.IsNullOrEmpty(lastname))
			{
				System.Windows.MessageBox.Show("Last name is emtpy.", "Ooops!");
				return;
			}

			// Email validation
			Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$");
			Match match = regex.Match(email);
			if (!match.Success)
			{
				System.Windows.MessageBox.Show("Invalid e-mail format.", "Ooops!");
				return;
			}

			// Insert DB
			IDBService database = Ioc.Default.GetService<IDBService>();

			if (database.IsOpen)
			{
				Dictionary<string, object> param = new Dictionary<string, object>();
				param.Add("@email", this.email);
				param.Add("@firstname", firstname);
				param.Add("@lastname", lastname);

				try
				{
					DataTable result = database.ExecuteReader(
						"SELECT Account.Password From Account INNER JOIN Admin ON Account.Id = Admin.AccountId " +
						"WHERE Account.Email = @email and Admin.FirstName = @firstname and Admin.LastName = @lastname;", param);

					if (result.Rows.Count > 0)
					{
						string password = result.Rows[0][0].ToString();
						System.Windows.MessageBox.Show($"Your password is '{password}'.", "Found password!");
						this.DialogResult = true;
					}
					else
					{
						System.Windows.MessageBox.Show($"Cannot found your password.", "Not found password!");
					}
				}
				catch (Exception e)
				{
					System.Windows.MessageBox.Show("Fail to find password.", "Ooops!");
					Debug.WriteLine(e.Message);
				}

			}
		}
	}

}
