using PROG8050_PROJECT.Models;
using PROG8050_PROJECT.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
	/// Interaction logic for Order.xaml
	/// </summary>
	public partial class Order : Page
	{
		SQLiteDBManager dbManager = SQLiteDBManager.Instance;
		public static long Id { get; set; }
		public Order()
		{
			InitializeComponent();
			FillData();
		}

		private void BtnSearch_Click(object sender, RoutedEventArgs e)
		{
			//MessageBox.Show($"Customer: {textBox_CustomerSearchbar.Text} details loaded!");
			string search_txt = textBox_CustomerSearchbar.Text;
			if (String.IsNullOrEmpty(search_txt) || String.IsNullOrWhiteSpace(search_txt)) {
				SQLiteDBManager dbManager = SQLiteDBManager.Instance;
				SQLiteCommand createCommand = new SQLiteCommand($"select Id, FirstName, LastName,PhoneNo from Customer", dbManager.Connection);
				createCommand.ExecuteNonQuery();
				SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(createCommand);
				DataTable dataTable = new DataTable("tblCustomerDetails");
				dataAdapter.Fill(dataTable);
				tblCustomerDetails.ItemsSource = dataTable.DefaultView;
				dataAdapter.Update(dataTable);
			}
			else
			{
				SQLiteDBManager dbManager = SQLiteDBManager.Instance;
				SQLiteCommand createCommand = new SQLiteCommand($"select Id, FirstName, LastName,PhoneNo from Customer where FirstName=\"{search_txt}\"", dbManager.Connection);
				createCommand.ExecuteNonQuery();
				SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(createCommand);
				DataTable dataTable = new DataTable("tblCustomerDetails");
				dataAdapter.Fill(dataTable);
				tblCustomerDetails.ItemsSource = dataTable.DefaultView;
				dataAdapter.Update(dataTable);
			}
		}

		private void BtnAddNew_Click(object sender, RoutedEventArgs e)
		{
			this.NavigationService.Navigate(new Uri("./Views/NewCustomer.xaml", UriKind.RelativeOrAbsolute));
		}

		private void BtnShowOrders_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show($"All Order will be populated in the order table..");
		}

		private void BtnDelete_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show($"Selected Products are deleted!");
		}

		private void BtnSelectAll_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show($"Orders are selected from the order table!");
		}

		private void btnCreateOrder_Click(object sender, RoutedEventArgs e)
		{
			//string time = DateTime.Now.ToString().Remove(DateTime.Now.ToString().Length-3);
			int OrderId = 0;
			Int64 quantiy = 0;
			Dictionary<string, object> param = new Dictionary<string, object>();
			param.Add("@id", Id);
			int result = dbManager.ExecuteNonQuery("INSERT INTO \"Order\"(CustomerId) VALUES(@id)", param);
			if (result == 1)
			{
				var temp= dbManager.ExecuteReader("Select Id from \"Order\"",param);
				
                while (temp.Read())
                {
					OrderId = Convert.ToInt32(temp["Id"].ToString());
                }
				foreach(CreateOrder order in CreateOrder.createOrders)
                {
					temp = dbManager.ExecuteReader($"Select Quantity from Product where Id={order.Id}", param);
                    while (temp.Read())
                    {
						quantiy= Convert.ToInt64(temp["Quantity"].ToString());
					}
					quantiy -= order.Quantity;
					result = dbManager.ExecuteNonQuery($"UPDATE Product set Quantity={quantiy} WHERE Id={order.Id}", param);
					result = dbManager.ExecuteNonQuery($"INSERT INTO OrderDetail (OrderId,ProductId,Quantity) VALUES({OrderId},{order.Id},{order.Quantity})");
				}

			}
			MessageBox.Show($"Order {OrderId} is placed successfully!", "Success", MessageBoxButton.OK);
			
			//tblProductDetails.Items.Clear();
			//tblCProductDetails.Items.Clear();
			PrintOrder.Visibility = Visibility.Visible;
		}
		private void Row_CustomerDetail_Click(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount == 2)
			{
				UpdateCustomerView.Visibility = System.Windows.Visibility.Visible;
				DataRowView dataRowView = (DataRowView)tblCustomerDetails.SelectedItem;
				textBox_CustId.Text = dataRowView.Row[0].ToString();
				textBox_FirstName.Text = dataRowView.Row[1].ToString();
				textBox_LastName.Text = dataRowView.Row[2].ToString();
				textBox_PhoneNo.Text = dataRowView.Row[3].ToString();
			}
			else
            {
				Row_CustomerforOrder(sender, e);

			}
		}

		private void Row_ProductDetail_Click(object sender, MouseButtonEventArgs e)
		{
			quantityProductOrder.Visibility = System.Windows.Visibility.Visible;
			DataRowView dataRowView = (DataRowView)tblProductDetails.SelectedItem;
			Product_Name.Text = dataRowView.Row[1].ToString();
			Product_Id.Text = dataRowView.Row[0].ToString();
			Product_Price.Text = dataRowView.Row[3].ToString();

			int quantity=Convert.ToInt32(dataRowView[2].ToString());
			for(int i = 1; i <= quantity; i++)
            {
				comboboxProductQuantity.Items.Add(i);
            }
			comboboxProductQuantity.SelectedIndex = 0;
		}

		private void FillData()
		{
			//load customer details
			
			SQLiteCommand createCommand = new SQLiteCommand("select Id, FirstName, LastName,PhoneNo from Customer", dbManager.Connection);
			createCommand.ExecuteNonQuery();
			SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(createCommand);
			DataTable dataTable = new DataTable("tblCustomerDetails");
			dataAdapter.Fill(dataTable);
			tblCustomerDetails.ItemsSource = dataTable.AsDataView();
			dataAdapter.Update(dataTable);
			this.tblCustomerDetails.Columns[0].Header = "Customer Id";
			this.tblCustomerDetails.Columns[1].Header = "First Name";
			this.tblCustomerDetails.Columns[2].Header = "Last Name";
			this.tblCustomerDetails.Columns[3].Header = "Phone No";

			//load category dropdown
			createCommand.CommandText = @"SELECT Name FROM Category";
			createCommand.Connection = dbManager.Connection;
			SQLiteDataReader dr = createCommand.ExecuteReader();
			while(dr.Read())
			{
				comboboxCategoryList.Items.Add(dr["Name"]);
			}
		}
        private void OnSelection(object sender, EventArgs e)
        {
			if (Id==null|Id==0)
			{
				System.Windows.MessageBox.Show($"Select a customer proceed further");
			}
			else
			{
				//load product table
				Dictionary<string, object> param = new Dictionary<string, object>();
				string categoryId = "";
				param.Add("@categoryName", comboboxCategoryList.SelectedItem.ToString());
				var temp = dbManager.ExecuteReader("Select * from Category where Name=@categoryName", param);
				if (temp != null)
				{
					while (temp.Read())
					{
						categoryId = temp[0].ToString();
					}
				}
				SQLiteCommand createCommand = new SQLiteCommand($"select Id, Name, Quantity, Price, Image from Product where CategoryId ={categoryId }", dbManager.Connection);
				createCommand.ExecuteNonQuery();
				SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(createCommand);
				DataTable dataTable = new DataTable("tblProductDetails");
				dataAdapter.Fill(dataTable);
				tblProductDetails.ItemsSource = dataTable.AsDataView();
				dataAdapter.Update(dataTable);
				this.tblProductDetails.Columns[0].Header = "Product Id";
				this.tblProductDetails.Columns[1].Header = "Name";
				this.tblProductDetails.Columns[2].Header = "Quantity";
				this.tblProductDetails.Columns[3].Header = "Price";
				this.tblProductDetails.Columns[4].Header = "Image";
			}
		}

        private void btnCUCancel_Click(object sender, RoutedEventArgs e)
        {
			UpdateCustomerView.Visibility = Visibility.Collapsed;
		}

        private void btnCUUpdate_Click(object sender, RoutedEventArgs e)
        {
			Dictionary<string, object> param = new Dictionary<string, object>();
			param.Add("@id", textBox_CustId.Text);
			param.Add("@firstname", textBox_FirstName.Text);
			param.Add("@lastname", textBox_LastName.Text);
			param.Add("@phoneNo", textBox_PhoneNo.Text);
			int result = dbManager.ExecuteNonQuery("UPDATE Customer set FirstName=@firstname, LastName=@lastname, PhoneNo=@phoneNo WHERE Id=@id;", param);
			if(result == 1)
            {
				System.Windows.MessageBox.Show($"Customer details updated!");
				UpdateCustomerView.Visibility = Visibility.Collapsed;
				FillData();
			}
            else
            {
				System.Windows.MessageBox.Show($"Customer details update failed!");
			}

		}

        private void btnUQCancel_Click(object sender, RoutedEventArgs e)
        {
			quantityProductOrder.Visibility= Visibility.Collapsed;

		}

        private void btnUQOk_Click(object sender, RoutedEventArgs e)
        {
			CreateOrder orders = new CreateOrder();
			orders.Id = Convert.ToInt64(Product_Id.Text);
			orders.Name=Product_Name.Text;
			orders.Price=Convert.ToInt64(Product_Price.Text);
			orders.Quantity = Convert.ToInt64(comboboxProductQuantity.SelectedValue);
			orders.TotalPrice = orders.Price * orders.Quantity;
			CreateOrder.createOrders.Add(orders);
			quantityProductOrder.Visibility = Visibility.Collapsed;
			ShowOrders();
		}
		private void ShowOrders()
		{
			tblCProductDetails.Items.Clear();
			long totalPrice=0L;
			foreach (CreateOrder createOrder in CreateOrder.createOrders)
			{
				tblCProductDetails.Items.Add(createOrder);
				totalPrice += createOrder.TotalPrice;
			}

			Total_Price.Text = totalPrice.ToString();
		}

        private void Row_ProductDetailEdit_Click(object sender, MouseButtonEventArgs e)
        {
			CreateOrder order = (CreateOrder)tblCProductDetails.SelectedItem;
			UProduct_Name.Text = order.Name;
			UProduct_Id.Text = order.Id.ToString();
			UProduct_Price.Text = order.Price.ToString();
			var temp = dbManager.ExecuteReader($"Select * from Product where Id={UProduct_Id.Text}");
			int quantity = 0; 
			while (temp.Read())
			{
				quantity = Convert.ToInt32(temp[5].ToString());
			}

			for (int i = 1; i <= quantity; i++)
			{
				comboboxUProductQuantity.Items.Add(i);
			}
			comboboxUProductQuantity.SelectedIndex= Convert.ToInt32(order.Quantity)-1;

			quantityUpdateProductOrder.Visibility = Visibility.Visible;
		}

        private void btnUOQCancel_Click(object sender, RoutedEventArgs e)
        {
			quantityUpdateProductOrder.Visibility=Visibility.Collapsed;

		}

        private void btnUOQUpdate_Click(object sender, RoutedEventArgs e)
        {
			CreateOrder order=new CreateOrder();
			foreach(CreateOrder createOrders in CreateOrder.createOrders)
            {
                if (createOrders.Id.Equals(Convert.ToInt64(UProduct_Id.Text)))
                {
					createOrders.Quantity = Convert.ToInt64(comboboxUProductQuantity.SelectedValue.ToString());
					createOrders.TotalPrice = createOrders.Price * createOrders.Quantity;
				}
            }
			quantityUpdateProductOrder.Visibility = Visibility.Collapsed;
			System.Windows.MessageBox.Show($"Order details updated!");
			ShowOrders();
		}

        private void Row_CustomerforOrder(object sender, MouseButtonEventArgs e)
        {
			if (e.ClickCount == 2)
			{
				Row_CustomerDetail_Click(sender, e);
			}
			else
			{
				DataRowView dataRowView = (DataRowView)tblCustomerDetails.SelectedItem;
				Id = Convert.ToInt64(dataRowView.Row[0].ToString());
			}
		}

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
			PrintOrder.Visibility = Visibility.Collapsed;	

		}

        private void btnPrintOrder_Click(object sender, RoutedEventArgs e)
        {
			PrintDialog printDialog = new PrintDialog();
			printDialog.ShowDialog();
		}
    }
}
