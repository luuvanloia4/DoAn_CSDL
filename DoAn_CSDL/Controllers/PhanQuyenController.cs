using DoAn_CSDL.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn_CSDL.Controllers
{
    public class PhanQuyenController : Controller
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
                    Session[Constants.UserID_SessionName] = rs.Data.ID;
                    return true;
                }
            }
            return false;
        }

        // GET: PhanQuyen
        public ActionResult Index()
        {
            if (!IsLogin())
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        // ...

        [HttpPost]
        public string GetListCombobox()
        {
            PhanQuyen_wsv.PhanQuyen_wsv pq_wsv = new PhanQuyen_wsv.PhanQuyen_wsv();
            var rs = pq_wsv.GetListCombobox(Session[Constants.LoginCode_SessionName].ToString());

            return JsonConvert.SerializeObject(rs);
        }
    }
}