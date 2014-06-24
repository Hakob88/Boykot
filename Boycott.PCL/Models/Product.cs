using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boycott.PCL.Models
{
    public class ProductList : List<object>
    {
        public object Key { get; set; }

        public new IEnumerator<object> GetEnumerator()
        {
            return (IEnumerator<object>)base.GetEnumerator();
        }
    }


    public class Product
    {
        #region Public Properties

        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Category { get; set; }
        public List<Int64> ItemBarCodes { get; set; }

        #endregion

        #region Internal Properties
        protected Models.Bootstrap.Product Bootstrap { get; set; }
        #endregion

        public Product()
        {
        }

        static public Product FromBootstrap(Models.Bootstrap.Product product)
        {
            List<Int64> barcodes = new List<Int64>();

            foreach (var code in product.itemBarCodes)
            {
                barcodes.Add(Int64.Parse(code));
            }

            return new Product
            {
                Name = product.itemName,
                ImageUrl = product.imageUrl,
                Category = product.category.categoryName,
                ItemBarCodes = barcodes,
                Bootstrap = product
            };
        }
    }
}
