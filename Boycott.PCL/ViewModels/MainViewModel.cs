using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Boycott.PCL.Models;

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
        private ProductDataCache _productCache = ProductDataCache.Instance;
        private List<Category> _categories = new List<Category>();
        private List<Product> _products = new List<Product>();

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

        public class Category
        {
            public string Title { get; set; }
            public List<Product> Products { get; set; }
        }

        public List<Category> Categories { get { return _categories; } }

        #endregion

        #region Commands

        /// <summary>
        /// Load data command
        /// </summary>
        public RelayCommand LoadData { get; private set;}

        private async void InitializeCommands()
        {
            LoadData = new RelayCommand(() =>
            {
                _productCache.LoadCacheAsync();
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

            _productCache.Refreshed += ProductCache_Refereshed;

            InitializeCommands();
            InitializeDataSync();
        }

        private async Task InitializeDataSync()
        {
            await _productCache.LoadCacheAsync();
        }

        void ProductCache_Refereshed()
        {
            var groupedCategories = _productCache.Products.GroupBy(x => x.Category)
                .Select(x => new Category { Title = x.Key, Products = x.ToList() });
            _categories.Clear();

            foreach(var _category in groupedCategories)
            {
                _categories.Add(_category);
            }

            Products = _productCache.Products;
            
        }

        
    }
}