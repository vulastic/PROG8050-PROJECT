using PROG8050_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PROG8050_PROJECT.ViewModels
{
   class NewCustomerViewModel:Notifier
    {
		private Customer customer = new Customer();
		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		public Customer Customer
		{
			get => customer;
			set
			{
				customer = value;
				OnPropertyChanged("Customer");
			}
		}

		private ICommand addCustomer;
		public ICommand AddCustomer => (this.addCustomer) ??= new RelayCommand(parameter =>
		{
			if (String.IsNullOrEmpty(customer.Email))
			{
				System.Windows.MessageBox.Show("Please Enter the user email");
				return;
			}
			if (String.IsNullOrEmpty(customer.Name))
			{
				System.Windows.MessageBox.Show("Please Enter the Name");
				return;
			}
			if (String.IsNullOrEmpty(customer.Gender.ToString()))
			{
				System.Windows.MessageBox.Show("Please select a Gender");
				return;
			}
			if (String.IsNullOrEmpty(customer.PhoneNo))
			{
				System.Windows.MessageBox.Show("Please Enter the user phoneNo");
				return;
			}


			SQLiteDBManager dbManager = SQLiteDBManager.Instance;

			if (!dbManager.IsOpen())
			{
				System.Windows.MessageBox.Show("Database is not connected");
			}

			Dictionary<string, object> param = new Dictionary<string, object>();
			param.Add("@email", customer.Email);
			param.Add("@name", customer.Name);
			param.Add("@gender", customer.Gender);
			param.Add("@phoneNo", customer.PhoneNo);
			param.Add("@password", new string(Enumerable.Repeat(chars, 8)
											.Select(s => s[new Random().Next(s.Length)]).ToArray()));


			List<Customer> tempUser = dbManager.ExecuteReader<Customer>("select Email from user where email = @email", param);

			//List<Customer> tempUser = dbManager.ExecuteReader<Customer>("select Email, name, gender, phone from user where email = @email", param);
			if (tempUser.Count() > 0)
			{
				System.Windows.MessageBox.Show($"User @email already exists!");
			}
			else
			{
				int result = dbManager.ExecuteNonQuery("insert into user (email, Password, Name, Gender, Phone) values(@email,@password,@name,@gender,@phoneNo)", param);
				if (result == 1)
				{
					System.Windows.MessageBox.Show($"User added successfully");
				}
				else
				{
					System.Windows.MessageBox.Show($"User addition failed");
					return;
				}
			}
		});


	}
}
