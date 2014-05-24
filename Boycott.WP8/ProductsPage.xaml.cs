using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Boycott.WP8.Resources;
using Boycott.WP8.ViewModels;

namespace Boycott.WP8
{
    public partial class ProductsPage : PhoneApplicationPage
    {
        public ProductsPage()
        {
            InitializeComponent();
        }
        public static ProductModel viewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            DataContext = App.ViewModel;
        }

        private void barSearchButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ProductSearchPage.xaml", UriKind.Relative));
        }
    }
   
}