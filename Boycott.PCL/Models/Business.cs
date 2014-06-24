using Boycott.PCL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Boycott.PCL.Models
{
    public class Business
    {
        #region Public Properties

        public string Name { get; set; }
        public string OwnerName { get; set; }
        public string Address { get; set; }
        public Double Radius { get; set; }
        public Location Location { get; set; }

        #endregion

        #region Internal Properties

        #endregion

        public Business()
        {
        }

        public static Business FromBootstrap(Models.Bootstrap.Business business)
        {
            return new Business
            {
                Name = business.businessName,
                OwnerName = business.ownerName,
            };
        }
    }
}
