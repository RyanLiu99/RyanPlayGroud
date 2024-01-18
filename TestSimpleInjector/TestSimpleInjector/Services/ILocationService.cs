using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.EnterpriseServices;
using System.Linq;
using System.Web;

namespace TestSimpleInjector.Services
{
    public interface ILocationService
    {
        string GetCityByZip(int zip);
    }

    internal class LocationService : ILocationService
    {
        private int _index =0;
        public string GetCityByZip(int zip)
        {

            return $"City_Location_{_index++}_Controller_{zip}";
        }

    }
}