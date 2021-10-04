using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Medrio.ActivityAuditLog;
using Medrio.Infrastructure.Ioc.Dependency;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Medrio.Logging.Gcp
{
    [RegisterAs(typeof(ILogRequestBuilder<Struct>))]
    internal class GcpLogRequestBuilder : LogRequestBuilder<Struct>
    {
        protected override Struct CreatePayLoad(HttpContext httpContext)
        {
            Exception exception = new Exception("Fake exception");
            EventId eventId = new EventId(88, "TestGcpLoggingEventId");
            var jsonStruct = new Struct();
            jsonStruct.Fields.Add("message", Value.ForString("message"));

            //if (_loggerOptions.ServiceContext != null)
            //{
            //    jsonStruct.Fields.Add("serviceContext", Value.ForStruct(_loggerOptions.ServiceContext));
            //}
            if (exception != null)
            {
                jsonStruct.Fields.Add("exception", Value.ForString(exception.ToString()));
            }

            if (eventId.Id != 0 || eventId.Name != null)
            {
                var eventStruct = new Struct();
                if (eventId.Id != 0)
                {
                    eventStruct.Fields.Add("id", Value.ForNumber(eventId.Id));
                }
                if (!string.IsNullOrWhiteSpace(eventId.Name))
                {
                    eventStruct.Fields.Add("name", Value.ForString(eventId.Name));
                }
                jsonStruct.Fields.Add("event_id", Value.ForStruct(eventStruct));
            }

            var requestStruct = new Struct();
            var requestHeaderStruct = new Struct();
            foreach (var header in httpContext.Request.Headers)
            {
                requestHeaderStruct.Fields.Add(
                    header.Key.Replace('-', '_').Replace(':', '_'),  //For Big query , Fields must contain only letters, numbers, and underscores. Not start with letter.  Use regex or use data flow clean up data before import to big query
                    header.Value.Count > 1 ?
                    Value.ForList(header.Value.Select(Value.ForString).ToArray()) :
                    Value.ForString(header.Value)
                    );
            }

            requestStruct.Fields.Add("header", Value.ForStruct(requestHeaderStruct));
            jsonStruct.Fields.Add("request", Value.ForStruct(requestStruct));

            return jsonStruct;
        }
    }
}
