
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#if NETFRAMEWORK
using HttpContext = System.Web.HttpContext;
#else
using HttpContext = Microsoft.AspNetCore.Http.HttpContext;
#endif

namespace Medrio.ActivityAuditLog
{
    public abstract class LogRequestBuilder<TPayload> : ILogRequestBuilder<TPayload>
    {
        public const string URL = "URL";
        public const string HttpMethod = "Method";
        public const string Header = "header";
        public const string Body = "body";
        public const string Request = "request";
        public static readonly Regex PasswordRegex = new Regex("[\"$&{][^$&\",]*(password|pwd)(.{1,3})?\"?(=|:)(?<replace>[^&},]*)", RegexOptions.IgnoreCase);
        public Task<LogRequest<TPayload>> BuildLogRequest(HttpContext httpContext)
        {
            var request = new LogRequest<TPayload>()
            {
                CustomerId = "CId From http context",
                StudyId = "Sid From http context",
                PayLoad = CreatePayLoad(httpContext)
            };

            return Task.FromResult(request);
        }

        protected abstract TPayload CreatePayLoad(HttpContext context);

#if NETFRAMEWORK
        public string GetRequestBody(HttpContext context)
        {
            var request = context.Request;
                try
                {
                    Stream inputStream = null;

                    switch (request.ReadEntityBodyMode)
                    {
                        case System.Web.ReadEntityBodyMode.Buffered:
                            inputStream = request.GetBufferedInputStream();
                            break;
                        case System.Web.ReadEntityBodyMode.Bufferless:
                            inputStream = request.GetBufferlessInputStream();
                            break;
                        case System.Web.ReadEntityBodyMode.None:
                        case System.Web.ReadEntityBodyMode.Classic:
                            inputStream = request.InputStream;
                            break;
                    }

                    if (inputStream != null && inputStream.Length > 0 && inputStream.CanRead)
                    {
                        var bodyStream = new StreamReader(inputStream,
                            Encoding.Default, true,
                            (int)inputStream.Length,
                            true);
                        string readToEnd = bodyStream.ReadToEnd();
                        inputStream.Position = 0;

                        string requestBody = System.Web.HttpUtility.UrlDecode(readToEnd);

                        if (!string.IsNullOrEmpty(requestBody))
                        {
                           
                            const string groupName = "replace";

                            if (PasswordRegex.GetGroupNames().Contains(groupName))
                            {
                                Match[] matches = PasswordRegex.Matches(requestBody).Cast<Match>().ToArray();

                                foreach (Match match in matches)
                                {
                                    string oldValue = match.Groups[groupName].Value;
                                    if (!string.IsNullOrEmpty(oldValue))
                                    {
                                        string newValue = match.Value.Replace(oldValue, "removed-by-Medrio");
                                        requestBody = requestBody.Replace(match.Value, newValue);
                                    }
                                }
                            }
                        }

                        return requestBody;
                    }
                }
                catch (Exception ex)
                {
                //ILog log = LogManager.GetLogger("Medrio.ActivityAudit.MedrioHttpHelper.RequestBody");
                //log.Error(ex.Message, ex);
                Console.WriteLine(ex);
                }
            

            return string.Empty;
        }
#else
        public string GetRequestBody(HttpContext context)
        {
            var request = context.Request;
            try
            {
                Stream inputStream = request.Body;


                if (inputStream != null && inputStream.Length > 0 && inputStream.CanRead)
                {
                    var bodyStream = new StreamReader(inputStream,
                        Encoding.Default, true,
                        (int)inputStream.Length,
                        true);
                    string readToEnd = bodyStream.ReadToEnd();
                    inputStream.Position = 0;

                    string requestBody = System.Web.HttpUtility.UrlDecode(readToEnd);

                    if (!string.IsNullOrEmpty(requestBody))
                    {

                        const string groupName = "replace";

                        if (PasswordRegex.GetGroupNames().Contains(groupName))
                        {
                            Match[] matches = PasswordRegex.Matches(requestBody).Cast<Match>().ToArray();

                            foreach (Match match in matches)
                            {
                                string oldValue = match.Groups[groupName].Value;
                                if (!string.IsNullOrEmpty(oldValue))
                                {
                                    string newValue = match.Value.Replace(oldValue, "removed-by-Medrio");
                                    requestBody = requestBody.Replace(match.Value, newValue);
                                }
                            }
                        }
                    }

                    return requestBody;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


            return string.Empty;
        }
#endif
    }
}
