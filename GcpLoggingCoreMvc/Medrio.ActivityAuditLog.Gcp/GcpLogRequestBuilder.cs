using Google.Protobuf.WellKnownTypes;
using Medrio.Infrastructure.Ioc.Dependency;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Medrio.ActivityAuditLog.Gcp
{
    [RegisterAs(typeof(ILogRequestBuilder<Struct>))]
    //TODO: make it internal
    public class GcpLogRequestBuilder : LogRequestBuilder<Struct>
    {
        protected override Struct CreatePayLoad(HttpContext httpContext)
        {
            EventId eventId = new EventId(88, "TestGcpLoggingEventId");
            var jsonStruct = new Struct();
            jsonStruct.Fields.Add("message", Value.ForString("message"));  //message has special meaning when showing on the log explorer

            //if (_loggerOptions.ServiceContext != null)
            //{
            //    jsonStruct.Fields.Add("serviceContext", Value.ForStruct(_loggerOptions.ServiceContext));
            //}

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
                    header.Key.Replace('-', '_')
                        .Replace(':',
                            '_'), //For Big query , Fields must contain only letters, numbers, and underscores. Not start with letter.  Use regex or use data flow clean up data before import to big query
                    header.Value.Count > 1
                        ? Value.ForList(header.Value.Select(Value.ForString).ToArray())
                        : Value.ForString(header.Value)
                );
            }

            requestStruct.Fields.Add("header", Value.ForStruct(requestHeaderStruct));
            jsonStruct.Fields.Add("request", Value.ForStruct(requestStruct));

            return jsonStruct;
        }
    }
}
