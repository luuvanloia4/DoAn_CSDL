//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using API.Models;
//using API.Models.Entity;
//using API.Shareds;

//namespace API.Ctrls
//{
//    public class LoaiSP_ctrl
//    {
//        private dbQuanLyKhoDataContext db = new dbQuanLyKhoDataContext();
//        string IncMaLSP(string MaLSP)
//        {
//            string result = "LSP";
//            int STT;
//            try
//            {
//                STT = int.Parse(MaLSP.Substring(3));
//            }
//            catch
//            {
//                STT = 0;
//            }
//            STT++;

//            return result + STT.ToString();
//        }

//        public
//        <bool> Create(string loginCode, tbl_LoaiSanPham obj)
//        {
//            API_Result<bool> rs = new API_Result<bool>();
//            try
//            {
//                tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
//                if (curUser != null)
//                {
//                    tbl_NhaCungCap ncc = db.tbl_NhaCungCaps.Where(u => u.MaNCC.Equals(obj.MaNCC)).FirstOrDefault();
//                    if (curUser.PhanQuyen == 1 || (curUser.MaTK == ncc.DaiDien) || curUser.PhanQuyen == 4)
//                    {
//                        //Admin hoac NCC
//                        string id = "LSP0";
//                        var lastRecord = db.tbl_LoaiSanPhams.OrderByDescending(u => u.Update).FirstOrDefault();
//                        if (lastRecord != null)
//                        {
//                            id = lastRecord.MaLSP;
//                        }

//                        tbl_LoaiSanPham newObj = new tbl_LoaiSanPham();
//                        newObj.MaLSP = IncMaLSP(id);
//                        newObj.TenLSP = obj.TenLSP;
//                        newObj.MoTa = obj.MoTa;
//                        newObj.DonGia = obj.DonGia;
//                        newObj.HinhAnh = obj.HinhAnh;
//                        newObj.Update = DateTime.Now;
//                        newObj.IsDelete = false;
//                        newObj.MaNCC = obj.MaNCC;

//                        try
//                        {
//                            db.tbl_LoaiSanPhams.InsertOnSubmit(newObj);
//                            db.SubmitChanges();

//                            rs.ErrCode = EnumErrCode.Success;
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

//        public API_Result<bool> Edit(string loginCode, tbl_LoaiSanPham obj)
//        {
//            API_Result<bool> rs = new API_Result<bool>();
//            try
//            {
//                tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
//                if (curUser != null)
//                {
//                    tbl_LoaiSanPham editObj = db.tbl_LoaiSanPhams.Where(u => u.MaLSP.Equals(obj.MaLSP)).FirstOrDefault();
//                    if (editObj != null)
//                    {
//                        tbl_NhaCungCap ncc = db.tbl_NhaCungCaps.Where(n => n.MaNCC.Equals(obj.MaNCC)).FirstOrDefault();
//                        if (curUser.PhanQuyen == 1 || (curUser.PhanQuyen == 3 && curUser.MaTK == ncc.DaiDien) || curUser.PhanQuyen == 4)
//                        {
//                            //Admin hoac NCC
//                            editObj.TenLSP = obj.TenLSP;
//                            editObj.MoTa = obj.MoTa;
//                            editObj.DonGia = obj.DonGia;
//                            editObj.HinhAnh = obj.HinhAnh;

//                            try
//                            {
//                                db.SubmitChanges();

//                                rs.ErrCode = EnumErrCode.Success;
//                                rs.Data = true;
//                            }
//                            catch (Exception ex)
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
//        public API_Result<string> Delete(string loginCode, string id)
//        {
//            API_Result<string> rs = new API_Result<string>();
//            try
//            {
//                tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
//                if (curUser != null)
//                {
//                    if (curUser.PhanQuyen == 1 || curUser.PhanQuyen == 3 || curUser.PhanQuyen == 4)
//                    {
//                        tbl_LoaiSanPham delObj = db.tbl_LoaiSanPhams.Where(u => u.MaLSP.Equals(id)).FirstOrDefault();
//                        LoaiSanPham_ett lsp_ett = new LoaiSanPham_ett(delObj, false);
//                        //Check
//                        if (curUser.PhanQuyen == 3)
//                        {

