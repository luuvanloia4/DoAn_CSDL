//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using API.Models;
//using API.Models.Entity;
//using API.Shareds;

//namespace API.Ctrls
//{
//    public class NCC_ctrl
//    {
//        private dbQuanLyKhoDataContext db = new dbQuanLyKhoDataContext();

//        public string IncMaNCC(string maNCC)
//        {
//            int STT;
//            try
//            {
//                STT = int.Parse(maNCC.Substring(3));
//            }
//            catch
//            {
//                STT = 0;
//            }
//            STT++;

//            return "NCC" + STT.ToString();
//        }

//        public
//
//        <bool> Create(string loginCode, tbl_NhaCungCap obj)
//        {
//            API_Result<bool> rs = new API_Result<bool>();
//            try
//            {
//                tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
//                if (curUser != null)
//                {
//                    if (curUser.PhanQuyen == 1 || (curUser.PhanQuyen == 3 && curUser.MaTK == obj.DaiDien) || curUser.PhanQuyen == 4)
//                    {
//                        //Admin hoac NCC
//                        string id = "NCC0";
//                        var lastRecord = db.tbl_NhaCungCaps.OrderByDescending(u => u.Update).FirstOrDefault();
//                        if (lastRecord != null)
//                        {
//                            id = lastRecord.MaNCC;
//                        }

//                        tbl_NhaCungCap newObj = new tbl_NhaCungCap();
//                        newObj.MaNCC = IncMaNCC(id);
//                        newObj.TenNCC = obj.TenNCC;
//                        newObj.DiaChi = obj.DiaChi;
//                        newObj.SDT = obj.SDT;
//                        newObj.STK = obj.STK;
//                        newObj.NganHang = obj.NganHang;
//                        newObj.DaiDien = obj.DaiDien;
//                        newObj.IsDelete = false;
//                        newObj.Update = DateTime.Now;

//                        try
//                        {
//                            db.tbl_NhaCungCaps.InsertOnSubmit(newObj);
//                            db.SubmitChanges();

//                            rs.ErrCode = EnumErrCode.Success;
//                            rs.ErrDes = newObj.MaNCC;
//                            rs.Data = true;
//                        }
//                        catch (Exception ex)
//                        {
//                            rs.ErrCode = EnumErrCode.Error;
//                            rs.ErrDes = ex.Message;
//                        }
//                    }
//                    else
//                    {
//                        rs.ErrCode = EnumErrCode.PermissionDenied;
//                        rs.ErrDes = Constants.MSG_Permission_Denied;
//                    }
//                }
//                else
//                {
//                    rs.ErrCode = EnumErrCode.NotYetLogin;
//                }
//            }
//            catch (Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }

//        public API_Result<bool> Edit(string loginCode, tbl_NhaCungCap obj)
//        {
//            API_Result<bool> rs = new API_Result<bool>();
//            try
//            {
//                tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
//                if (curUser != null)
//                {
//                    tbl_NhaCungCap editObj = db.tbl_NhaCungCaps.Where(u => u.MaNCC.Equals(obj.MaNCC)).FirstOrDefault();
//                    if (editObj != null)
//                    {
//                        if (curUser.PhanQuyen == 1 || (curUser.PhanQuyen == 3 && curUser.MaTK == editObj.DaiDien) || curUser.PhanQuyen == 4)
//                        {
//                            //Admin hoac NCC
//                            editObj.TenNCC = obj.TenNCC;
//                            editObj.DiaChi = obj.DiaChi;
//                            editObj.SDT = obj.SDT;
//                            editObj.STK = obj.STK;
//                            editObj.NganHang = obj.NganHang;
//                            editObj.DaiDien = obj.DaiDien;
//                            editObj.IsDelete = obj.IsDelete;
//                            editObj.Update = DateTime.Now;

//                            try
//                            {
//                                db.SubmitChanges();

//                                rs.ErrCode = EnumErrCode.Success;
//                                rs.Data = true;
//                            }
//                            catch(Exception ex)
//                            {
//                                rs.ErrCode = EnumErrCode.Error;
//                                rs.ErrDes = ex.Message;
//                            }
//                        }
//                        else
//                        {
//                            rs.ErrCode = EnumErrCode.PermissionDenied;
//                            rs.ErrDes = Constants.MSG_Permission_Denied;
//                        }
//                    }
//                    else
//                    {
//                        rs.ErrCode = EnumErrCode.Empty;
//                        rs.ErrDes = "Bản ghi không tồn tại!";
//                    }
//                }
//                else
//                {
//                    rs.ErrCode = EnumErrCode.NotYetLogin;
//                }
//            }
//            catch (Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }
//        public API_Result<string> Delete(string loginCode, string[] listID)
//        {
//            API_Result<string> rs = new API_Result<string>();
//            try
//            {
//                tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
//                if (curUser != null)
//                {
//                    if (curUser.PhanQuyen == 1 || curUser.PhanQuyen == 4)
//                    {
//                        int delSuccessCount = 0;
//                        int delFailCount = 0;
//                        foreach (var id in listID)
//                        {
//                            tbl_NhaCungCap delObj = db.tbl_NhaCungCaps.Where(u => u.MaNCC.Equals(id) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
//                            try
//                            {
//                                delObj.IsDelete = true;
//                                db.SubmitChanges();

