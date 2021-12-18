using Google.Protobuf.WellKnownTypes;
using Medrio.Infrastructure.Ioc.Dependency;
using System.Linq;



namespace Medrio.ActivityAuditLog.Gcp
{
    [RegisterAs(typeof(ILogRequestBuilder<Struct>))]
    //TODO: make it internal
    public class GcpLogRequestBuilder : LogRequestBuilder<Struct>
    {
#if NETFRAMEWORK
        protected override Struct CreatePayLoad(System.Web.HttpContext httpContext)
        {   
            //Url, Header, body already contains everything            
            
            var payLoadStruct = new Struct();
            payLoadStruct.Fields.Add(URL, Value.ForString(httpContext.Request.Url.ToString()));

            var requestStruct = new Struct();
            requestStruct.Fields.Add(HttpMethod, Value.ForString(httpContext.Request.HttpMethod));

            var requestHeaderStruct = new Struct();

            var headers = httpContext.Request.Headers;
            foreach (var header in headers)
            {
                //For Big query , Fields must contain only letters, numbers, and underscores. Not start with letter.
                //Use regex to replace special char or use data flow clean up data before import to big query
                requestHeaderStruct.Fields.Add(header.ToString().Replace('-', '_').Replace(':', '_'), 
                    Value.ForString(headers[header.ToString()]));
            }

            requestStruct.Fields.Add(Header, Value.ForStruct(requestHeaderStruct));
            var body = GetRequestBody(httpContext);
            if (!string.IsNullOrWhiteSpace(body)) requestStruct.Fields.Add(Body, Value.ForString(body));

            payLoadStruct.Fields.Add(Request, Value.ForStruct(requestStruct));

            return payLoadStruct;
        }

#else
        protected override Struct CreatePayLoad(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {            
            var payLoadStruct = new Struct();
            var requestStruct = new Struct();
            requestStruct.Fields.Add(HttpMethod, Value.ForString(httpContext.Request.Method));
            payLoadStruct.Fields.Add(URL, Value.ForString(Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(httpContext.Request)));

            var requestHeaderStruct = new Struct();
            foreach (var header in httpContext.Request.Headers)
            {
                //For Big query , Fields must contain only letters, numbers, and underscores. Not start with letter.
                //Use regex to replace special chars or use data flow clean up data before import to big query
                requestHeaderStruct.Fields.Add(
                    header.Key.Replace('-', '_').Replace(':',  '_'),                         
                    Value.ForString(string.Join(",",header.Value)) //we can use Value.ForList(), but make it same as .net framework version
                );
            }

            requestStruct.Fields.Add(Header, Value.ForStruct(requestHeaderStruct));
            payLoadStruct.Fields.Add(Request, Value.ForStruct(requestStruct));

            return payLoadStruct;
        }

#endif
    }
}
