using PROG8050_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using PROG8050_PROJECT.Core.Services;
using System.Text.RegularExpressions;
using System.Data;
using MvvmDialogs;
using PROG8050_PROJECT.ViewModels.Modals;
using System.Collections.ObjectModel;

namespace PROG8050_PROJECT.ViewModels
{
	class LoginPageViewModel : ObservableRecipient
	{
		private enum EnableCondition
		{
			Email	 = 1 << 0,				// 0b00000001
			Password = 1 << 1,				// 0b00000010
			Complete = Email | Password		// 0b00000011
				
		}

		private readonly IDialogService dialogService;

		public ICommand SignIn { get; }
		public ICommand ForgotPassword { get; }
		public ICommand CreateNewAccount { get; }

		private int isEnabled = (int)EnableCondition.Complete;	// Tempral. Real is 0;
		public bool IsEnabled
		{
			get => isEnabled == (int) EnableCondition.Complete;
			set
			{
				this.OnPropertyChanged("IsEnabled");
			}
		}

		private string email = "tester@gmail.com";
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

				this.OnPropertyChanged("Email");
			}
		}

		private SecureString password;
		public SecureString Password
		{
			get => password;
			set
			{
				password = value;
				if (value.Length > 0)
				{
					isEnabled |= (sbyte)EnableCondition.Password;
					IsEnabled = true;
				}
				else
				{
					isEnabled &= ~(sbyte)EnableCondition.Password;
					IsEnabled = false;
				}
			}
		}

		public LoginPageViewModel()
		{
			this.dialogService = Ioc.Default.GetService<IDialogService>();

			SignIn = new RelayCommand<object>(SignInEvent);
			ForgotPassword = new RelayCommand<object>(ForgotPasswordEvent);
			CreateNewAccount = new RelayCommand<object>(CreateNewAccountEvent);
		}

		private void SignInEvent(object sender)
		{
			if (email == null || String.IsNullOrEmpty(email))
			{
				System.Windows.MessageBox.Show("E-mail is empty.", "Ooops!");
				return;
			}

			if (password == null || password.Length == 0)
			{
				System.Windows.MessageBox.Show("Password is empty.", "Ooops!");
				return;
			}

			try
			{
				// Login
				IDBService database = Ioc.Default.GetService<IDBService>();

				if (!database.IsOpen)
				{
					System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
					return;
				}


				Dictionary<string, object> param = new Dictionary<string, object>();
				param.Add("@email", this.email);
				param.Add("@password", new System.Net.NetworkCredential(string.Empty, password).Password);

				DataTable result = database.ExecuteReader("SELECT id FROM account WHERE Email = @email AND Password = @password LIMIT 1;", param);
				if (result.Rows.Count == 0)
				{
					System.Windows.MessageBox.Show("Invalid e-mail or password", "Ooops!");
					return;
				}

				int accountId = Convert.ToInt32(result.Rows[0][0]);

				// Select all user account
				List<AdminUser> users = database.ExecuteReader<AdminUser>($"SELECT * FROM Admin WHERE AccountId = {accountId} LIMIT 1;");
				if (users.Count == 0)
				{
					System.Windows.MessageBox.Show("Cannot find admin user in the database.", "Ooops!");
					return;
				}

				ILoginService loginService = (LoginService)Ioc.Default.GetService<ILoginService>();
				loginService.SetLogin(this.email, users[0]);

				// Message
				System.Windows.MessageBox.Show("Login success.", "Welcome!");

				// Go back to previous page
				INavigationService navigation = Ioc.Default.GetService<INavigationService>();
				if (navigation.CanGoBack)
				{
					navigation.GoBack();
				}
			}
			catch (Exception e)
			{
				System.Windows.MessageBox.Show("Database Error", "Ooops!");
				Console.WriteLine(e.Message);
				return;
			}
		}

		private void ForgotPasswordEvent(object sender)
		{
			ForgotPasswordViewModel modalView = new ForgotPasswordViewModel();

			bool? success = dialogService.ShowDialog(this, modalView);
			if (success == true)
			{

			}
		}

		private void CreateNewAccountEvent(object sender)
		{
			CreateNewAccountViewModel modalView = new CreateNewAccountViewModel();

			bool? success = dialogService.ShowDialog(this, modalView);
			if (success == true)
			{
				if (modalView.Email != null)
				{
					this.Email = modalView.Email;
				}
			}
		}
	}
}
