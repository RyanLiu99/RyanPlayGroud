namespace AmbientContext.Shared.DotNetStandardLib.Models
{
    public class RequestData
    {
        public string UserName { get; set; }
        public long StudyId { get; set; }

        public RequestData(string userName, long studyId)
        {
                UserName = userName;
                StudyId = studyId;
        }
    }
}
