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
		private enum LoginCondition
		{
			Email = 1,
			Password = 2,
			Complete = 3
				
		}

		private readonly IDialogService dialogService;

		public ICommand SignIn { get; }
		public ICommand ForgotPassword { get; }
		public ICommand CreateNewAccount { get; }

		private sbyte canLogin = (sbyte)LoginCondition.Complete;
		public bool CanLogin
		{
			get => canLogin == (sbyte)LoginCondition.Complete;
			set
			{
				this.OnPropertyChanged("CanLogin");
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
					canLogin |= (sbyte)LoginCondition.Email;
					CanLogin = true;
				}
				else
				{
					canLogin &= ~(sbyte)LoginCondition.Email;
					CanLogin = false;
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
					canLogin |= (sbyte)LoginCondition.Password;
					CanLogin = true;
				}
				else
				{
					canLogin &= ~(sbyte)LoginCondition.Password;
					CanLogin = false;
				}
			}
		}

		public ObservableCollection<string> Texts { get; } = new ObservableCollection<string>();

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

		}

		private void CreateNewAccountEvent(object sender)
		{
			CreateNewAccountViewModel modalView = new CreateNewAccountViewModel();

			bool? success = dialogService.ShowDialog(this, modalView);
			if (success == true)
			{
				Texts.Add(modalView.Email);
				if (Texts.Count > 0)
				{
					this.Email = Texts[0];
				}
			}
		}
	}
}
