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
    /// Interaction logic for category.xaml
    /// </summary>
    public partial class Category : Page
    {

        SQLiteDataAdapter mAdapter;
        DataTable dtable;

        string editcategoryid, editcategoryname;
        object item;
        public Category()
        {
            InitializeComponent();
            FillDataGrid();
        }


        private void Button_Add_CategoryElement_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryElementInputBox.Visibility = System.Windows.Visibility.Visible;
        }

        private void Button_Edit_CategoryElement_Click(object sender, RoutedEventArgs e)
        {
            InputEditCategoryIdBox.Text = editcategoryid;
            InputEditCategoryBox.Text = editcategoryname;
            EditCategoryElementInputBox.Visibility = System.Windows.Visibility.Visible;
        }

        private void categoryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.categoryDataGrid.SelectedItem != null)
                {
                    item = this.categoryDataGrid.SelectedItem;
                    editcategoryid = (this.categoryDataGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                    this.InputEditCategoryIdBox.Text = editcategoryid;
                    editcategoryname = (this.categoryDataGrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
                    this.InputEditCategoryBox.Text = editcategoryname;
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Message:" + exp);
            }
        }
        //
        private void Button_Close_CategoryElement_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryElementInputBox.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Button_Submit_CategoryElement_Click(object sender, RoutedEventArgs e)
        {
            SQLiteDBManager dbManager = SQLiteDBManager.Instance;
            var conn = dbManager.Connection;
            using var cmd = new SQLiteCommand(conn);

            cmd.CommandText = "Insert into Category (CategoryName)VALUES('" + this.InputCategoryBox.Text.ToString() + "')";
            cmd.ExecuteNonQuery();
            FillDataGrid();

            //  conn.Close();
            AddCategoryElementInputBox.Visibility = System.Windows.Visibility.Hidden;
            MessageBox.Show($"{InputCategoryBox.Text} Category is succesfully inserted", "Success",
             MessageBoxButton.OK);



        }

        private void EditCategoryUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            SQLiteDBManager dbManager = SQLiteDBManager.Instance;
            var conn = dbManager.Connection;

            using (var cmd = new SQLiteCommand(conn))
            {
                conn.Open();
                cmd.CommandText = "update Category SET CategoryName = '" + this.InputEditCategoryBox.Text.ToString()
                  + "' where Id =" + Int32.Parse(this.InputEditCategoryIdBox.Text.ToString());
                cmd.ExecuteNonQuery();
                FillDataGrid();
                conn.Close();
                EditCategoryElementInputBox.Visibility = System.Windows.Visibility.Hidden;

            }
            //  
            MessageBox.Show($"{InputEditCategoryBox.Text} Product is succesfully Updated", "Success",
          MessageBoxButton.OK);

        }

        private void EditCategoryCancelButton_Click(object sender, RoutedEventArgs e)
        {
            EditCategoryElementInputBox.Visibility = System.Windows.Visibility.Hidden;

        }
        private void FillDataGrid()
        {
            SQLiteDBManager dbManager = SQLiteDBManager.Instance;
            var conn = dbManager.Connection;

            try
            {

                SQLiteCommand cmd = new SQLiteCommand(conn);

                cmd.CommandText = "select* from Category order by id";
                cmd.ExecuteNonQuery();
                mAdapter = new SQLiteDataAdapter(cmd);
                dtable = new DataTable("Category");
                mAdapter.Fill(dtable);
                categoryDataGrid.ItemsSource = dtable.DefaultView;
                mAdapter.Update(dtable);

                this.categoryDataGrid.Columns[0].Header = "Id";
                this.categoryDataGrid.Columns[1].Header = "Category";
                //    


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Message : " + ex);
            }


        }
        public void Button_Delete_CategoryElement_Click(object sender, RoutedEventArgs e)
        {
        }
    }

}
