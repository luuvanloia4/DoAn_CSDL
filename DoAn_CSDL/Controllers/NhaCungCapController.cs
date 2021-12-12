using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn_CSDL.Shared;
using Newtonsoft.Json;

namespace DoAn_CSDL.Controllers
{
    public class NhaCungCapController : Controller
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

            //Cache to current session
            string alias = "ncc";
            Session[alias + "_Size"] = size;
            Session[alias + "_Page"] = page;
            Session[alias + "_SearchType"] = searchType;
            Session[alias + "_SearchValue"] = searchValue;
            Session[alias + "_OrderBy"] = orderBy;
            Session[alias + "_IsDes"] = orderDes;

            //Call API
            NhaCungCap_wsv.NhaCungCap_wsv ncc_wsv = new NhaCungCap_wsv.NhaCungCap_wsv();
            var rs = ncc_wsv.SearchPaging(Session[Constants.LoginCode_SessionName].ToString(), startTime, endTime, searchValue, (NhaCungCap_wsv.EnumSearchType)searchType, page, size, (NhaCungCap_wsv.EnumOrderBy)orderBy, orderDes);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string Delete()
        {
            int id = SharedFunction.ParseID(Request["id"]);

            NhaCungCap_wsv.NhaCungCap_wsv ncc_wsv = new NhaCungCap_wsv.NhaCungCap_wsv();
            var rs = ncc_wsv.Delete(Session[Constants.LoginCode_SessionName].ToString(), id);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string DeleteAll()
        {
            List<int> listID = JsonConvert.DeserializeObject<List<int>>(Request["ListID"]);

            NhaCungCap_wsv.NhaCungCap_wsv ncc_wsv = new NhaCungCap_wsv.NhaCungCap_wsv();
            var rs = ncc_wsv.DeleteList(Session[Constants.LoginCode_SessionName].ToString(), listID.ToArray());

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
            NhaCungCap_wsv.tbl_NhaCungCap obj = new NhaCungCap_wsv.tbl_NhaCungCap();
            obj.Ten = ten;
            obj.DiaChi = diaChi;
            obj.SDT = SDT;
            obj.NganHang = nganHang;
            obj.STK = STK;
            obj.TaiKhoanID = daiDienID;

            //Call API
            NhaCungCap_wsv.NhaCungCap_wsv ncc_wsv = new NhaCungCap_wsv.NhaCungCap_wsv();
            var rs = ncc_wsv.Create(Session[Constants.LoginCode_SessionName].ToString(), obj);

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
            if(Session[Constants.NCCID_SessionName] == null)
            {
                id = SharedFunction.ParseID(Request["id"]);
            }
            else
            {
                id = SharedFunction.ParseID((Session[Constants.NCCID_SessionName] == null)? "": Session[Constants.NCCID_SessionName].ToString());
            }

            NhaCungCap_wsv.NhaCungCap_wsv ncc_wsv = new NhaCungCap_wsv.NhaCungCap_wsv();
            var rs = ncc_wsv.GetByID(id);

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
            NhaCungCap_wsv.tbl_NhaCungCap obj = new NhaCungCap_wsv.tbl_NhaCungCap();
            obj.ID = id;
            obj.Ten = ten;
            obj.DiaChi = diaChi;
            obj.SDT = SDT;
            obj.NganHang = nganHang;
            obj.STK = STK;
            obj.TaiKhoanID = daiDienID;

            //Call API
            NhaCungCap_wsv.NhaCungCap_wsv ncc_wsv = new NhaCungCap_wsv.NhaCungCap_wsv();
            var rs = ncc_wsv.Edit(Session[Constants.LoginCode_SessionName].ToString(), obj);

            return JsonConvert.SerializeObject(rs);
        }

        //Extra method:
        [HttpPost]
        public string GetListCombobox()
        {
            NhaCungCap_wsv.NhaCungCap_wsv ncc_wsv = new NhaCungCap_wsv.NhaCungCap_wsv();
            var rs = ncc_wsv.GetListCombobox();

            return JsonConvert.SerializeObject(rs);
        }
    }
}