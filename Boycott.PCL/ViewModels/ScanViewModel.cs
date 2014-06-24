using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Boycott.PCL.Models;

namespace Boycott.PCL.ViewModels
{
    public class ScanViewModel : ViewModelBase
    {
        private ProductDataCache _productCache = ProductDataCache.Instance;
        private DoubleResult _result = new DoubleResult() { UnderBoycott = null, NotFound = null };

        public class DoubleResult
        {
            public bool? UnderBoycott { get; set; }
            public bool? NotFound { get; set; }
        };

        #region Properties

        public DoubleResult Result
        {
            get { return _result; }
            set
            {
                _result = value;
                RaisePropertyChanged(() => Result);
            }
        } 

        #endregion

        #region Commands

        /// <summary>
        /// Sample command
        /// </summary>
        public RelayCommand<string> FindBarcode { get; private set;}

        private async void InitializeCommands()
        {
            FindBarcode = new RelayCommand<string>(param =>
            {
                bool result = _productCache.Products.Exists(x => x.ItemBarCodes.Exists(y =>
                    string.Compare(y.ToString(), param.ToString()) == 0) == true);

                Result = new DoubleResult() { UnderBoycott = result, NotFound = !result };
            });
        }

        #endregion

        public ScanViewModel()
        {
            InitializeCommands();
        }    
    }
}