using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using APIs.Models;
using APIs.Models.Entity;
using APIs.Shareds;

namespace APIs.Ctrls
{
    public class TaiKhoan_ctrl
    {
        private string tableName = "tài khoản";

        private DatabaseDataContext db = new DatabaseDataContext();
        //string IncMaTK(string maTK)
        //{
        //    //user0
        //    if (string.IsNullOrEmpty(maTK))
        //    {
        //        maTK = "USER0";
        //    }
        //    string tempStr = maTK.Substring(4);
        //    int stt;
        //    try
        //    {
        //        stt = int.Parse(tempStr);
        //    }
        //    catch
        //    {
        //        stt = 0;
        //    }
        //    stt++;

        //    return "USER" + stt.ToString();
        //}

        public API_Result<bool> IsEmpty()
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                rs.Data = db.view_TaiKhoans.Where(u => u.IsDelete == null || u.IsDelete == false).Count() == 0;
                rs.ErrCode = EnumErrCode.Success;
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<string> Login(string userName, string password)
        {
            API_Result<string> rs = Authentication.Login(userName, password);

            return rs;
        }

        public API_Result<bool> Create(string loginCode, tbl_TaiKhoan obj)
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if(curUser != null)
                {
                    if(Authentication.IsAdmin(curUser))
                    {
                        //Aditional condition
                        if(curUser.PhanQuyenID == Authentication.qAdmin && (obj.PhanQuyenID == Authentication.qAdmin || obj.PhanQuyenID == Authentication.qSuperAdmin))
                        {
                            rs.ErrCode = EnumErrCode.PermissionDenied;
                            rs.ErrDes = Constants.MSG_Permission_Denied;

                            return rs;
                        }

                        //Admin

                        int? resultCode = 0;
                        db.pr_User_Create(ref resultCode, curUser.ID, obj.UserName, obj.Pass, obj.HoTen, obj.NgaySinh.ToString("yyyy-MM-dd"), obj.SDT, obj.PhanQuyenID, obj.DiaChi, obj.ChucVu);

                        if (resultCode == 1)
                        {
                            rs.ErrCode = EnumErrCode.Success;
                            rs.ErrDes = string.Format(Constants.MSG_Insert_Success, tableName);
                            rs.Data = true;
                        }
                        else if(resultCode == -1)
                        {
                            rs.ErrCode = EnumErrCode.Fail;
                            rs.ErrDes = rs.ErrDes = string.Format(Constants.MSG_Insert_Fail, tableName);
                        }
                        else if (resultCode == -2)
                        {
                            rs.ErrCode = EnumErrCode.AlreadyExist;
                            rs.ErrDes = string.Format(Constants.MSG_Already_Exist, obj.UserName); ;
                        }
                    }
                    else
                    {
                        rs.ErrCode = EnumErrCode.PermissionDenied;
                        rs.ErrDes = Constants.MSG_Permission_Denied;
                    }
                }
                else
                {
                    rs.ErrCode = EnumErrCode.NotYetLogin;
                    rs.ErrDes = Constants.MSG_Not_Login;
                }
            }
            catch(Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<bool> Edit(string loginCode, tbl_TaiKhoan obj)
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if (curUser != null)
                {
                    if (Authentication.IsAdmin(curUser))
                    {
                        //Aditional condition
                        if (curUser.PhanQuyenID == Authentication.qAdmin && (obj.PhanQuyenID == Authentication.qAdmin || obj.PhanQuyenID == Authentication.qSuperAdmin))
                        {
                            rs.ErrCode = EnumErrCode.PermissionDenied;
                            rs.ErrDes = Constants.MSG_Permission_Denied;

                            return rs;
                        }

                        //Admin

                        int? resultCode = 0;
                        db.pr_User_Edit(ref resultCode, curUser.ID, obj.ID, obj.Pass, obj.HoTen, obj.NgaySinh.ToString("yyyy-MM-dd"), obj.SDT, obj.PhanQuyenID, obj.DiaChi, obj.ChucVu);

                        if (resultCode == 1)
                        {
                            rs.ErrCode = EnumErrCode.Success;
                            rs.Data = true;
                            rs.ErrDes = string.Format(Constants.MSG_Update_Success, tableName);
                        }
                        else if (resultCode == -1)
                        {
                            rs.ErrCode = EnumErrCode.Fail;
                            rs.Data = false;
                            rs.ErrDes = string.Format(Constants.MSG_Update_Fail, tableName);
                        }
                        else if (resultCode == -2)
                        {
                            rs.ErrCode = EnumErrCode.AlreadyExist;
                            rs.ErrDes = string.Format(Constants.MSG_Already_Exist, obj.UserName);
                        }
                        else if(resultCode == -3)
                        {
                            rs.ErrCode = EnumErrCode.DoesNotExist;
                            rs.ErrDes = string.Format(Constants.MSG_Object_Empty, obj.ID);
                        }
                    }
                    else
                    {
                        rs.ErrCode = EnumErrCode.PermissionDenied;
                        rs.ErrDes = Constants.MSG_Permission_Denied;
                    }
                }
                else
                {
                    rs.ErrCode = EnumErrCode.NotYetLogin;
                    rs.ErrDes = Constants.MSG_Not_Login;
                }
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }
        public API_Result<bool> Delete(string loginCode, int id)
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if (curUser != null)
                {
                    if (Authentication.IsAdmin(curUser))
                    {
                        int? resultCode = 0;
                        db.pr_User_Delete(ref resultCode, curUser.ID, (int?)id);

                        if (resultCode == 1)
                        {
                            rs.ErrCode = EnumErrCode.Success;
                            rs.Data = true;
                            rs.ErrDes = string.Format(Constants.MSG_Delete_Success, tableName);
                        }
                        else if (resultCode == -1)
                        {
                            rs.ErrCode = EnumErrCode.Fail;
                            rs.ErrDes = string.Format(Constants.MSG_Delete_Fail, tableName);
                        }
                        else if (resultCode == -3)
                        {
                            rs.ErrCode = EnumErrCode.DoesNotExist;
                            rs.ErrDes = string.Format(Constants.MSG_Object_Empty, id);
                        }
                    }
                    else
                    {
                        rs.ErrCode = EnumErrCode.PermissionDenied;
                        rs.ErrDes = Constants.MSG_Permission_Denied;
                    }
                }
                else
                {
                    rs.ErrCode = EnumErrCode.NotYetLogin;
                    rs.ErrDes = Constants.MSG_Not_Login;
                }
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<string> DeleteList(string loginCode, int[] listID)
        {
            API_Result<string> rs = new API_Result<string>();
            try
            {
                var curUser = Authentication.GetUser(loginCode).Data;
                int? resultCode = 0;
                if(curUser != null)
                {
                    if (Authentication.IsAdmin(curUser))
                    {
                        int delSuccessCount = 0;
                        int delFailCount = 0;
                        foreach(var id in listID)
                        {
                            try
                            {
                                db.pr_User_Delete(ref resultCode, curUser.ID, (int?)id);
                                if(resultCode == 1)
                                {
                                    delSuccessCount++;
                                }
                                else
                                {
                                    delFailCount++;
                                }
                            }
                            catch
                            {
                                delFailCount++;
                            }
                        }

                        rs.ErrCode = EnumErrCode.Success;
                        rs.ErrDes = string.Format("Xóa thành công {0} trên tổng số {1} bản ghi!", delSuccessCount, delSuccessCount + delFailCount);
                    }
                    else
                    {
                        rs.ErrCode = EnumErrCode.PermissionDenied;
                        rs.ErrDes = Constants.MSG_Permission_Denied;
                    }
                }
                else
                {
                    rs.ErrCode = EnumErrCode.NotYetLogin;
                    rs.ErrDes = Constants.MSG_Not_Login;
                }
            }
            catch(Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<List<ListCombobox_ett<int>>> GetListComboboxID(string loginCode, int phanQuyenID = -1)
        {
            API_Result<List<ListCombobox_ett<int>>> rs = new API_Result<List<ListCombobox_ett<int>>>();
            try
            {
                if (Authentication.CheckLogin(loginCode))
                {
                    IQueryable<view_TaiKhoan> qrs = db.view_TaiKhoans.Where(u => u.PhanQuyenID != 0 && (u.IsDelete == null || u.IsDelete == false));
                    if (phanQuyenID == Authentication.qSuperAdmin)
                    {
                        //Không được chọn quyền superadmin
                        qrs = qrs.Where(u => !u.PhanQuyenID.Equals(0));
                        
                    }
                    else
                    {
                        qrs = qrs.Where(u => u.PhanQuyenID.Equals(phanQuyenID));
                    }

                    List<ListCombobox_ett<int>> listCB = new List<ListCombobox_ett<int>>();
                    foreach (var item in qrs.ToList())
                    {
                        listCB.Add(new ListCombobox_ett<int>(item.ID, item.UserName + " - " + item.HoTen));
                    }

                    rs.Data = listCB;
                    rs.RecordCount = listCB.Count();
                    rs.ErrCode = EnumErrCode.Success;
                }
                else
                {
                    rs.ErrCode = EnumErrCode.NotYetLogin;
                    rs.ErrDes = Constants.MSG_Not_Login;
                }
            }
            catch(Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<List<ListCombobox_ett<string>>> GetListComboboxName(string loginCode, int phanQuyenID = -1)
        {
            API_Result<List<ListCombobox_ett<string>>> rs = new API_Result<List<ListCombobox_ett<string>>>();
            try
            {
                if (Authentication.CheckLogin(loginCode))
                {
                    IQueryable<view_TaiKhoan> qrs = db.view_TaiKhoans.Where(u => u.PhanQuyenID != 0 && (u.IsDelete == null || u.IsDelete == false));
                    if (phanQuyenID == Authentication.qSuperAdmin)
                    {
                        qrs = qrs.Where(u => !u.PhanQuyenID.Equals(0));
                    }
                    else
                    {
                        qrs = qrs.Where(u => u.PhanQuyenID.Equals(phanQuyenID));
                    }

                    List<ListCombobox_ett<string>> listCB = new List<ListCombobox_ett<string>>();
                    foreach (var item in qrs.ToList())
                    {
                        listCB.Add(new ListCombobox_ett<string>(item.UserName, item.UserName + " - " + item.HoTen));
                    }

                    rs.Data = listCB;
                    rs.RecordCount = listCB.Count();
                    rs.ErrCode = EnumErrCode.Success;
                }
                else
                {
                    rs.ErrCode = EnumErrCode.NotYetLogin;
                    rs.ErrDes = Constants.MSG_Not_Login;
                }
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<List<TaiKhoan_ett>> SearchPaging(string loginCode, DateTime startTime, DateTime endTime, int role = -1, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
        {
            API_Result<List<TaiKhoan_ett>> rs = new API_Result<List<TaiKhoan_ett>>();
            try
            {
                if (Authentication.CheckLogin(loginCode))
                {
                    IQueryable<tbl_TaiKhoan> qrs = db.tbl_TaiKhoans.Where(u => (u.IsDelete == null || u.IsDelete == false) && startTime <= u.NgayTao && u.NgayTao < endTime);

                    if(role >= 0)
                    {
                        qrs = qrs.Where(u => u.PhanQuyenID.Equals(role));
                    }

                    switch (searchType)
                    {
                        case EnumSearchType.All:
                            //
                            break;
                        case EnumSearchType.ID:
                            qrs = qrs.Where(u => u.ID.Equals(searchValue));
                            break;
                        case EnumSearchType.Name:
                            qrs = qrs.Where(u => u.UserName.Contains(searchValue));
                            break;
                        case EnumSearchType.Phone:
                            qrs = qrs.Where(u => u.SDT.Contains(searchValue));
                            break;
                    }

                    if (isDescending)
                    {
                        switch (orderBy)
                        {
                            case EnumOrderBy.Newest:
                                qrs = qrs.OrderByDescending(u => u.NgayTao);
                                break;
                            case EnumOrderBy.ID:
                                qrs = qrs.OrderByDescending(u => u.ID);
                                break;
                            case EnumOrderBy.LastEdited:
                                qrs = qrs.OrderByDescending(u => u.NgayCapNhat);
                                break;
                            case EnumOrderBy.Name:
                                qrs = qrs.OrderByDescending(u => u.UserName);
                                break;
                        }
                    }
                    else
                    {
                        switch (orderBy)
                        {
                            case EnumOrderBy.Newest:
                                qrs = qrs.OrderBy(u => u.NgayTao);
                                break;
                            case EnumOrderBy.ID:
                                qrs = qrs.OrderBy(u => u.ID);
                                break;
                            case EnumOrderBy.LastEdited:
                                qrs = qrs.OrderBy(u => u.NgayCapNhat);
                                break;
                            case EnumOrderBy.Name:
                                qrs = qrs.OrderBy(u => u.UserName);
                                break;
                        }
                    }

                    rs.RecordCount = qrs.Count();

                    if (rs.RecordCount > 0)
                    {
                        rs.PageCount = rs.RecordCount / pageSize;
                        if(rs.RecordCount % pageSize != 0)
                        {
                            rs.PageCount++;
                        }

                        List<TaiKhoan_ett> listResult = new List<TaiKhoan_ett>();
                        foreach (var obj in qrs.Skip((curPage - 1) * pageSize).Take(pageSize).ToArray())
                        {
                            listResult.Add(new TaiKhoan_ett(obj));
                        }

                        rs.Data = listResult;
                        rs.ErrCode = EnumErrCode.Success;
                    }
                    else
                    {
                        rs.ErrCode = EnumErrCode.Empty;
                        rs.ErrDes = string.Format(Constants.MSG_Search_Empty, tableName);
                    }
                }
                else
                {
                    rs.ErrCode = EnumErrCode.NotYetLogin;
                    rs.ErrDes = Constants.MSG_Not_Login;
                }
            }
            catch(Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }
        
        //public API_Result<List<TaiKhoan_ett>> SearchFullPaging(string loginCode, DateTime startTime, DateTime endTime, int status = -1, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
        //{
        //    API_Result<List<TaiKhoan_ett>> rs = new API_Result<List<TaiKhoan_ett>>();
        //    try
        //    {
        //        var preResult = SearchPaging(loginCode, startTime, endTime, status, searchValue, searchType, curPage, pageSize, orderBy, isDescending);
        //        rs.ErrCode = preResult.ErrCode;
        //        rs.ErrDes = preResult.ErrDes;
        //        rs.PageCount = preResult.PageCount;
        //        rs.RecordCount = preResult.RecordCount;
        //        if(preResult.ErrCode == EnumErrCode.Success)
        //        {
        //            List<TaiKhoan_ett> list = new List<TaiKhoan_ett>();
        //            foreach(var item in preResult.Data) {
        //                list.Add(new TaiKhoan_ett(item, false));
        //            }

        //            rs.Data = list;
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        rs.ErrCode = EnumErrCode.Error;
        //        rs.ErrDes = ex.Message;
        //    }

        //    return rs;
        //}

        //public API_Result<List<TaiKhoan_ett>> ExportData(string loginCode, DateTime startTime, DateTime endTime, int status = -1, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
        //{
        //    API_Result<List<TaiKhoan_ett>> rs = new API_Result<List<TaiKhoan_ett>>();
        //    try
        //    {
        //        if (Authentication.CheckLogin(loginCode))
        //        {
        //            IQueryable<tbl_TaiKhoan> qrs = null;
        //            IQueryable<tbl_TaiKhoan> qr = null;

        //            switch (searchType)
        //            {
        //                case EnumSearchType.All:
        //                    qrs = db.tbl_TaiKhoans.Where(u => u.IsDelete == null || u.IsDelete == false);
        //                    break;
        //                case EnumSearchType.ID:
        //                    qrs = db.tbl_TaiKhoans.Where(u => u.MaTK.Equals(searchValue) && (u.IsDelete == null || u.IsDelete == false));
        //                    break;
        //                case EnumSearchType.Name:
        //                    qrs = db.tbl_TaiKhoans.Where(u => u.HoTen.Contains(searchValue) && (u.IsDelete == null || u.IsDelete == false));
        //                    break;
        //                case EnumSearchType.UserName:
        //                    qrs = db.tbl_TaiKhoans.Where(u => u.TenTK.Contains(searchValue) && (u.IsDelete == null || u.IsDelete == false));
        //                    break;
        //                case EnumSearchType.Phone:
        //                    qrs = db.tbl_TaiKhoans.Where(u => u.SDT.Equals(searchValue) && (u.IsDelete == null || u.IsDelete == false));
        //                    break;
        //                case EnumSearchType.ListAll:
        //                    qrs = db.tbl_TaiKhoans;
        //                    break;
        //            }

        //            qrs = qrs.Where(u => startTime <= u.NgayLap && u.NgayLap <= endTime);

        //            if (status != -1)
        //            {
        //                qrs = qrs.Where(u => u.PhanQuyen.Equals(status));
        //            }

        //            if (qrs.Count() > 0)
        //            {
        //                if (isDescending)
        //                {
        //                    switch (orderBy)
        //                    {
        //                        case EnumOrderBy.Newest:
        //                            qrs = qrs.OrderByDescending(u => u.NgayLap);
        //                            break;
        //                        case EnumOrderBy.ID:
        //                            qrs = qrs.OrderByDescending(u => u.MaTK);
        //                            break;
        //                        case EnumOrderBy.Name:
        //                            qrs = qrs.OrderByDescending(u => u.TenTK);
        //                            break;
        //                    }
        //                }
        //                else
        //                {
        //                    switch (orderBy)
        //                    {
        //                        case EnumOrderBy.Newest:
        //                            qrs = qrs.OrderBy(u => u.NgayLap);
        //                            break;
        //                        case EnumOrderBy.ID:
        //                            qrs = qrs.OrderBy(u => u.MaTK);
        //                            break;
        //                        case EnumOrderBy.Name:
        //                            qrs = qrs.OrderBy(u => u.TenTK);
        //                            break;
        //                    }
        //                }

        //                rs.RecordCount = qrs.Count();
        //                qr = qrs;
        //                var list = qr.ToList();
        //                List<TaiKhoan_ett> listTK = new List<TaiKhoan_ett>();
        //                foreach(var item in list)
        //                {
        //                    listTK.Add(new TaiKhoan_ett(item, false));
        //                }
        //                rs.Data = listTK;
        //                rs.ErrCode = EnumErrCode.Success;
        //            }
        //            else
        //            {
        //                rs.ErrCode = EnumErrCode.Empty;
        //            }
        //        }
        //        else
        //        {
        //            rs.ErrCode = EnumErrCode.NotYetLogin;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        rs.ErrCode = EnumErrCode.Error;
        //        rs.ErrDes = ex.Message;
        //    }

        //    return rs;
        //}

        //public API_Result<TaiKhoan_ett> GetFull(string loginCode, string id)
        //{
        //    API_Result<TaiKhoan_ett> rs = new API_Result<TaiKhoan_ett>();
        //    try
        //    {
        //        if (Authentication.CheckLogin(loginCode))
        //        {
        //            TaiKhoan_ett obj = new TaiKhoan_ett(id, true);

        //            rs.ErrCode = EnumErrCode.Success;
        //            rs.Data = obj;
        //        }
        //        else
        //        {
        //            rs.ErrCode = EnumErrCode.NotYetLogin;
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        rs.ErrCode = EnumErrCode.Error;
        //        rs.ErrDes = ex.Message;
        //    }

        //    return rs;
        //}

        public API_Result<TaiKhoan_ett> GetCurrentUser(string loginCode)
        {
            API_Result<TaiKhoan_ett> rs = new API_Result<TaiKhoan_ett>();
            var preResult = Authentication.GetUser(loginCode);
            rs.ErrCode = preResult.ErrCode;
            rs.ErrDes = preResult.ErrDes;

            if (preResult.ErrCode == EnumErrCode.Success)
            {
                rs.Data = new TaiKhoan_ett(preResult.Data);
            }

            return rs;
        }
        
        public API_Result<TaiKhoan_ett> GetByName(string loginCode, string name)
        {
            API_Result<TaiKhoan_ett> rs = new API_Result<TaiKhoan_ett>();
            try
            {
                var obj = db.view_TaiKhoans.Where(u => u.UserName.Equals(name) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
                rs.Data = new TaiKhoan_ett(obj);
                rs.ErrCode = EnumErrCode.Success;
            }
            catch(Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }
        
        public API_Result<TaiKhoan_ett> GetByID(string loginCode, int id)
        {
            API_Result<TaiKhoan_ett> rs = new API_Result<TaiKhoan_ett>();
            try
            {
                var obj = db.view_TaiKhoans.Where(u => u.ID.Equals(id) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
                rs.Data = new TaiKhoan_ett(obj);
                rs.ErrCode = EnumErrCode.Success;
            }
            catch(Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        //public API_Result<TaiKhoan_ett> GetAllByName(string loginCode, string name)
        //{
        //    API_Result<TaiKhoan_ett> rs = new API_Result<TaiKhoan_ett>();
        //    try
        //    {
        //        var preResult = GetByName(loginCode, name);
        //        rs.ErrCode = preResult.ErrCode;
        //        rs.ErrDes = preResult.ErrDes;
        //        if (preResult.ErrCode == EnumErrCode.Success)
        //        {
        //            rs.Data = new TaiKhoan_ett(preResult.Data);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        rs.ErrCode = EnumErrCode.Error;
        //        rs.ErrDes = ex.Message;
        //    }

        //    return rs;
        //}

        public API_Result<string> PhanQuyen(string loginCode, int[] listUserID, int phanQuyenID)
        {
            API_Result<string> rs = new API_Result<string>();
            try
            {
                var curUser = Authentication.GetUser(loginCode).Data;
                if (curUser != null)
                {
                    if (Authentication.IsAdmin(curUser))
                    {
                        int pSuccessCount = 0;
                        int pFailCount = 0;

                        foreach (var id in listUserID)
                        {
                            view_TaiKhoan obj = db.view_TaiKhoans.Where(u => u.ID.Equals(id)).FirstOrDefault();
                            if (!Authentication.IsAdmin(obj))
                            {
                                try
                                {
                                    obj.PhanQuyenID = phanQuyenID;
                                    db.SubmitChanges();
                                    pSuccessCount++;
                                }
                                catch (Exception ex)
                                {
                                    foreach (var change in db.GetChangeSet().Updates)
                                    {
                                        db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, change);
                                    }
                                    pFailCount++;
                                }
                            }
                            else
                            {
                                pFailCount++;
                            }
                        }

                        rs.ErrCode = EnumErrCode.Success;
                        rs.ErrDes = "Phân quyền thành công " + pSuccessCount.ToString() + " trên tổng số " + (pSuccessCount + pFailCount) + " bản ghi!";
                    }
                    else
                    {
                        rs.ErrCode = EnumErrCode.PermissionDenied;
                    }
                }
                else
                {
                    rs.ErrCode = EnumErrCode.NotYetLogin;
                }
            }
            catch(Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }
    }
}