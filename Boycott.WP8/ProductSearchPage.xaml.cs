using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Boycott.WP8.ViewModels;

namespace Boycott.WP8
{
    public partial class ProductSearchPage : PhoneApplicationPage
    {
        public ProductSearchPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            DataContext = App.ViewModel;
        }

       private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            /*TextBox obj = (TextBox)sender; 
            System.Diagnostics.Debug.WriteLine("Worked");
            if (obj.Text.Length >= 2)
            {
                ProductModel result = App.ViewModel.SearchInCategory("All", obj.Text);
                DataContext = result;
            //    foreach (ProductData item in App.ViewModel.Drinks.Items)
            //    {
            //        if (item.Title.ToUpper().Contains(obj.Text.ToUpper()))
            //        {
            //            System.Diagnostics.Debug.WriteLine("ok");
            //       }
            //    }
            }
            else
            {
                DataContext = App.ViewModel;
            }
            InvalidateMeasure();
             */
            this.SearchProduct();
        }

        private void ProductGroupPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Pivot obj = (Pivot)sender;
            //string headerName = ((PivotItem)obj.SelectedItem).Header.ToString();
           
            //if(((PivotItem)obj.SelectedItem).Header.Equals("Meat"))
            //{
              //  System.Diagnostics.Debug.WriteLine("MEAT");
            //}
            this.SearchProduct();
        }

        private void SearchProduct()
        {
            if(this.SearchTextBox.Text.Length >= 2)
            {
                string categoryName = ((PivotItem)this.ProductGroupPivot.SelectedItem).Header.ToString();
                System.Diagnostics.Debug.WriteLine(categoryName);
                ProductModel result = App.ViewModel.SearchInCategory(categoryName, this.SearchTextBox.Text);
                DataContext = result;
            }
            else
            {
                DataContext = App.ViewModel;
            }
            
        }

        private void SearchTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("MEAT");
            if (e.Key.Equals(Windows.System.VirtualKey.B))
            {
                // focus the page in order to remove focus from the text box
                // and hide the soft keyboard
                System.Diagnostics.Debug.WriteLine("MEAT");
            }
        }

        
    }
}