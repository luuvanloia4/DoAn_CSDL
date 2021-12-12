using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APIs.Models;
using APIs.Models.Entity;
using APIs.Models.Form;
using APIs.Shareds;

namespace APIs.Ctrls
{
    public class MatHang_ctrl
    {
        private string tableName = "mặt hàng";

        private DatabaseDataContext db = new DatabaseDataContext();

        public API_Result<bool> IsEmpty()
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                rs.Data = db.view_MatHangs.Where(u => u.IsDelete == null || u.IsDelete == false).Count() == 0;
                rs.ErrCode = EnumErrCode.Success;
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<int> Create(string loginCode, tbl_MatHang obj, int[] listLMH_ID)
        {
            API_Result<int> rs = new API_Result<int>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if (curUser != null)
                {
                    if (Authentication.IsSuperAdmin(curUser) || Authentication.IsOwnNCC(curUser.ID, obj.NhaCungCapID.Value))
                    {
                        //Admin
                        tbl_MatHang sameNameObj = db.tbl_MatHangs.Where(u => (u.IsDelete == null || u.IsDelete == false) && u.Ten.Equals(obj.Ten) && u.NhaCungCapID.Equals(obj.NhaCungCapID)).FirstOrDefault();
                        if (sameNameObj != null)
                        {
                            rs.ErrCode = EnumErrCode.AlreadyExist;
                            rs.ErrDes = string.Format(Constants.MSG_Already_Exist, obj.Ten);
                        }
                        else
                        {
                            tbl_MatHang newObj = new tbl_MatHang();
                            newObj.Ten = obj.Ten;
                            newObj.Img = obj.Img;
                            newObj.MoTa = obj.MoTa;
                            newObj.Gia = obj.Gia;
                            newObj.NhaCungCapID = obj.NhaCungCapID;

                            newObj.NgayTao = DateTime.Now;
                            newObj.NgayCapNhat = newObj.NgayTao;
                            newObj.IsDelete = false;

                            db.tbl_MatHangs.InsertOnSubmit(newObj);
                            db.SubmitChanges();

                            rs.ErrCode = EnumErrCode.Success;
                            rs.Data = newObj.ID;
                            rs.ErrDes = string.Format(Constants.MSG_Insert_Success, tableName);

                            Add_MH_LMH(newObj.ID, listLMH_ID);
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

        public API_Result<bool> Edit(string loginCode, tbl_MatHang obj, int[] listLMH_ID)
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if (curUser != null)
                {
                    tbl_MatHang editObj = db.tbl_MatHangs.Where(u => u.ID.Equals(obj.ID)).FirstOrDefault();
                    if (editObj != null)
                    {
                        if (Authentication.IsSuperAdmin(curUser) || Authentication.IsOwnNCC(curUser.ID, editObj.NhaCungCapID.Value))
                        {
                            tbl_HeThong sameUserNameObj = db.tbl_HeThongs.Where(u => (u.IsDelete == null || u.IsDelete == false) && u.Ten.Equals(obj.Ten)).FirstOrDefault();

                            if (!editObj.Ten.Equals(obj.Ten) && sameUserNameObj != null)
                            {
                                rs.ErrCode = EnumErrCode.AlreadyExist;
                                rs.ErrDes = string.Format(Constants.MSG_Already_Exist, obj.Ten);
                            }
                            else
                            {
                                editObj.Ten = obj.Ten;
                                editObj.Img = obj.Img;
                                editObj.MoTa = obj.MoTa;
                                editObj.Gia = obj.Gia;
                                if (string.IsNullOrEmpty(obj.Img))
                                {
                                    editObj.Img = obj.Img;
                                }
                                editObj.NgayCapNhat = DateTime.Now;

                                db.SubmitChanges();

                                rs.ErrCode = EnumErrCode.Success;
                                rs.Data = true;
                                rs.ErrDes = string.Format(Constants.MSG_Update_Success, tableName);

                                Add_MH_LMH(editObj.ID, listLMH_ID);
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
                        rs.ErrCode = EnumErrCode.DoesNotExist;
                        rs.ErrDes = string.Format(Constants.MSG_Object_Empty, obj.ID);
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
                    tbl_MatHang delObj = db.tbl_MatHangs.Where(u => (u.IsDelete == null || u.IsDelete == false) && u.ID.Equals(id)).FirstOrDefault();
                    if (delObj != null)
                    {
                        if (Authentication.IsSuperAdmin(curUser) || Authentication.IsOwnNCC(curUser.ID, delObj.NhaCungCapID.Value))
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
                    if (Authentication.IsSuperAdmin(curUser) || Authentication.IsNCC(curUser))
                    {
                        int delSuccessCount = 0;
                        int delFailCount = 0;
                        foreach (var id in listID)
                        {
                            tbl_MatHang delObj = db.tbl_MatHangs.Where(u => (u.IsDelete == null || u.IsDelete == false) && u.ID.Equals(id)).FirstOrDefault();
                            try
                            {
                                if(delObj != null && Authentication.IsOwnNCC(curUser.ID, delObj.NhaCungCapID.Value))
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

        public API_Result<List<view_MatHang>> SearchPaging(string loginCode, int nccID, DateTime startTime, DateTime endTime, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
        {
            API_Result<List<view_MatHang>> rs = new API_Result<List<view_MatHang>>();
            try
            {
                if (Authentication.CheckLogin(loginCode))
                {
                    IQueryable<view_MatHang> qrs = db.view_MatHangs.Where(u => (u.IsDelete == null || u.IsDelete == false) && startTime <= u.NgayTao && u.NgayTao < endTime);

                    if(nccID > 0)
                    {
                        qrs = qrs.Where(u => u.NhaCungCapID.Equals(nccID));
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
                            qrs = qrs.Where(u => u.TenNCC.Contains(searchValue));
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

        public API_Result<List<view_MatHang>> SearchPagingHD(string loginCode, int hdID, DateTime startTime, DateTime endTime, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
        {
            API_Result<List<view_MatHang>> rs = new API_Result<List<view_MatHang>>();
            try
            {
                view_HopDong hd = db.view_HopDongs.Where(u => u.ID.Equals(hdID)).First();
                if (Authentication.CheckLogin(loginCode))
                {
                    IQueryable<view_MatHang> qrs = db.view_MatHangs.Where(u => u.NhaCungCapID.Equals(hd.NhaCungCapID) && (startTime <= u.NgayTao && u.NgayTao < endTime) && (u.IsDelete == null || u.IsDelete == false));

                    List<int> listMH_CTHD = db.tbl_ChiTietHDs.Where(u => u.HopDongID.Equals(hd.ID)).Select(u => u.MatHangID).ToList();

                    qrs = qrs.Where(u => !listMH_CTHD.Contains(u.ID));

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
                            qrs = qrs.Where(u => u.TenNCC.Contains(searchValue));
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

        public API_Result<List<view_MH_HT>> SearchPagingHT(string loginCode, int htID, DateTime startTime, DateTime endTime, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
        {
            API_Result<List<view_MH_HT>> rs = new API_Result<List<view_MH_HT>>();
            try
            {
                if (Authentication.CheckLogin(loginCode))
                {
                    IQueryable<view_MH_HT> qrs = db.view_MH_HTs.Where(u => (u.IsDelete == null || u.IsDelete == false) && startTime <= u.NgayTao && u.NgayTao < endTime);

                    if (htID > 0)
                    {
                        qrs = qrs.Where(u => u.HeThongID.Equals(htID));
                    }

                    switch (searchType)
                    {
                        case EnumSearchType.All:
                            //
                            break;
                        case EnumSearchType.ID:
                            qrs = qrs.Where(u => u.MatHangID.Equals(searchValue));
                            break;
                        case EnumSearchType.Name:
                            qrs = qrs.Where(u => u.Ten.Contains(searchValue));
                            break;
                        case EnumSearchType.UserName:
                            qrs = qrs.Where(u => u.TenNCC.Contains(searchValue));
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
                                qrs = qrs.OrderByDescending(u => u.MatHangID);
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
                                qrs = qrs.OrderBy(u => u.MatHangID);
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

        public API_Result<List<ListCombobox_ett<int>>> GetListCombobox(int nccID)
        {
            API_Result<List<ListCombobox_ett<int>>> rs = new API_Result<List<ListCombobox_ett<int>>>();
            try
            {
                IQueryable<view_MatHang> qrs = db.view_MatHangs.Where(u => u.NhaCungCapID.Equals(nccID) && (u.IsDelete == null || u.IsDelete == false) && u.NhaCungCapID.Equals(nccID));

                List<ListCombobox_ett<int>> listCB = new List<ListCombobox_ett<int>>();
                foreach (var item in qrs.ToList())
                {
                    listCB.Add(new ListCombobox_ett<int>(item.ID, item.Ten + " (" + item.TenNCC + ")"));
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

        public API_Result<view_MatHang> GetByID(int id)
        {
            API_Result<view_MatHang> rs = new API_Result<view_MatHang>();
            try
            {
                var obj = db.view_MatHangs.Where(u => u.ID.Equals(id) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
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

        public API_Result<List<ListCombobox_ett<int>>> GetListLMH(int matHangID)
        {
            API_Result<List<ListCombobox_ett<int>>> rs = new API_Result<List<ListCombobox_ett<int>>>();
            try
            {
                IQueryable<view_LoaiMatHang> qrs = (from lmh in db.view_LoaiMatHangs
                                                    join mh_lmh in db.tbl_MH_LMHs on lmh.ID equals mh_lmh.LoaiMatHangID
                                                    where mh_lmh.MatHangID.Equals(matHangID)
                                                    select lmh);

                List<ListCombobox_ett<int>> listCB = new List<ListCombobox_ett<int>>();
                foreach (var item in qrs.ToList())
                {
                    listCB.Add(new ListCombobox_ett<int>(item.ID, item.Ten));
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
        /* */
        public void Add_MH_LMH(int matHangID, int[] listLMH_ID)
        {
            try
            {
                List<tbl_MH_LMH> listDel = db.tbl_MH_LMHs.Where(u => u.MatHangID.Equals(matHangID)).ToList();
                db.tbl_MH_LMHs.DeleteAllOnSubmit(listDel);
                db.SubmitChanges();
            }
            catch(Exception ex)
            {
                foreach(var item in db.GetChangeSet().Deletes)
                {
                    db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, item);
                }
            }

            try
            {
                foreach(var item in listLMH_ID)
                {
                    Add_MH_LMH(matHangID, item);
                }
            }
            catch(Exception ex)
            {
                //
            }
        }

        public void Add_MH_LMH(int matHangID, int loaiMatHangID)
        {
            try
            {
                tbl_MH_LMH link = new tbl_MH_LMH();
                link.MatHangID = matHangID;
                link.LoaiMatHangID = loaiMatHangID;

                db.tbl_MH_LMHs.InsertOnSubmit(link);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                //
            }
        }
    }
}