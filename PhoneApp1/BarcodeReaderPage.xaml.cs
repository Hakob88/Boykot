﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp1.Resources;
using Microsoft.Devices;
using ZXing;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.Xna.Framework.Media;
using Windows.Storage;

namespace PhoneApp1
{
    public partial class BarcodeReaderPage : PhoneApplicationPage
    {
        private PhotoCamera _phoneCamera;
        private IBarcodeReader _barcodeReader;
        private DispatcherTimer _scanTimer;
        private WriteableBitmap _previewBuffer;
        private BoycottBarcodes _boycott;
        private bool isTakingPhoto = false;
        private MediaLibrary _library;



        public BarcodeReaderPage()
        {
            InitializeComponent();
            _boycott = new BoycottBarcodes();
            MediaLibrary _library = new MediaLibrary();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (PhotoCamera.IsCameraTypeSupported(CameraType.Primary) == true)
            {

            }
            else
            {
                MessageBox.Show("A Camera is not available on this device.");
            }
            textBlockCapture.Visibility = System.Windows.Visibility.Collapsed;
            tbBarcodeType.Visibility = Visibility.Visible;
            tbBarcodeData.Visibility = System.Windows.Visibility.Visible;
            // Initialize the camera object
            _phoneCamera = new PhotoCamera(CameraType.Primary);
            _phoneCamera.Initialized += cam_Initialized;
            _phoneCamera.AutoFocusCompleted += _phoneCamera_AutoFocusCompleted;
            _phoneCamera.CaptureImageAvailable += _phoneCamera_CaptureImageAvailable;

            CameraButtons.ShutterKeyHalfPressed += CameraButtons_ShutterKeyHalfPressed;

            //Display the camera feed in the UI
            viewfinderBrush.SetSource(_phoneCamera);


            // This timer will be used to scan the camera buffer every 250ms and scan for any barcodes
            _scanTimer = new DispatcherTimer();
            _scanTimer.Interval = TimeSpan.FromMilliseconds(250);
            _scanTimer.Tick += (o, arg) => ScanForBarcode();

            viewfinderCanvas.Tap += new EventHandler<GestureEventArgs>(focus_Tapped);

            base.OnNavigatedTo(e);
        }

