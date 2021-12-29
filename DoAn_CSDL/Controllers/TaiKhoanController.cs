using System;
using System.Collections.Generic;
using System.IO;
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
            int phanQuyen = SharedFunction.ParseInt(Request["phanQuyen"]);
            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();
            SharedFunction.ParseDualTime(Request["startTime"], Request["endTime"], ref startTime, ref endTime);
            int heThongID = SharedFunction.ParseID(Request["heThongID"]);

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
            var rs = tk_wsv.SearchPaging(Session[Constants.LoginCode_SessionName].ToString(), heThongID, startTime, endTime, phanQuyen, searchValue, (TaiKhoan_wsv.EnumSearchType)searchType, page, size, (TaiKhoan_wsv.EnumOrderBy)orderBy, orderDes);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string GetDetail()
        {
            int id = SharedFunction.ParseID(Request["id"]);
            if(id <= 0)
            {
                id = SharedFunction.ParseID(Session[Constants.UserID_SessionName].ToString());
            }

            TaiKhoan_wsv.TaiKhoan_wsv tk_wsv = new TaiKhoan_wsv.TaiKhoan_wsv();
            var rs = tk_wsv.GetByID(Session[Constants.LoginCode_SessionName].ToString(), id);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string Delete()
        {
            int id = SharedFunction.ParseID(Request["id"]);

            TaiKhoan_wsv.TaiKhoan_wsv tk_wsv = new TaiKhoan_wsv.TaiKhoan_wsv();
            var rs = tk_wsv.Delete(Session[Constants.LoginCode_SessionName].ToString(), id);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string DeleteAll()
        {
            List<int> listID = JsonConvert.DeserializeObject<List<int>>(Request["ListID"]);

            TaiKhoan_wsv.TaiKhoan_wsv tk_wsv = new TaiKhoan_wsv.TaiKhoan_wsv();
            var rs = tk_wsv.DeleteList(Session[Constants.LoginCode_SessionName].ToString(), listID.ToArray());

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
            string userName = Request["txt_UserName"];
            string password = Request["txt_Password"];

            string hoTen = Request["txt_HoTen"];
            DateTime ngaySinh = SharedFunction.ParseDate(Request["txt_NgaySinh"]);
            string diaChi = Request["txt_DiaChi"];
            string SDT = Request["txt_SDT"];

            int phanQuyenID = SharedFunction.ParseID(Request["sel_PhanQuyen"]);
            int heThongID = SharedFunction.ParseID(Request["sel_HeThong"]);

            string img = Constants.DefaultAvatar;
            string fileName = Request["txt_ImgName"];
            HttpPostedFileBase imgFile = SharedFunction.GetFileByName(fileName, Request.Files, SharedFunction.ListImageAcceptType);
            if (imgFile != null)
            {
                string fileType = Path.GetExtension(imgFile.FileName);
                string newFileName = imgFile.FileName.Split('.')[0] + Guid.NewGuid().ToString() + fileType;
                string filePath = Constants.UserImageUploadPath;
                string fullPath = Server.MapPath(filePath);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                imgFile.SaveAs(fullPath + newFileName);
                img = Request.Url.GetLeftPart(UriPartial.Authority) + filePath + newFileName;
            }

            //Create obj
            TaiKhoan_wsv.tbl_TaiKhoan obj = new TaiKhoan_wsv.tbl_TaiKhoan();
            obj.UserName = userName;
            obj.Pass = password;
            obj.Img = img;

            obj.HoTen = hoTen;
            obj.NgaySinh = ngaySinh;
            obj.DiaChi = diaChi;
            obj.SDT = SDT;
            obj.PhanQuyenID = phanQuyenID;

            //Call API
            TaiKhoan_wsv.TaiKhoan_wsv tk_wsv = new TaiKhoan_wsv.TaiKhoan_wsv();
            var rs = tk_wsv.Create(Session[Constants.LoginCode_SessionName].ToString(), heThongID, obj);

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

        [HttpPost]
        public string PostEdit()
        {
            //Get param
            int id = SharedFunction.ParseID(Request["txt_ID"]);
            string userName = Request["txt_UserName"];
            string password = Request["txt_Password"];

            string hoTen = Request["txt_HoTen"];
            DateTime ngaySinh = SharedFunction.ParseDate(Request["txt_NgaySinh"]);
            string diaChi = Request["txt_DiaChi"];
            string SDT = Request["txt_SDT"];

            int phanQuyenID = SharedFunction.ParseID(Request["sel_PhanQuyen"]);

            string img = string.Empty;
            string fileName = Request["txt_ImgName"];
            HttpPostedFileBase imgFile = SharedFunction.GetFileByName(fileName, Request.Files, SharedFunction.ListImageAcceptType);
            if(imgFile != null)
            {
                string fileType = Path.GetExtension(imgFile.FileName);
                string newFileName = imgFile.FileName.Split('.')[0] + Guid.NewGuid().ToString() + fileType;
                string filePath = Constants.UserImageUploadPath;
                string fullPath = Server.MapPath(filePath);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                imgFile.SaveAs(fullPath + newFileName);
                img = Request.Url.GetLeftPart(UriPartial.Authority) + filePath + newFileName;
            }

            //Create obj
            TaiKhoan_wsv.tbl_TaiKhoan obj = new TaiKhoan_wsv.tbl_TaiKhoan();
            obj.ID = id;
            obj.UserName = userName;
            obj.Pass = password;
            obj.Img = img;

            obj.HoTen = hoTen;
            obj.NgaySinh = ngaySinh;
            obj.DiaChi = diaChi;
            obj.SDT = SDT;
            obj.PhanQuyenID = phanQuyenID;

            //Call API
            TaiKhoan_wsv.TaiKhoan_wsv tk_wsv = new TaiKhoan_wsv.TaiKhoan_wsv();
            var rs = tk_wsv.Edit(Session[Constants.LoginCode_SessionName].ToString(), obj);

            return JsonConvert.SerializeObject(rs);
        }

        //Extra method
        [HttpPost]
        public string GetListCombobox()
        {
            string roleName = Request["role"];
            int roleID;
            switch (roleName)
            {
                case "quanly":
                    roleID = 1;
                    break;
                case "nhanvien":
                    roleID = 2;
                    break;
                case "cuahang":
                    roleID = 3;
                    break;
                case "nhacungcap":
                    roleID = 4;
                    break;
                default:
                    roleID = 404;
                    break;
            }

            int htID = SharedFunction.ParseID(Request["htID"]);
            if(htID <= 0)
            {
                htID = SharedFunction.ParseID(Session[Constants.HeThongID_SessionName].ToString());
            }

            TaiKhoan_wsv.TaiKhoan_wsv tk_wsv = new TaiKhoan_wsv.TaiKhoan_wsv();
            var rs = tk_wsv.GetListComboboxID(Session[Constants.LoginCode_SessionName].ToString(), roleID, htID);

            return JsonConvert.SerializeObject(rs);
        }
        
    }
}