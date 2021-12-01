using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APIs.Models;
using APIs.Models.Entity;
using APIs.Shareds;

namespace APIs.Ctrls
{
    public class ViTri_ctrl
    {
        private string tableName = "vị trí";

        private DatabaseDataContext db = new DatabaseDataContext();

        public API_Result<bool> IsEmpty()
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                rs.Data = db.tbl_ViTris.Where(u => (u.IsDelete == null || u.IsDelete == false)).Count() == 0;
                rs.ErrCode = EnumErrCode.Success;
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<int> Create(string loginCode, tbl_ViTri obj)
        {
            API_Result<int> rs = new API_Result<int>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if (curUser != null)
                {
                    if (Authentication.IsSuperAdmin(curUser) || (Authentication.IsOwnKho(curUser.ID, obj.KhoID.Value) && curUser.PhanQuyenID == Authentication.qAdmin))
                    {
                        //Admin
                        tbl_ViTri newObj = new tbl_ViTri();
                        var sameNameObj = db.tbl_ViTris.Where(u => u.KhoID.Equals(obj.KhoID) && u.HangID.Equals(obj.HangID) && u.CotID.Equals(obj.CotID) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
                        if (sameNameObj == null)
                        {
                            newObj.HangID = obj.HangID;
                            newObj.CotID = obj.CotID;
                            newObj.KhoID = obj.KhoID;

                            newObj.TrangThai = false;
                            newObj.IsDelete = false;
                            newObj.NgayTao = DateTime.Now;
                            newObj.NgayCapNhat = newObj.NgayTao;

                            db.tbl_ViTris.InsertOnSubmit(newObj);
                            db.SubmitChanges();

                            rs.ErrCode = EnumErrCode.Success;
                            rs.Data = newObj.ID;
                            rs.ErrDes = string.Format(Constants.MSG_Insert_Success, tableName);
                        }
                        else
                        {
                            rs.ErrCode = EnumErrCode.AlreadyExist;
                            rs.ErrDes = string.Format(Constants.MSG_Already_Exist, "hàng " + obj.HangID.ToString() + " cột " + obj.CotID.ToString());
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

        public API_Result<bool> Edit(string loginCode, tbl_ViTri obj)
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if (curUser != null)
                {
                    if (Authentication.IsSuperAdmin(curUser) || (Authentication.IsOwnKho(curUser.ID, obj.KhoID.Value) && curUser.PhanQuyenID == Authentication.qAdmin))
                    {
                        tbl_ViTri editObj = db.tbl_ViTris.Where(u => u.ID.Equals(obj.ID)).FirstOrDefault();
                        var sameNameObj = db.tbl_ViTris.Where(u => u.KhoID.Equals(obj.KhoID) && u.HangID.Equals(obj.HangID) && u.CotID.Equals(obj.CotID) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
                        if (editObj != null)
                        {
                            if (!(editObj.HangID.Equals(obj.HangID) && editObj.CotID.Equals(obj.CotID)) && sameNameObj != null)
                            {
                                rs.ErrCode = EnumErrCode.AlreadyExist;
                                rs.ErrDes = string.Format(Constants.MSG_Already_Exist, "hàng " + obj.HangID.ToString() + " cột " + obj.CotID.ToString());
                            }
                            else
                            {
                                editObj.HangID = obj.HangID;
                                editObj.CotID = obj.CotID;
                                if(obj.KhoID > 0)
                                {
                                    editObj.KhoID = obj.KhoID;
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
                    tbl_ViTri delObj = db.tbl_ViTris.Where(u => (u.IsDelete == null || u.IsDelete == false) && u.ID.Equals(id)).FirstOrDefault();

                    if (delObj != null)
                    {
                        if (Authentication.IsSuperAdmin(curUser) || (Authentication.IsOwnKho(curUser.ID, delObj.KhoID.Value) && curUser.PhanQuyenID == Authentication.qAdmin))
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
                    if (Authentication.IsAdmin(curUser))
                    {
                        int delSuccessCount = 0;
                        int delFailCount = 0;
                        foreach (var id in listID)
                        {
                            try
                            {
                                tbl_ViTri delObj = db.tbl_ViTris.Where(u => (u.IsDelete == null || u.IsDelete == false) && u.ID.Equals(id)).FirstOrDefault();
                                if (Authentication.IsSuperAdmin(curUser) || (Authentication.IsOwnKho(curUser.ID, delObj.ID) && curUser.PhanQuyenID == Authentication.qAdmin))
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

        public API_Result<List<view_ViTri>> SearchPaging(int khoID, DateTime startTime, DateTime endTime, string searchValue, EnumSearchType searchType, int curPage, int pageSize, EnumOrderBy orderBy, bool isDes)
        {
            API_Result<List<view_ViTri>> rs = new API_Result<List<view_ViTri>>();
            try
            {
                IQueryable<view_ViTri> qrs = db.view_ViTris.Where(u => (u.IsDelete == null || u.IsDelete == false) && startTime <= u.NgayTao && u.NgayTao < endTime);
                IQueryable<view_Kho> qr = null;

                if (khoID > 0)
                {
                    qrs = qrs.Where(u => u.KhoID.Equals(khoID));
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
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<List<ListCombobox_ett<int>>> GetListCombobox(int khoID)
        {
            API_Result<List<ListCombobox_ett<int>>> rs = new API_Result<List<ListCombobox_ett<int>>>();
            try
            {
                IQueryable<view_ViTri> qrs = db.view_ViTris.Where(u => (u.IsDelete == null || u.IsDelete == false)).OrderBy(u => u.KhoID);
                if (khoID > 0)
                {
                    qrs = qrs.Where(u => u.KhoID.Equals(khoID));
                }

                List<ListCombobox_ett<int>> listCB = new List<ListCombobox_ett<int>>();
                foreach (var item in qrs.ToList())
                {
                    listCB.Add(new ListCombobox_ett<int>(item.ID, "Hàng " + item.HangID.ToString() + " Cột " + item.CotID.ToString() + " (" + item.Ten + ")"));
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

        public API_Result<view_ViTri> GetByID(int id)
        {
            API_Result<view_ViTri> rs = new API_Result<view_ViTri>();
            try
            {
                var obj = db.view_ViTris.Where(u => u.ID.Equals(id) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
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

        public API_Result<view_ViTri> GetByLocation(int khoID, int hangID, int cotID)
        {
            API_Result<view_ViTri> rs = new API_Result<view_ViTri>();
            try
            {
                var obj = db.view_ViTris.Where(u => u.KhoID.Equals(khoID) && u.HangID.Equals(hangID) && u.CotID.Equals(cotID) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
                rs.Data = obj;
                rs.ErrCode = EnumErrCode.Success;
            }
            catch(Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<List<view_ViTri>> GetList(int khoID, int hangID = -1, int cotID = -1, int index = 0, int size = -1)
        {
            API_Result<List<view_ViTri>> rs = new API_Result<List<view_ViTri>>();
            try
            {
                IQueryable<view_ViTri> qrs = db.view_ViTris.Where(u => u.KhoID.Equals(khoID) && (u.IsDelete == null || u.IsDelete == false)).OrderBy(u => u.CotID).OrderBy(u => u.HangID);
                //Sap xep theo COT truoc HANG sau

                if (hangID > 0)
                {
                    qrs = qrs.Where(u => u.HangID.Equals(hangID));
                }
                else
                {
                    qrs = qrs.Where(u => u.CotID.Equals(cotID));
                }
                
                qrs = qrs.Skip(index);
                if (size > 0)
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
                    rs.Data = new List<view_ViTri>();
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