//                                delSuccessCount++;
//                            }
//                            catch
//                            {
//                                foreach (var change in db.GetChangeSet().Updates)
//                                {
//                                    db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, change);
//                                    delFailCount++;
//                                }
//                            }
//                        }

//                        rs.ErrCode = EnumErrCode.Success;
//                        rs.ErrDes = "Xóa thành công " + delSuccessCount.ToString() + " trên tổng số " + (delSuccessCount + delFailCount).ToString() + " bản ghi!";
//                    }
//                    else
//                    {
//                        rs.ErrCode = EnumErrCode.PermissionDenied;
//                        rs.ErrDes = Constants.MSG_Permission_Denied;
//                    }
//                }
//                else
//                {
//                    rs.ErrCode = EnumErrCode.NotYetLogin;
//                }
//            }
//            catch (Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }

//        public API_Result<List<ListCombobox_ett<string>>> GetListCombobox(string loginCode)
//        {
//            API_Result<List<ListCombobox_ett<string>>> rs = new API_Result<List<ListCombobox_ett<string>>>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    IQueryable<tbl_NhaCungCap> qrs = db.tbl_NhaCungCaps.Where(u => u.IsDelete == null || u.IsDelete == false);

//                    var list = qrs.ToList();

//                    List<ListCombobox_ett<string>> listCB = new List<ListCombobox_ett<string>>();
//                    foreach (var item in list)
//                    {
//                        listCB.Add(new ListCombobox_ett<string>(item.MaNCC, item.TenNCC + " - " + item.MaNCC));
//                    }

//                    rs.Data = listCB;
//                    rs.RecordCount = listCB.Count();
//                    rs.ErrCode = EnumErrCode.Success;
//                }
//                else
//                {
//                    rs.ErrCode = EnumErrCode.NotYetLogin;
//                }
//            }
//            catch (Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }

//        public API_Result<List<tbl_NhaCungCap>> SearchPaging(string loginCode, DateTime startTime, DateTime endTime, int status = -1, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
//        {
//            API_Result<List<tbl_NhaCungCap>> rs = new API_Result<List<tbl_NhaCungCap>>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    if(db.tbl_NhaCungCaps.Count() > 0)
//                    {
//                        IQueryable<tbl_NhaCungCap> qrs = null;
//                        IQueryable<tbl_NhaCungCap> qr = null;

//                        switch (searchType)
//                        {
//                            case EnumSearchType.All:
//                                qrs = db.tbl_NhaCungCaps.Where(u => u.IsDelete == null || u.IsDelete == false);
//                                break;
//                            case EnumSearchType.ID:
//                                qrs = db.tbl_NhaCungCaps.Where(u => u.MaNCC.Equals(searchValue) && (u.IsDelete == null || u.IsDelete == false));
//                                break;
//                            case EnumSearchType.Name:
//                                qrs = db.tbl_NhaCungCaps.Where(u => u.TenNCC.Contains(searchValue) && (u.IsDelete == null || u.IsDelete == false));
//                                break;
//                            case EnumSearchType.Phone:
//                                qrs = db.tbl_NhaCungCaps.Where(u => u.SDT.Equals(searchValue) && (u.IsDelete == null || u.IsDelete == false));
//                                break;
//                            case EnumSearchType.ListAll:
//                                qrs = db.tbl_NhaCungCaps;
//                                break;
//                        }

//                        qrs = qrs.Where(u => startTime <= u.Update && u.Update <= endTime);

//                        if (status != -1)
//                        {
//                            //
//                        }

