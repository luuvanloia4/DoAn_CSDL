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
    /// Summary description for MatHang_wsv
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MatHang_wsv : System.Web.Services.WebService
    {
        private MatHang_ctrl ctrl = new MatHang_ctrl();

        [WebMethod]
        public API_Result<bool> IsEmpty()
        {
            return ctrl.IsEmpty();
        }

        [WebMethod]
        public API_Result<int> Create(string loginCode, tbl_MatHang obj, int[] listLMH_ID)
        {
            return ctrl.Create(loginCode, obj, listLMH_ID);
        }

        [WebMethod]
        public API_Result<bool> Edit(string loginCode, tbl_MatHang obj, int[] listLMH_ID)
        {
            return ctrl.Edit(loginCode, obj, listLMH_ID);
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
        public API_Result<List<view_MatHang>> SearchPaging(string loginCode, int nccID, DateTime startTime, DateTime endTime, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
        {
            return ctrl.SearchPaging(loginCode, nccID, startTime, endTime, searchValue, searchType, curPage, pageSize, orderBy, isDescending);
        }

        [WebMethod]
        public API_Result<List<view_MatHang>> SearchPagingHD(string loginCode, int hdID, DateTime startTime, DateTime endTime, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
        {
            return ctrl.SearchPagingHD(loginCode, hdID, startTime, endTime, searchValue, searchType, curPage, pageSize, orderBy, isDescending);
        }

        [WebMethod]
        public API_Result<List<view_MH_HT>> SearchPagingHT(string loginCode, int htID, DateTime startTime, DateTime endTime, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
        {
            return ctrl.SearchPagingHT(loginCode, htID, startTime, endTime, searchValue, searchType, curPage, pageSize, orderBy, isDescending);
        }

        [WebMethod]
        public API_Result<List<ListCombobox_ett<int>>> GetListCombobox(int nccID)
        {
            return ctrl.GetListCombobox(nccID);
        }

        [WebMethod]
        public API_Result<List<ListCombobox_ett<int>>> GetListLMH(int matHangID)
        {
            return ctrl.GetListLMH(matHangID);
        }

        [WebMethod]
        public API_Result<view_MatHang> GetByID(int id)
        {
            return ctrl.GetByID(id);
        }
    }
}
