using Microsoft.Toolkit.Mvvm.DependencyInjection;
using PROG8050_PROJECT.Core.Services;
using System;
using System.Collections.Generic;
using System.Data;
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
	/// Interaction logic for HomePage.xaml
	/// </summary>
	public partial class HomePage : Page
	{
		public HomePage()
		{
			InitializeComponent();
			FillData();
		}
		public void FillData()
		{
			IDBService database = Ioc.Default.GetService<Core.Services.IDBService>();
			if (!database.IsOpen)
			{
				System.Windows.MessageBox.Show("Cannot connect to the database", "Ooops!");
				return;
			}

			DataTable table = database.ExecuteReader("SELECT count(Id) FROM Customer;");
			if (table.Rows.Count > 0)
			{
				int totalUserCount = 0;
				Int32.TryParse(table.Rows[0][0].ToString(), out totalUserCount);

				textBox_CustomerCount.Text = totalUserCount.ToString();
			}

			// TO DO
			// Active Customer
			textBox_ActiveCustomerCount.Text = table.Rows[0][0].ToString();

			DataTable dataTable = database.ExecuteReader("SELECT count(o.Quantity), sum(o.Quantity), sum(p.Price) FROM OrderDetail o INNER JOIN Product p ON o.ProductId = p.Id;");
			if (dataTable.Rows.Count > 0)
			{
				var row = dataTable.Rows[0];

				long totalCount, totalQuantity, totalPrice;
				Int64.TryParse(row[0].ToString(), out totalCount);
				Int64.TryParse(row[1].ToString(), out totalQuantity);
				Int64.TryParse(row[2].ToString(), out totalPrice);

				textBox_TotalOrderAmount.Text = (totalQuantity * totalPrice).ToString();    //Total order amount
				textBox_OrderCountW.Text = totalCount.ToString(); //Order Count
				textBox_TotalProductsSoldW.Text = totalQuantity.ToString();//total products sold
			}

			DataTable timeTable = database.ExecuteReader("SELECT datetime FROM 'Order';");
			if (timeTable.Rows.Count > 0)
			{
				int unixTimestamp = 0;
				Int32.TryParse(timeTable.Rows[0][0].ToString(), out unixTimestamp);
				DateTime lastOrderDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimestamp);
				textBox_LastOrderDate.Text = lastOrderDate.ToString(); //Last Order Date
			}
		}
	}
}