//                            if(lsp_ett.SL > 0)
//                            {
//                                try
//                                {
//                                    delObj.IsDelete = true;
//                                    db.SubmitChanges();

//                                    rs.ErrCode = EnumErrCode.Success;
//                                }
//                                catch (Exception ex)
//                                {
//                                    rs.ErrCode = EnumErrCode.Error;
//                                    rs.ErrDes = ex.Message;

//                                    foreach (var change in db.GetChangeSet().Updates)
//                                    {
//                                        db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, change);
//                                    }
//                                }
//                            }
//                            else
//                            {
//                                try
//                                {
//                                    db.tbl_LoaiSanPhams.DeleteOnSubmit(delObj);
//                                    db.SubmitChanges();

//                                    rs.ErrCode = EnumErrCode.Success;
//                                }
//                                catch (Exception ex)
//                                {
//                                    rs.ErrCode = EnumErrCode.Error;
//                                    rs.ErrDes = ex.Message;
//                                }
//                            }
//                        }
//                        else
//                        {
//                            if(lsp_ett.SL == 0 && delObj.IsDelete == true)
//                            {
//                                List<tbl_SanPham> listSP = db.tbl_SanPhams.Where(sp => sp.MaLSP.Equals(lsp_ett.MaLSP)).ToList();
//                                try
//                                {
//                                    db.tbl_SanPhams.DeleteAllOnSubmit(listSP);
//                                    db.SubmitChanges();
//                                }
//                                catch
//                                {
//                                    foreach (var change in db.GetChangeSet().Deletes)
//                                    {
//                                        db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, change);
//                                    }
//                                }

//                                try
//                                {
//                                    db.tbl_LoaiSanPhams.DeleteOnSubmit(delObj);
//                                    db.SubmitChanges();
//                                    rs.ErrCode = EnumErrCode.Success;
//                                }
//                                catch(Exception ex)
//                                {
//                                    rs.ErrCode = EnumErrCode.Error;
//                                    rs.ErrDes = ex.Message;
//                                }
//                            }
//                            else
//                            {
//                                rs.ErrCode = EnumErrCode.AlreadyExist;
//                                rs.ErrDes = "Loại sản phẩm chưa bị xóa bởi nhà cung cấp hoặc vẫn còn sản phẩm trong kho!";
//                            }
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

//        public API_Result<List<ListCombobox_ett<string>>> GetListCombobox(string loginCode, string id)
//        {
//            API_Result<List<ListCombobox_ett<string>>> rs = new API_Result<List<ListCombobox_ett<string>>>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    IQueryable<tbl_LoaiSanPham> qrs = db.tbl_LoaiSanPhams.Where(u => u.IsDelete == null || u.IsDelete == false);

//                    if (!string.IsNullOrEmpty(id))
//                    {
//                        qrs = qrs.Where(u => u.MaNCC.Equals(id));
//                    }

//                    var list = qrs.ToList();

//                    List<ListCombobox_ett<string>> listCB = new List<ListCombobox_ett<string>>();
//                    foreach (var item in list)
//                    {
//                        listCB.Add(new ListCombobox_ett<string>(item.MaLSP, item.TenLSP + " - " + item.MaNCC));
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

//        public API_Result<List<tbl_LoaiSanPham>> SearchPaging(string loginCode, DateTime startTime, DateTime endTime, int status = -1, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
//        {
//            API_Result<List<tbl_LoaiSanPham>> rs = new API_Result<List<tbl_LoaiSanPham>>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    IQueryable<tbl_LoaiSanPham> qrs = null;
//                    IQueryable<tbl_LoaiSanPham> qr = null;

