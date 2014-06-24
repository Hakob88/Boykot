using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Storage;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhoneApp1
{
    public partial class ImagePage : PhoneApplicationPage
    {
        public ImagePage()
        {
            InitializeComponent();
            Loaded += ImagePage_Loaded;
            

            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            string fileName =  localFolder.Path + "\\temp.jpg";
            capturedImage.Source = new BitmapImage(new Uri(fileName, UriKind.Absolute));
        }

        void ImagePage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.Text = "Sending the image";
        }

        private void sendImageButton_Click(object sender, RoutedEventArgs e)
        {

            Deployment.Current.Dispatcher.BeginInvoke(delegate()
            {
                //send the request with the image
                SystemTray.ProgressIndicator.IsVisible = true;
                SystemTray.ProgressIndicator.IsIndeterminate = true;
                //TODO: stop the progress indication when the request is completed and remove the image.

            });

        }
    }
}