using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using MvvmDialogs;
using PROG8050_PROJECT.Core;
using PROG8050_PROJECT.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
	class CreateNewAccountViewModel : ObservableRecipient, IModalDialogViewModel
	{
		private bool? dialogResult;
		public bool? DialogResult
		{
			get => dialogResult;
			private set => SetProperty(ref dialogResult, value);
		}

		private string email;
		public string Email
		{
			get => email;
			set
			{
				email = value.Replace(" ", String.Empty);
				this.OnPropertyChanged("Email");
			}
		}

		private string firstname;
		public string FirstName
		{
			get => firstname;
			set
			{
				firstname = value;
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
				this.OnPropertyChanged("LastName");
			}
		}

		private Gender gender = Core.Gender.Male;
		public int Gender
		{
			get => (int)gender;
			set
			{
				gender = (Gender)value;
				this.OnPropertyChanged("Gender");
			}
		}

		private string phoneNo;
		public string PhoneNo
		{
			get => phoneNo;
			set
			{
				string temp = Regex.Replace(value, @"[^\d]", "");
				if (temp.Length >= 13)
				{
					temp = temp.Substring(0, 13);
				}

				switch (temp.Length)
				{
					case 5:
						phoneNo = Regex.Replace(temp, @"(\d{1})(\d{4})", "$1-$2");
						break;
					case 6:
						phoneNo = Regex.Replace(temp, @"(\d{2})(\d{4})", "$1-$2");
						break;
					case 7:
						phoneNo = Regex.Replace(temp, @"(\d{3})(\d{4})", "$1-$2");
						break;
					case 8:
						phoneNo = Regex.Replace(temp, @"(\d{1})(\d{3})(\d{4})", "$1-$2-$3");
						break;
					case 9:
						phoneNo = Regex.Replace(temp, @"(\d{2})(\d{3})(\d{4})", "$1-$2-$3");
						break;
					case 10:
						phoneNo = Regex.Replace(temp, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
						break;
					case 11:
						phoneNo = Regex.Replace(temp, @"(\d{1})(\d{3})(\d{3})(\d{4})", "(+$1) $2-$3-$4");
						break;
					case 12:
						phoneNo = Regex.Replace(temp, @"(\d{2})(\d{3})(\d{3})(\d{4})", "(+$1) $2-$3-$4");
						break;
					case 13:
						phoneNo = Regex.Replace(temp, @"(\d{3})(\d{3})(\d{3})(\d{4})", "(+$1) $2-$3-$4");
						break;
					default:
						phoneNo = temp;
						break;

				}
				this.OnPropertyChanged("PhoneNo");
			}
		}

		public SecureString Password { get; set; }
		public SecureString ConfirmPassword { get; set; }

		public ICommand Close { get; }
		public ICommand SignUp { get; }

		public CreateNewAccountViewModel()
		{
			Close = new RelayCommand<object>(CloseWindow);
			SignUp = new RelayCommand<object>(SignUpEvent);
		}

		private void CloseWindow(object sender)
		{
			this.DialogResult = false;
		}

		private void SignUpEvent(object sender)
		{
			// Input validations
			if (email == null || String.IsNullOrEmpty(email))
			{
				System.Windows.MessageBox.Show("E-mail is empty.", "Ooops!");
				return;
			}

			if (Password == null || Password.Length == 0)
			{
				System.Windows.MessageBox.Show("Password is empty.", "Ooops!");
				return;
			}

			if (ConfirmPassword == null || ConfirmPassword.Length == 0)
			{
				System.Windows.MessageBox.Show("Confirm password is emtpy.", "Ooops!");
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

			if (phoneNo == null || String.IsNullOrEmpty(phoneNo))
			{
				System.Windows.MessageBox.Show("Phone number is emtpy.", "Ooops!");
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

			// Password validation
			string passwd = new System.Net.NetworkCredential(string.Empty, Password).Password;
			string confirmPasswd = new System.Net.NetworkCredential(string.Empty, ConfirmPassword).Password;
			if (passwd != confirmPasswd)
			{
				System.Windows.MessageBox.Show("Mismatched Passwords.", "Ooops!");
				return;
			}

			// Phone number
			int numericPhoneNumber = 0;
			if (!int.TryParse(Regex.Replace(phoneNo, @"[^\d]", ""), out numericPhoneNumber))
			{
				System.Windows.MessageBox.Show("Invalid phone number format.", "Ooops!");
				return;
			}

			// Insert DB
			IDBService database = Ioc.Default.GetService<IDBService>();

			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			try
			{
				DataTable user = database.ExecuteReader($"SELECT * FROM Account WHERE Email = '{this.Email}';");
				if (user.Rows.Count > 0)
				{
					System.Windows.MessageBox.Show($"'{this.Email}' is already exist.", "Opps!");
					return;
				}

				Dictionary<string, object> param = new Dictionary<string, object>();
				param.Add("@email", this.email);
				param.Add("@password", passwd);

				int affectedRow = database.ExecuteNonQuery("insert into account (email, password) values (@email, @password);", param);
				if (affectedRow <= 0)
				{
					System.Windows.MessageBox.Show("Fail to create new account (1).", "Opps!");
					return;
				}

				DataTable result = database.ExecuteReader("select id from account where email = @email and password = @password limit 1;", param);
				if (result.Rows.Count <= 0)
				{
					System.Windows.MessageBox.Show("Fail to create new account (2).", "Opps!");
					return;
				}

				int accountId = Convert.ToInt32(result.Rows[0][0]);

				Dictionary<string, object> adminParam = new Dictionary<string, object>();
				adminParam.Add(@"id", accountId);
				adminParam.Add(@"firstname", firstname);
				adminParam.Add(@"lastname", lastname);
				adminParam.Add(@"gender", (int)gender);
				adminParam.Add(@"phoneno", numericPhoneNumber);

				// Insert into admin
				affectedRow = database.ExecuteNonQuery("INSERT INTO Admin (AccountId, FirstName, LastName, Gender, PhoneNo) VALUES (@id, @firstname, @lastname, @gender, @phoneno);", adminParam);
				if (affectedRow <= 0)
				{
					System.Windows.MessageBox.Show("Fail to create new account (3).", "Opps!");
					return;
				}

				// Success
				System.Windows.MessageBox.Show("Welcome to join us.", "Congraturations!");
				this.DialogResult = true;
			}
			catch (Exception e)
			{
				System.Windows.MessageBox.Show("Database Error.", "Ooops!");
				Debug.WriteLine(e.Message);
			}
		}
	}
}