//                    switch (searchType)
//                    {
//                        case EnumSearchType.All:
//                            qrs = db.tbl_LoaiSanPhams;
//                            break;
//                        case EnumSearchType.ID:
//                            qrs = db.tbl_LoaiSanPhams.Where(u => u.MaLSP.Equals(searchValue));
//                            break;
//                        case EnumSearchType.Name:
//                            qrs = db.tbl_LoaiSanPhams.Where(u => u.TenLSP.Contains(searchValue));
//                            break;
//                        case EnumSearchType.ParentID:
//                            qrs = db.tbl_LoaiSanPhams.Where(u => u.MaNCC.Equals(searchValue));
//                            break;
//                        case EnumSearchType.ListAll:
//                            qrs = db.tbl_LoaiSanPhams;
//                            break;
//                    }

//                    qrs = qrs.Where(u => startTime <= u.Update && u.Update <= endTime);

//                    if (status != -1)
//                    {
//                        //
//                    }

//                    if (qrs.Count() > 0)
//                    {
//                        if (isDescending)
//                        {
//                            switch (orderBy)
//                            {
//                                case EnumOrderBy.Newest:
//                                    qrs = qrs.OrderByDescending(u => u.Update);
//                                    break;
//                                case EnumOrderBy.ID:
//                                    qrs = qrs.OrderByDescending(u => u.MaLSP);
//                                    break;
//                                case EnumOrderBy.Name:
//                                    qrs = qrs.OrderByDescending(u => u.TenLSP);
//                                    break;
//                                case EnumOrderBy.Count:
//                                    qrs = qrs.OrderByDescending(u => u.DonGia);
//                                    break;
//                            }
//                        }
//                        else
//                        {
//                            switch (orderBy)
//                            {
//                                case EnumOrderBy.Newest:
//                                    qrs = qrs.OrderBy(u => u.Update);
//                                    break;
//                                case EnumOrderBy.ID:
//                                    qrs = qrs.OrderBy(u => u.MaNCC);
//                                    break;
//                                case EnumOrderBy.Name:
//                                    qrs = qrs.OrderBy(u => u.TenLSP);
//                                    break;
//                                case EnumOrderBy.Count:
//                                    qrs = qrs.OrderBy(u => u.DonGia);
//                                    break;
//                            }
//                        }

//                        rs.RecordCount = qrs.Count();
//                        rs.PageCount = rs.RecordCount / pageSize;
//                        if (rs.RecordCount % pageSize != 0)
//                        {
//                            rs.PageCount++;
//                        }
//                        qr = qrs.Skip((curPage - 1) * pageSize).Take(pageSize);
//                        rs.Data = qr.ToList();
//                        rs.ErrCode = EnumErrCode.Success;
//                    }
//                    else
//                    {
//                        rs.ErrCode = EnumErrCode.Empty;
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

//        public API_Result<List<LoaiSanPham_ett>> SearchFullPaging(string loginCode, DateTime startTime, DateTime endTime, int status = -1, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
//        {
//            API_Result<List<LoaiSanPham_ett>> rs = new API_Result<List<LoaiSanPham_ett>>();
//            var preResult = SearchPaging(loginCode, startTime, endTime, status, searchValue, searchType, curPage, pageSize, orderBy, isDescending);
//            rs.ErrCode = preResult.ErrCode;
//            rs.ErrDes = preResult.ErrDes;
//            rs.PageCount = preResult.PageCount;
//            rs.RecordCount = preResult.RecordCount;
//            if(rs.ErrCode == EnumErrCode.Success)
//            {
//                List<LoaiSanPham_ett> list = new List<LoaiSanPham_ett>();
//                foreach(var item in preResult.Data)
//                {
//                    list.Add(new LoaiSanPham_ett(item.MaLSP, false));
//                }

//                rs.Data = list;
//            }

//            return rs;
//        }

//        public API_Result<List<LoaiSanPham_ett>> ExportData(string loginCode, DateTime startTime, DateTime endTime, int status = -1, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
//        {
//            API_Result<List<LoaiSanPham_ett>> rs = new API_Result<List<LoaiSanPham_ett>>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    IQueryable<tbl_LoaiSanPham> qrs = null;
//                    IQueryable<tbl_LoaiSanPham> qr = null;

