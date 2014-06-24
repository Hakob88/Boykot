using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Boycott.PCL.Models;

namespace Boycott.PCL.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {
        private BoycottDataCache _boycottCache = BoycottDataCache.Instance;
        private List<Product> _filteredProductList = new List<Product>();
        private List<Business> _filteredBusinessList = new List<Business>();

        #region Properties

        public List<Product> FilteredProductList
        {
            get { return _filteredProductList; }
            set
            {
                _filteredProductList = value;
                RaisePropertyChanged(() => FilteredProductList);
            }
        }

        public List<Business> FilteredBusinessList
        {
            get { return _filteredBusinessList; }
            set
            {
                _filteredBusinessList = value;
                RaisePropertyChanged(() => FilteredBusinessList);
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Filter products with keyword
        /// </summary>
        public RelayCommand<string> FilterProductsWithKeyword { get; private set; }

        /// <summary>
        /// Filter businesses with keyword
        /// </summary>
        public RelayCommand<string> FilterBusinessesWithKeyword { get; private set; }

        private async void InitializeCommands()
        {
            FilterProductsWithKeyword = new RelayCommand<string>(param =>
            {
                FilteredProductList.Clear();
                var result = new List<Product>();

                if (!string.IsNullOrEmpty(param.Trim()))
                {   
                    result = _boycottCache.Products.FindAll(x => x.Name.ToLower().Contains(param.Trim().ToLower()));
                }

                FilteredProductList = result;
            });

            FilterBusinessesWithKeyword = new RelayCommand<string>(param =>
            {
                FilteredBusinessList.Clear();
                var result = new List<Business>();

                if (!string.IsNullOrEmpty(param.Trim()))
                {
                    result = _boycottCache.Businesses.FindAll(x => x.Name.ToLower().Contains(param.Trim().ToLower()) ||
                        x.OwnerName.ToLower().Contains(param.Trim().ToLower()));
                    result = result.GroupBy(x => x.Name).Select(grp => grp.First()).ToList();
                }

                FilteredBusinessList = result;
            });
        }

        #endregion

        public SearchViewModel()
        {
            InitializeCommands();
        }    
    }
}
