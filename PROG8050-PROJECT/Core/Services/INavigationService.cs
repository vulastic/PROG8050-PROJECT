using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG8050_PROJECT.Core.Services
{
	interface INavigationService
	{
		string Next { get; set; }

		bool CanGoBack { get; }
		void GoBack();
		void Navigate<T>(object args = null);
		void Navigate(string name);
	}
}
