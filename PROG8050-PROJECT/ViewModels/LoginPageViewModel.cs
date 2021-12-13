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

namespace PROG8050_PROJECT.ViewModels
{
	class LoginPageViewModel : ObservableObject
	{
		private enum LoginCondition
		{
			Email = 1,
			Password = 2,
			Complete = 3
				
		}

		public ICommand SignIn { get; }

		public LoginPageViewModel()
		{
			SignIn = new RelayCommand<object>(SignInEvent);
		}

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

			// Login
			IDBService database = Ioc.Default.GetService<IDBService>();

			if (database.IsOpen)
			{
				Dictionary<string, object> param = new Dictionary<string, object>();
				param.Add("@email", this.email);
				param.Add("@password", new System.Net.NetworkCredential(string.Empty, password).Password);

				DataTable result = database.ExecuteReader("select id from Account where email = @email and password = @password limit 1;", param);

				System.Windows.MessageBox.Show($"{result.Rows.Count}");
			}

			System.Windows.MessageBox.Show("sign ok");

			INavigationService navigation = Ioc.Default.GetService<INavigationService>();
			if (navigation.CanGoBack)
			{
				navigation.GoBack();
			}
			/*
			SQLiteDBManager dbManager = SQLiteDBManager.Instance;

			if (!dbManager.IsOpen())
			{
				System.Windows.MessageBox.Show("Database is not connected");
			}

			Dictionary<string, object> param = new Dictionary<string, object>();
			param.Add("@email", loginUser.Email);
			List<Account> tempUser = dbManager.ExecuteReader<Account>("select Email, Password from Account where email = @email", param);

			if (tempUser.Count == 0)
			{
				System.Windows.MessageBox.Show($"Cannot found the user");
				return;
			}

			Account dbUser = tempUser[0];
			if (pwBox.Password != dbUser.Password)
			{
				System.Windows.MessageBox.Show($"Wrong Password");
				return;
			}

			System.Windows.MessageBox.Show($"Login Successful");
			return;
			*/
		}
	}
}
