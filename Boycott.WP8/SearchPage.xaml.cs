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
    public partial class SearchPage : PhoneApplicationPage
    {
        private DispatcherTimer _timer;
        private string filterListType = "";

        public SearchPage()
        {
            InitializeComponent();

            this.Loaded += SearchPage_Loaded;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(600);
            _timer.Tick += timerTicker;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("type"))
            {
                string typeString = NavigationContext.QueryString["type"].ToLower();

                if (!String.IsNullOrEmpty(typeString))
                {
                    filterListType = typeString;
                    pageName.Text += " " + typeString;

                    if (typeString == "product")
                    {
                        filteredProductLongListSelector.Visibility = Visibility.Visible;
                    }
                    else if (typeString == "business")
                    {
                        filteredBusinessLongListSelector.Visibility = Visibility.Visible;
                    }
                }
            }
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
                if (filterListType == "product")
                {
                    vm.FilterProductsWithKeyword.Execute(query);
                }
                else if (filterListType == "business")
                {
                    vm.FilterBusinessesWithKeyword.Execute(query);
                }
            }
        }

        private void timerTicker(object sender, EventArgs e)
        {
            SearchWithKeyword(SearchTextBox.Text);

            _timer.Stop();
        }
    }
}