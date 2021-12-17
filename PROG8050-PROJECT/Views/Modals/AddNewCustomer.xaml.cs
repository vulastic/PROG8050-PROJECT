﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PROG8050_PROJECT.Views.Modals
{
	/// <summary>
	/// Interaction logic for AddNewCustomer.xaml
	/// </summary>
	public partial class AddNewCustomer : Window
	{
		public AddNewCustomer()
		{
			InitializeComponent();
		}

		private void password_PasswordChanged(object sender, RoutedEventArgs e)
		{
			if (this.DataContext != null)
			{
				((dynamic)this.DataContext).Password = ((PasswordBox)sender).SecurePassword;
			}
		}
	}
}