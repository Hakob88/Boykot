using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boycott.PCL.Common.Interfaces
{
    public interface ILocationTracker
    {
        PCL.Models.Location GetCurrentLocation();
    }
}

