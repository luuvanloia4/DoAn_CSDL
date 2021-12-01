using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using APIs.Models;
using APIs.Models.Entity;
using APIs.Ctrls;

namespace APIs.View
{
    /// <summary>
    /// Summary description for TaiKhoan_wsv
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TaiKhoan_wsv : System.Web.Services.WebService
    {
        private TaiKhoan_ctrl ctrl = new TaiKhoan_ctrl();

        [WebMethod]
        public API_Result<string> Login(string userName, string password)
        {
            return ctrl.Login(userName, password);
        }

        [WebMethod]
        public API_Result<bool> Create(string loginCode, int heThongID, tbl_TaiKhoan obj)
        {
            return ctrl.Create(loginCode, heThongID, obj);
        }

        [WebMethod]
        public API_Result<bool> Edit(string loginCode, tbl_TaiKhoan obj)
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
        public API_Result<List<ListCombobox_ett<int>>> GetListComboboxID(string loginCode, int phanQuyenID = -1)
        {
            return ctrl.GetListComboboxID(loginCode, phanQuyenID);
        }

        [WebMethod]
        public API_Result<List<ListCombobox_ett<string>>> GetListComboboxName(string loginCode, int phanQuyenID = -1)
        {
            return ctrl.GetListComboboxName(loginCode, phanQuyenID);
        }

        [WebMethod]
        public API_Result<List<TaiKhoan_ett>> SearchPaging(string loginCode, int heThongID, DateTime startTime, DateTime endTime, int status = -1, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
        {
            return ctrl.SearchPaging(loginCode, heThongID, startTime, endTime, status, searchValue, searchType, curPage, pageSize, orderBy, isDescending);
        }

        [WebMethod]
        public API_Result<TaiKhoan_ett> GetCurrentUser(string loginCode)
        {
            return ctrl.GetCurrentUser(loginCode);
        }

        [WebMethod]
        public API_Result<TaiKhoan_ett> GetByName(string loginCode, string name)
        {
            return ctrl.GetByName(loginCode, name);
        }

        [WebMethod]
        public API_Result<TaiKhoan_ett> GetByID(string loginCode, int id)
        {
            return ctrl.GetByID(loginCode, id);
        }

        [WebMethod]
        public API_Result<string> PhanQuyen(string loginCode, int[] listUserID, int phanQuyenID)
        {
            return ctrl.PhanQuyen(loginCode, listUserID, phanQuyenID);
        }
    }
}