//                    switch (searchType)
//                    {
//                        case EnumSearchType.All:
//                            qrs = db.tbl_LoaiSanPhams;
//                            break;
//                        case EnumSearchType.ID:
//                            qrs = db.tbl_LoaiSanPhams.Where(u => u.MaLSP.Equals(searchValue));
//                            break;
//                        case EnumSearchType.Name:
//                            qrs = db.tbl_LoaiSanPhams.Where(u => u.TenLSP.Contains(searchValue));
//                            break;
//                        case EnumSearchType.ParentID:
//                            qrs = db.tbl_LoaiSanPhams.Where(u => u.MaNCC.Equals(searchValue));
//                            break;
//                        case EnumSearchType.ListAll:
//                            qrs = db.tbl_LoaiSanPhams;
//                            break;
//                    }

//                    qrs = qrs.Where(u => startTime <= u.Update && u.Update <= endTime);

//                    if (status != -1)
//                    {
//                        //
//                    }

//                    if (qrs.Count() > 0)
//                    {
//                        if (isDescending)
//                        {
//                            switch (orderBy)
//                            {
//                                case EnumOrderBy.Newest:
//                                    qrs = qrs.OrderByDescending(u => u.Update);
//                                    break;
//                                case EnumOrderBy.ID:
//                                    qrs = qrs.OrderByDescending(u => u.MaLSP);
//                                    break;
//                                case EnumOrderBy.Name:
//                                    qrs = qrs.OrderByDescending(u => u.TenLSP);
//                                    break;
//                                case EnumOrderBy.Count:
//                                    qrs = qrs.OrderByDescending(u => u.DonGia);
//                                    break;
//                            }
//                        }
//                        else
//                        {
//                            switch (orderBy)
//                            {
//                                case EnumOrderBy.Newest:
//                                    qrs = qrs.OrderBy(u => u.Update);
//                                    break;
//                                case EnumOrderBy.ID:
//                                    qrs = qrs.OrderBy(u => u.MaNCC);
//                                    break;
//                                case EnumOrderBy.Name:
//                                    qrs = qrs.OrderBy(u => u.TenLSP);
//                                    break;
//                                case EnumOrderBy.Count:
//                                    qrs = qrs.OrderBy(u => u.DonGia);
//                                    break;
//                            }
//                        }

//                        rs.RecordCount = qrs.Count();
//                        qr = qrs;
//                        var list = qr.ToList();
//                        List<LoaiSanPham_ett> listLSP = new List<LoaiSanPham_ett>();
//                        foreach(var item in list)
//                        {
//                            listLSP.Add(new LoaiSanPham_ett(item, false));
//                        }
//                        rs.Data = listLSP;
//                        rs.ErrCode = EnumErrCode.Success;
//                    }
//                    else
//                    {
//                        rs.ErrCode = EnumErrCode.Empty;
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
//                tbl_LoaiSanPham obj = db.tbl_LoaiSanPhams.Where(n => n.IsDelete == null || n.IsDelete == false).FirstOrDefault();

//                rs.Data = obj == null;
//                rs.ErrCode = EnumErrCode.Success;
//            }
//            catch (Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }

//        public API_Result<LoaiSanPham_ett> Get(string loginCode, string id)
//        {
//            API_Result<LoaiSanPham_ett> rs = new API_Result<LoaiSanPham_ett>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    rs.ErrCode = EnumErrCode.Success;
//                    rs.Data = new LoaiSanPham_ett(id, true);
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

//        public API_Result<List<LoaiSanPham_ett>> GetTopValue(string loginCode, DateTime timeStart, DateTime timeEnd)
//        {
//            API_Result<List<LoaiSanPham_ett>> rs = new API_Result<List<LoaiSanPham_ett>>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    var listTop = (from px in db.tbl_PhieuXuats
//                                  join ctpx in db.tbl_ChiTietPXes on px.MaPX equals ctpx.MaPX
//                                  where px.TrangThai.Equals(2) && (px.NgayLap >= timeStart && px.NgayLap <= timeEnd)
//                                  group ctpx by ctpx.MaLSP into gLSP
//                                  select new {
//                                    MaLSP = gLSP.Key,
//                                    TongSL = gLSP.Sum(lpx => lpx.SL)
//                                  }).OrderByDescending(lpx => lpx.TongSL).Take(10);

