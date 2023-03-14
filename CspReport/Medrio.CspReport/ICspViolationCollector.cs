namespace Medrio.CspReport
{
    public interface ICspViolationCollector
    {
        Task Collect(Stream violation);
        IList<string>? BlockToHandOverData(int millisecondsTimeout = 5000);

        IList<string> WindDown();
    }
}
