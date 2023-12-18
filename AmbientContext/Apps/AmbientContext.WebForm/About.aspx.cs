using System;
using System.Web.UI;
using AmbientContext.Shared.DotNetStandardLib;

namespace AmbientContextWebForm
{
    public partial class About : Page
    {
        public DataRecord dr = new DataRecord("Aby", 30);

        protected void Page_Load(object sender, EventArgs e)
        {


        }
    }
}