//                    List<LoaiSanPham_ett> list = new List<LoaiSanPham_ett>();
//                    foreach(var item in listTop)
//                    {
//                        list.Add(new LoaiSanPham_ett(item.MaLSP, true, item.TongSL));
//                    }

//                    rs.Data = list;
//                    rs.ErrCode = EnumErrCode.Success;
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
    
//        public API_Result<List<tbl_LoaiSanPham>> GetUnSlected(string loginCode, string maHD)
//        {
//            API_Result<List<tbl_LoaiSanPham>> rs = new API_Result<List<tbl_LoaiSanPham>>();
//            try
//            {
//                tbl_HopDong hd = db.tbl_HopDongs.Where(u => u.MaHD.Equals(maHD)).FirstOrDefault();
//                List<tbl_LoaiSanPham> list = db.tbl_LoaiSanPhams.Where(u => u.MaNCC.Equals(hd.MaNCC)).ToList();
//                List<tbl_LoaiSanPham> rsList = new List<tbl_LoaiSanPham>();
//                foreach (var item in list)
//                {
//                    tbl_ChiTietHD cthd = db.tbl_ChiTietHDs.Where(u => u.MaHD.Equals(maHD) && u.MaLSP.Equals(item.MaLSP) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
//                    if (cthd == null)
//                    {
//                        rsList.Add(item);
//                    }
//                }

//                if (rsList.Any())
//                {
//                    rs.ErrCode = EnumErrCode.Success;
//                    rs.Data = rsList;
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

//        public API_Result<List<LoaiSanPham_ett>> GetThongKe(string loginCode, int mode, int size, DateTime startTime, DateTime endTime)
//        {
//            API_Result<List<LoaiSanPham_ett>> rs = new API_Result<List<LoaiSanPham_ett>>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    if (Authentication.IsAdmin(loginCode))
//                    {
//                        List<LoaiSanPham_ett> listLSP = new List<LoaiSanPham_ett>();

//                        switch (mode)
//                        {
//                            case 0:
//                                var listNhap = (from ctpn in db.tbl_ChiTietPNs
//                                            join pn in db.tbl_PhieuNhaps on ctpn.MaPN equals pn.MaPN
//                                            where (startTime <= pn.NgayNhap && pn.NgayNhap <= endTime)
//                                            group ctpn by ctpn.MaLSP into gLSP
//                                            select new
//                                            {
//                                                MaLSP = gLSP.Key,
//                                                SL = gLSP.Sum(lsp => lsp.SL)
//                                            }).OrderByDescending(u => u.SL).Take(size);

//                                foreach(var item in listNhap)
//                                {
//                                    LoaiSanPham_ett lsp = new LoaiSanPham_ett(item.MaLSP, false, item.SL);
//                                    listLSP.Add(lsp);
//                                }
//                                break;

//                            case 1:
//                                var listXuat = (from ctpx in db.tbl_ChiTietPXes
//                                            join px in db.tbl_PhieuXuats on ctpx.MaPX equals px.MaPX
//                                            where (startTime <= px.NgayLap && px.NgayLap <= endTime)
//                                            && px.TrangThai != 0 
//                                            group ctpx by ctpx.MaLSP into gLSP
//                                            select new
//                                            {
//                                                MaLSP = gLSP.Key,
//                                                SL = gLSP.Sum(lsp => lsp.SL)
//                                            }).OrderByDescending(u => u.SL).Take(size);

//                                foreach (var item in listXuat)
//                                {
//                                    LoaiSanPham_ett lsp = new LoaiSanPham_ett(item.MaLSP, false, item.SL);
//                                    listLSP.Add(lsp);
//                                }
//                                break;
//                        }

//                        rs.ErrCode = EnumErrCode.Success;
//                        rs.Data = listLSP;
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
//            catch(Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }
//    }
//}