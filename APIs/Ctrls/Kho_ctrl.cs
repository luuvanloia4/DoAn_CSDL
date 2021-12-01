using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APIs.Models;
using APIs.Models.Entity;
using APIs.Shareds;

namespace APIs.Ctrls
{
    public class Kho_ctrl
    {
        private string tableName = "kho";

        private DatabaseDataContext db = new DatabaseDataContext();

        public API_Result<bool> IsEmpty()
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                rs.Data = db.tbl_Khos.Where(u => (u.IsDelete == null || u.IsDelete == false)).Count() == 0;
                rs.ErrCode = EnumErrCode.Success;
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<int> Create(string loginCode, tbl_Kho obj)
        {
            API_Result<int> rs = new API_Result<int>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if (curUser != null)
                {
                    if (Authentication.IsSuperAdmin(curUser) || (Authentication.IsOwnKho(curUser.ID, obj.ID) && curUser.PhanQuyenID == Authentication.qAdmin))
                    {
                        //Admin
                        tbl_Kho newObj = new tbl_Kho();
                        var sameNameObj = db.tbl_Khos.Where(u => u.HeThongID.Equals(obj.HeThongID) && u.Ten.Equals(obj.Ten) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
                        if (sameNameObj == null)
                        {
                            newObj.Ten = obj.Ten;
                            newObj.DiaChi = obj.DiaChi;
                            newObj.HeThongID = obj.HeThongID;

                            newObj.IsDelete = false;
                            newObj.NgayTao = DateTime.Now;
                            newObj.NgayCapNhat = newObj.NgayTao;

                            db.tbl_Khos.InsertOnSubmit(newObj);
                            db.SubmitChanges();

                            rs.ErrCode = EnumErrCode.Success;
                            rs.Data = newObj.ID;
                            rs.ErrDes = string.Format(Constants.MSG_Insert_Success, tableName);
                        }
                        else
                        {
                            rs.ErrCode = EnumErrCode.AlreadyExist;
                            rs.ErrDes = string.Format(Constants.MSG_Already_Exist, obj.Ten);
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

        public API_Result<bool> Edit(string loginCode, tbl_Kho obj)
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if (curUser != null)
                {
                    if (Authentication.IsSuperAdmin(curUser) || (Authentication.IsOwnKho(curUser.ID, obj.ID) && curUser.PhanQuyenID == Authentication.qAdmin))
                    {
                        tbl_Kho editObj = db.tbl_Khos.Where(u => u.ID.Equals(obj.ID)).FirstOrDefault();
                        var sameNameObj = db.tbl_Khos.Where(u => u.HeThongID.Equals(obj.HeThongID) && u.Ten.Equals(obj.Ten) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
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

                        //Admin

                        //int? resultCode = 0;
                        //db.pr_User_Edit(ref resultCode, curUser.ID, obj.ID, obj.Pass, obj.HoTen, obj.NgaySinh.ToString("yyyy-MM-dd"), obj.SDT, obj.PhanQuyenID, obj.DiaChi, obj.ChucVu);

                        //if (resultCode == 1)
                        //{
                        //    rs.ErrCode = EnumErrCode.Success;
                        //    rs.Data = true;
                        //    rs.ErrDes = string.Format(Constants.MSG_Update_Success, tableName);
                        //}
                        //else if (resultCode == -1)
                        //{
                        //    rs.ErrCode = EnumErrCode.Fail;
                        //    rs.Data = false;
                        //    rs.ErrDes = string.Format(Constants.MSG_Update_Fail, tableName);
                        //}
                        //else if (resultCode == -2)
                        //{
                        //    rs.ErrCode = EnumErrCode.AlreadyExist;
                        //    rs.ErrDes = string.Format(Constants.MSG_Already_Exist, obj.UserName);
                        //}
                        //else if(resultCode == -3)
                        //{
                        //    rs.ErrCode = EnumErrCode.DoesNotExist;
                        //    rs.ErrDes = string.Format(Constants.MSG_Object_Empty, obj.ID);
                        //}
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
                    if (Authentication.IsSuperAdmin(curUser) || (Authentication.IsOwnKho(curUser.ID, id) && curUser.PhanQuyenID == Authentication.qAdmin))
                    {
                        tbl_Kho delObj = db.tbl_Khos.Where(u => (u.IsDelete == null || u.IsDelete == false) && u.ID.Equals(id)).FirstOrDefault();
                        if (delObj != null)
                        {
                            delObj.IsDelete = true;
                            db.SubmitChanges();

                            rs.ErrCode = EnumErrCode.Success;
                            rs.Data = true;
                            rs.ErrDes = string.Format(Constants.MSG_Delete_Success, tableName);
                        }
                        else
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
                if (curUser != null)
                {
                    if (Authentication.IsAdmin(curUser))
                    {
                        int delSuccessCount = 0;
                        int delFailCount = 0;
                        foreach (var id in listID)
                        {
                            if(Authentication.IsSuperAdmin(curUser) || (Authentication.IsOwnKho(curUser.ID, id) && curUser.PhanQuyenID == Authentication.qAdmin))
                            {
                                tbl_Kho delObj = db.tbl_Khos.Where(u => (u.IsDelete == null || u.IsDelete == false) && u.ID.Equals(id)).FirstOrDefault();
                                try
                                {
                                    delObj.IsDelete = true;
                                    db.SubmitChanges();
                                    delSuccessCount++;
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
                            else
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
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<List<view_Kho>> SearchPaging(int heThongID, DateTime startTime, DateTime endTime, string searchValue, EnumSearchType searchType, int curPage, int pageSize, EnumOrderBy orderBy, bool isDes)
        {
            API_Result<List<view_Kho>> rs = new API_Result<List<view_Kho>>();
            try
            {
                IQueryable<view_Kho> qrs = db.view_Khos.Where(u => (u.IsDelete == null || u.IsDelete == false) && startTime <= u.NgayTao && u.NgayTao < endTime);
                IQueryable<view_Kho> qr = null;

                if(heThongID > 0)
                {
                    qrs = qrs.Where(u => u.HeThongID.Equals(heThongID));
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
                }

                if (isDes)
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
                            qrs = qrs.OrderByDescending(u => u.Ten);
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
                            qrs = qrs.OrderBy(u => u.Ten);
                            break;
                    }
                }

                rs.RecordCount = qrs.Count();
                if(rs.RecordCount > 0)
                {
                    rs.PageCount = rs.RecordCount / pageSize;
                    if(rs.RecordCount % pageSize != 0)
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
            catch(Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<List<ListCombobox_ett<int>>> GetListCombobox(int heThongID)
        {
            API_Result<List<ListCombobox_ett<int>>> rs = new API_Result<List<ListCombobox_ett<int>>>();
            try
            {
                IQueryable<view_Kho> qrs = db.view_Khos.Where(u => (u.IsDelete == null || u.IsDelete == false)).OrderBy(u => u.HeThongID);
                if (heThongID > 0)
                {
                    qrs = qrs.Where(u => u.HeThongID.Equals(heThongID));
                }

                List<ListCombobox_ett<int>> listCB = new List<ListCombobox_ett<int>>();
                foreach (var item in qrs.ToList())
                {
                    listCB.Add(new ListCombobox_ett<int>(item.ID, item.Ten + " (" + item.TenHeThong + ")"));
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

        public API_Result<view_Kho> GetByID(int id)
        {
            API_Result<view_Kho> rs = new API_Result<view_Kho>();
            try
            {
                var obj = db.view_Khos.Where(u => u.ID.Equals(id) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
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

        public API_Result<List<view_Kho>> GetList(int heThongID, int index = 0, int size = -1)
        {
            API_Result<List<view_Kho>> rs = new API_Result<List<view_Kho>>();
            try
            {
                IQueryable<view_Kho> qrs = db.view_Khos.Where(u => (u.IsDelete == null || u.IsDelete == false));
                if(heThongID > 0)
                {
                    qrs = qrs.Where(u => u.HeThongID.Equals(heThongID));
                }
                qrs = qrs.Skip(index);
                if(size > 0)
                {
                    qrs = qrs.Take(size);
                }

                rs.RecordCount = rs.Data.Count();
                if (rs.RecordCount > 0)
                {
                    rs.Data = qrs.ToList();
                }
                else
                {
                    rs.Data = new List<view_Kho>();
                }
                
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