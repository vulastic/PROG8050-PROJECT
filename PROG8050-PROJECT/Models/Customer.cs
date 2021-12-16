using PROG8050_PROJECT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG8050_PROJECT.Models
{
	class Customer
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int PhoneNo { get; set; }
		public Gender Gender { get; set; }
	}
}
