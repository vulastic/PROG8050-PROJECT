using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PROG8050_PROJECT.Models
{
	class Product
	{

		public int Id { get; set; }
		public int Category { get; set; }
		public string ProductName { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
		public int Quantity { get; set; }

		public int Image { get; set; }


		public override string ToString()
		{
			return $"{ProductName}-{Category}-{Description}--{Quantity}--{Price}--{Image}";
		}

	}
}
