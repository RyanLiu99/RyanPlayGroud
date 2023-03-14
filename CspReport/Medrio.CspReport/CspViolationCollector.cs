using Medrio.BulkDataChannel;
using Medrio.Infrastructure.Ioc.Dependency;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Medrio.CspReport
{
    [RegisterAs(typeof(ICspViolationCollector), Lifetime = ServiceLifetime.Singleton)]
    internal class CspViolationCollector : ICspViolationCollector, IDisposable
    {
        private readonly BufferedBucketDataChannel<string> _dataChannel;
        public CspViolationCollector(IOptions<CspReportOption> option)
        {
            _dataChannel = new BufferedBucketDataChannel<string>(option.Value.BufferSize, TimeSpan.FromMilliseconds(option.Value.BufferTimeInMs));
        }

        public async Task Collect(Stream violation)
        {
            using var reader = new StreamReader(violation) ; //pass stream direct to ListValue.Parser cause error, let get string first
            var result = await reader.ReadToEndAsync();
            _dataChannel.TakeInData(result);
        }

        public IList<string>? BlockToHandOverData(int millisecondsTimeout = 5000)
        {
            return _dataChannel.BlockToHandOverData();
        }

        public IList<string> WindDown()
        {
            return _dataChannel.WindDown();
        }

        public void Dispose()
        {
            _dataChannel.Dispose();
        }
    }
}