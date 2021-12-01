using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using APIs.Ctrls;
using APIs.Models;
using APIs.Models.Entity;

namespace APIs.View
{
    /// <summary>
    /// Summary description for PhanQuyen_wsv
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PhanQuyen_wsv : System.Web.Services.WebService
    {
        private PhanQuyen_ctrl ctrl = new PhanQuyen_ctrl();

        [WebMethod]
        public API_Result<bool> IsEmpty()
        {
            return ctrl.IsEmpty();
        }

        [WebMethod]
        public API_Result<bool> Create(string loginCode, tbl_PhanQuyen obj)
        {
            return ctrl.Create(loginCode, obj);
        }

        [WebMethod]
        public API_Result<bool> Edit(string loginCode, tbl_PhanQuyen obj)
        {
            return ctrl.Edit(loginCode, obj);
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
        public API_Result<List<ListCombobox_ett<int>>> GetListCombobox(string loginCode)
        {
            return ctrl.GetListCombobox(loginCode);
        }

        [WebMethod]
        public API_Result<List<view_PhanQuyen>> SearchPaging(string loginCode, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
        {
            return ctrl.SearchPaging(loginCode, searchValue, searchType, curPage, pageSize, orderBy, isDescending);
        }

        [WebMethod]
        public API_Result<view_PhanQuyen> GetByID(string loginCode, int id)
        {
            return ctrl.GetByID(loginCode, id);
        }
    }
}
