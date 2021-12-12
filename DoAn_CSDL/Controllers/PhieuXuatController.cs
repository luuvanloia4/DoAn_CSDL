using DoAn_CSDL.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn_CSDL.Controllers
{
    public class PhieuXuatController : Controller
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
            int trangThai = SharedFunction.ParseID(Request["trangThai"]);

            int htID = SharedFunction.ParseID((Session[Constants.HeThongID_SessionName] == null ? "" : Session[Constants.HeThongID_SessionName].ToString()));
            int chID = SharedFunction.ParseID(Session[Constants.CuaHangID_SessionName] == null ? "" : Session[Constants.CuaHangID_SessionName].ToString());

            //Cache to current session
            string alias = "phieuxuat";
            Session[alias + "_Size"] = size;
            Session[alias + "_Page"] = page;
            Session[alias + "_SearchType"] = searchType;
            Session[alias + "_SearchValue"] = searchValue;
            Session[alias + "_OrderBy"] = orderBy;
            Session[alias + "_IsDes"] = orderDes;

            //Call API
            PhieuXuat_wsv.PhieuXuat_wsv px_wsv = new PhieuXuat_wsv.PhieuXuat_wsv();
            var rs = px_wsv.SearchPaging(Session[Constants.LoginCode_SessionName].ToString(), htID, chID, trangThai, startTime, endTime, searchValue, (PhieuXuat_wsv.EnumSearchType)searchType, page, size, (PhieuXuat_wsv.EnumOrderBy)orderBy, orderDes);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string Delete()
        {
            int id = SharedFunction.ParseID(Request["id"]);

            PhieuXuat_wsv.PhieuXuat_wsv px_wsv = new PhieuXuat_wsv.PhieuXuat_wsv();
            var rs = px_wsv.Delete(Session[Constants.LoginCode_SessionName].ToString(), id);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string DeleteAll()
        {
            List<int> listID = JsonConvert.DeserializeObject<List<int>>(Request["ListID"]);

            PhieuXuat_wsv.PhieuXuat_wsv px_wsv = new PhieuXuat_wsv.PhieuXuat_wsv();
            var rs = px_wsv.DeleteList(Session[Constants.LoginCode_SessionName].ToString(), listID.ToArray());

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
            int chID = SharedFunction.ParseID(Request["id"]);
            List<PhieuXuat_wsv.ID_SL> listCTPX = JsonConvert.DeserializeObject<List<PhieuXuat_wsv.ID_SL>>(Request["ListCTPX"]);

            //Create obj
            PhieuXuat_wsv.tbl_PhieuXuat obj = new PhieuXuat_wsv.tbl_PhieuXuat();
            obj.CuaHangID = chID;

            //Call API
            PhieuXuat_wsv.PhieuXuat_wsv px_wsv = new PhieuXuat_wsv.PhieuXuat_wsv();
            var rs = px_wsv.Create(Session[Constants.LoginCode_SessionName].ToString(), obj, listCTPX.ToArray());

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
            int id = SharedFunction.ParseID(Request["id"]);
            PhieuXuat_wsv.PhieuXuat_wsv px_wsv = new PhieuXuat_wsv.PhieuXuat_wsv();
            var rs = px_wsv.GetByID(id);

            return JsonConvert.SerializeObject(rs);
        }

        //CTHD
        [HttpPost]
        public string UpdateListCTPX()
        {
            int pxID = SharedFunction.ParseID(Request["pxID"]);
            List<PhieuXuat_wsv.ID_SL> listCTPX = JsonConvert.DeserializeObject<List<PhieuXuat_wsv.ID_SL>>(Request["ListCTPX"]);

            PhieuXuat_wsv.tbl_PhieuXuat obj = new PhieuXuat_wsv.tbl_PhieuXuat();
            obj.ID = pxID;

            PhieuXuat_wsv.PhieuXuat_wsv px_wsv = new PhieuXuat_wsv.PhieuXuat_wsv();
            var rs = px_wsv.Edit(Session[Constants.LoginCode_SessionName].ToString(), obj, listCTPX.ToArray());

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string DeleteCTPX()
        {
            int pxID = SharedFunction.ParseID(Request["pxID"]);
            int mhID = SharedFunction.ParseID(Request["mhID"]);

            PhieuXuat_wsv.PhieuXuat_wsv px_wsv = new PhieuXuat_wsv.PhieuXuat_wsv();
            var rs = px_wsv.DeleteChiTietPX(Session[Constants.LoginCode_SessionName].ToString(), pxID, mhID);

            return JsonConvert.SerializeObject(rs);
        }
        //End CTHD

        //public ActionResult Edit()
        //{
        //    if (!IsLogin())
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }

        //    return View();
        //}

        //[HttpPost]
        //public string PostEdit()
        //{
        //    //Get param
        //    int id = SharedFunction.ParseID(Request["txt_ID"]);
        //    string ten = Request["txt_Ten"];
        //    string diaChi = Request["txt_DiaChi"];
        //    string SDT = Request["txt_SDT"];
        //    string nganHang = Request["txt_NganHang"];
        //    string STK = Request["txt_STK"];

        //    int daiDienID = SharedFunction.ParseID(Request["sel_DaiDien"]);

        //    //string img = Constants.DefaultAvatar;
        //    //string fileName = Request["txt_ImgName"];
        //    //HttpPostedFileBase imgFile = SharedFunction.GetFileByName(fileName, Request.Files, SharedFunction.ListImageAcceptType);
        //    //if (imgFile != null)
        //    //{
        //    //    string fileType = Path.GetExtension(imgFile.FileName);
        //    //    string newFileName = imgFile.FileName.Split('.')[0] + Guid.NewGuid().ToString() + fileType;
        //    //    string filePath = Constants.UserImageUploadPath;
        //    //    string fullPath = Server.MapPath(filePath);
        //    //    if (!Directory.Exists(fullPath))
        //    //    {
        //    //        Directory.CreateDirectory(fullPath);
        //    //    }
        //    //    imgFile.SaveAs(fullPath + newFileName);
        //    //    img = Request.Url.GetLeftPart(UriPartial.Authority) + filePath + newFileName;
        //    //}

        //    //Create obj
        //    HeThong_wsv.tbl_HeThong obj = new HeThong_wsv.tbl_HeThong();
        //    obj.ID = id;
        //    obj.Ten = ten;
        //    obj.DiaChi = diaChi;
        //    obj.SDT = SDT;
        //    obj.NganHang = nganHang;
        //    obj.STK = STK;
        //    obj.TaiKhoanID = daiDienID;

        //    //Call API
        //    HeThong_wsv.HeThong_wsv ht_wsv = new HeThong_wsv.HeThong_wsv();
        //    var rs = ht_wsv.Edit(Session[Constants.LoginCode_SessionName].ToString(), obj);

        //    return JsonConvert.SerializeObject(rs);
        //}

        //Exten Method
        [HttpPost]
        public string PheDuyet()
        {
            int pxID = SharedFunction.ParseID(Request["id"]);
            PhieuXuat_wsv.PhieuXuat_wsv px_wsv = new PhieuXuat_wsv.PhieuXuat_wsv();
            var rs = px_wsv.PheDuyet(Session[Constants.LoginCode_SessionName].ToString(), pxID);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string HoanThanh()
        {
            int pxID = SharedFunction.ParseID(Request["id"]);
            PhieuXuat_wsv.PhieuXuat_wsv px_wsv = new PhieuXuat_wsv.PhieuXuat_wsv();
            var rs = px_wsv.HoanThanh(Session[Constants.LoginCode_SessionName].ToString(), pxID);

            return JsonConvert.SerializeObject(rs);
        }
    }
}