using System;
using System.IO;
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
using Microsoft.Win32;

namespace PROG8050_PROJECT.Views
{
    /// <summary>
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class Product : Page
    {
        
        SQLiteDataAdapter sAdapter;
        DataTable datable;
        string image;
        byte[] data;
        string editpdctid;
        string editcategoryProduct, editproductname, editproductdescription, editproductprice, editproductquantity, editproductimage;
        object item;
        public Product()
        {
            InitializeComponent();
            fill_category();
            FillDataGrid();
        }


        private void Button_Add_ItemElement_Click(object sender, RoutedEventArgs e)
        {
            AddItemElementInputBox.Visibility = System.Windows.Visibility.Visible;
      
            InputProductNameBox.Text = null;
            InputProductDescriptionBox.Text = null;
            InputProductPriceBox.Text = null;
            InputProducQuantityBox.Text = null;
             InputCategoryIdBox = null;
            imgProduct.Source=null;
        }

        private void Button_Edit_ItemElement_Click(object sender, RoutedEventArgs e)
        {
            
            InputEditCategoryIdBox.SelectedItem = editcategoryProduct;
            InputEditProductNameBox.Text = editproductname;
            InputEditProductDescriptionBox.Text = editproductdescription;
            InputEditProductPriceBox.Text = editproductprice;
            InputEditProductQuantityBox.Text = editproductquantity;
      // byte[] imgEditProduct = editproductimage;
            EditItemElementInputBox.Visibility = System.Windows.Visibility.Visible;
           



        }

        private void productDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.productDataGrid.SelectedItem != null)
                {
                    item = this.productDataGrid.SelectedItem;
                    editpdctid = (this.productDataGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                    this.InputEditId.Text = editpdctid;
                    editcategoryProduct = (this.productDataGrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
                    this.InputEditCategoryIdBox.Text = editcategoryProduct;
                    editproductname = (this.productDataGrid.SelectedCells[2].Column.GetCellContent(item) as TextBlock).Text;
                    this.InputEditProductNameBox.Text = editproductname;
                    editproductdescription = (this.productDataGrid.SelectedCells[3].Column.GetCellContent(item) as TextBlock).Text;
                    this.InputEditProductDescriptionBox.Text = editproductdescription;
                    editproductprice = (this.productDataGrid.SelectedCells[4].Column.GetCellContent(item) as TextBlock).Text;
                    this.InputEditProductPriceBox.Text = editproductprice;
                    editproductquantity = (this.productDataGrid.SelectedCells[5].Column.GetCellContent(item) as TextBlock).Text;
                    this.InputEditProductQuantityBox.Text = editproductquantity;
                  //  byte[] imgEditProduct  = (byte[])productDataGrid.SelectedCells[6].Column.GetCellContent;
                   // this.imgEditProduct.Source = editproductimage;
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
     private void  fill_category() {
            SQLiteDBManager dbManager = SQLiteDBManager.Instance;
            var conn = dbManager.Connection;

            try
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"SELECT * FROM Category order by Id";
                cmd.ExecuteNonQuery();
                datable = new DataTable();
                sAdapter = new SQLiteDataAdapter(cmd);
                sAdapter.Fill(datable);
                foreach (DataRow dr in datable.Rows)
                {
                    InputCategoryIdBox.Items.Add(dr["Name"].ToString());
                    InputEditCategoryIdBox.Items.Add(dr["Name"].ToString());
                }

            }
            catch (Exception ex)
            {
                // write exception info to log or anything else
                MessageBox.Show("Error occured!");
            }
        }
        private void ProductSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = null;

            SQLiteDBManager dbManager = SQLiteDBManager.Instance;
            var conn = dbManager.Connection;

            using (var cmd = new SQLiteCommand(conn))
            {
                if (InputProductNameBox.Text != "" || InputProductDescriptionBox.Text != "")
                {
                    try
                    {
                        data = File.ReadAllBytes(image);
                        cmd.CommandText = @"Insert into Product(Name, CategoryId, Description, Price, Quantity, Image)VALUES(@name,@cid,@description,@price,@quantity,@img)";
                        cmd.Parameters.Add(new SQLiteParameter("@name", InputProductNameBox.Text));
                        cmd.Parameters.Add(new SQLiteParameter("@cid", InputCategoryIdBox.SelectedIndex));
                        cmd.Parameters.Add(new SQLiteParameter("@description", InputProductDescriptionBox.Text));
                        cmd.Parameters.Add(new SQLiteParameter("@price", InputProductPriceBox.Text));
                        cmd.Parameters.Add(new SQLiteParameter("@quantity", InputProducQuantityBox.Text));
                        cmd.Parameters.Add("@img", DbType.Binary, data.Length);
                        cmd.Parameters["@img"].Value = data;
                        cmd.ExecuteNonQuery();
                        FillDataGrid();
                        AddItemElementInputBox.Visibility = System.Windows.Visibility.Hidden;
                        MessageBox.Show($"{InputProductNameBox.Text} Product is succesfully inserted", "Success",
                         MessageBoxButton.OK);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                else
                {
                    MessageBox.Show(" Field Cannot be empty!");
                }
            }
        }
        private void Button_DeleteProductClick(object sender, RoutedEventArgs e)
        {

            var Result = MessageBox.Show($"Do you want to delete {this.editproductname} ? ", "Confirmation",
                            MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                try { SQLiteDBManager dbManager = SQLiteDBManager.Instance;
                    var conn = dbManager.Connection;
                    var cmd = new SQLiteCommand(conn);
                    cmd.CommandText = "delete from Product where Id = " + Int32.Parse(editpdctid);
                    MessageBox.Show("Succesfully Deleted");
                    cmd.ExecuteNonQuery();
                    FillDataGrid();
                }catch (Exception ex)
                      {
                    MessageBox.Show("Select Product to delete");
                        }
            }
            else
            {
            }
        }

        private void btnSearchProduct_Click(object sender, RoutedEventArgs e)
        {

            SQLiteDBManager dbManager = SQLiteDBManager.Instance;
            var conn = dbManager.Connection;
            try
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = @"select * from Product where Id Like @id OR Name Like @name OR Description Like @decs OR Price Like @price OR Quantity Like @qty  Order by Id";
                cmd.Parameters.AddWithValue("@id", textBox_Search.Text);
                cmd.Parameters.AddWithValue("@name", textBox_Search.Text); 
                cmd.Parameters.AddWithValue("@decs", textBox_Search.Text);
                cmd.Parameters.AddWithValue("@price", textBox_Search.Text);
                cmd.Parameters.AddWithValue("@qty", textBox_Search.Text);
                
                
                cmd.ExecuteNonQuery();
                sAdapter = new SQLiteDataAdapter(cmd);
                datable = new DataTable("Category");
                sAdapter.Fill(datable);
                productDataGrid.ItemsSource = datable.DefaultView;
                sAdapter.Update(datable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Message : " + ex);
            }

        }

        private void EditItemUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = null;
            SQLiteDBManager dbManager = SQLiteDBManager.Instance;
            var conn = dbManager.Connection;
         
          using (var cmd = new SQLiteCommand(conn))
            {
                if (InputEditProductDescriptionBox.Text != "" || InputEditProductNameBox.Text != "")
                {
                    try
                    {

                        data = File.ReadAllBytes(image);
                        cmd.CommandText ="update Product set Name=:name,CategoryId=:cid,Description=:description,Price=:price,Quantity=:quantity,Image=:img where Id =:pid";
                        cmd.Parameters.Add("name", DbType.String).Value =  InputEditProductNameBox.Text;
                        cmd.Parameters.Add(":cid", DbType.Int32).Value= InputEditCategoryIdBox.SelectedIndex;
                        cmd.Parameters.Add(":description",DbType.String).Value= InputEditProductDescriptionBox.Text;
                        cmd.Parameters.Add(":price",DbType.Currency).Value= InputEditProductPriceBox.Text;
                        cmd.Parameters.Add(":quantity",DbType.Int32).Value= InputEditProductQuantityBox.Text;
                        cmd.Parameters.Add(":img", DbType.Binary, data.Length);
                        cmd.Parameters[":img"].Value = data;
                        cmd.Parameters.Add(":pid",DbType.Int32).Value= InputEditId.Text;
                      

                        // cmd.CommandText=  "update Product SET Name = '" + this.InputEditProductNameBox.Text.ToString()
                        //     + "', CategoryId ='" + this.InputEditCategoryIdBox.Text.ToString()
                        //  + "', Description ='" + this.InputEditProductDescriptionBox.Text.ToString()
                        //  + "', Price ='" + this.InputEditProductPriceBox.Text.ToString()
                        //   + "', Quantity ='" + this.InputProducQuantityBox.Text.ToString()
                      //     + "', Image ='" + this.imgEditProduct
                        // + "' where Id =" + Int32.Parse(this.InputEditId.Text.ToString());
                        cmd.ExecuteNonQuery();
                        FillDataGrid();
                        EditItemElementInputBox.Visibility = System.Windows.Visibility.Hidden;
                        MessageBox.Show($"{InputEditProductNameBox.Text} Product is succesfully Updated", "Success",
                        MessageBoxButton.OK);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                else
                {
                    MessageBox.Show(" Field Cannot be empty!");
                }
           
          //  
           
            }
        }
        private void BtnLoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imgProduct.Source = new BitmapImage(new Uri(op.FileName));
                image = op.FileName;
            }

        }
        private void BtnEditFromFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imgEditProduct.Source = new BitmapImage(new Uri(op.FileName));
                image = op.FileName;
            }

        }
        private void EditItemCancelButton_Click(object sender, RoutedEventArgs e)
        {
            EditItemElementInputBox.Visibility = System.Windows.Visibility.Hidden;

        }
       private void FillDataGrid()
       {
           
            SQLiteDBManager dbManager = SQLiteDBManager.Instance;
           var con = dbManager.Connection;
           try { 
           var cmd = new SQLiteCommand(con);
           cmd.CommandText = "SELECT Product.Id,Product.Name,Category.Name,Product.Description,Product.Price," +
                    "Product.Quantity,Product.Image FROM Product Left outer JOIN Category ON Product.CategoryId= Category.Id"; 
           cmd.ExecuteNonQuery();
           
           sAdapter = new SQLiteDataAdapter(cmd);
           datable = new DataTable();
                sAdapter.Fill(datable);
           productDataGrid.ItemsSource = datable.AsDataView();
           sAdapter.Update(datable);
              
           }
           catch (Exception ex)
           {
               MessageBox.Show("Error Message : " + ex);
           }
    
       }

    }



}
