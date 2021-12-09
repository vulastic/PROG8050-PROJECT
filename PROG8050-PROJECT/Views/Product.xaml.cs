using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Controls.Primitives;
using System.Configuration;
using System.Diagnostics;

using System.Data;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Collections;
using System.ComponentModel;
namespace PROG8050_PROJECT.Views
{
    /// <summary>
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class Product : Page
    {

        SQLiteDataAdapter mAdapter;
        DataTable dtable;

        string editcategoryid, editcategoryname, editcategorydescription;
        string editcategoryProduct, editproductname, editproductdescription, editproductprice, editproductquantity, editproductdimage;
        object item;
        public Product()
        {
            InitializeComponent();
            FillDataGrid();
        }


        private void Button_Add_ItemElement_Click(object sender, RoutedEventArgs e)
        {
            AddItemElementInputBox.Visibility = System.Windows.Visibility.Visible;
        }

        private void Button_Edit_ItemElement_Click(object sender, RoutedEventArgs e)
        {
            InputEditCategoryIdBox.Text = editcategoryProduct;
            InputEditProductNameBox.Text = editproductname;
            InputEditProductDescriptionBox.Text = editproductdescription;
            InputEditProductPriceBox.Text = editproductprice;
            InputEditProductQuantityBox.Text = editproductquantity;
            //  InputProductImageBox.bu = editproductdimage;
            EditItemElementInputBox.Visibility = System.Windows.Visibility.Visible;



        }

        private void productDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.productDataGrid.SelectedItem != null)
                {
                    item = this.productDataGrid.SelectedItem;
                    editcategoryid = (this.productDataGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                    this.InputEditId.Text = editcategoryid;
                    editcategoryProduct = (this.productDataGrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
                    this.InputEditCategoryIdBox.Text = editcategoryProduct;
                    editproductname = (this.productDataGrid.SelectedCells[2].Column.GetCellContent(item) as TextBlock).Text;
                    this.InputEditProductNameBox.Text = editproductname;
                    editproductdescription = (this.productDataGrid.SelectedCells[3].Column.GetCellContent(item) as TextBlock).Text;
                    this.InputEditProductDescriptionBox.Text = editcategorydescription;
                    editproductprice = (this.productDataGrid.SelectedCells[4].Column.GetCellContent(item) as TextBlock).Text;
                    this.InputEditProductPriceBox.Text = editproductprice;
                    editproductquantity = (this.productDataGrid.SelectedCells[5].Column.GetCellContent(item) as TextBlock).Text;
                    this.InputEditProductQuantityBox.Text = editproductquantity;
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Message:" + exp);
            }
        }
        //
        private void ProductCancelButton_Click(object sender, RoutedEventArgs e)
        {
            AddItemElementInputBox.Visibility = System.Windows.Visibility.Hidden;
        }

        private void ProductSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            SQLiteDBManager dbManager = SQLiteDBManager.Instance;
            var conn = dbManager.Connection;
            using var cmd = new SQLiteCommand(conn);

            cmd.CommandText = "Insert into Product (Category,ProductName,Description,Price,Quantity)VALUES('" + this.InputCategoryIdBox.Text.ToString()
                + "','" + this.InputProductNameBox.Text.ToString() + "','" + this.InputProductDescriptionBox.Text.ToString()
                + "','" + this.InputProductPriceBox.Text.ToString() + "','" + this.InputProducQuantityBox.Text.ToString()
                + "')";
            cmd.ExecuteNonQuery();
            FillDataGrid();

            conn.Close();
            AddItemElementInputBox.Visibility = System.Windows.Visibility.Hidden;
            MessageBox.Show($"{InputProductNameBox.Text} Product is succesfully inserted", "Success",
             MessageBoxButton.OK);



        }

        private void EditItemUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            SQLiteDBManager dbManager = SQLiteDBManager.Instance;
            var conn = dbManager.Connection;

            using (var cmd = new SQLiteCommand(conn))
            {
                conn.Open();
                cmd.CommandText = "update Product SET ProductName = '" + this.InputEditProductNameBox.Text.ToString()

                   + "', Description ='" + this.InputEditProductDescriptionBox.Text.ToString()
                   + "', Price ='" + this.InputEditProductPriceBox.Text.ToString()
                    + "', Quantity ='" + this.InputProducQuantityBox.Text.ToString()
                  + "' where Id =" + Int32.Parse(this.InputEditId.Text.ToString());
                cmd.ExecuteNonQuery();
                FillDataGrid();
                conn.Close();
                EditItemElementInputBox.Visibility = System.Windows.Visibility.Hidden;

            }
            //  
            MessageBox.Show($"{InputProductNameBox.Text} Product is succesfully Updated", "Success",
          MessageBoxButton.OK);

        }

        private void EditItemCancelButton_Click(object sender, RoutedEventArgs e)
        {
            EditItemElementInputBox.Visibility = System.Windows.Visibility.Hidden;

        }
        private void FillDataGrid()
        {
            SQLiteDBManager dbManager = SQLiteDBManager.Instance;
            var conn = dbManager.Connection;

            //List<Product> tempProduct = dbManager.ExecuteReader<Product> ("SELECT * FROM Product order by id");

            using var cmd = new SQLiteCommand(conn);
            cmd.CommandText = "SELECT * FROM Product order by id";
            // var dataReader = cmd.ExecuteReader();
            //dtable = new DataTable("Product");
            //dtable.Load(dataReader);
            cmd.ExecuteNonQuery();
            mAdapter = new SQLiteDataAdapter(cmd);
            dtable = new DataTable("Product");
            mAdapter.Fill(dtable);
            productDataGrid.ItemsSource = dtable.DefaultView;
            mAdapter.Update(dtable);


            this.productDataGrid.Columns[0].Header = "Id";
            this.productDataGrid.Columns[1].Header = "Category";
            this.productDataGrid.Columns[2].Header = "Product Name";
            this.productDataGrid.Columns[3].Header = "Description";
            this.productDataGrid.Columns[4].Header = "Price";
            this.productDataGrid.Columns[5].Header = "Quantity";


        }


    }



}
