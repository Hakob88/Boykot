using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boycott.PCL.Helpers
{
    public class LocationTrackerUtils
    {
        public static readonly double LOCATION_UPDATE_THREASHOLD = 1; // in Kms

        public static double GetDistanceBetweenPoints(PCL.Models.Location src, PCL.Models.Location dest)
        {
            double distance = 0;

            double dLat = (dest.Latitude - src.Latitude) / 180 * Math.PI;
            double dLong = (dest.Longitude - src.Longitude) / 180 * Math.PI;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                        + Math.Cos(dest.Latitude) * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            //Calculate radius of earth
            // For this you can assume any of the two points.
            double radiusE = 6378135; // Equatorial radius, in metres
            double radiusP = 6356750; // Polar Radius

            //Numerator part of function
            double nr = Math.Pow(radiusE * radiusP * Math.Cos(src.Latitude / 180 * Math.PI), 2);
            //Denominator part of the function
            double dr = Math.Pow(radiusE * Math.Cos(src.Latitude / 180 * Math.PI), 2)
                             + Math.Pow(radiusP * Math.Sin(src.Latitude / 180 * Math.PI), 2);
            double radius = Math.Sqrt(nr / dr);

            distance = radius * c;

            distance = Math.Truncate(distance);
            distance = distance / 1000;
            //distance = Math.Round(distance, 1);

            return distance;
        }
    }
}
