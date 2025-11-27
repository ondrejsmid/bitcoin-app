using System;
using System.Web.UI;

namespace BitcoinCourseUI
{
    public partial class Stored : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // data loading will be implemented later; for now page is reachable
            }
        }
    }
}