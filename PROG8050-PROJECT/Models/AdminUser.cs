using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROG8050_PROJECT.Core;

namespace PROG8050_PROJECT.Models
{
	class AdminUser
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
			}
		}

		private string firstname;
		public string FirstName
		{
			get => firstname;
			set
			{
				firstname = value;
			}
		}

		private string lastname;
		public string LastName
		{
			get => lastname;
			set
			{
				lastname = value;
			}
		}

		private Gender gender;
		public Gender Gender
		{
			get => gender;
			set
			{
				gender = value;
			}
		}

		private int phoneno;
		public int PhoneNo
		{
			get => phoneno;
			set
			{
				phoneno = value;
			}
		}
	}
}
