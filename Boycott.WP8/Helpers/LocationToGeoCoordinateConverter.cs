using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Boycott.PCL.Models;

namespace Boycott.WP8.Helpers
{
    public class LocationToGeoCoordinateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GeoCoordinate coordinate = new GeoCoordinate(0,0);
            
            if (value != null)
            {
                var location = value as Location;
                coordinate.Latitude = location.Latitude;
                coordinate.Longitude = location.Longitude;
            }

            return coordinate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

