namespace Medrio.CspReport
{
    public interface ICspReportPusher
    {
        Task Push(IList<string> violations);
    }
}
