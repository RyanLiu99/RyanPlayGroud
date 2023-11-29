using System;
using System.Web.UI;
using AmbientContextDotNetFrameworkWebLib;

namespace AmbientContextWebForm
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TestHelper.Verify(this.Context);
        }
    }
}