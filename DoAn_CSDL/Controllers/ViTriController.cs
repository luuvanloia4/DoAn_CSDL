using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn_CSDL.Shared;
using Newtonsoft.Json;

namespace DoAn_CSDL.Controllers
{
    public class ViTriController : Controller
    {
        public bool IsLogin()
        {
            TaiKhoan_wsv.TaiKhoan_wsv wsv = new TaiKhoan_wsv.TaiKhoan_wsv();
            if (Session[Constants.LoginCode_SessionName] != null)
            {
                var rs = wsv.GetCurrentUser(Session[Constants.LoginCode_SessionName].ToString());
                if (rs.ErrCode == TaiKhoan_wsv.EnumErrCode.Success)
                {
                    Session[Constants.UserRole_SessionName] = rs.Data.PhanQuyenID;
                    return true;
                }
            }
            return false;
        }

        // Index
        public ActionResult Index()
        {
            if (!IsLogin())
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        [HttpPost]
        public string SearchPaging()
        {
            //Get param
            string searchValue = Request["searchValue"];
            int searchType = SharedFunction.ParseInt(Request["searchType"]);
            int orderBy = SharedFunction.ParseInt(Request["orderBy"]);
            bool orderDes = SharedFunction.ParseBool(Request["orderDes"]);
            int size = SharedFunction.ParseInt(Request["size"]);
            int page = SharedFunction.ParseInt(Request["page"]);
            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();
            SharedFunction.ParseDualTime(Request["startTime"], Request["endTime"], ref startTime, ref endTime);

            int khoID = SharedFunction.ParseID(Request["khoID"]);

            //Cache to current session
            string alias = "vitri";
            Session[alias + "_Size"] = size;
            Session[alias + "_Page"] = page;
            Session[alias + "_SearchType"] = searchType;
            Session[alias + "_SearchValue"] = searchValue;
            Session[alias + "_OrderBy"] = orderBy;
            Session[alias + "_IsDes"] = orderDes;

            //Call API
            ViTri_wsv.ViTri_wsv vitri_wsv = new ViTri_wsv.ViTri_wsv();
            var rs = vitri_wsv.SearchPaging(khoID, startTime, endTime, searchValue, (ViTri_wsv.EnumSearchType)searchType, page, size, (ViTri_wsv.EnumOrderBy)orderBy, orderDes);

            return JsonConvert.SerializeObject(rs);
        }
    }
}