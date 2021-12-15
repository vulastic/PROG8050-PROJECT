using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG8050_PROJECT.Models
{
	class Promotion : ObservableRecipient
	{
		private int id;
		public int Id
		{
			get => id;
			set => id = value;
		}

		private string name;
		public string Name
		{
			get => name;
			set => name = value;
		}

		private string description;
		public string Description
		{
			get => description;
			set => description = value;
		}

		private int startDatetime;
		public int StartDatetime
		{
			get => startDatetime;
			set
			{
				startDatetime = value;
				this.start = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(value);
			}
		}

		private int endDatetime;
		public int EndDatetime
		{
			get => endDatetime;
			set
			{
				endDatetime = value;
				end = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(value);
			}
		}

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

		private DateTime start;
		public DateTime Start => start;

		private DateTime end;
		public DateTime End => end;
	}
}
