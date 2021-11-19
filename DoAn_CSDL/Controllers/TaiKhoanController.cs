using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn_CSDL.Shared;
using Newtonsoft.Json;

namespace DoAn_CSDL.Controllers
{
    public class TaiKhoanController : Controller
    {

        public bool IsLogin()
        {
            TaiKhoan_wsv.TaiKhoan_wsv wsv = new TaiKhoan_wsv.TaiKhoan_wsv();
            if (Session[Constants.LoginCode_SessionName] != null)
            {
                var rs = wsv.GetCurrentUser(Session[Constants.LoginCode_SessionName].ToString());
                if (rs.ErrCode == TaiKhoan_wsv.EnumErrCode.Success)
                {
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
            int phanQuyen = SharedFunction.ParseInt(Request["phanQuyen"]);
            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();
            SharedFunction.ParseDualTime(Request["startTime"], Request["endTime"], ref startTime, ref endTime);

            //Cache to current session
            string alias = "taikhoan";
            Session[alias + "_Size"] = size;
            Session[alias + "_Page"] = page;
            Session[alias + "_SearchType"] = searchType;
            Session[alias + "_SearchValue"] = searchValue;
            Session[alias + "_OrderBy"] = orderBy;
            Session[alias + "_IsDes"] = orderDes;

            //Call API
            TaiKhoan_wsv.TaiKhoan_wsv tk_wsv = new TaiKhoan_wsv.TaiKhoan_wsv();
            var rs = tk_wsv.SearchPaging(Session[Constants.LoginCode_SessionName].ToString(), startTime, endTime, phanQuyen, searchValue, (TaiKhoan_wsv.EnumSearchType)searchType, page, size, (TaiKhoan_wsv.EnumOrderBy)orderBy, orderDes);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string GetDetail()
        {
            int id = SharedFunction.ParseID(Request["id"]);
            TaiKhoan_wsv.TaiKhoan_wsv tk_wsv = new TaiKhoan_wsv.TaiKhoan_wsv();
            var rs = tk_wsv.GetByID(Session[Constants.LoginCode_SessionName].ToString(), id);

            return JsonConvert.SerializeObject(rs);
        }

        //
        public ActionResult Edit()
        {
            if (!IsLogin())
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }
    }
}