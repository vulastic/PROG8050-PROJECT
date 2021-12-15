using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG8050_PROJECT.Models
{
	class Account : Notifier
	{
		private Int64 id;
		public Int64 Id
		{
			get => id;
			set
			{
				id = value;
				OnPropertyChanged("Id");
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
		private int type;
		public int Type
		{
			get => type;
			set
			{
				type = value;
				OnPropertyChanged("Type");
			}
		}
	}
}
