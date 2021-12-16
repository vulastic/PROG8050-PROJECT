using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG8050_PROJECT.Models
{
	class PromotionDetail : ObservableRecipient
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

		private bool isSlected = false;
		public bool IsSelected
		{
			get => isSlected;
			set
			{
				isSlected = value;
				this.OnPropertyChanged("IsSelected");
			}
		}
	}
}
