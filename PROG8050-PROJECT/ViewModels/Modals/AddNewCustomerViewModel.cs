using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using MvvmDialogs;
using PROG8050_PROJECT.Core;
using PROG8050_PROJECT.Core.Services;
using PROG8050_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PROG8050_PROJECT.ViewModels.Modals
{
	class AddNewCustomerViewModel : ObservableRecipient, IModalDialogViewModel
	{
		private bool? dialogResult;
		public bool? DialogResult
		{
			get => dialogResult;
			private set => SetProperty(ref dialogResult, value);
		}

		public string Email { get; set; }
		public Customer Customer { get; set; } = new Customer();
		
		public ICommand Close { get; }
		public ICommand AddNewCustomer { get; }

		public SecureString Password { get; set; }

		public AddNewCustomerViewModel()
		{
			Close = new RelayCommand<object>(CloseWindow);
			AddNewCustomer = new RelayCommand<object>(AddNewCustomerEvent);
		}

		private void CloseWindow(object sender)
		{
			this.DialogResult = false;
		}

		private void AddNewCustomerEvent(object sender)
		{
			if (String.IsNullOrEmpty(Email))
			{
				System.Windows.MessageBox.Show("Please Enter the user email");
				return;
			}

			if (Password.Length <= 0)
			{
				System.Windows.MessageBox.Show("Please Enter the Password");
				return;
			}

			if (String.IsNullOrEmpty(Customer.FirstName))
			{
				System.Windows.MessageBox.Show("Please Enter the First Name");
				return;
			}

			if (String.IsNullOrEmpty(Customer.LastName))
			{
				System.Windows.MessageBox.Show("Please Enter the Last Name");
				return;
			}

			if (String.IsNullOrEmpty(Customer.Gender.ToString()))
			{
				System.Windows.MessageBox.Show("Please select a Gender");
				return;
			}

			if (Customer.PhoneNo == 0)
			{
				System.Windows.MessageBox.Show("Please Enter the user phoneNo");
				return;
			}

			// Email validation
			Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$");
			Match match = regex.Match(Email);
			if (!match.Success)
			{
				System.Windows.MessageBox.Show("Invalid e-mail format.", "Ooops!");
				return;
			}

			string passwd = new System.Net.NetworkCredential(string.Empty, Password).Password;

			IDBService database = Ioc.Default.GetService<IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			string password = new System.Net.NetworkCredential(string.Empty, Password).Password;

			Dictionary<string, object> param = new Dictionary<string, object>();
			param.Add("@email", Email);
			param.Add("@password", password);
			param.Add("@firstname", Customer.FirstName);
			param.Add("@lastname", Customer.LastName);
			param.Add("@gender", Customer.Gender);
			param.Add("@phoneNo", Customer.PhoneNo);
			param.Add("@accounttype", (int)Customer.Gender);

			List<Customer> tempUser = database.ExecuteReader<Customer>("select Email from Account where email = @email", param);
			if (tempUser.Count() > 0)
			{
				System.Windows.MessageBox.Show($"User @email already exists!");
				return;
			}

			try
			{
				int result = database.ExecuteNonQuery("insert into Account (Email, Password,Type) values(@email,@password,@accounttype)", param);
				if (result == 1)
				{
					List<Account> accounts = database.ExecuteReader<Account>("Select * from Account where email=@email", param);
					if (accounts.Count > 0)
					{
						param.Add("@accountId", accounts[0].Id);
						result = database.ExecuteNonQuery("insert into Customer (AccountId, FirstName, LastName, PhoneNo,Gender) values(@accountId,@firstname,@lastname,@phoneNo,@gender)", param);
						if (result == 1)
						{
							System.Windows.MessageBox.Show($"Customer {Email} added successfully");
						}
						else
						{
							System.Windows.MessageBox.Show($"Customer addition failed");
						}

						this.DialogResult = true;
					}
				}
				else
				{
					System.Windows.MessageBox.Show($"Customer addition failed");
					return;
				}
			}
			catch (Exception ex)
			{
				System.Windows.MessageBox.Show("Database Error.", "Ooops!");
				Debug.WriteLine(ex.Message);
			}
		}
	}
}
