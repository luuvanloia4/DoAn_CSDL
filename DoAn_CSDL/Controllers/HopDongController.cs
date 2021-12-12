using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn_CSDL.Shared;
using Newtonsoft.Json;

namespace DoAn_CSDL.Controllers
{
    public class HopDongController : Controller
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

            int htID = SharedFunction.ParseID((Session[Constants.HeThongID_SessionName] == null? "": Session[Constants.HeThongID_SessionName].ToString()));
            int nccID = SharedFunction.ParseID(Session[Constants.NCCID_SessionName] == null? "": Session[Constants.NCCID_SessionName].ToString());

            //Cache to current session
            string alias = "hopdong";
            Session[alias + "_Size"] = size;
            Session[alias + "_Page"] = page;
            Session[alias + "_SearchType"] = searchType;
            Session[alias + "_SearchValue"] = searchValue;
            Session[alias + "_OrderBy"] = orderBy;
            Session[alias + "_IsDes"] = orderDes;

            //Call API
            HopDong_wsv.HopDong_wsv hd_wsv = new HopDong_wsv.HopDong_wsv();
            var rs = hd_wsv.SearchPaging(Session[Constants.LoginCode_SessionName].ToString(), htID, nccID, trangThai, startTime, endTime, searchValue, (HopDong_wsv.EnumSearchType)searchType, page, size, (HopDong_wsv.EnumOrderBy)orderBy, orderDes);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string Delete()
        {
            int id = SharedFunction.ParseID(Request["id"]);

            HopDong_wsv.HopDong_wsv hd_wsv = new HopDong_wsv.HopDong_wsv();
            var rs = hd_wsv.Delete(Session[Constants.LoginCode_SessionName].ToString(), id);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string DeleteAll()
        {
            List<int> listID = JsonConvert.DeserializeObject<List<int>>(Request["ListID"]);

            HopDong_wsv.HopDong_wsv hd_wsv = new HopDong_wsv.HopDong_wsv();
            var rs = hd_wsv.DeleteList(Session[Constants.LoginCode_SessionName].ToString(), listID.ToArray());

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
            int nccID = SharedFunction.ParseID(Request["id"]);
            int htID = SharedFunction.ParseID(Session[Constants.HeThongID_SessionName].ToString());

            //Create obj
            HopDong_wsv.tbl_HopDong obj = new HopDong_wsv.tbl_HopDong();
            obj.NhaCungCapID = nccID;
            obj.HeThongID = htID;

            //Call API
            HopDong_wsv.HopDong_wsv hd_wsv = new HopDong_wsv.HopDong_wsv();
            var rs = hd_wsv.Create(Session[Constants.LoginCode_SessionName].ToString(), obj);

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
            HopDong_wsv.HopDong_wsv hd_wsv = new HopDong_wsv.HopDong_wsv();
            var rs = hd_wsv.GetByID(id);

            return JsonConvert.SerializeObject(rs);
        }
        
        [HttpPost]
        public string GetProgress()
        {
            int id = SharedFunction.ParseID(Request["id"]);

            HopDong_wsv.HopDong_wsv hd_wsv = new HopDong_wsv.HopDong_wsv();
            var rs = hd_wsv.GetProgress(id);

            return JsonConvert.SerializeObject(rs);
        }

        //CTHD
        [HttpPost]
        public string AddCTHD()
        {
            int hdID = SharedFunction.ParseID(Request["hdID"]);
            int mhID = SharedFunction.ParseID(Request["mhID"]);
            int soLuong = SharedFunction.ParseInt(Request["SL"]);

            HopDong_wsv.ID_SL id_sl = new HopDong_wsv.ID_SL();
            id_sl.ID = mhID;
            id_sl.SL = soLuong;

            HopDong_wsv.HopDong_wsv hd_wsv = new HopDong_wsv.HopDong_wsv();
            var rs = hd_wsv.AddChiTietHD(Session[Constants.LoginCode_SessionName].ToString(), hdID, id_sl);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string UpdateCTHD()
        {
            int hdID = SharedFunction.ParseID(Request["hdID"]);
            List<HopDong_wsv.ID_SL> listCTHD = JsonConvert.DeserializeObject<List<HopDong_wsv.ID_SL>>(Request["ListCTHD"]);
            HopDong_wsv.HopDong_wsv hd_wsv = new HopDong_wsv.HopDong_wsv();
            var rs = hd_wsv.UpdateChiTietHD(Session[Constants.LoginCode_SessionName].ToString(), hdID, listCTHD.ToArray());

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string DeleteCTHD()
        {
            int hdID = SharedFunction.ParseID(Request["hdID"]);
            int mhID = SharedFunction.ParseID(Request["mhID"]);

            HopDong_wsv.HopDong_wsv hd_wsv = new HopDong_wsv.HopDong_wsv();
            var rs = hd_wsv.DeleteChiTietHD(Session[Constants.LoginCode_SessionName].ToString(), hdID, mhID);

            return JsonConvert.SerializeObject(rs);
        }
        //End CTHD

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
            HeThong_wsv.tbl_HeThong obj = new HeThong_wsv.tbl_HeThong();
            obj.ID = id;
            obj.Ten = ten;
            obj.DiaChi = diaChi;
            obj.SDT = SDT;
            obj.NganHang = nganHang;
            obj.STK = STK;
            obj.TaiKhoanID = daiDienID;

            //Call API
            HeThong_wsv.HeThong_wsv ht_wsv = new HeThong_wsv.HeThong_wsv();
            var rs = ht_wsv.Edit(Session[Constants.LoginCode_SessionName].ToString(), obj);

            return JsonConvert.SerializeObject(rs);
        }

        //PN
        [HttpPost]
        public string GetListPhieuNhap()
        {
            int id = SharedFunction.ParseID(Request["id"]);
            HopDong_wsv.HopDong_wsv hd_wsv = new HopDong_wsv.HopDong_wsv();
            var rs = hd_wsv.GetListPhieuNhap(id);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string CreatePhieuNhap()
        {
            int hdID = SharedFunction.ParseID(Request["hdID"]);
            string nguoiGiao = Request["txt_NguoiGiao"];
            List<HopDong_wsv.ID_SL> listCTHD = JsonConvert.DeserializeObject<List<HopDong_wsv.ID_SL>>(Request["ListCTPN"]);
            HopDong_wsv.tbl_PhieuNhap obj = new HopDong_wsv.tbl_PhieuNhap();
            obj.HopDongID = hdID;
            obj.NguoiGiao = nguoiGiao;
            HopDong_wsv.HopDong_wsv hd_wsv = new HopDong_wsv.HopDong_wsv();
            var rs = hd_wsv.CreatePhieuNhap(Session[Constants.LoginCode_SessionName].ToString(), obj, listCTHD.ToArray());

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost] 
        public string GetListCTPN()
        {
            int pnID = SharedFunction.ParseID(Request["pnID"]);
            HopDong_wsv.HopDong_wsv hd_wsv = new HopDong_wsv.HopDong_wsv();
            var rs = hd_wsv.GetListCTPN(pnID);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string DeletePhieuNhap()
        {
            int pnID = SharedFunction.ParseID(Request["pnID"]);
            HopDong_wsv.HopDong_wsv hd_wsv = new HopDong_wsv.HopDong_wsv();
            var rs = hd_wsv.DeletePhieuNhap(Session[Constants.LoginCode_SessionName].ToString(), pnID);

            return JsonConvert.SerializeObject(rs);
        }
        //End PN

        //Extra method:
        [HttpPost]
        public string GetListCombobox()
        {
            HopDong_wsv.HopDong_wsv hd_wsv = new HopDong_wsv.HopDong_wsv();
            var rs = hd_wsv.GetListCombobox();

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string GetListCTHD()
        {
            int hdID = SharedFunction.ParseID(Request["id"]);
            HopDong_wsv.HopDong_wsv hd_wsv = new HopDong_wsv.HopDong_wsv();
            var rs = hd_wsv.GetListCTHD(Session[Constants.LoginCode_SessionName].ToString(), hdID);

            return JsonConvert.SerializeObject(rs);
        }
    }
}