//                        if (qrs.Count() > 0)
//                        {
//                            if (isDescending)
//                            {
//                                switch (orderBy)
//                                {
//                                    case EnumOrderBy.Newest:
//                                        qrs = qrs.OrderByDescending(u => u.Update);
//                                        break;
//                                    case EnumOrderBy.ID:
//                                        qrs = qrs.OrderByDescending(u => u.MaNCC);
//                                        break;
//                                    case EnumOrderBy.Name:
//                                        qrs = qrs.OrderByDescending(u => u.TenNCC);
//                                        break;
//                                }
//                            }
//                            else
//                            {
//                                switch (orderBy)
//                                {
//                                    case EnumOrderBy.Newest:
//                                        qrs = qrs.OrderBy(u => u.Update);
//                                        break;
//                                    case EnumOrderBy.ID:
//                                        qrs = qrs.OrderBy(u => u.MaNCC);
//                                        break;
//                                    case EnumOrderBy.Name:
//                                        qrs = qrs.OrderBy(u => u.TenNCC);
//                                        break;
//                                }
//                            }

//                            rs.RecordCount = qrs.Count();
//                            rs.PageCount = rs.RecordCount / pageSize;
//                            if (rs.RecordCount % pageSize != 0)
//                            {
//                                rs.PageCount++;
//                            }
//                            qr = qrs.Skip((curPage - 1) * pageSize).Take(pageSize);
//                            rs.Data = qr.ToList();
//                            rs.ErrCode = EnumErrCode.Success;
//                        }
//                        else
//                        {
//                            rs.ErrCode = EnumErrCode.Empty;
//                        }
//                    }
//                    else
//                    {
//                        rs.ErrCode = EnumErrCode.DoesNotExist;
//                    }
//                }
//                else
//                {
//                    rs.ErrCode = EnumErrCode.NotYetLogin;
//                }
//            }
//            catch (Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }

//        public API_Result<List<NhaCungCap_ett>> ExportData(string loginCode, DateTime startTime, DateTime endTime, int status = -1, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
//        {
//            API_Result<List<NhaCungCap_ett>> rs = new API_Result<List<NhaCungCap_ett>>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    if (db.tbl_NhaCungCaps.Count() > 0)
//                    {
//                        IQueryable<tbl_NhaCungCap> qrs = null;
//                        IQueryable<tbl_NhaCungCap> qr = null;

//                        switch (searchType)
//                        {
//                            case EnumSearchType.All:
//                                qrs = db.tbl_NhaCungCaps.Where(u => u.IsDelete == null || u.IsDelete == false);
//                                break;
//                            case EnumSearchType.ID:
//                                qrs = db.tbl_NhaCungCaps.Where(u => u.MaNCC.Equals(searchValue) && (u.IsDelete == null || u.IsDelete == false));
//                                break;
//                            case EnumSearchType.Name:
//                                qrs = db.tbl_NhaCungCaps.Where(u => u.TenNCC.Contains(searchValue) && (u.IsDelete == null || u.IsDelete == false));
//                                break;
//                            case EnumSearchType.Phone:
//                                qrs = db.tbl_NhaCungCaps.Where(u => u.SDT.Equals(searchValue) && (u.IsDelete == null || u.IsDelete == false));
//                                break;
//                            case EnumSearchType.ListAll:
//                                qrs = db.tbl_NhaCungCaps;
//                                break;
//                        }

//                        qrs = qrs.Where(u => startTime <= u.Update && u.Update <= endTime);

//                        if (status != -1)
//                        {
//                            //
//                        }

//                        if (qrs.Count() > 0)
//                        {
//                            if (isDescending)
//                            {
//                                switch (orderBy)
//                                {
//                                    case EnumOrderBy.Newest:
//                                        qrs = qrs.OrderByDescending(u => u.Update);
//                                        break;
//                                    case EnumOrderBy.ID:
//                                        qrs = qrs.OrderByDescending(u => u.MaNCC);
//                                        break;
//                                    case EnumOrderBy.Name:
//                                        qrs = qrs.OrderByDescending(u => u.TenNCC);
//                                        break;
//                                }
//                            }
//                            else
//                            {
//                                switch (orderBy)
//                                {
//                                    case EnumOrderBy.Newest:
//                                        qrs = qrs.OrderBy(u => u.Update);
//                                        break;
//                                    case EnumOrderBy.ID:
//                                        qrs = qrs.OrderBy(u => u.MaNCC);
//                                        break;
//                                    case EnumOrderBy.Name:
//                                        qrs = qrs.OrderBy(u => u.TenNCC);
//                                        break;
//                                }
//                            }