        void _phoneCamera_CaptureImageAvailable(object sender, ContentReadyEventArgs e)
        {
            
            isTakingPhoto = false;
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            string fileName =  localFolder.Path + "\\sss.jpg";

            try
            {   // Write message to the UI thread.

                // Save photo to the media library camera roll.
                //_library.SavePictureToCameraRoll(fileName, e.ImageStream);


                // Set the position of the stream back to start
                e.ImageStream.Seek(0, System.IO.SeekOrigin.Begin);

                // Save photo as JPEG to the local folder.
                using (System.IO.IsolatedStorage.IsolatedStorageFile isStore = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (System.IO.IsolatedStorage.IsolatedStorageFileStream targetStream = isStore.OpenFile(fileName, FileMode.Create, FileAccess.Write))
                    {
                        // Initialize the buffer for 4KB disk pages.
                        byte[] readBuffer = new byte[4096];
                        int bytesRead = -1;

                        // Copy the image to the local folder. 
                        while ((bytesRead = e.ImageStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                        {
                            targetStream.Write(readBuffer, 0, bytesRead);
                        }
                    }
                }

            }
            finally
            {
                // Close image stream
                e.ImageStream.Close();
            }

            // send the request
            Deployment.Current.Dispatcher.BeginInvoke(delegate()
            {
                NavigationService.Navigate(new Uri("/ImagePage.xaml", UriKind.Relative));
                
            });
            
            

        }

        void _phoneCamera_AutoFocusCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(delegate()
            {
                focusBrackets.Visibility = Visibility.Collapsed;
            });
            if (isTakingPhoto)
            {
                _phoneCamera.CaptureImage();
            }
        }

        void focus_Tapped(object sender, GestureEventArgs e)
        {
            if (_phoneCamera != null)
            {
                if (_phoneCamera.IsFocusAtPointSupported == true)
                {
                    // Determine the location of the tap.
                    Point tapLocation = e.GetPosition(viewfinderCanvas);

                    // Position the focus brackets with the estimated offsets.
                    focusBrackets.SetValue(Canvas.LeftProperty, tapLocation.X - 30);
                    focusBrackets.SetValue(Canvas.TopProperty, tapLocation.Y - 28);

                    // Determine the focus point.
                    double focusXPercentage = tapLocation.X / viewfinderCanvas.ActualWidth;
                    double focusYPercentage = tapLocation.Y / viewfinderCanvas.ActualHeight;

                    // Show the focus brackets and focus at point.
                    focusBrackets.Visibility = Visibility.Visible;
                    _phoneCamera.FocusAtPoint(focusXPercentage, focusYPercentage);
                }
            }
        }

        void CameraButtons_ShutterKeyHalfPressed(object sender, EventArgs e)
        {
            _phoneCamera.Focus();
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            //we're navigating away from this page, we won't be scanning any barcodes
            _scanTimer.Stop();

            if (_phoneCamera != null)
            {
                // Cleanup
                _phoneCamera.Dispose();
                _phoneCamera.Initialized -= cam_Initialized;
                CameraButtons.ShutterKeyHalfPressed -= CameraButtons_ShutterKeyHalfPressed;
            }
        }

        void cam_Initialized(object sender, Microsoft.Devices.CameraOperationCompletedEventArgs e)
        {
            if (e.Succeeded)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    _phoneCamera.FlashMode = FlashMode.Off;
                    _previewBuffer = new WriteableBitmap((int)_phoneCamera.PreviewResolution.Width, (int)_phoneCamera.PreviewResolution.Height);

                    _barcodeReader = new BarcodeReader();

                    // By default, BarcodeReader will scan every supported barcode type
                    // If we want to limit the type of barcodes our app can read, 
                    // we can do it by adding each format to this list object

                    //var supportedBarcodeFormats = new List<BarcodeFormat>();
                    //supportedBarcodeFormats.Add(BarcodeFormat.QR_CODE);
                    //supportedBarcodeFormats.Add(BarcodeFormat.DATA_MATRIX);
                    //_bcReader.PossibleFormats = supportedBarcodeFormats;

                    _barcodeReader.TryHarder = true;

                    _barcodeReader.ResultFound += _bcReader_ResultFound;
                    _scanTimer.Start();
                });
            }
            else
            {
                Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Unable to initialize the camera");
                    });
            }
        }

        void _bcReader_ResultFound(Result obj)
        {
            // If a new barcode is found, vibrate the device and display the barcode details in the UI
            if (!obj.Text.Equals(tbBarcodeData.Text))
            {
                if (_boycott.isBoycottBarcode(obj.Text))
                {
                    MessageBox.Show("This product is under boycott!! please don't buy it");
                }
                else
                {
                    popupNotFound.IsOpen = true;
                }
                tbBarcodeType.Text = obj.BarcodeFormat.ToString();
                tbBarcodeData.Text = obj.Text;
            }
        }

        private void ScanForBarcode()
        {
            if (popupNotFound.IsOpen || isTakingPhoto)
            {
                return;
            }
            //grab a camera snapshot
            _phoneCamera.GetPreviewBufferArgb32(_previewBuffer.Pixels);
            _previewBuffer.Invalidate();

            //scan the captured snapshot for barcodes
            //if a barcode is found, the ResultFound event will fire
            _barcodeReader.Decode(_previewBuffer);

        }

        private void buttonYes_Click(object sender, RoutedEventArgs e)
        {
            popupNotFound.IsOpen = false;
            textBlockCapture.Visibility = System.Windows.Visibility.Visible;
            tbBarcodeType.Visibility = Visibility.Collapsed;
            tbBarcodeData.Visibility = System.Windows.Visibility.Collapsed;
            isTakingPhoto = true;
            _scanTimer.Stop();
        }

        private void buttonNo_Click(object sender, RoutedEventArgs e)
        {
            popupNotFound.IsOpen = false;
        }
    }
}