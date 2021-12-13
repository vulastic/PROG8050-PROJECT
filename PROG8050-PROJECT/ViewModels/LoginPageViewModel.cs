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

namespace PROG8050_PROJECT.ViewModels
{
	class LoginPageViewModel : ObservableObject
	{
		/*
		private Account loginUser = new Account();
		public Account LoginUser
		{
			get => loginUser;
			set
			{
				loginUser = value;
				OnPropertyChanged("LoginUser");
			}
		}

		private ICommand login;
		public ICommand Login => (this.login) ??= new RelayCommand(parameter =>
		{
			if (String.IsNullOrEmpty(loginUser.Email))
			{
				System.Windows.MessageBox.Show("Please Enter the user email");
				return;
			}

			PasswordBox pwBox = parameter as PasswordBox;
			if (String.IsNullOrEmpty(pwBox.Password))
			{
				System.Windows.MessageBox.Show("Please Enter the password");
				return;
			}

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
		});
		*/
	}
}
