using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PROG8050_PROJECT.Models
{
	class Category:Notifier
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
	}
}
