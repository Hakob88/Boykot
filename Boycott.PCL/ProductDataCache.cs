using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Boycott.PCL.Models;

namespace Boycott.PCL
{
    public class ProductDataCache
    {
        private ProductDataSync _sync = new ProductDataSync();
        private static volatile ProductDataCache instance;
        private static object syncRoot = new Object();

        #region Events
        public delegate void RefreshEventDelegate();
        public event RefreshEventDelegate Refreshed;
        #endregion

        #region Properties
        public List<Product> Products { get; set; }
        public List<String> Categories { get; set; }
        #endregion

        public ProductDataCache()
        {
            Products = new List<Product>();
            Categories = new List<String>();
        }

        public static ProductDataCache Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ProductDataCache();
                    }
                }

                return instance;
            }
        }

        public async Task LoadCacheAsync(bool forceUpdate = false)
        {
            Models.Bootstrap.BootstrapInfo bootstrap = null;
            bootstrap = await _sync.GetBootstrapInfo();

            if (bootstrap != null)
            {
                if (bootstrap.productList != null)
                {
                    Products.Clear();

                    foreach (var bs_product in bootstrap.productList)
                    {
                        var product = Product.FromBootstrap(bs_product);
                        Products.Add(product);
                    }
                }

                if (bootstrap.categoryList != null)
                {
                    Categories.Clear();

                    foreach (var bs_category in bootstrap.categoryList)
                    {
                        Categories.Add(bs_category.categoryName);
                    }
                }
            }

            if (Refreshed != null)
                Refreshed();
        }
    }
}