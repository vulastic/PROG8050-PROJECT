using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG8050_PROJECT.Models
{
    class Customer:Notifier
    {
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

		private string name;
		public string Name
		{
			get => name;
			set
			{
				name = value;
				OnPropertyChanged("Name");
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
	}
}
