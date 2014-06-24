using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Boycott.PCL.Models;
using Boycott.PCL.Helpers;

namespace Boycott.PCL
{
    public class BoycottDataCache
    {
        private BoycottDataSync _sync = new BoycottDataSync();
        private static volatile BoycottDataCache instance;
        private static object syncRoot = new Object();

        #region Events
        public delegate void RefreshEventDelegate();
        public event RefreshEventDelegate Refreshed;

        public delegate void RefreshProductsEventDelegate();
        public event RefreshProductsEventDelegate RefreshedProducts;

        public delegate void RefreshBusinessesEventDelegate();
        public event RefreshBusinessesEventDelegate RefreshedBusinesses;
        #endregion

        #region Properties
        public List<Product> Products { get; set; }
        public List<Business> Businesses { get; set; }
        #endregion

        public BoycottDataCache()
        {
            Products = new List<Product>();
            Businesses = new List<Business>();
        }

        public static BoycottDataCache Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new BoycottDataCache();
                    }
                }

                return instance;
            }
        }

        public async Task LoadCacheAsync()
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

                    if (RefreshedProducts != null)
                        RefreshedProducts();
                }

                if (bootstrap.businessList != null)
                {
                    Businesses.Clear();

                    foreach (var bs_business in bootstrap.businessList)
                    {
                        foreach (var bs_location_list_item in bs_business.locationList)
                        {
                            var business_copy = Business.FromBootstrap(bs_business);
                            business_copy.Location = new Location(bs_location_list_item.location.latitude,
                                                                  bs_location_list_item.location.longitude);
                            business_copy.Address = bs_location_list_item.address;
                            business_copy.Radius = bs_location_list_item.radius;

                            Businesses.Add(business_copy);
                        }
                    }

                    if (RefreshedBusinesses != null)
                        RefreshedBusinesses();
                }
            }

            if (Refreshed != null)
                Refreshed();
        }
    }
}