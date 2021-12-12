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
    public class MatHangController : Controller
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

            int nccID = SharedFunction.ParseID(Request["nccID"]);

            //Cache to current session
            string alias = "mathang";
            Session[alias + "_Size"] = size;
            Session[alias + "_Page"] = page;
            Session[alias + "_SearchType"] = searchType;
            Session[alias + "_SearchValue"] = searchValue;
            Session[alias + "_OrderBy"] = orderBy;
            Session[alias + "_IsDes"] = orderDes;

            //Call API
            MatHang_wsv.MatHang_wsv mh_wsv = new MatHang_wsv.MatHang_wsv();
            var rs = mh_wsv.SearchPaging(Session[Constants.LoginCode_SessionName].ToString(), nccID, startTime, endTime, searchValue, (MatHang_wsv.EnumSearchType)searchType, page, size, (MatHang_wsv.EnumOrderBy)orderBy, orderDes);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string SearchPagingHT()
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

            int htID = SharedFunction.ParseID(Session[Constants.HeThongID_SessionName] == null ? "" : Session[Constants.HeThongID_SessionName].ToString());

            //Cache to current session
            string alias = "mathang_hethong";
            Session[alias + "_Size"] = size;
            Session[alias + "_Page"] = page;
            Session[alias + "_SearchType"] = searchType;
            Session[alias + "_SearchValue"] = searchValue;
            Session[alias + "_OrderBy"] = orderBy;
            Session[alias + "_IsDes"] = orderDes;

            //Call API
            MatHang_wsv.MatHang_wsv mh_wsv = new MatHang_wsv.MatHang_wsv();
            var rs = mh_wsv.SearchPagingHT(Session[Constants.LoginCode_SessionName].ToString(), htID, startTime, endTime, searchValue, (MatHang_wsv.EnumSearchType)searchType, page, size, (MatHang_wsv.EnumOrderBy)orderBy, orderDes);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string SearchPagingHD()
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

            int hdID = SharedFunction.ParseID(Request["hdID"]);

            //Cache to current session
            string alias = "mathang_hopdong";
            Session[alias + "_Size"] = size;
            Session[alias + "_Page"] = page;
            Session[alias + "_SearchType"] = searchType;
            Session[alias + "_SearchValue"] = searchValue;
            Session[alias + "_OrderBy"] = orderBy;
            Session[alias + "_IsDes"] = orderDes;

            //Call API
            MatHang_wsv.MatHang_wsv mh_wsv = new MatHang_wsv.MatHang_wsv();
            var rs = mh_wsv.SearchPagingHD(Session[Constants.LoginCode_SessionName].ToString(), hdID, startTime, endTime, searchValue, (MatHang_wsv.EnumSearchType)searchType, page, size, (MatHang_wsv.EnumOrderBy)orderBy, orderDes);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string Delete()
        {
            int id = SharedFunction.ParseID(Request["id"]);

            MatHang_wsv.MatHang_wsv mh_wsv = new MatHang_wsv.MatHang_wsv();
            var rs = mh_wsv.Delete(Session[Constants.LoginCode_SessionName].ToString(), id);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string DeleteAll()
        {
            List<int> listID = JsonConvert.DeserializeObject<List<int>>(Request["ListID"]);

            MatHang_wsv.MatHang_wsv mh_wsv = new MatHang_wsv.MatHang_wsv();
            var rs = mh_wsv.DeleteList(Session[Constants.LoginCode_SessionName].ToString(), listID.ToArray());

            return JsonConvert.SerializeObject(rs);
        }

        //
        //public ActionResult Create()
        //{
        //    if (!IsLogin())
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }

        //    return View();
        //}

        public string PostCreate()
        {
            //Get param
            string ten = Request["txt_Ten"];
            string moTa = Request["txt_MoTa"];
            int donGia = SharedFunction.ParseInt(Request["txt_DonGia"]);
            int nccID = SharedFunction.ParseID(Request["txt_ID"]);
            List<int> listLMH_ID = JsonConvert.DeserializeObject<List<int>>(Request["ListLMH_ID"]);

            string img = Constants.DefaultImage;
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
            MatHang_wsv.tbl_MatHang obj = new MatHang_wsv.tbl_MatHang();
            obj.Ten = ten;
            obj.MoTa = moTa;
            obj.Gia = donGia;
            obj.Img = img;
            obj.NhaCungCapID = nccID;

            //Call API
            MatHang_wsv.MatHang_wsv mh_wsv = new MatHang_wsv.MatHang_wsv();
            var rs = mh_wsv.Create(Session[Constants.LoginCode_SessionName].ToString(), obj, listLMH_ID.ToArray());

            return JsonConvert.SerializeObject(rs);
        }

        //
        //public ActionResult Detail()
        //{
        //    if (!IsLogin())
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }

        //    return View();
        //}

        [HttpPost]
        public string GetDetail()
        {
            int id = SharedFunction.ParseID(Request["id"]);
            MatHang_wsv.MatHang_wsv mh_wsv = new MatHang_wsv.MatHang_wsv();
            var rs = mh_wsv.GetByID(id);

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
            string moTa = Request["txt_MoTa"];
            int donGia = SharedFunction.ParseInt(Request["txt_DonGia"]);
            List<int> listLMH_ID = JsonConvert.DeserializeObject<List<int>>(Request["ListLMH_ID"]);

            string img = String.Empty;
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
            MatHang_wsv.tbl_MatHang obj = new MatHang_wsv.tbl_MatHang();
            obj.ID = id;
            obj.Ten = ten;
            obj.MoTa = moTa;
            obj.Gia = donGia;
            obj.Img = img;

            //Call API
            MatHang_wsv.MatHang_wsv mh_wsv = new MatHang_wsv.MatHang_wsv();
            var rs = mh_wsv.Edit(Session[Constants.LoginCode_SessionName].ToString(), obj, listLMH_ID.ToArray());

            return JsonConvert.SerializeObject(rs);
        }

        //Extra method:
        [HttpPost]
        public string GetListCombobox()
        {
            int nccID = SharedFunction.ParseID(Request["nccID"]);
            MatHang_wsv.MatHang_wsv mh_wsv = new MatHang_wsv.MatHang_wsv();
            var rs = mh_wsv.GetListCombobox(nccID);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string GetListComboboxLMH()
        {
            LoaiMatHang_wsv.LoaiMatHang_wsv lmh_wsv = new LoaiMatHang_wsv.LoaiMatHang_wsv();
            var rs = lmh_wsv.GetListCombobox();

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string GetListLMH_ID()
        {
            int id = SharedFunction.ParseID(Request["id"]);
            MatHang_wsv.MatHang_wsv mh_wsv = new MatHang_wsv.MatHang_wsv();
            var rs = mh_wsv.GetListLMH(id);

            return JsonConvert.SerializeObject(rs);
        }
    }
}