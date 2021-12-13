using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PROG8050_PROJECT.Models
{
	class Product : Notifier
	{


		private int id;
		public int Id
		{
			get => id;
			set
			{
				id = value;
				OnPropertyChanged("id");
			}
		}
		private int categoryId;
		public int CategoryId
		{
			get => categoryId;
			set
			{
				categoryId = value;
				OnPropertyChanged("CategoryId");
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

		private string description;
		public string Description
		{
			get => description;
			set
			{
				description = value;
				OnPropertyChanged("Description");
			}
		}
		private decimal price;
		public decimal Price
		{
			get => price;
			set
			{
				price = value;
				OnPropertyChanged("Price");
			}
		}

		private int quantity;
		public int Quantity
		{
			get => quantity;
			set
			{
				quantity = value;
				OnPropertyChanged("Quantity");
			}
		}


		private byte image;
		public byte Image
		{
			get => image;
			set
			{
				image = value;
				OnPropertyChanged("Image");
			}
		}

	}
}
