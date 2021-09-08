using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LED.Web
{
    public partial class bigip : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string bigIP = "";
            string server = "";

            try
            {
                if (Request.QueryString["CODBIGIP"] != null)
                {
                    bigIP = Request.QueryString["CODBIGIP"].ToString();
                    server = Environment.MachineName.ToUpper();
                    lblServer.Text = "";
                    lblServer.Text = String.Concat("Nome do Servidor: ", server);
                    lblBigIp.Text = "";
                    lblBigIp.Text = String.Concat("BigIP: ", bigIP); 
                   
                }
            }
            catch (Exception ex)
            {
                lblBigIp.Text = ex.Message;
            }
        }
    }
}