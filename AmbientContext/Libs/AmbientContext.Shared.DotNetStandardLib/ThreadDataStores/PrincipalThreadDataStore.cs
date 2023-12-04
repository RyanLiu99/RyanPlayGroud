using System.Collections.Concurrent;
using System.Threading;
using AmbientContext.Shared.DotNetStandardLib.Models;

namespace AmbientContext.Shared.DotNetStandardLib.ThreadDataStores
{
    //Temporary solution

    internal class PrincipalThreadDataStore : IImpThreadDataStore
    {
        
        public void AddObject(string key, object value)
        {
            MedrioPrincipal principal = AuthHelper.GetCurrentPrincipal();

            principal.DataBag[key] = value;
        }

        public object GetObject(string key)
        {
            MedrioPrincipal principal = AuthHelper.GetCurrentPrincipal();
            return principal.DataBag[key];
        }

        public bool HasKey(string key) => AuthHelper.GetCurrentPrincipal().DataBag.ContainsKey(key) ;

        public bool ClearObject(string key)
        {
            var bag = AuthHelper.GetCurrentPrincipal().DataBag;
            return bag.Remove(key);
        }
    }
}
