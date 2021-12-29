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
                    if(oldObj.SoLuongDaGiao > cthd.SL)
                    {
                        return false;
                    }

                    oldObj.SoLuong = cthd.SL;
                    db.SubmitChanges();
                }
                else
                {
                    tbl_ChiTietHD newObj = new tbl_ChiTietHD();
                    newObj.HopDongID = hdID;
                    newObj.MatHangID = cthd.ID;
                    newObj.SoLuong = cthd.SL;
                    newObj.SoLuongDaGiao = 0;

                    db.tbl_ChiTietHDs.InsertOnSubmit(newObj);
                    db.SubmitChanges();
                }

                tbl_HopDong hopDong = db.tbl_HopDongs.Where(u => u.ID.Equals(hdID)).FirstOrDefault();
                int htID = (hopDong == null) ? -1 : hopDong.HeThongID.Value;

                if (htID > 0)
                {
                    tbl_MH_HT mh_htObj = db.tbl_MH_HTs.Where(u => u.HeThongID.Equals(htID) && u.MatHangID.Equals(cthd.ID)).FirstOrDefault();
                    if (mh_htObj != null)
                    {
                        try
                        {
                            mh_htObj = new tbl_MH_HT();
                            mh_htObj.HeThongID = htID;
                            mh_htObj.MatHangID = cthd.ID;
                            mh_htObj.SoLuong = 0;

                            db.tbl_MH_HTs.InsertOnSubmit(mh_htObj);
                            db.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            //Không thêm đc thì chịu
                        }
                    }
                }

                return true;
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

        public API_Result<bool> AddChiTietHD(string loginCode, int hdID, ID_SL cthd)
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if (curUser != null)
                {
                    tbl_HopDong editObj = db.tbl_HopDongs.Where(u => u.ID.Equals(hdID)).FirstOrDefault();
                   if (Authentication.IsSuperAdmin(curUser) || editObj.NguoiLap.Equals(curUser.ID))
                    {
                        if (editObj != null)
                        {
                            if (editObj.TrangThai == 1)
                            {
                                rs.ErrCode = EnumErrCode.Fail;
                                rs.ErrDes = "Không thể chỉnh sửa nội dung hợp đồng đã hoàn thành!";
                            }
                            else
                            {
                                rs.ErrCode = EnumErrCode.Success;
                                rs.Data = AddCTHD(editObj.ID, cthd);
                                rs.ErrDes = string.Format(Constants.MSG_Update_Success, tableName);
                            }
                        }
                        else
                        {
                            rs.ErrCode = EnumErrCode.DoesNotExist;
                            rs.ErrDes = string.Format(Constants.MSG_Object_Empty, hdID);
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

        public API_Result<string> UpdateChiTietHD(string loginCode, int hdID, List<ID_SL> listCTHD)
        {
            API_Result<string> rs = new API_Result<string>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if (curUser != null)
                {
                    tbl_HopDong editObj = db.tbl_HopDongs.Where(u => u.ID.Equals(hdID)).FirstOrDefault();
                    if (Authentication.IsSuperAdmin(curUser) || editObj.NguoiLap.Equals(curUser.ID))
                    {
                        if (editObj != null)
                        {
                            if (editObj.TrangThai == 1)
                            {
                                rs.ErrCode = EnumErrCode.Fail;
                                rs.ErrDes = "Không thể chỉnh sửa nội dung hợp đồng đã hoàn thành!";
                            }
                            else
                            {
                                int updateSuccessCount = 0;
                                int updateFailCount = 0;
                                foreach (var cthd in listCTHD)
                                {
                                    if (AddCTHD(hdID, cthd))
                                    {
                                        updateSuccessCount++;
                                    }
                                    else
                                    {
                                        updateFailCount++;
                                    }
                                }

                                rs.ErrCode = EnumErrCode.Success;
                                rs.Data = "Cập nhật thành công " + updateSuccessCount.ToString() + " trên tổng số " + (updateSuccessCount + updateFailCount).ToString() + " bản ghi!";
                            }
                        }
                        else
                        {
                            rs.ErrCode = EnumErrCode.DoesNotExist;
                            rs.ErrDes = string.Format(Constants.MSG_Object_Empty, hdID);
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

        public API_Result<bool> DeleteChiTietHD(string loginCode, int hdID, int mhID)
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if (curUser != null)
                {
                    tbl_HopDong editObj = db.tbl_HopDongs.Where(u => u.ID.Equals(hdID)).FirstOrDefault();
                    tbl_ChiTietHD cthd = db.tbl_ChiTietHDs.Where(u => u.HopDongID.Equals(hdID) && u.MatHangID.Equals(mhID)).FirstOrDefault();
                    if (editObj != null && cthd != null)
                    {
                        if (Authentication.IsSuperAdmin(curUser) || editObj.NguoiLap.Equals(curUser.ID))
                        {
                            if (editObj.TrangThai == 1)
                            {
                                rs.ErrCode = EnumErrCode.Fail;
                                rs.ErrDes = "Không thể chỉnh sửa nội dung hợp đồng đã hoàn thành!";
                            }
                            else if (cthd.SoLuongDaGiao == null || cthd.SoLuongDaGiao == 0)
                            {
                                db.tbl_ChiTietHDs.DeleteOnSubmit(cthd);
                                db.SubmitChanges();

                                rs.ErrCode = EnumErrCode.Success;
                                rs.Data = true;
                                rs.ErrDes = string.Format(Constants.MSG_Delete_Success, tableName);
                            }
                            else
                            {
                                rs.ErrCode = EnumErrCode.AlreadyExist;
                                rs.ErrDes = "Không thể xóa chi tiết hợp đồng đã giao!";
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
                        rs.ErrDes = string.Format(Constants.MSG_Object_Empty, hdID);
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

        public API_Result<int> Create(string loginCode, tbl_HopDong obj)
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
                        newObj.HeThongID = obj.HeThongID;
                        newObj.NgayLap = DateTime.Now;
                        newObj.NgayHT = null;
                        newObj.TrangThai = 0;
                        newObj.IsDelete = false;

                        db.tbl_HopDongs.InsertOnSubmit(newObj);
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

        public API_Result<List<view_HopDong>> SearchPaging(string loginCode, int htID, int nccID, int trangThai, DateTime startTime, DateTime endTime, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
        {
            API_Result<List<view_HopDong>> rs = new API_Result<List<view_HopDong>>();
            try
            {
                if (Authentication.CheckLogin(loginCode))
                {
                    IQueryable<view_HopDong> qrs = db.view_HopDongs.Where(u => (u.IsDelete == null || u.IsDelete == false) && startTime <= u.NgayLap && u.NgayLap < endTime);

                    if(htID > 0)
                    {
                        qrs = qrs.Where(u => u.HeThongID.Equals(htID));
                    }

                    if(nccID > 0)
                    {
                        qrs = qrs.Where(u => u.HeThongID.Equals(nccID));
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
                            qrs = qrs.Where(u => u.TenNCC.Contains(searchValue));
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
                    listCB.Add(new ListCombobox_ett<int>(item.ID, item.ID + " (" + item.UserName + " - " + item.TenNCC + ")"));
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

        public API_Result<float> GetProgress(int id)
        {
            API_Result<float> rs = new API_Result<float>();
            try
            {
                var procResult = db.pr_HopDong_GetProgress(id);
                int tongMH = 0, tongMHDaGiao = 0;
                foreach(var item in procResult)
                {
                    tongMH = (item.TongMatHang == null)? 0 : item.TongMatHang.Value;
                    tongMHDaGiao = (item.TongMatHangDaGiao == null)? 0 : item.TongMatHangDaGiao.Value;
                }

                if(tongMH > 0)
                {
                    rs.Data = (float)Math.Round((double)tongMHDaGiao * 100 / tongMH, 2);
                }
                else
                {
                    rs.Data = 0;
                }

                rs.ErrCode = EnumErrCode.Success;
            }
            catch(Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<List<view_ChiTietHD>> GetListCTHD(string loginCode, int hdID)
        {
            API_Result<List<view_ChiTietHD>> rs = new API_Result<List<view_ChiTietHD>>();
            try
            {
                var curUser = Authentication.GetUser(loginCode);
                if(curUser != null)
                {
                    IQueryable<view_ChiTietHD> qrs = db.view_ChiTietHDs.Where(u => u.HopDongID.Equals(hdID) && (u.IsDelete == null || u.IsDelete == false));
                    if(qrs.Count() > 0)
                    {
                        rs.Data = qrs.ToList();
                    }
                    else
                    {
                        rs.Data = new List<view_ChiTietHD>();
                    }

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

        // Phieu nhap
        private bool AddCTPN(int pnID, ID_SL ctpn)
        {
            try
            {
                tbl_ChiTietPN oldObj = db.tbl_ChiTietPNs.Where(u => u.PhieuNhapID.Equals(pnID) && u.MatHangID.Equals(ctpn.ID)).FirstOrDefault();
                if (oldObj != null)
                {
                    oldObj.SoLuong = ctpn.SL;
                    db.SubmitChanges();
                }
                else
                {
                    tbl_ChiTietPN newObj = new tbl_ChiTietPN();
                    newObj.PhieuNhapID = pnID;
                    newObj.MatHangID = ctpn.ID;
                    newObj.SoLuong = ctpn.SL;
                    newObj.ViTriID = -1; //Chưa code đoạn select vị trí

                    db.tbl_ChiTietPNs.InsertOnSubmit(newObj);
                    db.SubmitChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                //
            }

            return false;
        }

        private void AddCTPN(int pnID, List<ID_SL> listCTPN)
        {
            foreach (var item in listCTPN)
            {
                AddCTPN(pnID, item);
            }
        }

        public API_Result<int> CreatePhieuNhap(string loginCode, tbl_PhieuNhap obj, List<ID_SL> listCTPN)
        {
            API_Result<int> rs = new API_Result<int>();
            try
            {
                var curUser = Authentication.GetUser(loginCode).Data;
                if(curUser != null)
                {
                    bool isFull = false;

                    foreach(var item in listCTPN)
                    {
                        tbl_ChiTietHD cthd = db.tbl_ChiTietHDs.Where(u => u.HopDongID.Equals(obj.HopDongID) && u.MatHangID.Equals(item.ID)).FirstOrDefault();
                        if(isFull && (cthd == null || (cthd.SoLuongDaGiao + item.SL) > cthd.SoLuong)) //Nhập quá số lượng
                        {
                            isFull = true;
                        }
                    }

                    if (isFull)
                    {
                        rs.ErrCode = EnumErrCode.Fail;
                        rs.ErrDes = "Không thể nhập quá số lượng mặt hàng trong chi tiết hợp đồng!";
                    }
                    else if(Authentication.IsAdmin(curUser) || (curUser.PhanQuyenID == Authentication.qNhanVien))
                    {
                        tbl_PhieuNhap newObj = new tbl_PhieuNhap();
                        newObj.HopDongID = obj.HopDongID;
                        newObj.NguoiGiao = obj.NguoiGiao;
                        newObj.TrangThai = true;
                        newObj.IsDelete = false;
                        newObj.NguoiLap = curUser.ID;
                        newObj.NgayLap = DateTime.Now;

                        db.tbl_PhieuNhaps.InsertOnSubmit(newObj);
                        db.SubmitChanges();

                        try
                        {
                            AddCTPN(newObj.ID, listCTPN);
                        }
                        catch(Exception ex)
                        {
                            //Không tạo được chi tiết phiếu nhập thì chịu
                        }

                        rs.ErrCode = EnumErrCode.Success;
                        rs.Data = newObj.ID;
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

        public API_Result<bool> DeletePhieuNhap(string loginCode, int pnID)
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                var curUser = Authentication.GetUser(loginCode).Data;
                if(curUser != null)
                {
                    if (Authentication.IsAdmin(curUser))
                    {
                        tbl_PhieuNhap delObj = db.tbl_PhieuNhaps.Where(u => u.ID.Equals(pnID)).FirstOrDefault();
                        if(delObj != null)
                        {
                            db.tbl_PhieuNhaps.DeleteOnSubmit(delObj);
                            db.SubmitChanges();
                            //Delete all CTPN with trigger

                            rs.ErrCode = EnumErrCode.Success;
                        }
                        else
                        {
                            rs.ErrCode = EnumErrCode.Empty;
                            rs.ErrDes = string.Format(Constants.MSG_Object_Empty, pnID);
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

        public API_Result<List<view_PhieuNhap>> GetListPhieuNhap(int hdID)
        {
            API_Result<List<view_PhieuNhap>> rs = new API_Result<List<view_PhieuNhap>>();
            try
            {
                IQueryable<view_PhieuNhap> qrs = db.view_PhieuNhaps.Where(u => u.HopDongID.Equals(hdID) && (u.IsDelete == null || u.IsDelete == false));
                rs.RecordCount = qrs.Count();
                if (rs.RecordCount > 0)
                {
                    rs.Data = qrs.ToList();
                    rs.ErrCode = EnumErrCode.Success;
                }
                else
                {
                    rs.ErrCode = EnumErrCode.Empty;
                }
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public API_Result<view_PhieuNhap> GetPhieuNhap(int pnID)
        {
            API_Result<view_PhieuNhap> rs = new API_Result<view_PhieuNhap>();
            try
            {
                view_PhieuNhap obj = db.view_PhieuNhaps.Where(u => u.ID.Equals(pnID) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
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

        public API_Result<List<view_ChiTietPN>> GetListCTPN(int pnID)
        {
            API_Result<List<view_ChiTietPN>> rs = new API_Result<List<view_ChiTietPN>>();
            try
            {
                IQueryable<view_ChiTietPN> qrs = db.view_ChiTietPNs.Where(u => u.PhieuNhapID.Equals(pnID));
                rs.RecordCount = qrs.Count();
                if(rs.RecordCount > 0)
                {
                    rs.Data = qrs.ToList();
                    rs.ErrCode = EnumErrCode.Success;
                }
                else
                {
                    rs.ErrCode = EnumErrCode.Empty;
                    rs.ErrDes = string.Format(Constants.MSG_Object_Empty, pnID);
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