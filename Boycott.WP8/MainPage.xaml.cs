using Boycott.PCL.Common.Interfaces;
using Boycott.PCL.ViewModels;
using Boycott.WP8.Resources;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Boycott.WP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        #region Private properties 
        private Pushpin infoPushpinRef = null;
        private ILocationTracker _tracker = null;
        #endregion

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            var vm = DataContext as MainViewModel;

            if (vm != null)
            {
                vm.CheckedLocation += UserLocation_Checked;
            }

            ObservableCollection<DependencyObject> children = MapExtensions.GetChildren(map);
            infoPushpinRef = children.FirstOrDefault(x => x is Pushpin) as Pushpin;

            _tracker = SimpleIoc.Default.GetInstance<ILocationTracker>();
        }


        private void AppBarScanButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/BarcodeReaderPage.xaml", UriKind.Relative));
        }

        private void AppBarSearchButton_Click(object sender, EventArgs e)
        {
            string uriString = "/SearchPage.xaml";
            uriString += "?type=";

            if (MainPivot.SelectedItem == ProductsPivotItem)
            {
                uriString += "Product";
            }
            else if (MainPivot.SelectedItem == BusinessesPivotItem)
            {
                uriString += "Business";
            }

            NavigationService.Navigate(new Uri(uriString, UriKind.Relative));
        }

        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainPivot.SelectedItem == ProductsPivotItem)
            {
                ApplicationBar = Resources["ProductListAppBar"] as ApplicationBar;
            }
            else if (MainPivot.SelectedItem == BusinessesPivotItem)
            {
                ApplicationBar = Resources["BusinessMapAppBar"] as ApplicationBar;
            }
        }

        #region Map methods

        private void mapPushpin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Pushpin pinTapped = sender as Pushpin;

            if (pinTapped != null && infoPushpinRef != null)
            {
                infoPushpinRef.DataContext = pinTapped.Tag;
                infoPushpinRef.Visibility = Visibility.Visible;
            }

            //stop the event from going to map control
            e.Handled = true;
        }

        private void infoPushpin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Pushpin infoPushpinTapped = sender as Pushpin;

            if (infoPushpinTapped != null)
            {
                infoPushpinTapped.Visibility = Visibility.Collapsed;

                if (infoPushpinRef == null)
                {
                    infoPushpinRef = infoPushpinTapped;
                }
            }
        }

        private void map_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (infoPushpinRef != null)
            {
                infoPushpinRef.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region Popup methods

        private void checkBoycotted_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;

            if(vm != null)
            {
                var userLocation = _tracker.GetCurrentLocation();
                vm.FindNearestBoycottedBusiness.Execute(userLocation);
            }
        }

        private void UserLocation_Checked()
        {
            var vm = DataContext as MainViewModel;

            if (vm != null)
            {
                if(vm.NearestBoycottedBusiness != null)
                {
                    boycottedLabel.Visibility = Visibility.Visible;
                    notBoycottedLabel.Visibility = Visibility.Collapsed;
                }
                else 
                {
                    boycottedLabel.Visibility = Visibility.Collapsed;
                    notBoycottedLabel.Visibility = Visibility.Visible;
                }

                underBoycottPopup.Visibility = Visibility.Visible;
                checkBoycotted.IsEnabled = false;
            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            underBoycottPopup.Visibility = Visibility.Collapsed;
            checkBoycotted.IsEnabled = true;
        }

        #endregion


        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}