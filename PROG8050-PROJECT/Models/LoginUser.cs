using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PROG8050_PROJECT.Models
{
	class LoginUser : Notifier
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
	}
}
