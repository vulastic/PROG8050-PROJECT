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
	/// Interaction logic for Home.xaml
	/// </summary>
	public partial class Home : Page
	{
		SQLiteDBManager dbManager = SQLiteDBManager.Instance;
		public Home()
		{
			InitializeComponent();
			FillData();
		}
		public void FillData()
        {
			var temp = dbManager.ExecuteReader($"Select * from Customer");
			int count = 0;
			string date = "";
			int orderCount = 0;
			int Quantity = 0;
			while (temp.Read())
			{
				count++;
			}
			textBox_CustomerCount.Text = count.ToString(); //Total customers

			count = 0;
			temp = dbManager.ExecuteReader($"select distinct CustomerId from \"Order\"");
			while (temp.Read())
			{
				count++;
			}
			textBox_ActiveCustomerCount.Text = count.ToString(); //Active customer -all customers with order history

			temp = dbManager.ExecuteReader($"SELECT o.Quantity, p.Price FROM OrderDetail o INNER JOIN Product p ON o.ProductId = p.Id; ");
			count = 0;
			while (temp.Read())
			{
				count += Convert.ToInt32(temp["Quantity"].ToString()) * Convert.ToInt32(temp["Price"].ToString());
				orderCount++;
				Quantity += Convert.ToInt32(temp["Quantity"].ToString());
			}
			textBox_TotalOrderAmount.Text = count.ToString(); //Total order amount
			count = 0;

			temp = dbManager.ExecuteReader($"select Datetime from \"Order\"");
			while (temp.Read())
			{
				date = temp["Datetime"].ToString();
			}
			textBox_LastOrderDate.Text = date.ToString(); //Last Order Date
			textBox_OrderCountW.Text = orderCount.ToString(); //Order Count
			textBox_TotalProductsSoldW.Text = Quantity.ToString();//total products sold

		}
	}
}
