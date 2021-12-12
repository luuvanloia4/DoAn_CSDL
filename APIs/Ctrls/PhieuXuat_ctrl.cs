using System;
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
    public class PhieuXuat_ctrl
    {
        private string tableName = "Phiếu xuất";

        private DatabaseDataContext db = new DatabaseDataContext();

        public API_Result<bool> IsEmpty()
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                rs.Data = db.view_PhieuXuats.Where(u => u.IsDelete == null || u.IsDelete == false).Count() == 0;
                rs.ErrCode = EnumErrCode.Success;
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        private bool AddCTPX(int pxID, ID_SL cthd)
        {
            try
            {
                tbl_ChiTietPX oldObj = db.tbl_ChiTietPXes.Where(u => u.PhieuXuatID.Equals(pxID) && u.MatHangID.Equals(cthd.ID)).FirstOrDefault();
                if (oldObj != null)
                {
                    oldObj.SoLuong = cthd.SL;
                    db.SubmitChanges();
                    return true;
                }
                else
                {
                    tbl_ChiTietPX newObj = new tbl_ChiTietPX();
                    newObj.PhieuXuatID = pxID;
                    newObj.MatHangID = cthd.ID;
                    newObj.SoLuong = cthd.SL;

                    db.tbl_ChiTietPXes.InsertOnSubmit(newObj);
                    db.SubmitChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                //
            }

            return false;
        }

        private void AddCTPX(int pxID, List<ID_SL> listCTPX)
        {
            foreach (var item in listCTPX)
            {
                AddCTPX(pxID, item);
            }
        }

        public API_Result<bool> DeleteChiTietPX(string loginCode, int pxID, int mhID)
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if (curUser != null)
                {
                    tbl_PhieuXuat editObj = db.tbl_PhieuXuats.Where(u => u.ID.Equals(pxID)).FirstOrDefault();
                    tbl_ChiTietPX ctpx = db.tbl_ChiTietPXes.Where(u => u.PhieuXuatID.Equals(pxID) && u.MatHangID.Equals(mhID)).FirstOrDefault();
                    if (editObj != null && ctpx != null)
                    {
                        if(editObj.TrangThai == 1 || editObj.TrangThai == 2)
                        {
                            rs.ErrCode = EnumErrCode.Fail;
                            rs.ErrDes = "Không thể chỉnh sửa nội dung phiếu xuất đã được phê duyệt hoặc hoàn thành!";
                        }
                        else if (Authentication.IsSuperAdmin(curUser) || Authentication.IsOwnCuaHang(curUser.ID, editObj.CuaHangID.Value))
                        {
                            db.tbl_ChiTietPXes.DeleteOnSubmit(ctpx);
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
                        rs.ErrDes = string.Format(Constants.MSG_Object_Empty, pxID);
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

        public API_Result<int> Create(string loginCode, tbl_PhieuXuat obj, List<ID_SL> listCTPX)
        {
            API_Result<int> rs = new API_Result<int>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if (curUser != null)
                {
                    if (Authentication.IsSuperAdmin(curUser) || Authentication.IsOwnCuaHang(curUser.ID, obj.CuaHangID.Value))
                    {
                        //Admin
                        tbl_PhieuXuat newObj = new tbl_PhieuXuat();
                        newObj.CuaHangID = obj.CuaHangID;
                        newObj.NgayLap = DateTime.Now;

                        newObj.NguoiDuyet = null;
                        newObj.NgayDuyet = null;
                        newObj.NgayHT = null;
                        newObj.TrangThai = 0;
                        newObj.IsDelete = false;

                        db.tbl_PhieuXuats.InsertOnSubmit(newObj);
                        db.SubmitChanges();

                        rs.ErrCode = EnumErrCode.Success;
                        rs.Data = newObj.ID;
                        rs.ErrDes = string.Format(Constants.MSG_Insert_Success, tableName);

                        try
                        {
                            AddCTPX(newObj.ID, listCTPX);
                        }
                        catch(Exception ex)
                        {
                            //Không thêm được chi tiết phiếu xuất thì chịu
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

        public API_Result<bool> Edit(string loginCode, tbl_PhieuXuat obj, List<ID_SL> listCTPX)
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if (curUser != null)
                {
                    tbl_PhieuXuat editObj = db.tbl_PhieuXuats.Where(u => u.ID.Equals(obj.ID)).FirstOrDefault();
                    if (editObj != null)
                    {
                        if (Authentication.IsSuperAdmin(curUser) || Authentication.IsOwnCuaHang(curUser.ID, editObj.CuaHangID.Value))
                        {
                            if (editObj.TrangThai == 1 || editObj.TrangThai == 2)
                            {
                                rs.ErrCode = EnumErrCode.Fail;
                                rs.ErrDes = "Không thể chỉnh sửa nội dung phiếu xuất đã được phê duyệt hoặc hoàn thành!";
                            }
                            else
                            {
                                AddCTPX(editObj.ID, listCTPX);

                                rs.ErrCode = EnumErrCode.Success;
                                rs.Data = true;
                                rs.ErrDes = string.Format(Constants.MSG_Update_Success, tableName);
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
                    tbl_PhieuXuat delObj = db.tbl_PhieuXuats.Where(u => (u.IsDelete == null || u.IsDelete == false) && u.ID.Equals(id)).FirstOrDefault();
                    if (delObj != null)
                    {
                        if (Authentication.IsSuperAdmin(curUser) || Authentication.IsOwnCuaHang(curUser.ID, delObj.CuaHangID.Value))
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
                    if (Authentication.IsSuperAdmin(curUser) || Authentication.IsCuaHang(curUser))
                    {
                        int delSuccessCount = 0;
                        int delFailCount = 0;
                        foreach (var id in listID)
                        {
                            tbl_PhieuXuat delObj = db.tbl_PhieuXuats.Where(u => (u.IsDelete == null || u.IsDelete == false) && u.ID.Equals(id)).FirstOrDefault();
                            try
                            {
                                if (Authentication.IsSuperAdmin(curUser) || Authentication.IsOwnCuaHang(curUser.ID, delObj.CuaHangID.Value))
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

        public API_Result<List<view_PhieuXuat>> SearchPaging(string loginCode, int htID, int chID, int trangThai, DateTime startTime, DateTime endTime, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
        {
            API_Result<List<view_PhieuXuat>> rs = new API_Result<List<view_PhieuXuat>>();
            try
            {
                if (Authentication.CheckLogin(loginCode))
                {
                    IQueryable<view_PhieuXuat> qrs = db.view_PhieuXuats.Where(u => (u.IsDelete == null || u.IsDelete == false) && startTime <= u.NgayLap && u.NgayLap < endTime);

                    if (chID > 0)
                    {
                        qrs = qrs.Where(u => u.CuaHangID.Equals(chID));
                    }
                    else if (htID > 0)
                    {
                        List<int> listCHID = db.tbl_CuaHangs.Where(u => u.HeThongID.Equals(htID)).Select(u => u.ID).ToList();
                        qrs = qrs.Where(u => listCHID.Contains(u.CuaHangID.Value));
                    }

                    if(trangThai >= 0)
                    {
                        qrs = qrs.Where(u => u.TrangThai.Equals(trangThai));
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
                            qrs = qrs.Where(u => u.TenCH.Contains(searchValue));
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
                IQueryable<view_PhieuXuat> qrs = db.view_PhieuXuats.Where(u => (u.IsDelete == null || u.IsDelete == false));

                List<ListCombobox_ett<int>> listCB = new List<ListCombobox_ett<int>>();
                foreach (var item in qrs.ToList())
                {
                    listCB.Add(new ListCombobox_ett<int>(item.ID, item.ID + " (" + item.TenCH + ")"));
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

        public API_Result<view_PhieuXuat> GetByID(int id)
        {
            API_Result<view_PhieuXuat> rs = new API_Result<view_PhieuXuat>();
            try
            {
                var obj = db.view_PhieuXuats.Where(u => u.ID.Equals(id) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
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

        //public API_Result<float> GetProgress(int id)
        //{
        //    API_Result<float> rs = new API_Result<float>();
        //    try
        //    {
        //        var procResult = db.pr_HopDong_GetProgress(id);
        //        int tongMH = 0, tongMHDaGiao = 0;
        //        foreach (var item in procResult)
        //        {
        //            tongMH = (item.TongMatHang == null) ? 0 : item.TongMatHang.Value;
        //            tongMHDaGiao = (item.TongMatHangDaGiao == null) ? 0 : item.TongMatHangDaGiao.Value;
        //        }

        //        if (tongMH > 0)
        //        {
        //            rs.Data = (float)Math.Round((double)tongMHDaGiao * 100 / tongMH, 2);
        //        }
        //        else
        //        {
        //            rs.Data = 0;
        //        }

        //        rs.ErrCode = EnumErrCode.Success;
        //    }
        //    catch (Exception ex)
        //    {
        //        rs.ErrCode = EnumErrCode.Error;
        //        rs.ErrDes = ex.Message;
        //    }

        //    return rs;
        //}

        public API_Result<List<view_ChiTietPX>> GetListCTPX(string loginCode, int pxID)
        {
            API_Result<List<view_ChiTietPX>> rs = new API_Result<List<view_ChiTietPX>>();
            try
            {
                var curUser = Authentication.GetUser(loginCode);
                if (curUser != null)
                {
                    IQueryable<view_ChiTietPX> qrs = db.view_ChiTietPXes.Where(u => u.PhieuXuatID.Equals(pxID));
                    if (qrs.Count() > 0)
                    {
                        rs.Data = qrs.ToList();
                        rs.ErrCode = EnumErrCode.Success;
                    }
                    else
                    {
                        rs.ErrCode = EnumErrCode.Empty;
                        rs.ErrDes = string.Format(Constants.MSG_Search_Empty, pxID);
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

        //Exten Method
        public API_Result<bool> PheDuyet(string loginCode, int pxID)
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                var curUser = Authentication.GetUser(loginCode).Data;
                tbl_PhieuXuat editObj = db.tbl_PhieuXuats.Where(u => u.ID.Equals(pxID) && u.TrangThai.Equals(0) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
                if(editObj == null)
                {
                    rs.ErrCode = EnumErrCode.Empty;
                    rs.ErrDes = string.Format(Constants.MSG_Object_Empty, pxID);
                }
                else if(curUser == null)
                {
                    rs.ErrCode = EnumErrCode.NotYetLogin;
                    rs.ErrDes = Constants.MSG_Not_Login;
                }
                else
                {
                    tbl_CuaHang curCH = db.tbl_CuaHangs.Where(u => u.ID.Equals(editObj.ID) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
                    if (Authentication.IsSuperAdmin(loginCode) || Authentication.IsOwnHeThong(curUser.ID, curCH.HeThongID.Value)){
                        //Check hàng trong kho
                        List<tbl_ChiTietPX> listCTPX = db.tbl_ChiTietPXes.Where(u => u.PhieuXuatID.Equals(pxID)).ToList();
                        bool isEnough = true;
                        foreach(var item in listCTPX)
                        {
                            if (isEnough)
                            {
                                tbl_MH_HT checkObj = db.tbl_MH_HTs.Where(u => u.HeThongID.Equals(curCH.HeThongID)).FirstOrDefault();
                                if(checkObj != null)
                                {
                                    isEnough = item.SoLuong <= checkObj.SoLuong;
                                }
                                else
                                {
                                    isEnough = false;
                                }
                            }
                        }

                        if (isEnough)
                        {
                            editObj.TrangThai = 1; //Phe duyet
                            rs.ErrCode = EnumErrCode.Success;
                        }
                        else
                        {
                            rs.ErrCode = EnumErrCode.Fail;
                            rs.ErrDes = "Hệ thống hiện không đủ mặt hàng để đáp ứng yêu cầu xuất hàng, vui lòng nhập thêm hàng và thử lại!";
                        }
                    }
                    else
                    {
                        rs.ErrCode = EnumErrCode.PermissionDenied;
                        rs.ErrDes = Constants.MSG_Permission_Denied;
                    }
                }
            }
            catch(Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<bool> HoanThanh(string loginCode, int pxID)
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                var curUser = Authentication.GetUser(loginCode).Data;
                tbl_PhieuXuat editObj = db.tbl_PhieuXuats.Where(u => u.ID.Equals(pxID) && u.TrangThai.Equals(1) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
                if (editObj == null)
                {
                    rs.ErrCode = EnumErrCode.Empty;
                    rs.ErrDes = string.Format(Constants.MSG_Object_Empty, pxID);
                }
                else if (curUser == null)
                {
                    rs.ErrCode = EnumErrCode.NotYetLogin;
                    rs.ErrDes = Constants.MSG_Not_Login;
                }
                else if(Authentication.IsSuperAdmin(loginCode) || Authentication.IsOwnCuaHang(curUser.ID, editObj.CuaHangID.Value))
                    {
                    editObj.TrangThai = 2; //Hoan thanh
                    rs.ErrCode = EnumErrCode.Success;
                }
                else
                {
                    rs.ErrCode = EnumErrCode.PermissionDenied;
                    rs.ErrDes = Constants.MSG_Permission_Denied;
                }
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