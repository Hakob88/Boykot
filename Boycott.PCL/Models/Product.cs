using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boycott.PCL.Models
{
    public class Product
    {
        #region Public Properties
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Category { get; set; }
        public List<String> ItemBarCodes { get; set; }
        #endregion

        #region Internal Properties
        protected Models.Bootstrap.Product Bootstrap { get; set; }
        #endregion

        public Product()
        {
        }

        static public Product FromBootstrap(Models.Bootstrap.Product product)
        {
            return new Product
            {
                Name = product.itemName,
                ImageUrl = product.imageUrl,
                Category = product.category.categoryName,
                ItemBarCodes = product.itemBarCodes,
                Bootstrap = product
            };
        }
    }
}
