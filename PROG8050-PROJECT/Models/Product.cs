﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG8050_PROJECT.Models
{
	class Product
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int CategoryId { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
		public int Quantity { get; set; }
		public byte[] Image { get; set; }
	}
}
