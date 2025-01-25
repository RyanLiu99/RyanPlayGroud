
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CoreWebApp1
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Create a new pipe for this request
            var pipe = new Pipe();

            // Start logging the request asynchronously
            await LogRequestAsync(context.Request, pipe);

            // Continue processing the request
            await _next(context);            
        }

        private async Task LogRequestAsync(HttpRequest request, Pipe pipe)
        {
            // Capture the request details
            var requestInfo = $"[{DateTime.UtcNow}] {request.Method} {request.Path}{request.QueryString}\n";
            await WriteToPipeAsync(requestInfo, pipe);

            // Log the headers
            foreach (var header in request.Headers)
            {
                var headerInfo = $"{header.Key}: {header.Value}\n";
                await WriteToPipeAsync(headerInfo, pipe);
            }

            // Log the body if it's not empty
            if (request.ContentLength > 0)
            {
                await LogRequestBodyAsync(request.Body, pipe);
            }

            await pipe.Writer.CompleteAsync(); // Complete the pipe writer
        }

        private async Task LogRequestBodyAsync(Stream bodyStream, Pipe pipe)
        {
            while (true)
            {
                // Get memory from the pipe writer
                Memory<byte> memory = pipe.Writer.GetMemory();

                // Read data from the body stream into the pipe's memory
                int bytesRead = await bodyStream.ReadAsync(memory);
                if (bytesRead == 0)
                {
                    break;
                }

                // Tell the pipe writer how much data was written
                pipe.Writer.Advance(bytesRead);

                // Flush the data to the reader
                await pipe.Writer.FlushAsync();
            }
        }

        private async Task WriteToPipeAsync(string data, Pipe pipe)
        {
            var writer = pipe.Writer;
            var bytes = Encoding.UTF8.GetBytes(data);
            await writer.WriteAsync(bytes);
            await writer.FlushAsync();
        }
    }

    // Extension method to add the middleware to the pipeline
    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
