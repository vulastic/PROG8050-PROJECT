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
        Image Image;
        SQLiteDataAdapter sAdapter;
        DataTable datable;

        string editpdctid;
        string editcategoryProduct, editproductname, editproductdescription, editproductprice, editproductquantity, editproductimage;
        object item;
        public Product()
        {
            InitializeComponent();
         
            FillDataGrid();
        }


        private void Button_Add_ItemElement_Click(object sender, RoutedEventArgs e)
        {
            AddItemElementInputBox.Visibility = System.Windows.Visibility.Visible;
            fill_category();
            InputProductNameBox.Text = null;
            InputProductDescriptionBox.Text = null;
            InputProductPriceBox.Text = null;
            InputProducQuantityBox.Text = null;
         InputCategoryIdBox.SelectedItem = null;
            imgProduct.Source=null;
        }

        private void Button_Edit_ItemElement_Click(object sender, RoutedEventArgs e)
        {
            fill_category();
            InputEditCategoryIdBox.Text = editcategoryProduct;
            InputEditProductNameBox.Text = editproductname;
            InputEditProductDescriptionBox.Text = editproductdescription;
            InputEditProductPriceBox.Text = editproductprice;
            InputEditProductQuantityBox.Text = editproductquantity;
           // imgEditProduct.Source = editproductimage;
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
                   // editproductimage = (this.productDataGrid.SelectedCells[6].Column.GetCellContent(item) as TextBlock).Text;
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
                cmd.CommandText = "SELECT Name FROM Category order by Id";
                cmd.ExecuteNonQuery();
                datable = new DataTable();
                sAdapter = new SQLiteDataAdapter(cmd);
                sAdapter.Fill(datable);
                foreach (DataRow dr in datable.Rows)
                {
                    InputCategoryIdBox.Items.Add(dr["Name"].ToString());
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
            SQLiteDBManager dbManager = SQLiteDBManager.Instance;

            SQLiteCommand cmd = new SQLiteCommand("Insert into Product(Name, CategoryId, Description, Price, Quantity, Image)VALUES('" + this.InputProductNameBox.Text.ToString()
               + "','" + this.InputCategoryIdBox.Text.ToString() + "','" + this.InputProductDescriptionBox.Text.ToString()
               + "','" + this.InputProductPriceBox.Text.ToString() + "','" + this.InputProducQuantityBox.Text.ToString() + "','" + this.imgProduct.Source + "') ", dbManager.Connection);

           
            cmd.ExecuteNonQuery();
            FillDataGrid();
            AddItemElementInputBox.Visibility = System.Windows.Visibility.Hidden;
            MessageBox.Show($"{InputProductNameBox.Text} Product is succesfully inserted", "Success",
             MessageBoxButton.OK);
        
        }

        private void Button_DeleteProductClick(object sender, RoutedEventArgs e)
        {
            SQLiteDBManager dbManager = SQLiteDBManager.Instance;
            var conn = dbManager.Connection;
            var cmd = new SQLiteCommand(conn);
             cmd.CommandText= "delete from Product where Id = " + Int32.Parse(editpdctid);
            cmd.ExecuteNonQuery();
            FillDataGrid();

        }

       

        private void EditItemUpdateButton_Click(object sender, RoutedEventArgs e)
        {
          SQLiteDBManager dbManager = SQLiteDBManager.Instance;
          var conn = dbManager.Connection;
         
          using (var cmd = new SQLiteCommand(conn))
          {
           
              cmd.CommandText = "update Product SET Name = '" + this.InputEditProductNameBox.Text.ToString() 
                    + "', CategoryId ='" + this.InputEditCategoryIdBox.Text.ToString()
                 + "', Description ='" + this.InputEditProductDescriptionBox.Text.ToString()
                 + "', Price ='" + this.InputEditProductPriceBox.Text.ToString()
                  + "', Quantity ='" + this.InputProducQuantityBox.Text.ToString()
                  + "', Image ='" + this.imgEditProduct.Source
                + "' where Id =" + Int32.Parse(this.InputEditId.Text.ToString());
              cmd.ExecuteNonQuery();
              FillDataGrid();
              
           EditItemElementInputBox.Visibility = System.Windows.Visibility.Hidden;
         
          }
          //  
            MessageBox.Show($"{InputProductNameBox.Text} Product is succesfully Updated", "Success",
          MessageBoxButton.OK);

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
           cmd.CommandText = "SELECT * FROM Product order by Id"; 
                cmd.ExecuteNonQuery();
           sAdapter = new SQLiteDataAdapter(cmd);
           datable = new DataTable("Product");
           sAdapter.Fill(datable);
           productDataGrid.ItemsSource = datable.DefaultView;
           sAdapter.Update(datable);
              
           }
           catch (Exception ex)
           {
               MessageBox.Show("Error Message : " + ex);
           }
    
       }
    
    
    }



}
