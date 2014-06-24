using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boycott.WP8.ViewModels
{
    public class ProductModel
    {
        public ProductGroup OtherProducts { get; set; }
        public ProductGroup Meat { get; set; }
        public ProductGroup Baby_Food { get; set; }
        public ProductGroup Seafood { get; set; }
        public ProductGroup Drinks { get; set; }
        public ProductGroup AllProducts { get; set; }

        public bool IsDataLoaded { get; set; }

        public void LoadData()
        {
            Meat = CreateMeatGroup();
            Drinks = CreateDrinksGroup();
            Baby_Food = CreateBabyFoodGroup();
            Seafood = CreateSeafoodGroup();
            AllProducts = CreateAllProductGroup();

            IsDataLoaded = true;
        }

        public ProductModel SearchInCategory(string categoryName, string searchString)
        {
            ProductModel searchProd = new ProductModel();
            searchProd.AllProducts = new ProductGroup();
            searchProd.Drinks = new ProductGroup();
            searchProd.Baby_Food = new ProductGroup();
            searchProd.Seafood = new ProductGroup();
            searchProd.Meat = new ProductGroup();

            //if category is All products
            if (categoryName.Equals("All"))
            {
                searchProd.AllProducts.Title = this.AllProducts.Title;

                foreach (ProductData item in this.AllProducts.Items)
                {
                    if (item.Title.ToUpper().Contains(searchString.ToUpper()))
                    {
                        searchProd.AllProducts.Items.Add(item);
                    }
                }
            }
            else
            {
                searchProd.AllProducts = this.AllProducts;
            }

            if (categoryName.Equals("Meat"))
            {
                searchProd.Meat.Title = this.Meat.Title;

                foreach (ProductData item in this.Meat.Items)
                {
                    if (item.Title.ToUpper().Contains(searchString.ToUpper()))
                    {
                        searchProd.Meat.Items.Add(item);
                    }
                }
            }
            else
            {
                searchProd.Meat = this.Meat;
            }

            if (categoryName.Equals("Baby food"))
            {
                searchProd.Baby_Food.Title = this.Baby_Food.Title;

                foreach (ProductData item in this.Baby_Food.Items)
                {
                    if (item.Title.ToUpper().Contains(searchString.ToUpper()))
                    {
                        searchProd.Baby_Food.Items.Add(item);
                    }
                }
            }
            else
            {
                searchProd.Baby_Food = this.Baby_Food;
            }

            if (categoryName.Equals("Seafood"))
            {
                searchProd.Seafood.Title = this.Seafood.Title;

                foreach (ProductData item in this.Seafood.Items)
                {
                    if (item.Title.ToUpper().Contains(searchString.ToUpper()))
                    {
                        searchProd.Seafood.Items.Add(item);
                    }
                }
            }
            else
            {
                searchProd.Seafood = this.Seafood;
            }

            if (categoryName.Equals("Drinks"))
            {
                searchProd.Drinks.Title = this.Drinks.Title;

                foreach (ProductData item in this.Drinks.Items)
                {
                    if (item.Title.ToUpper().Contains(searchString.ToUpper()))
                    {
                        searchProd.Drinks.Items.Add(item);
                    }
                }
            }
            else
            {
                searchProd.Drinks = this.Drinks;
            }
            
            return searchProd;
        }
        private ProductGroup CreateAllProductGroup()
        {
            ProductGroup data = new ProductGroup();
            data.Title = "All";

            data.Items.AddRange(this.Drinks.Items);
            data.Items.AddRange(this.Seafood.Items);
            data.Items.AddRange(this.Baby_Food.Items);
            data.Items.AddRange(this.Meat.Items);
         
            return data;
        }

        private ProductGroup CreateMeatGroup()
        {
            ProductGroup data = new ProductGroup();
            data.Title = "Meat";
            string basePath = "assets/ProductImages/";

            data.Items.Add(new ProductData
            {
                Title = "Xoz",
                FilePath = basePath + "finlandia.png"
            });

            return data;
        }

        private ProductGroup CreateBabyFoodGroup()
        {
            ProductGroup data = new ProductGroup();
            data.Title = "Baby food";
            string basePath = "assets/ProductImages/baby_food/";

            data.Items.Add(new ProductData
            {
                Title = "Kasha",
                FilePath = basePath + "kasha.jpg"
            });

            return data;
        }

        private ProductGroup CreateDrinksGroup()
        {
            ProductGroup data = new ProductGroup();
            data.Title = "Drinks";
            string basePath = "assets/ProductImages/";

            for (int i = 0; i < 10; i++)
            {
                if(i%2==0)
                data.Items.Add(new ProductData
                {
                    Title = "Finlandia",
                    FilePath = basePath + "finlandia.png"
                });

                data.Items.Add(new ProductData
                {
                    Title = "Sandora",
                    FilePath = basePath + "sandora.png"
                });

                data.Items.Add(new ProductData
                {
                    Title = "Jermuk",
                    FilePath = basePath + "jermuk.png"
                });
            }

            return data;
        }

        private ProductGroup CreateSeafoodGroup()
        {
            ProductGroup data = new ProductGroup();
            data.Title = "Seafood";
            string basePath = "assets/ProductImages/seafood/";

            data.Items.Add(new ProductData
            {
                Title = "Siga",
                FilePath = basePath + "siga.jpg"
            });

            return data;
        }
    }
}
