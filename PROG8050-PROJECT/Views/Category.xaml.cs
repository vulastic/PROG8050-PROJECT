﻿using System;
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
        using (var cmd = new SQLiteCommand(conn))
        {
                if (InputCategoryBox.Text != "")
                {
                    try
                    {
                        cmd.CommandText = "Insert into Category (Name)VALUES('" + this.InputCategoryBox.Text.ToString() + "')";
                        cmd.ExecuteNonQuery();
                        FillDataGrid();
                        AddCategoryElementInputBox.Visibility = System.Windows.Visibility.Hidden;
                        MessageBox.Show($"{InputCategoryBox.Text} Category is succesfully inserted", "Success",
                         MessageBoxButton.OK);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error Message : " + ex);
                    }
                }
                else
                {
                    MessageBox.Show(" Field Cannot be empty!");
                }      
        }
    }

        private void EditCategoryUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            SQLiteDBManager dbManager = SQLiteDBManager.Instance;
            var conn = dbManager.Connection;
            using (var cmd = new SQLiteCommand(conn))
            {
                if (InputEditCategoryBox.Text != "")
                {
                    try
                    {
                        cmd.CommandText = "update Category SET Name = '" + this.InputEditCategoryBox.Text.ToString()
                       + "' where Id =" + Int32.Parse(this.InputEditCategoryIdBox.Text.ToString());
                        cmd.ExecuteNonQuery();
                        FillDataGrid();
                        EditCategoryElementInputBox.Visibility = System.Windows.Visibility.Hidden;
                        MessageBox.Show($"{InputEditCategoryBox.Text} Product is succesfully Updated", "Success",
                      MessageBoxButton.OK);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error Message : " + ex);
                    }
                }
                else
                {
                    MessageBox.Show(" Field Cannot be empty!");
                }
            }
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

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Message : " + ex);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            SQLiteDBManager dbManager = SQLiteDBManager.Instance;
            var conn = dbManager.Connection;
            try
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = @"select * from Category where Id Like @id OR Name Like @name Order by Id";
                cmd.Parameters.AddWithValue("@id", textBox_Search.Text);
                cmd.Parameters.AddWithValue("@name", textBox_Search.Text);
                cmd.ExecuteNonQuery();
               mAdapter = new SQLiteDataAdapter(cmd);
               dtable= new DataTable("Category");
                mAdapter.Fill(dtable);
                categoryDataGrid.ItemsSource = dtable.DefaultView;
                mAdapter.Update(dtable);
            }catch (Exception ex)
            {
                MessageBox.Show("Error Message : " + ex);
            }
        }

        public void Button_Delete_CategoryElement_Click(object sender, RoutedEventArgs e)
        {
            var Result = MessageBox.Show($"Do you want to delete {this.editcategoryname} ?", "Confirmation",
                             MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {

                try
                {
                   
                SQLiteDBManager dbManager = SQLiteDBManager.Instance;
                var conn = dbManager.Connection;
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "delete from Category where Id = " + Int32.Parse(editcategoryid);
                    MessageBox.Show("Succesfully Deleted");
                    cmd.ExecuteNonQuery();
                FillDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select Category to delete");
            }
            }
            else
            {
            }
        }
    }

}
