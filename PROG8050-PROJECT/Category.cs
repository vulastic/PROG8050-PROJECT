using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PROG8050_PROJECT.Models
{
	class Category
	{

		public int Id { get; set; }
		public int CategoryName { get; set; }



		public override string ToString()
		{
			return $"{CategoryName}";
		}

	}
}