//                            rs.RecordCount = qrs.Count();
//                            qr = qrs;
//                            var list = qr.ToList();
//                            List<NhaCungCap_ett> listNCC = new List<NhaCungCap_ett>();
//                            foreach(var item in list)
//                            {
//                                listNCC.Add(new NhaCungCap_ett(item));
//                            }
//                            rs.Data = listNCC;
//                            rs.ErrCode = EnumErrCode.Success;
//                        }
//                        else
//                        {
//                            rs.ErrCode = EnumErrCode.Empty;
//                        }
//                    }
//                    else
//                    {
//                        rs.ErrCode = EnumErrCode.DoesNotExist;
//                    }
//                }
//                else
//                {
//                    rs.ErrCode = EnumErrCode.NotYetLogin;
//                }
//            }
//            catch (Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }

//        public API_Result<bool> IsEmpty(string loginCode)
//        {
//            API_Result<bool> rs = new API_Result<bool>();
//            try
//            {
//                tbl_NhaCungCap ncc = db.tbl_NhaCungCaps.Where(n => n.IsDelete == null || n.IsDelete == false).FirstOrDefault();

//                rs.Data = ncc == null;
//                rs.ErrCode = EnumErrCode.Success;
//            }
//            catch(Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }

//        public API_Result<tbl_NhaCungCap> Get(string loginCode, string id)
//        {
//            API_Result<tbl_NhaCungCap> rs = new API_Result<tbl_NhaCungCap>();
//            try
//            {
//                tbl_NhaCungCap obj = db.tbl_NhaCungCaps.Where(u => u.MaNCC.Equals(id)).FirstOrDefault();
//                if(obj != null)
//                {
//                    rs.Data = obj;
//                    rs.ErrCode = EnumErrCode.Success;
//                }
//                else
//                {
//                    rs.ErrCode = EnumErrCode.Empty;
//                }
//            }
//            catch (Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }

//        public API_Result<NhaCungCap_ett> GetAll(string loginCode, string id)
//        {
//            API_Result<NhaCungCap_ett> rs = new API_Result<NhaCungCap_ett>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    rs.ErrCode = EnumErrCode.Success;
//                    rs.Data = new NhaCungCap_ett(id);
//                }
//                else
//                {
//                    rs.ErrCode = EnumErrCode.NotYetLogin;
//                }
//            }
//            catch(Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }
    
//        public API_Result<tbl_NhaCungCap> GetByDaiDien(string loginCode, string id)
//        {
//            API_Result<tbl_NhaCungCap> rs = new API_Result<tbl_NhaCungCap>();
//            try
//            {
//                tbl_NhaCungCap obj = db.tbl_NhaCungCaps.Where(u => u.DaiDien == id).FirstOrDefault();
//                if(obj != null)
//                {
//                    rs.ErrCode = EnumErrCode.Success;
//                    rs.Data = obj;
//                }
//                else
//                {
//                    rs.ErrCode = EnumErrCode.Empty;
//                }
//            }
//            catch(Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }

//        public API_Result<List<tbl_LoaiSanPham>> GetListSanPham(string loginCode, string id)
//        {
//            API_Result<List<tbl_LoaiSanPham>> rs = new API_Result<List<tbl_LoaiSanPham>>();
//            try
//            {
//                List<tbl_LoaiSanPham> list = db.tbl_LoaiSanPhams.Where(sp => sp.MaNCC.Equals(id) && (sp.IsDelete == null || sp.IsDelete == false)).OrderBy(u => u.Update).ToList();

//                rs.Data = list;
//                rs.ErrCode = EnumErrCode.Success;
//            }
//            catch(Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }
    
//        public API_Result<bool> AddListSanPham(string loginCode, string id, TempLoaiSP[] listSP)
//        {
//            API_Result<bool> rs = new API_Result<bool>();
//            try
//            {
//                int addSuccessCount = 0;
//                int addFailCount = 0;
//                LoaiSP_ctrl lsp_ctrl = new LoaiSP_ctrl();

//                foreach(var item in listSP)
//                {
//                    tbl_LoaiSanPham newObj = new tbl_LoaiSanPham();
//                    newObj.TenLSP = item.TenSP;
//                    newObj.MoTa = item.MoTa;
//                    newObj.HinhAnh = item.HinhAnh;
//                    newObj.DonGia = item.DonGia;
//                    newObj.MaNCC = id;

//                    var prePresult = lsp_ctrl.Create(loginCode, newObj);

//                    if(prePresult.ErrCode == EnumErrCode.Success)
//                    {
//                        addSuccessCount++;
//                    }
//                    else
//                    {
//                        addFailCount++;
//                    }
//                }

//                rs.ErrCode = EnumErrCode.Success;
//                rs.ErrDes = "Thêm thành công " + addSuccessCount.ToString() + " trên tổng số " + (addSuccessCount + addFailCount).ToString() + " bản ghi!";
//            }
//            catch(Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }
//    }
//}