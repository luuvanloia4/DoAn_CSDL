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
    /// Summary description for ViTri_wsv
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ViTri_wsv : System.Web.Services.WebService
    {
        private ViTri_ctrl ctrl = new ViTri_ctrl();

        [WebMethod]
        public API_Result<bool> IsEmpty()
        {
            return ctrl.IsEmpty();
        }

        [WebMethod]
        public API_Result<int> Create(string loginCode, tbl_ViTri obj)
        {
            return ctrl.Create(loginCode, obj);
        }

        [WebMethod]
        public API_Result<bool> Edit(string loginCode, tbl_ViTri obj)
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
        public API_Result<List<view_ViTri>> SearchPaging(int khoID, DateTime startTime, DateTime endTime, string searchValue, EnumSearchType searchType, int curPage, int pageSize, EnumOrderBy orderBy, bool isDes)
        {
            return ctrl.SearchPaging(khoID, startTime, endTime, searchValue, searchType, curPage, pageSize, orderBy, isDes);
        }

        [WebMethod]
        public API_Result<List<ListCombobox_ett<int>>> GetListCombobox(int khoID)
        {
            return ctrl.GetListCombobox(khoID);
        }

        [WebMethod]
        public API_Result<view_ViTri> GetByID(int id)
        {
            return ctrl.GetByID(id);
        }

        [WebMethod]
        public API_Result<view_ViTri> GetByLocation(int khoID, int hang, int cot)
        {
            return ctrl.GetByLocation(khoID, hang, cot);
        }

        [WebMethod]
        public API_Result<List<view_ViTri>> GetList(int khoID, int hangID = -1, int cotID = -1, int index = 0, int size = -1)
        {
            return ctrl.GetList(khoID, hangID, cotID, index, size);
        }
    }
}
