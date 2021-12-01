﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using APIs.Models;
using APIs.Models.Entity;
using APIs.Shareds;
using APIs.Models.Form;

namespace APIs.Ctrls
{
    public class HopDong_ctrl
    {
        private string tableName = "hợp đồng";

        private DatabaseDataContext db = new DatabaseDataContext();

        public API_Result<bool> IsEmpty()
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                rs.Data = db.view_HopDongs.Where(u => u.IsDelete == null || u.IsDelete == false).Count() == 0;
                rs.ErrCode = EnumErrCode.Success;
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        private bool AddCTHD(int hdID, ID_SL cthd)
        {
            try
            {
                tbl_ChiTietHD oldObj = db.tbl_ChiTietHDs.Where(u => u.HopDongID.Equals(hdID) && u.MatHangID.Equals(cthd.ID)).FirstOrDefault();
                if (oldObj != null)
                {
                    oldObj.SoLuong = cthd.SL;
                    db.SubmitChanges();
                }
                else
                {
                    tbl_ChiTietHD newObj = new tbl_ChiTietHD();
                    newObj.HopDongID = hdID;
                    newObj.MatHangID = cthd.ID;
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                //
            }

            return false;
        }

        private void AddCTHD(int hdID, List<ID_SL> listCTHD)
        {
            foreach (var item in listCTHD)
            {
                AddCTHD(hdID, item);
            }
        }

        public API_Result<int> Create(string loginCode, tbl_HopDong obj, List<ID_SL> listCTHD)
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
                        tbl_HopDong newObj = new tbl_HopDong();
                        newObj.NguoiLap = curUser.ID;
                        newObj.NhaCungCapID = obj.NhaCungCapID;
                        newObj.NgayLap = DateTime.Now;
                        newObj.NgayHT = null;
                        newObj.TrangThai = 0;
                        newObj.IsDelete = false;

                        db.tbl_HopDongs.InsertOnSubmit(newObj);
                        db.SubmitChanges();

                        rs.ErrCode = EnumErrCode.Success;
                        rs.Data = newObj.ID;
                        rs.ErrDes = string.Format(Constants.MSG_Insert_Success, tableName);

                        try
                        {
                            AddCTHD(newObj.ID, listCTHD);
                        }
                        catch (Exception ex)
                        {
                            //
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

        public API_Result<bool> Edit(string loginCode, tbl_HopDong obj, List<ID_SL> listCTHD)
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if (curUser != null)
                {
                    tbl_HopDong editObj = db.tbl_HopDongs.Where(u => u.ID.Equals(obj.ID)).FirstOrDefault();
                    if (Authentication.IsSuperAdmin(curUser) || editObj.NguoiLap.Equals(curUser.ID))
                    {
                        if (editObj != null)
                        {
                            AddCTHD(editObj.ID, listCTHD);

                            rs.ErrCode = EnumErrCode.Success;
                            rs.Data = true;
                            rs.ErrDes = string.Format(Constants.MSG_Update_Success, tableName);
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
                    tbl_HopDong delObj = db.tbl_HopDongs.Where(u => (u.IsDelete == null || u.IsDelete == false) && u.ID.Equals(id)).FirstOrDefault();
                    if (Authentication.IsSuperAdmin(curUser) || delObj.NguoiLap.Equals(curUser.ID))
                    {
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
                            tbl_HopDong delObj = db.tbl_HopDongs.Where(u => (u.IsDelete == null || u.IsDelete == false) && u.ID.Equals(id)).FirstOrDefault();

                            if (Authentication.IsSuperAdmin(curUser) || delObj.NguoiLap.Equals(curUser.ID))
                            {
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

        public API_Result<List<view_HopDong>> SearchPaging(string loginCode, DateTime startTime, DateTime endTime, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
        {
            API_Result<List<view_HopDong>> rs = new API_Result<List<view_HopDong>>();
            try
            {
                if (Authentication.CheckLogin(loginCode))
                {
                    IQueryable<view_HopDong> qrs = db.view_HopDongs.Where(u => (u.IsDelete == null || u.IsDelete == false) && startTime <= u.NgayLap && u.NgayLap < endTime);

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
                    }

                    if (isDescending)
                    {
                        switch (orderBy)
                        {
                            case EnumOrderBy.Newest:
                                qrs = qrs.OrderByDescending(u => u.NgayLap);
                                break;
                            case EnumOrderBy.ID:
                                qrs = qrs.OrderByDescending(u => u.ID);
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
                                qrs = qrs.OrderBy(u => u.NgayLap);
                                break;
                            case EnumOrderBy.ID:
                                qrs = qrs.OrderBy(u => u.ID);
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

        public API_Result<List<ListCombobox_ett<int>>> GetListCombobox()
        {
            API_Result<List<ListCombobox_ett<int>>> rs = new API_Result<List<ListCombobox_ett<int>>>();
            try
            {
                IQueryable<view_HopDong> qrs = db.view_HopDongs.Where(u => (u.IsDelete == null || u.IsDelete == false));

                List<ListCombobox_ett<int>> listCB = new List<ListCombobox_ett<int>>();
                foreach (var item in qrs.ToList())
                {
                    listCB.Add(new ListCombobox_ett<int>(item.ID, item.ID + " (" + item.UserName + " - " + item.Ten + ")"));
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

        public API_Result<view_HopDong> GetByID(int id)
        {
            API_Result<view_HopDong> rs = new API_Result<view_HopDong>();
            try
            {
                var obj = db.view_HopDongs.Where(u => u.ID.Equals(id) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
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