namespace AmbientContext.Shared.DotNetStandardLib.Models
{
    public class ResearchStudy
    {
        public long ID { get;  }
        public string GUID { get; set; }
        public string Name { get; set; }

        public ResearchStudy(long id)
        {
            ID = id;
        }
    }
}
