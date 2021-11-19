using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using APIs.Ctrls;
using APIs.Models;

namespace APIs.View
{
    /// <summary>
    /// Summary description for Menu_wsv
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Menu_wsv : System.Web.Services.WebService
    {
        private Menu_ctrl ctrl = new Menu_ctrl();

        [WebMethod]
        public API_Result<List<view_Menu>> GetListMenu(string loginCode)
        {
            return ctrl.GetListMenu(loginCode);
        }
    }
}
