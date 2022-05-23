using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace AspNetMvc
{
    public class ConditionalResponseCacheFilter : IActionFilter, IFilterMetadata
    {
        private readonly CacheProfile _cacheProfile;
        private static volatile int _c;

        public ConditionalResponseCacheFilter(CacheProfile cacheProfile)
        {
            this._cacheProfile = cacheProfile;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            IHeaderDictionary headers = context.HttpContext.Response.Headers;

            ((IDictionary<string, StringValues>)headers).Remove(HeaderNames.Vary);
            ((IDictionary<string, StringValues>)headers).Remove(HeaderNames.CacheControl);
            ((IDictionary<string, StringValues>)headers).Remove(HeaderNames.Pragma);

            if (_c++ % 2 == 0) return;

            //if (!string.IsNullOrEmpty(this.VaryByHeader))
            //    headers[HeaderNames.Vary] = (StringValues)this.VaryByHeader;
            //if (this.VaryByQueryKeys != null)
            //{
            //    IResponseCachingFeature responseCachingFeature = context.HttpContext.Features.Get<IResponseCachingFeature>();
            //    if (responseCachingFeature == null)
            //        throw new InvalidOperationException(Resources.FormatVaryByQueryKeys_Requires_ResponseCachingMiddleware((object)"VaryByQueryKeys"));
            //    responseCachingFeature.VaryByQueryKeys = this.VaryByQueryKeys;
            //}

            if (this._cacheProfile.NoStore.GetValueOrDefault())
            {
                headers[HeaderNames.CacheControl] = (StringValues) "no-store";
                if (this._cacheProfile.Location != ResponseCacheLocation.None)
                    return;
                headers.AppendCommaSeparatedValues(HeaderNames.CacheControl, "no-cache");
                headers[HeaderNames.Pragma] = (StringValues) "no-cache";
            }
            else
            {
                string str1;
                switch (this._cacheProfile.Location)
                {
                    case ResponseCacheLocation.Any:
                        str1 = "public,";
                        break;
                    case ResponseCacheLocation.Client:
                        str1 = "private,";
                        break;
                    case ResponseCacheLocation.None:
                        str1 = "no-cache,";
                        headers[HeaderNames.Pragma] = (StringValues) "no-cache";
                        break;
                    default:
                        str1 = (string) null;
                        break;
                }

                string str2 = string.Format("{0}max-age={1}", (object) str1,
                    (object) this._cacheProfile.Duration.GetValueOrDefault());
                headers[HeaderNames.CacheControl] = (StringValues) str2;

            }
        }
    }
}

