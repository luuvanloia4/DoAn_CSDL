using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using APIs.Ctrls;
using APIs.Models;
using APIs.Models.Entity;
using APIs.Models.Form;

namespace APIs.View
{
    /// <summary>
    /// Summary description for LoaiMatHang_wsv
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class LoaiMatHang_wsv : System.Web.Services.WebService
    {
        private LoaiMatHang_ctrl ctrl = new LoaiMatHang_ctrl();

        [WebMethod]
        public API_Result<bool> IsEmpty()
        {
            return ctrl.IsEmpty();
        }

        [WebMethod]
        public API_Result<List<ListCombobox_ett<int>>> GetListCombobox()
        {
            return ctrl.GetListCombobox();
        }
    }
}
