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
using Boycott.PCL.ViewModels;
using System.Windows.Threading;

namespace Boycott.WP8
{
    public partial class ProductSearchPage : PhoneApplicationPage
    {
        private DispatcherTimer _timer;

        public ProductSearchPage()
        {
            InitializeComponent();

            this.Loaded += SearchPage_Loaded;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(600);
            _timer.Tick += timerTicker;
        }

        private void SearchPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            SearchTextBox.Focus();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e != null)
            {
                _timer.Start();
            }
        }

        private void SearchTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.Equals(Windows.System.VirtualKey.B))
            {
                // focus the page in order to remove focus from the text box
                // and hide the soft keyboard
            }
        }

        private void SearchWithKeyword(string query)
        {
            var vm = DataContext as SearchViewModel;

            if (vm != null)
            {
                vm.FindProduct.Execute(query);
            }
        }

        private void timerTicker(object sender, EventArgs e)
        {
            SearchWithKeyword(SearchTextBox.Text);

            _timer.Stop();
        }
    }
}