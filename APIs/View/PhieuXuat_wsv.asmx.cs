using APIs.Ctrls;
using APIs.Models;
using APIs.Models.Entity;
using APIs.Models.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace APIs.View
{
    /// <summary>
    /// Summary description for PhieuXuat_wsv
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PhieuXuat_wsv : System.Web.Services.WebService
    {
        PhieuXuat_ctrl ctrl = new PhieuXuat_ctrl();

        [WebMethod]
        public API_Result<bool> IsEmpty()
        {
            return ctrl.IsEmpty();
        }

        [WebMethod]
        public API_Result<bool> DeleteChiTietPX(string loginCode, int pxID, int mhID)
        {
            return ctrl.DeleteChiTietPX(loginCode, pxID, mhID);
        }

        [WebMethod]
        public API_Result<int> Create(string loginCode, tbl_PhieuXuat obj, List<ID_SL> listCTPX)
        {
            return ctrl.Create(loginCode, obj, listCTPX);
        }

        [WebMethod]
        public API_Result<bool> Edit(string loginCode, tbl_PhieuXuat obj, List<ID_SL> listCTPX)
        {
            return ctrl.Edit(loginCode, obj, listCTPX);
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
        public API_Result<List<view_PhieuXuat>> SearchPaging(string loginCode, int htID, int chID, int trangThai, DateTime startTime, DateTime endTime, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
        {
            return ctrl.SearchPaging(loginCode, htID, chID, trangThai, startTime, endTime, searchValue, searchType, curPage, pageSize, orderBy, isDescending);
        }

        [WebMethod]
        public API_Result<List<ListCombobox_ett<int>>> GetListCombobox()
        {
            return ctrl.GetListCombobox();
        }

        [WebMethod]
        public API_Result<view_PhieuXuat> GetByID(int id)
        {
            return ctrl.GetByID(id);
        }

        [WebMethod]
        public API_Result<List<view_ChiTietPX>> GetListCTPX(string loginCode, int pxID)
        {
            return ctrl.GetListCTPX(loginCode, pxID);
        }

        [WebMethod]
        public API_Result<bool> PheDuyet(string loginCode, int pxID)
        {
            return ctrl.PheDuyet(loginCode, pxID);
        }

        [WebMethod]
        public API_Result<bool> HoanThanh(string loginCode, int pxID)
        {
            return ctrl.HoanThanh(loginCode, pxID);
        }
    }
}
