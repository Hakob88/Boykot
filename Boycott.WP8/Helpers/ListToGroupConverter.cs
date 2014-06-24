using Boycott.PCL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Boycott.WP8.Helpers
{
    public class ListToGroupConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetCityGroups(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


        public class Group<T> : List<T>
        {
            public Group(string name, IEnumerable<T> items)
                : base(items)
            {
                this.Title = name;
            }

            public string Title
            {
                get;
                set;
            }
        }

        private List<Group<Product>> GetCityGroups(object value)
        {
            IEnumerable<Product> list = value as List<Product>;

            IEnumerable<Group<Product>> groupList = from item in list
                                                    group item by item.Category into g
                                                    orderby g.Key
                                                    select new Group<Product>(g.Key, g);
            return groupList.ToList();
        }

    }

        
}
