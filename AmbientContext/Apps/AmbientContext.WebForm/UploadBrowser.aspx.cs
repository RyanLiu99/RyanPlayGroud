using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

namespace AmbientContextWebForm
{
    public partial class UploadBrowser : System.Web.UI.Page
    {
        private const int BufferSize = 2048;
        public string FileContent;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (this.Request.HttpMethod.Equals("POST", StringComparison.CurrentCultureIgnoreCase)) //always redirect to get

            if (Request.Files.Count > 0)
            {

                HttpPostedFile file = Request.Files[0];


                using (var bodyStream = new StreamReader(file.InputStream,
                           Encoding.Default,
                           true,
                           BufferSize,
                           true))
                {


                    FileContent = bodyStream.ReadToEnd();
                    FileContent = FileContent.Substring(0, Math.Min(100, FileContent.Length));

                }
            }

        }

    }
}