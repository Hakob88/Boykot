using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boycott.PCL.Models.Bootstrap
{
    public class Category
    {
        public string categoryId { get; set; }
        public string categoryName { get; set;}
    }

    public class Product
    {
        public string itemName { get; set; }
        public string imageHash { get; set; }
        public string imageUrl { get; set; }
        public Category category { get; set; }
        public List<String> itemBarCodes { get; set; }
    }

    public class BootstrapInfo
    {
        public List<Category> categoryList { get; set; }
        public List<Product> productList { get; set; }
    }
}

