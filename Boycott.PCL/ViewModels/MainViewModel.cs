using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Boycott.PCL.Models;
using Boycott.PCL.Helpers;
using Boycott.PCL.Common.Interfaces;

namespace Boycott.PCL.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private BoycottDataCache _productCache = BoycottDataCache.Instance;
        private List<Product> _products = new List<Product>();
        private ObservableCollection<Business> _businesses = new ObservableCollection<Business>();
        private Business _nearestBoycottedBusiness = null;

        #region Properties

        public List<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                RaisePropertyChanged(() => Products);
            }
        }

        public Business NearestBoycottedBusiness
        {
            get { return _nearestBoycottedBusiness; }
            set
            {
                _nearestBoycottedBusiness = value;
                RaisePropertyChanged(() => NearestBoycottedBusiness);
            }
        }

        public ObservableCollection<Business> Businesses
        {
            get { return _businesses; }
            set
            {
                _businesses = value;
                RaisePropertyChanged(() => Businesses);
            }
        }

        #endregion

        #region Events
        public delegate void CheckedLocationEventDelegate();
        public event CheckedLocationEventDelegate CheckedLocation;

        #endregion

        #region Commands

        /// <summary>
        /// Load data command
        /// </summary>
        public RelayCommand LoadData { get; private set;}

        /// <summary>
        /// Check if user current location for boycotted
        /// </summary>
        public RelayCommand<Location> FindNearestBoycottedBusiness { get; private set; }

        private async void InitializeCommands()
        {
            LoadData = new RelayCommand(() =>
            {
                _productCache.LoadCacheAsync();
            });

            FindNearestBoycottedBusiness = new RelayCommand<Location>(param =>
            {
                NearestBoycottedBusiness = null;
                var location = param as Location;

                if(location != null)
                {
                    Business nearestBusiness = null;

                    foreach (var business in _businesses)
                    {
                        if (LocationTrackerUtils.GetDistanceBetweenPoints(location, business.Location) <= business.Radius)
                        {
                            nearestBusiness = business;
                            break;
                        }
                    }

                    if (nearestBusiness != null)
                    {
                        NearestBoycottedBusiness = nearestBusiness;
                    }

                    if (CheckedLocation != null)
                        CheckedLocation();
                }
            });
        }

        #endregion

        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            _productCache.RefreshedProducts += ProductList_Refereshed;
            _productCache.RefreshedBusinesses += BusinessList_Refereshed;

            InitializeCommands();
            InitializeDataSync();
        }

        private async Task InitializeDataSync()
        {
            await _productCache.LoadCacheAsync();
        }


        void ProductList_Refereshed()
        {
            Products = _productCache.Products;
        }

        void BusinessList_Refereshed()
        {
            foreach (var b in _productCache.Businesses)
            {
                Businesses.Add(b);
            }
        }

    }
}