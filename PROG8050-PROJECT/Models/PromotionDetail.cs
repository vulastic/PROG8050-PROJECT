using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG8050_PROJECT.Models
{
	class PromotionDetail
	{
		public int Id { get; set; }
		public int PromotionId { get; set; }
		public int ProductId { get; set; }
		public int Category { get; set; }
		public string CategoryName { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }
		public int Quantity { get; set; }
		public double Discount { get; set; }
		public bool IsSelected { get; set; } = false;
	}
}
