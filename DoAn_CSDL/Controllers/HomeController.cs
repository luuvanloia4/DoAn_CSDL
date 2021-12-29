using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn_CSDL.Shared;
using Newtonsoft.Json;

namespace DoAn_CSDL.Controllers
{
    public class HomeController : Controller
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

        public ActionResult Login()
        {
            //Logout => Clear session
            Session.Clear();

            return View();
        }

        [HttpPost]
        public string Login(string txt_UserName, string txt_Password)
        {
            TaiKhoan_wsv.TaiKhoan_wsv wsv = new TaiKhoan_wsv.TaiKhoan_wsv();
            var rs = wsv.Login(txt_UserName, txt_Password);

            if(rs.ErrCode == TaiKhoan_wsv.EnumErrCode.Success)
            {
                Session[Constants.LoginCode_SessionName] = rs.Data;
                Session[Constants.HeThongID_SessionName] = "";
                Session[Constants.NCCID_SessionName] = "";
                Session[Constants.CuaHangID_SessionName] = "";
                var groupID = wsv.GetGroupID(rs.Data);
                if(groupID.ErrCode == TaiKhoan_wsv.EnumErrCode.Success)
                {
                    Session[Constants.HeThongID_SessionName] = groupID.Data.HeThongID;
                    Session[Constants.CuaHangID_SessionName] = groupID.Data.CuaHangID;
                    Session[Constants.NCCID_SessionName] = groupID.Data.NhaCungCapID;
                }
                Session.Timeout = 60;
            }

            return JsonConvert.SerializeObject(rs);
        }

        public ActionResult Index()
        {
            if (!IsLogin())
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        public string GetDashBoardData()
        {
            //Chưa code bộ lọc nên để ngày tháng null
            string txt_StartTime = Request["txt_StartTime"];
            string txt_EndTime = Request["txt_EndTime"];
            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();
            SharedFunction.ParseDualTime(txt_StartTime, txt_EndTime, ref startTime, ref endTime);

            TaiKhoan_wsv.TaiKhoan_wsv tk_wsv = new TaiKhoan_wsv.TaiKhoan_wsv();
            var rs = tk_wsv.GetDashBoardData(Session[Constants.LoginCode_SessionName].ToString(), startTime, endTime);

            return JsonConvert.SerializeObject(rs);
        }

        #region Message control
        ////Message
        //[HttpPost]
        //public string GetListMessage()
        //{
        //    Notify_wsv.Notify_wsv ntf_wsv = new Notify_wsv.Notify_wsv();
        //    var rs = ntf_wsv.GetList(Authentication.loginCode);
        //    return JsonConvert.SerializeObject(rs);
        //}

        //[HttpPost]
        //public string GetSmallListMessage()
        //{
        //    Notify_wsv.Notify_wsv ntf_wsv = new Notify_wsv.Notify_wsv();
        //    var rs = ntf_wsv.GetStatus(Authentication.loginCode);
        //    return JsonConvert.SerializeObject(rs);
        //}

        //[HttpPost]
        //public string GetDetailMessage(string id)
        //{
        //    Notify_wsv.Notify_wsv ntf_wsv = new Notify_wsv.Notify_wsv();
        //    var rs = ntf_wsv.Get(Authentication.loginCode, id);

        //    return JsonConvert.SerializeObject(rs);
        //}

        //public ActionResult Message()
        //{
        //    if (!Authentication.CheckLogin())
        //    {
        //        return RedirectToAction("Login");
        //    }


        //    return View();
        //}

        //[HttpPost]
        //public string GetAllMessage(int size, int curCount)
        //{
        //    Notify_wsv.Notify_wsv ntf_wsv = new Notify_wsv.Notify_wsv();

        //    return JsonConvert.SerializeObject(ntf_wsv.LoadMore(Authentication.loginCode, size, curCount));
        //}

        //[HttpPost]
        //public string DeleteMessage(int curID)
        //{
        //    Notify_wsv.Notify_wsv ntf_wsv = new Notify_wsv.Notify_wsv();

        //    return JsonConvert.SerializeObject(ntf_wsv.Delete(Authentication.loginCode, new int[] { curID }));
        //}

        //[HttpPost]
        //public string DeleteAllMessage()
        //{
        //    Notify_wsv.Notify_wsv ntf_wsv = new Notify_wsv.Notify_wsv();

        //    return JsonConvert.SerializeObject(ntf_wsv.DeleteAll(Authentication.loginCode));
        //}

        //[HttpPost]
        //public string GetHeThongInfo()
        //{
        //    return JsonConvert.SerializeObject(ht_wsv.GetAllNewst(Authentication.loginCode));
        //}
        #endregion

        //Render partial
        [ChildActionOnly]
        public PartialViewResult RenderMenu()
        {
            Menu_wsv.Menu_wsv m_wsv = new Menu_wsv.Menu_wsv();
            List<Menu_wsv.view_Menu> listAllMenu = m_wsv.GetListMenu(Session[Constants.LoginCode_SessionName].ToString()).Data.ToList();

            return PartialView("~/Views/Shared/Partial/_Navbar.cshtml", listAllMenu);
        }

        [ChildActionOnly]
        public PartialViewResult RenderHeader()
        {
            TaiKhoan_wsv.TaiKhoan_wsv tk_wsv = new TaiKhoan_wsv.TaiKhoan_wsv();
            var curUser = tk_wsv.GetCurrentUser(Session[Constants.LoginCode_SessionName].ToString()).Data;

            return PartialView("~/Views/Shared/Partial/_Header.cshtml", curUser);
        }
    }
}