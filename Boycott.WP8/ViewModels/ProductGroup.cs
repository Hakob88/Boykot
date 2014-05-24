using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boycott.WP8.ViewModels
{
    public class ProductGroup
    {
        public ProductGroup() 
        {
            Items = new List<ProductData>();
        }

        public List<ProductData> Items { get; set; }
        public string Title { get; set; }
    }
}
