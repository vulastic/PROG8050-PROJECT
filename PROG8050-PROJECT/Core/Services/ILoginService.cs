using PROG8050_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG8050_PROJECT.Core.Services
{
	interface ILoginService
	{
		public bool IsLogin { get; }

		public bool SetLogin(string email, AdminUser adminUser);
	}
}
