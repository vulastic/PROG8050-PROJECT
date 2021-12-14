using Microsoft.Toolkit.Mvvm.ComponentModel;
using PROG8050_PROJECT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG8050_PROJECT.Models
{
	class AdminUser : ObservableRecipient
	{
		private int id;
		public int Id
		{
			get => id;
			set
			{
				id = value;
			}
		}

		private int accountId;
		public int AccountId
		{
			get => accountId;
			set
			{
				accountId = value;
				this.OnPropertyChanged("CanLogin");
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

		private Gender gender;
		public Gender Gender
		{
			get => gender;
			set
			{
				gender = value;
				this.OnPropertyChanged("CanLogin");
			}
		}

		private int phoneno;
		public int PhoneNo
		{
			get => phoneno;
			set
			{
				phoneno = value;
				this.OnPropertyChanged("PhoneNo");
			}
		}
	}
}
