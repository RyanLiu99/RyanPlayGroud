using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace AspNetMvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]

    public class ConditionalCacheAttribute : Attribute, IFilterFactory, IOrderedFilter
    {
        public bool IsReusable => true;
        public int Order { get; set; }

        private int? _duration;

        public int Duration
        {
            get => this._duration.GetValueOrDefault();
            set => this._duration = new int?(value);
        }


        private bool? _noStore;

        public bool NoStore
        {
            get => this._noStore.GetValueOrDefault();
            set => this._noStore = new bool?(value);
        }


        private ResponseCacheLocation? _location;
        public ResponseCacheLocation Location
        {
            get => this._location.GetValueOrDefault();
            set => this._location = new ResponseCacheLocation?(value);
        }

        private CacheProfile GetCacheProfile()
        {
            return new CacheProfile()
            {
                 Duration = this.Duration,
                 Location =  this.Location,
                 NoStore = this.NoStore
            };
        }

        public new IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new ConditionalResponseCacheFilter( this.GetCacheProfile());
        }

    }
}