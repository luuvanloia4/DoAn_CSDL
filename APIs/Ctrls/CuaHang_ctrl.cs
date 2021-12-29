using APIs.Models;
using APIs.Models.Entity;
using APIs.Shareds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIs.Ctrls
{
    public class CuaHang_ctrl
    {
        private string tableName = "cửa hàng";

        private DatabaseDataContext db = new DatabaseDataContext();

        public API_Result<bool> IsEmpty()
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                rs.Data = db.view_CuaHangs.Where(u => u.IsDelete == null || u.IsDelete == false).Count() == 0;
                rs.ErrCode = EnumErrCode.Success;
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<int> Create(string loginCode, tbl_CuaHang obj)
        {
            API_Result<int> rs = new API_Result<int>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if (curUser != null)
                {
                    if (Authentication.IsAdmin(curUser))
                    {
                        //Admin
                        tbl_CuaHang newObj = new tbl_CuaHang();
                        newObj.Ten = obj.Ten;
                        newObj.DiaChi = obj.DiaChi;
                        newObj.SDT = obj.SDT;
                        newObj.STK = obj.STK;
                        newObj.NganHang = obj.NganHang;
                        newObj.HeThongID = obj.HeThongID;
                        if (Authentication.IsSuperAdmin(curUser))
                        {
                            newObj.TaiKhoanID = obj.TaiKhoanID;
                        }
                        else
                        {
                            newObj.TaiKhoanID = curUser.ID;
                        }
                        newObj.NgayTao = DateTime.Now;
                        newObj.NgayCapNhat = newObj.NgayTao;
                        newObj.IsDelete = false;

                        db.tbl_CuaHangs.InsertOnSubmit(newObj);
                        db.SubmitChanges();

                        rs.ErrCode = EnumErrCode.Success;
                        rs.Data = newObj.ID;
                        rs.ErrDes = string.Format(Constants.MSG_Insert_Success, tableName);
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

        public API_Result<bool> Edit(string loginCode, tbl_CuaHang obj)
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if (curUser != null)
                {
                    tbl_CuaHang editObj = db.tbl_CuaHangs.Where(u => u.ID.Equals(obj.ID)).FirstOrDefault();
                    if (Authentication.IsSuperAdmin(curUser) || (editObj.TaiKhoanID.Equals(curUser.ID)))
                    {
                        tbl_CuaHang sameNameObj = db.tbl_CuaHangs.Where(u => (u.IsDelete == null || u.IsDelete == false) && u.Ten.Equals(obj.Ten) && u.HeThongID.Equals(obj.HeThongID)).FirstOrDefault();
                        if (editObj != null)
                        {
                            if (!editObj.Ten.Equals(obj.Ten) && sameNameObj != null)
                            {
                                rs.ErrCode = EnumErrCode.AlreadyExist;
                                rs.ErrDes = string.Format(Constants.MSG_Already_Exist, obj.Ten);
                            }
                            else
                            {
                                editObj.Ten = obj.Ten;
                                editObj.DiaChi = obj.DiaChi;
                                editObj.SDT = obj.SDT;
                                editObj.STK = obj.STK;
                                editObj.NganHang = obj.NganHang;
                                if (Authentication.IsSuperAdmin(curUser))
                                {
                                    editObj.TaiKhoanID = obj.TaiKhoanID;
                                }

                                editObj.NgayCapNhat = DateTime.Now;

                                db.SubmitChanges();

                                rs.ErrCode = EnumErrCode.Success;
                                rs.Data = true;
                                rs.ErrDes = string.Format(Constants.MSG_Update_Success, tableName);
                            }
                        }
                        else
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
                    tbl_CuaHang delObj = db.tbl_CuaHangs.Where(u => (u.IsDelete == null || u.IsDelete == false) && u.ID.Equals(id)).FirstOrDefault();
                    if (delObj != null)
                    {
                        if (Authentication.IsSuperAdmin(curUser) || delObj.TaiKhoanID.Equals(curUser.ID))
                        {
                            delObj.IsDelete = true;
                            db.SubmitChanges();

                            rs.ErrCode = EnumErrCode.Success;
                            rs.Data = true;
                            rs.ErrDes = string.Format(Constants.MSG_Delete_Success, tableName);
                        }
                        else
                        {
                            rs.ErrCode = EnumErrCode.PermissionDenied;
                            rs.ErrDes = Constants.MSG_Permission_Denied;
                        }
                    }
                    else
                    {
                        rs.ErrCode = EnumErrCode.DoesNotExist;
                        rs.ErrDes = string.Format(Constants.MSG_Object_Empty, id);
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
                if (curUser != null)
                {
                    if (Authentication.IsSuperAdmin(curUser))
                    {
                        int delSuccessCount = 0;
                        int delFailCount = 0;
                        foreach (var id in listID)
                        {
                            tbl_CuaHang delObj = db.tbl_CuaHangs.Where(u => (u.IsDelete == null || u.IsDelete == false) && u.ID.Equals(id)).FirstOrDefault();
                            try
                            {
                                if (Authentication.IsSuperAdmin(curUser) || delObj.TaiKhoanID.Equals(curUser.ID))
                                {
                                    delObj.IsDelete = true;
                                    db.SubmitChanges();
                                    delSuccessCount++;
                                }
                                else
                                {
                                    delFailCount++;
                                }
                            }
                            catch (Exception ex)
                            {
                                foreach (var change in db.GetChangeSet().Updates)
                                {
                                    db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, change);
                                }
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
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<List<view_CuaHang>> SearchPaging(string loginCode, int htID, DateTime startTime, DateTime endTime, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
        {
            API_Result<List<view_CuaHang>> rs = new API_Result<List<view_CuaHang>>();
            try
            {
                if (Authentication.CheckLogin(loginCode))
                {
                    IQueryable<view_CuaHang> qrs = db.view_CuaHangs.Where(u => (u.IsDelete == null || u.IsDelete == false) && startTime <= u.NgayTao && u.NgayTao < endTime);

                    if(htID > 0)
                    {
                        qrs = qrs.Where(u => u.HeThongID.Equals(htID));
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
                            qrs = qrs.Where(u => u.Ten.Contains(searchValue));
                            break;
                        case EnumSearchType.UserName:
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
                        if (rs.RecordCount % pageSize != 0)
                        {
                            rs.PageCount++;
                        }

                        rs.Data = qrs.Skip((curPage - 1) * pageSize).Take(pageSize).ToList();
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
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<List<ListCombobox_ett<int>>> GetListCombobox(int htID)
        {
            API_Result<List<ListCombobox_ett<int>>> rs = new API_Result<List<ListCombobox_ett<int>>>();
            try
            {
                IQueryable<view_CuaHang> qrs = db.view_CuaHangs.Where(u => (u.IsDelete == null || u.IsDelete == false)).OrderBy(u => u.Ten);

                if(htID > 0)
                {
                    qrs = qrs.Where(u => u.HeThongID.Equals(htID));
                }

                List<ListCombobox_ett<int>> listCB = new List<ListCombobox_ett<int>>();
                foreach (var item in qrs.ToList())
                {
                    listCB.Add(new ListCombobox_ett<int>(item.ID, item.Ten + " (" + item.HoTen + ")"));
                }

                rs.Data = listCB;
                rs.RecordCount = listCB.Count();
                rs.ErrCode = EnumErrCode.Success;
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<view_CuaHang> GetByID(int id)
        {
            API_Result<view_CuaHang> rs = new API_Result<view_CuaHang>();
            try
            {
                var obj = db.view_CuaHangs.Where(u => u.ID.Equals(id) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
                rs.Data = obj;
                rs.ErrCode = EnumErrCode.Success;
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }
    }
}