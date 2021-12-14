using PROG8050_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG8050_PROJECT.Core.Services
{
	class LoginService : ILoginService
	{
		public bool IsLogin { get; private set; }

		private string email;
		public string Email
		{
			get => email;
		}

		private AdminUser adminUser;
		public AdminUser AdminUesr
		{
			get => adminUser;
		}

		public bool SetLogin(string email, AdminUser adminUser)
		{
			this.email = email;
			this.adminUser = adminUser;
			this.IsLogin = true;
			return true;
		}

	}
}
