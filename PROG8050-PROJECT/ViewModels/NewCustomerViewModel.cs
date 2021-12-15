using PROG8050_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PROG8050_PROJECT.ViewModels
{
   class NewCustomerViewModel:Notifier
    {
		private Customer customer = new Customer();
		private Account account = new Account();
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
			PasswordBox pwBox = parameter as PasswordBox;
			if (String.IsNullOrEmpty(pwBox.Password))
			{
				System.Windows.MessageBox.Show("Please Enter the Password");
				return;
			}
			if (String.IsNullOrEmpty(customer.FirstName))
			{
				System.Windows.MessageBox.Show("Please Enter the First Name");
				return;
			}
			if (String.IsNullOrEmpty(customer.LastName))
			{
				System.Windows.MessageBox.Show("Please Enter the Last Name");
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
			param.Add("@password", pwBox.Password);
			param.Add("@firstname", customer.FirstName);
			param.Add("@lastname", customer.LastName);
			param.Add("@gender", customer.Gender);
			param.Add("@phoneNo", customer.PhoneNo);
			param.Add("@accounttype", 1);


			List<Customer> tempUser = dbManager.ExecuteReader<Customer>("select Email from Account where email = @email", param);

            if (tempUser.Count() > 0)
            {
                System.Windows.MessageBox.Show($"User @email already exists!");
            }
            else
            {
                int result = dbManager.ExecuteNonQuery("insert into Account (Email, Password,Type) values(@email,@password,@accounttype)", param);
                if (result == 1)
                {
                    List<Account> accounts=dbManager.ExecuteReader<Account>("Select * from Account where email=@email", param);
                    if (accounts.Count>0)
                    {
						param.Add("@accountId",accounts[0].Id); 
						result = dbManager.ExecuteNonQuery("insert into Customer (AccountId, FirstName, LastName, PhoneNo,Gender) values(@accountId,@firstname,@lastname,@phoneNo,@gender)", param);
                        if (result == 1)
                        {
							MessageBoxResult messageBox = System.Windows.MessageBox.Show($"Customer {customer.Email} added successfully");
							//System.Windows.Controls.btnClose.Click();
							return;
						}
						else
						{
							System.Windows.MessageBox.Show($"Customer addition failed");
							return;
						}
					}
                }
                else
                {
                    System.Windows.MessageBox.Show($"Customer addition failed");
                    return;
                }
            }
        });


	}
}
