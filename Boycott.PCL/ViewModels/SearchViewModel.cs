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
        private ProductDataCache _productCache = ProductDataCache.Instance;
        private List<Product> _results = new List<Product>();

        #region Properties

        public List<Product> Results
        {
            get { return _results; }
            set
            {
                _results = value;
                RaisePropertyChanged(() => Results);
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Find product command
        /// </summary>
        public RelayCommand<string> FindProduct { get; private set;}

        private async void InitializeCommands()
        {
            FindProduct = new RelayCommand<string>(param =>
            {
                Results.Clear();
                var result = new List<Product>();

                if (!string.IsNullOrEmpty(param.Trim()))
                {   
                    result = _productCache.Products.FindAll(x => x.Name.ToLower().Contains(param.Trim().ToLower()));
                }

                Results = result;
            });
        }

        #endregion

        public SearchViewModel()
        {
            InitializeCommands();
        }    
    }
}
