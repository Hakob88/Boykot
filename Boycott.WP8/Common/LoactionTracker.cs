using Boycott.PCL.Common.Interfaces;
using Boycott.PCL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Boycott.WP8.Common.Interfaces
{
    class LocationTracker : ILocationTracker
    {
        private Geolocator _geolocator;
        private PCL.Models.Location _location = new PCL.Models.Location(-1, -1);

        public LocationTracker()
        {
            // initializing geolocator
            _geolocator = new Geolocator();
            _geolocator.DesiredAccuracy = PositionAccuracy.High;
            _geolocator.MovementThreshold = 10;
            _geolocator.PositionChanged += geolocator_PositionChanged;
        }

        private void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            if (args != null && args.Position != null)
            {
                _location.Longitude = args.Position.Coordinate.Longitude;
                _location.Latitude = args.Position.Coordinate.Latitude;
            }
        }

        public PCL.Models.Location GetCurrentLocation()
        {
            return _location;
        }
    }
}



