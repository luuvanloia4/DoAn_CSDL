using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn_CSDL.Shared;
using Newtonsoft.Json;

namespace DoAn_CSDL.Controllers
{
    public class CuaHangController : Controller
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

            int htID = SharedFunction.ParseID(Request["htID"]);
            if(htID <= 0)
            {
                htID = SharedFunction.ParseID(Session[Constants.HeThongID_SessionName].ToString());
            }

            //Cache to current session
            string alias = "cuahang";
            Session[alias + "_Size"] = size;
            Session[alias + "_Page"] = page;
            Session[alias + "_SearchType"] = searchType;
            Session[alias + "_SearchValue"] = searchValue;
            Session[alias + "_OrderBy"] = orderBy;
            Session[alias + "_IsDes"] = orderDes;

            //Call API
            CuaHang_wsv.CuaHang_wsv ch_wsv = new CuaHang_wsv.CuaHang_wsv();
            var rs = ch_wsv.SearchPaging(Session[Constants.LoginCode_SessionName].ToString(), htID, startTime, endTime, searchValue, (CuaHang_wsv.EnumSearchType)searchType, page, size, (CuaHang_wsv.EnumOrderBy)orderBy, orderDes);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string Delete()
        {
            int id = SharedFunction.ParseID(Request["id"]);

            CuaHang_wsv.CuaHang_wsv ch_wsv = new CuaHang_wsv.CuaHang_wsv();
            var rs = ch_wsv.Delete(Session[Constants.LoginCode_SessionName].ToString(), id);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string DeleteAll()
        {
            List<int> listID = JsonConvert.DeserializeObject<List<int>>(Request["ListID"]);

            CuaHang_wsv.CuaHang_wsv ch_wsv = new CuaHang_wsv.CuaHang_wsv();
            var rs = ch_wsv.DeleteList(Session[Constants.LoginCode_SessionName].ToString(), listID.ToArray());

            return JsonConvert.SerializeObject(rs);
        }

        //
        public ActionResult Create()
        {
            if (!IsLogin())
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        public string PostCreate()
        {
            //Get param
            string ten = Request["txt_Ten"];
            string diaChi = Request["txt_DiaChi"];
            string SDT = Request["txt_SDT"];
            string nganHang = Request["txt_NganHang"];
            string STK = Request["txt_STK"];

            int daiDienID = SharedFunction.ParseID(Request["sel_DaiDien"]);
            int htID = SharedFunction.ParseID(Request["sel_HeThong"]);
            if(htID <= 0)
            {
                htID = SharedFunction.ParseID(Session[Constants.HeThongID_SessionName].ToString());
            }

            //string img = Constants.DefaultAvatar;
            //string fileName = Request["txt_ImgName"];
            //HttpPostedFileBase imgFile = SharedFunction.GetFileByName(fileName, Request.Files, SharedFunction.ListImageAcceptType);
            //if (imgFile != null)
            //{
            //    string fileType = Path.GetExtension(imgFile.FileName);
            //    string newFileName = imgFile.FileName.Split('.')[0] + Guid.NewGuid().ToString() + fileType;
            //    string filePath = Constants.UserImageUploadPath;
            //    string fullPath = Server.MapPath(filePath);
            //    if (!Directory.Exists(fullPath))
            //    {
            //        Directory.CreateDirectory(fullPath);
            //    }
            //    imgFile.SaveAs(fullPath + newFileName);
            //    img = Request.Url.GetLeftPart(UriPartial.Authority) + filePath + newFileName;
            //}

            //Create obj
            CuaHang_wsv.tbl_CuaHang obj = new CuaHang_wsv.tbl_CuaHang();
            obj.Ten = ten;
            obj.DiaChi = diaChi;
            obj.SDT = SDT;
            obj.NganHang = nganHang;
            obj.STK = STK;
            obj.TaiKhoanID = daiDienID;
            obj.HeThongID = htID;

            //Call API
            CuaHang_wsv.CuaHang_wsv ch_wsv = new CuaHang_wsv.CuaHang_wsv();
            var rs = ch_wsv.Create(Session[Constants.LoginCode_SessionName].ToString(), obj);

            return JsonConvert.SerializeObject(rs);
        }

        //
        public ActionResult Detail()
        {
            if (!IsLogin())
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        [HttpPost]
        public string GetDetail()
        {
            int id;
            if (!string.IsNullOrEmpty(Request["id"]))
            {
                id = SharedFunction.ParseID(Request["id"]);
            }
            else
            {
                id = SharedFunction.ParseID((Session[Constants.CuaHangID_SessionName] == null)? "": Session[Constants.CuaHangID_SessionName].ToString());
            }

            CuaHang_wsv.CuaHang_wsv ch_wsv = new CuaHang_wsv.CuaHang_wsv();
            var rs = ch_wsv.GetByID(id);

            return JsonConvert.SerializeObject(rs);
        }


        public ActionResult Edit()
        {
            if (!IsLogin())
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        [HttpPost]
        public string PostEdit()
        {
            //Get param
            int id = SharedFunction.ParseID(Request["txt_ID"]);
            string ten = Request["txt_Ten"];
            string diaChi = Request["txt_DiaChi"];
            string SDT = Request["txt_SDT"];
            string nganHang = Request["txt_NganHang"];
            string STK = Request["txt_STK"];

            int daiDienID = SharedFunction.ParseID(Request["sel_DaiDien"]);

            //string img = Constants.DefaultAvatar;
            //string fileName = Request["txt_ImgName"];
            //HttpPostedFileBase imgFile = SharedFunction.GetFileByName(fileName, Request.Files, SharedFunction.ListImageAcceptType);
            //if (imgFile != null)
            //{
            //    string fileType = Path.GetExtension(imgFile.FileName);
            //    string newFileName = imgFile.FileName.Split('.')[0] + Guid.NewGuid().ToString() + fileType;
            //    string filePath = Constants.UserImageUploadPath;
            //    string fullPath = Server.MapPath(filePath);
            //    if (!Directory.Exists(fullPath))
            //    {
            //        Directory.CreateDirectory(fullPath);
            //    }
            //    imgFile.SaveAs(fullPath + newFileName);
            //    img = Request.Url.GetLeftPart(UriPartial.Authority) + filePath + newFileName;
            //}

            //Create obj
            CuaHang_wsv.tbl_CuaHang obj = new CuaHang_wsv.tbl_CuaHang();
            obj.ID = id;
            obj.Ten = ten;
            obj.DiaChi = diaChi;
            obj.SDT = SDT;
            obj.NganHang = nganHang;
            obj.STK = STK;
            obj.TaiKhoanID = daiDienID;

            //Call API
            CuaHang_wsv.CuaHang_wsv ch_wsv = new CuaHang_wsv.CuaHang_wsv();
            var rs = ch_wsv.Edit(Session[Constants.LoginCode_SessionName].ToString(), obj);

            return JsonConvert.SerializeObject(rs);
        }

        //Extra method:
        [HttpPost]
        public string GetListCombobox()
        {

            int htID = SharedFunction.ParseID(Request["htID"]);
            if(htID < 0)
            {
                htID = SharedFunction.ParseID(Session[Constants.HeThongID_SessionName].ToString());
            }

            CuaHang_wsv.CuaHang_wsv ch_wsv = new CuaHang_wsv.CuaHang_wsv();
            var rs = ch_wsv.GetListCombobox(htID);

            return JsonConvert.SerializeObject(rs);
        }
    }
}