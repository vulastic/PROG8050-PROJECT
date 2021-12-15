using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG8050_PROJECT.Models
{
    class Customer:Notifier
    {
		private int id;
		public int Id
		{
			get => id;
			set
			{
				id = value;
				OnPropertyChanged("Id");
			}
		}
		private int accountId;
		public int AccountId
		{
			get => accountId;
			set
			{
				accountId = value;
				OnPropertyChanged("AccountId");
			}
		}
		private string email;
		public string Email
		{
			get => email;
			set
			{
				email = value;
				OnPropertyChanged("Email");
			}
		}
        private string password;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        private string firstName;
		public string FirstName
		{
			get => firstName;
			set
			{
				firstName = value;
				OnPropertyChanged("FirstName");
			}
		}
		

		private string lastName;
		public string LastName
		{
			get => lastName;
			set
			{
				lastName = value;
				OnPropertyChanged("LastName");
			}
		}
		private string phoneNo;
		public string PhoneNo
		{
			get => phoneNo;
			set
			{
				phoneNo = value;
				OnPropertyChanged("PhoneNo");
			}
		}

		private int gender;
		public int Gender
		{
			get => gender;
			set
			{
				gender = value;
				OnPropertyChanged("Gender");
			}
		}

		
	}
}
