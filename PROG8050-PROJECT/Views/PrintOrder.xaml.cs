using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PROG8050_PROJECT.View
{
	/// <summary>
	/// Interaction logic for PrintOrder.xaml
	/// </summary>
	public partial class PrintOrder : Page
	{
		public PrintOrder()
		{
			InitializeComponent();
		}

        private void btnPrintOrder_Click(object sender, RoutedEventArgs e)
        {
			MessageBoxResult result = MessageBox.Show($"Order printed successfully!");
		}

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
			this.NavigationService.Navigate(new Uri("./Views/Order.xaml", UriKind.RelativeOrAbsolute));
		}
    }
}
