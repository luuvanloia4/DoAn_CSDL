using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APIs.Models;
using APIs.Models.Entity;
using APIs.Shareds;

namespace APIs.Ctrls
{
    public class PhanQuyen_ctrl
    {
        private string tableName = "phân quyền";

        private DatabaseDataContext db = new DatabaseDataContext();

        public API_Result<bool> IsEmpty()
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                rs.Data = db.view_PhanQuyens.Where(u => (u.IsDelete == null || u.IsDelete == false)).Count() == 0;
                rs.ErrCode = EnumErrCode.Success;
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<bool> Create(string loginCode, tbl_PhanQuyen obj)
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if (curUser != null)
                {
                    if (Authentication.IsSuperAdmin(curUser))
                    {
                        //Admin
                        tbl_PhanQuyen newObj = new tbl_PhanQuyen();
                        var sameIdObj = db.tbl_PhanQuyens.Where(u => u.ID.Equals(obj.ID) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
                        if (sameIdObj == null) {
                            newObj.ID = obj.ID;
                            newObj.Ten = obj.Ten;
                            newObj.GhiChu = obj.GhiChu;
                            newObj.IsDelete = false;

                            newObj.NgayTao = DateTime.Now;
                            newObj.NgayCapNhat = newObj.NgayTao;

                            db.tbl_PhanQuyens.InsertOnSubmit(newObj);
                            db.SubmitChanges();

                            rs.ErrCode = EnumErrCode.Success;
                            rs.Data = true;
                            rs.ErrDes = string.Format(Constants.MSG_Insert_Success, tableName);
                        }
                        else
                        {
                            rs.ErrCode = EnumErrCode.AlreadyExist;
                            rs.ErrDes = string.Format(Constants.MSG_Already_Exist, obj.ID);
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

        public API_Result<bool> Edit(string loginCode, tbl_PhanQuyen obj)
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if (curUser != null)
                {
                    if (Authentication.IsSuperAdmin(curUser))
                    {
                        tbl_PhanQuyen editObj = db.tbl_PhanQuyens.Where(u => u.ID.Equals(obj.ID)).FirstOrDefault();
                        var sameIdObj = db.tbl_PhanQuyens.Where(u => u.ID.Equals(obj.ID) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
                        if (editObj != null)
                        {
                            if(!editObj.ID.Equals(obj.ID) && sameIdObj != null)
                            {
                                rs.ErrCode = EnumErrCode.AlreadyExist;
                                rs.ErrDes = string.Format(Constants.MSG_Already_Exist, obj.ID);
                            }
                            else
                            {
                                editObj.ID = obj.ID;
                                editObj.Ten = obj.Ten;
                                editObj.GhiChu = obj.GhiChu;

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
                    if (Authentication.IsSuperAdmin(curUser))
                    {
                        tbl_PhanQuyen delObj = db.tbl_PhanQuyens.Where(u => (u.IsDelete == null || u.IsDelete == false) && u.ID.Equals(id)).FirstOrDefault();
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
                    if (Authentication.IsSuperAdmin(curUser))
                    {
                        int delSuccessCount = 0;
                        int delFailCount = 0;
                        foreach (var id in listID)
                        {
                            tbl_PhanQuyen delObj = db.tbl_PhanQuyens.Where(u => (u.IsDelete == null || u.IsDelete == false) && u.ID.Equals(id)).FirstOrDefault();
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

        public API_Result<List<ListCombobox_ett<int>>> GetListCombobox(string loginCode)
        {
            API_Result<List<ListCombobox_ett<int>>> rs = new API_Result<List<ListCombobox_ett<int>>>();
            try
            {
                if (Authentication.CheckLogin(loginCode))
                {
                    IQueryable<view_PhanQuyen> qrs = db.view_PhanQuyens.Where(u => !u.ID.Equals(Authentication.qSuperAdmin));

                    List<ListCombobox_ett<int>> listCB = new List<ListCombobox_ett<int>>();
                    foreach (var item in qrs.ToList())
                    {
                        listCB.Add(new ListCombobox_ett<int>(item.ID, item.Ten));
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

        public API_Result<List<view_PhanQuyen>> SearchPaging(string loginCode, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
        {
            API_Result<List<view_PhanQuyen>> rs = new API_Result<List<view_PhanQuyen>>();
            try
            {
                if (Authentication.CheckLogin(loginCode))
                {
                    IQueryable<view_PhanQuyen> qrs = db.view_PhanQuyens.Where(u => (u.IsDelete == null || u.IsDelete == false));

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

        public API_Result<view_PhanQuyen> GetByID(string loginCode, int id)
        {
            API_Result<view_PhanQuyen> rs = new API_Result<view_PhanQuyen>();
            try
            {
                var obj = db.view_PhanQuyens.Where(u => u.ID.Equals(id) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
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