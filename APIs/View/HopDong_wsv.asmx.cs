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
    /// Summary description for HopDong_wsv
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class HopDong_wsv : System.Web.Services.WebService
    {
        private HopDong_ctrl ctrl = new HopDong_ctrl();

        [WebMethod]
        public API_Result<bool> IsEmpty()
        {
            return ctrl.IsEmpty();
        }

        [WebMethod]
        public API_Result<int> Create(string loginCode, tbl_HopDong obj, List<ID_SL> listCTHD)
        {
            return ctrl.Create(loginCode, obj, listCTHD);
        }

        [WebMethod]
        public API_Result<bool> Edit(string loginCode, tbl_HopDong obj, List<ID_SL> listCTHD)
        {
            return ctrl.Edit(loginCode, obj, listCTHD);
        }

        [WebMethod]
        public API_Result<bool> Delete(string loginCode, int id)
        {
            return ctrl.Delete(loginCode, id);
        }

        [WebMethod]
        public API_Result<string> DeleteList(string loginCode, int[] listID)
        {
            return ctrl.DeleteList(loginCode, listID);
        }

        [WebMethod]
        public API_Result<List<view_HopDong>> SearchPaging(string loginCode, DateTime startTime, DateTime endTime, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
        {
            return ctrl.SearchPaging(loginCode, startTime, endTime, searchValue, searchType, curPage, pageSize, orderBy, isDescending);
        }

        [WebMethod]
        public API_Result<List<ListCombobox_ett<int>>> GetListCombobox()
        {
            return ctrl.GetListCombobox();
        }

        [WebMethod]
        public API_Result<view_HopDong> GetByID(int id)
        {
            return ctrl.GetByID(id);
        }
    }
}
