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
        public API_Result<int> Create(string loginCode, tbl_HopDong obj)
        {
            return ctrl.Create(loginCode, obj);
        }

        [WebMethod]
        public API_Result<bool> AddChiTietHD(string loginCode, int hdID, ID_SL cthd)
        {
            return ctrl.AddChiTietHD(loginCode, hdID, cthd);
        }

        [WebMethod]
        public API_Result<string> UpdateChiTietHD(string loginCode, int hdID, List<ID_SL> listCTHD)
        {
            return ctrl.UpdateChiTietHD(loginCode, hdID, listCTHD);
        }

        [WebMethod]
        public API_Result<bool> DeleteChiTietHD(string loginCode, int hdID, int mhID)
        {
            return ctrl.DeleteChiTietHD(loginCode, hdID, mhID);
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
        public API_Result<List<view_HopDong>> SearchPaging(string loginCode, int htID, int nccID, int trangThai, DateTime startTime, DateTime endTime, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
        {
            return ctrl.SearchPaging(loginCode, htID, nccID, trangThai, startTime, endTime, searchValue, searchType, curPage, pageSize, orderBy, isDescending);
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

        [WebMethod]
        public API_Result<float> GetProgress(int id)
        {
            return ctrl.GetProgress(id);
        }

        [WebMethod]
        public API_Result<List<view_ChiTietHD>> GetListCTHD(string loginCode, int hdID)
        {
            return ctrl.GetListCTHD(loginCode, hdID);
        }

        //Phieu nhap
        [WebMethod]
        public API_Result<int> CreatePhieuNhap(string loginCode, tbl_PhieuNhap obj, List<ID_SL> listCTPN)
        {
            return ctrl.CreatePhieuNhap(loginCode, obj, listCTPN);
        }

        [WebMethod]
        public API_Result<bool> DeletePhieuNhap(string loginCode, int pnID)
        {
            return ctrl.DeletePhieuNhap(loginCode, pnID);
        }

        [WebMethod]
        public API_Result<List<view_PhieuNhap>> GetListPhieuNhap(int hdID)
        {
            return ctrl.GetListPhieuNhap(hdID);
        }

        [WebMethod]
        public API_Result<view_PhieuNhap> GetPhieuNhap(int pnID)
        {
            return ctrl.GetPhieuNhap(pnID);
        }

        [WebMethod]
        public API_Result<List<view_ChiTietPN>> GetListCTPN(int pnID)
        {
            return ctrl.GetListCTPN(pnID);
        }
    }
}
