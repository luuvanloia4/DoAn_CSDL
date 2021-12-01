﻿using DoAn_CSDL.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn_CSDL.Controllers
{
    public class KhoController : Controller
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

            int heThongID = SharedFunction.ParseID(Request["heThongID"]);

            //Cache to current session
            string alias = "kho";
            Session[alias + "_Size"] = size;
            Session[alias + "_Page"] = page;
            Session[alias + "_SearchType"] = searchType;
            Session[alias + "_SearchValue"] = searchValue;
            Session[alias + "_OrderBy"] = orderBy;
            Session[alias + "_IsDes"] = orderDes;

            //Call API
            Kho_wsv.Kho_wsv kho_wsv = new Kho_wsv.Kho_wsv();
            var rs = kho_wsv.SearchPaging(heThongID, startTime, endTime, searchValue, (Kho_wsv.EnumSearchType)searchType, page, size, (Kho_wsv.EnumOrderBy)orderBy, orderDes);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string Delete()
        {
            int id = SharedFunction.ParseID(Request["id"]);

            Kho_wsv.Kho_wsv kho_wsv = new Kho_wsv.Kho_wsv();
            var rs = kho_wsv.Delete(Session[Constants.LoginCode_SessionName].ToString(), id);

            return JsonConvert.SerializeObject(rs);
        }

        [HttpPost]
        public string DeleteAll()
        {
            List<int> listID = JsonConvert.DeserializeObject<List<int>>(Request["ListID"]);

            Kho_wsv.Kho_wsv kho_wsv = new Kho_wsv.Kho_wsv();
            var rs = kho_wsv.DeleteList(Session[Constants.LoginCode_SessionName].ToString(), listID.ToArray());

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

            int heThongID = SharedFunction.ParseID(Request["sel_HeThong"]);

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
            Kho_wsv.tbl_Kho obj = new Kho_wsv.tbl_Kho();
            obj.Ten = ten;
            obj.DiaChi = diaChi;
            obj.HeThongID = heThongID;

            //Call API
            Kho_wsv.Kho_wsv kho_wsv = new Kho_wsv.Kho_wsv();
            var rs = kho_wsv.Create(Session[Constants.LoginCode_SessionName].ToString(), obj);

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
            Kho_wsv.Kho_wsv kho_wsv = new Kho_wsv.Kho_wsv();
            var rs = kho_wsv.GetByID(id);

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
            //Get param
            string ten = Request["txt_Ten"];
            string diaChi = Request["txt_DiaChi"];

            int heThongID = SharedFunction.ParseID(Request["sel_HeThong"]);

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
            Kho_wsv.tbl_Kho obj = new Kho_wsv.tbl_Kho();
            obj.ID = id;
            obj.Ten = ten;
            obj.DiaChi = diaChi;
            obj.HeThongID = heThongID;

            //Call API
            Kho_wsv.Kho_wsv kho_wsv = new Kho_wsv.Kho_wsv();
            var rs = kho_wsv.Edit(Session[Constants.LoginCode_SessionName].ToString(), obj);

            return JsonConvert.SerializeObject(rs);
        }

        //Extra method:
        [HttpPost]
        public string GetListCombobox()
        {
            int heThongID = SharedFunction.ParseID(Request["heThongID"]);

            Kho_wsv.Kho_wsv kho_wsv = new Kho_wsv.Kho_wsv();
            var rs = kho_wsv.GetListCombobox(heThongID);

            return JsonConvert.SerializeObject(rs);
        }
    }
}