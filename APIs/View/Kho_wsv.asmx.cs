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
    /// Summary description for Kho_wsv
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Kho_wsv : System.Web.Services.WebService
    {
        private Kho_ctrl ctrl = new Kho_ctrl();

        [WebMethod]
        public API_Result<bool> IsEmpty()
        {
            return ctrl.IsEmpty();
        }

        [WebMethod]
        public API_Result<int> Create(string loginCode, tbl_Kho obj)
        {
            return ctrl.Create(loginCode, obj);
        }

        [WebMethod]
        public API_Result<bool> Edit(string loginCode, tbl_Kho obj)
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
        public API_Result<List<view_Kho>> SearchPaging(int heThongID, DateTime startTime, DateTime endTime, string searchValue, EnumSearchType searchType, int curPage, int pageSize, EnumOrderBy orderBy, bool isDes)
        {
            return ctrl.SearchPaging(heThongID, startTime, endTime, searchValue, searchType, curPage, pageSize, orderBy, isDes);
        }

        [WebMethod]
        public API_Result<List<ListCombobox_ett<int>>> GetListCombobox(int heThongID)
        {
            return ctrl.GetListCombobox(heThongID);
        }

        [WebMethod]
        public API_Result<view_Kho> GetByID(int id)
        {
            return ctrl.GetByID(id);
        }

        [WebMethod]
        public API_Result<List<view_Kho>> GetList(int heThongID, int index = 0, int size = -1)
        {
            return ctrl.GetList(heThongID, index, size);
        }
    }
}
