using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PROG8050_PROJECT.Views
{
	/// <summary>
	/// Interaction logic for Product.xaml
	/// </summary>
	public partial class Product : Page
	{
		public Product()
		{
			InitializeComponent();
		}

		private void btnAddProductModal_Click(object sender, RoutedEventArgs e)
		{
			AddPdctModal addPdctModal = new AddPdctModal();
			addPdctModal.ShowDialog();
		}

		private void btnUpdateProductModal_Click(object sender, RoutedEventArgs e)
		{
			UpdateModal updatemodal = new UpdateModal();
			updatemodal.ShowDialog();
		}
	}
}
