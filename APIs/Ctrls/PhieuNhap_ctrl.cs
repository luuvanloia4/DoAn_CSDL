//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using API.Models;
//using API.Shareds;
//using API.Models.Entity;

//namespace API.Ctrls
//{
//    public class PhieuNhap_ctrl
//    {
//        private dbQuanLyKhoDataContext db = new dbQuanLyKhoDataContext();

//        private Notify_ctrl ntf_ctrl = new Notify_ctrl();
//        private HopDong_ctrl hd_ctrl = new HopDong_ctrl();

//        public string IncMaPN(string id)
//        {
//            int STT;
//            try
//            {
//                STT = int.Parse(id.Substring(2));
//            }
//            catch
//            {
//                STT = 0;
//            }
//            STT++;

//            return "PN" + STT.ToString();
//        }

//        public
//
//        <bool> Create(string loginCode, tbl_PhieuNhap obj, tbl_ChiTietPN[] listCTPN)
//        {
//            API_Result<bool> rs = new API_Result<bool>();
//            try
//            {
//                tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
//                if (curUser != null)
//                {
//                    tbl_HopDong hd = db.tbl_HopDongs.Where(u => u.MaHD.Equals(obj.MaHD)).FirstOrDefault();
//                    if (curUser.PhanQuyen == 1 || (curUser.PhanQuyen == 0) || curUser.PhanQuyen == 4)
//                    {
//                        //Admin hoac NCC
//                        string id = "PN0";
//                        var lastRecord = db.tbl_PhieuNhaps.OrderByDescending(u => u.NgayNhap).FirstOrDefault();
//                        if (lastRecord != null)
//                        {
//                            id = lastRecord.MaPN;
//                        }

//                        tbl_PhieuNhap newObj = new tbl_PhieuNhap();
//                        newObj.MaPN = IncMaPN(id);
//                        newObj.MaHD = obj.MaHD;
//                        newObj.MaTK = obj.MaTK;
//                        newObj.NgayNhap = DateTime.Now;
//                        newObj.TrangThai = 0;
//                        newObj.NguoiGiao = obj.NguoiGiao;

//                        try
//                        {
//                            db.tbl_PhieuNhaps.InsertOnSubmit(newObj);
//                            db.SubmitChanges();

//                            rs.ErrCode = EnumErrCode.Success;
//                            rs.Data = true;
//                            rs.ErrDes = newObj.MaPN;

//                            try
//                            {
//                                string maHD = obj.MaHD;
//                                string maTK = (from hd1 in db.tbl_HopDongs
//                                               join ncc in db.tbl_NhaCungCaps on hd1.MaNCC equals ncc.MaNCC
//                                               join tk in db.tbl_TaiKhoans on ncc.DaiDien equals tk.MaTK
//                                               where hd1.MaHD.Equals(maHD)
//                                               select tk).FirstOrDefault().MaTK;
//                                ntf_ctrl.Create(maTK,
//                                    "Hợp đồng " + maHD + " đã được tạo thêm phiếu nhập!",
//                                    "Hợp đồng " + maHD + " đã được thêm phiếu nhập có mã " + obj.MaPN + " vào " + DateTime.Now.ToString("HH:mm dd/MM/yyyy") + ".\n Xem chi tiết <a href=\"/HopDong/Detail/" + maHD + "\">Tại đây</a>!");
//                            }
//                            catch
//                            {
//                                //Không tạo được thông báo => kệ
//                            }
//                        }
//                        catch (Exception ex)
//                        {
//                            rs.ErrCode = EnumErrCode.Error;
//                            rs.ErrDes = ex.Message;
//                        }

//                        foreach(var item in listCTPN)
//                        {
//                            tbl_ChiTietPN ct = new tbl_ChiTietPN();
//                            ct.MaLSP = item.MaLSP;
//                            ct.MaPN = newObj.MaPN;
//                            ct.SL = item.SL;
//                            ct.ChieuCao = 0;
//                            ct.ChieuDai = 0;
//                            ct.ChieuRong = 0;

//                            try
//                            {
//                                db.tbl_ChiTietPNs.InsertOnSubmit(ct);
//                                db.SubmitChanges();
//                            }
//                            catch
//                            {
//                                foreach (var change in db.GetChangeSet().Inserts)
//                                {
//                                    db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, change);
//                                }
//                            }

//                            for(int i = 0; i < item.SL; i++)
//                            {
//                                tbl_SanPham sp = new tbl_SanPham();
//                                sp.MaLSP = item.MaLSP;
//                                sp.MaPN = newObj.MaPN;
//                                sp.MaO = 0;
//                                sp.MaVach = 0;
//                                sp.MaPX = string.Empty;
//                                sp.TrangThai = 1;

//                                try
//                                {
//                                    db.tbl_SanPhams.InsertOnSubmit(sp);
//                                    db.SubmitChanges();
//                                }
//                                catch
//                                {
//                                    foreach (var change in db.GetChangeSet().Inserts)
//                                    {
//                                        db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, change);
//                                    }
//                                }
//                            }
//                        }
//                        HopDong_ctrl.UpdateStatus(obj.MaHD);
//                        try
//                        {
//                            if (hd_ctrl.Get(loginCode, obj.MaHD).Data.TrangThai == 1)
//                            {
//                                try
//                                {
//                                    string maHD = obj.MaHD;
//                                    string maTK = (from hd1 in db.tbl_HopDongs
//                                                   join ncc in db.tbl_NhaCungCaps on hd1.MaNCC equals ncc.MaNCC
//                                                   join tk in db.tbl_TaiKhoans on ncc.DaiDien equals tk.MaTK
//                                                   where hd1.MaHD.Equals(maHD)
//                                                   select tk).FirstOrDefault().MaTK;
//                                    ntf_ctrl.Create(maTK,
//                                        "Hợp đồng " + maHD + " đã hoàn thành!",
//                                        "Hợp đồng " + maHD + " đã cập nhật trạng thái hoàn thành vào " + DateTime.Now.ToString("HH:mm dd/MM/yyyy") + ".\n Xem chi tiết <a href=\"/HopDong/Detail/" + maHD + "\">Tại đây</a>!");

//                                    //Thông báo cho tài khoản admin //1
//                                    TaiKhoan_ctrl tk_ctrl = new TaiKhoan_ctrl();
//                                    foreach(var ad in tk_ctrl.GetListComboboxID(loginCode, 1).Data)
//                                    {
//                                        ntf_ctrl.Create(ad.Data,
//                                        "Hợp đồng " + maHD + " đã hoàn thành!",
//                                        "Hợp đồng " + maHD + " đã cập nhật trạng thái hoàn thành vào " + DateTime.Now.ToString("HH:mm dd/MM/yyyy") + ".\n Xem chi tiết <a href=\"/HopDong/Detail/" + maHD + "\">Tại đây</a>!");
//                                    }
//                                }
//                                catch
//                                {
//                                    //Không tạo được thông báo => kệ
//                                }
//                            }
//                        }
//                        catch
//                        {
//                            //
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

//        public API_Result<bool> Edit(string loginCode, tbl_PhieuNhap obj)
//        {
//            API_Result<bool> rs = new API_Result<bool>();
//            try
//            {
//                tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
//                if (curUser != null)
//                {
//                    tbl_PhieuNhap editObj = db.tbl_PhieuNhaps.Where(u => u.MaPN.Equals(obj.MaPN)).FirstOrDefault();
//                    if (editObj != null)
//                    {
//                        tbl_HopDong hd = db.tbl_HopDongs.Where(u => u.MaHD.Equals(obj.MaHD)).First();
//                        if (curUser.PhanQuyen == 1 || curUser.PhanQuyen == 4)
//                        {
//                            //Admin hoac NCC
//                            editObj.MaHD = obj.MaHD;
//                            editObj.MaTK = obj.MaTK;
//                            editObj.NguoiGiao = obj.NguoiGiao;
//                            editObj.TrangThai = 0;

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

//        public API_Result<string> Delete(string loginCode, string maPN)
//        {
//            API_Result<string> rs = new API_Result<string>();
//            try
//            {
//                if (Authentication.IsAdmin(loginCode))
//                {
//                    tbl_PhieuNhap pn = db.tbl_PhieuNhaps.Where(u => u.MaPN.Equals(maPN)).FirstOrDefault();
//                    tbl_HopDong hd = db.tbl_HopDongs.Where(u => u.MaHD.Equals(pn.MaHD)).FirstOrDefault();
//                    if(hd == null || hd.TrangThai != 1)
//                    {
//                        if (pn != null)
//                        {
//                            List<tbl_ChiTietPN> listCTPN = db.tbl_ChiTietPNs.Where(u => u.MaPN.Equals(maPN)).ToList();
//                            try
//                            {
//                                db.tbl_ChiTietPNs.DeleteAllOnSubmit(listCTPN);
//                                db.SubmitChanges();
//                            }
//                            catch
//                            {
//                                foreach (var change in db.GetChangeSet().Deletes)
//                                {
//                                    db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, change);
//                                }
//                            }

//                            List<tbl_SanPham> listSP = db.tbl_SanPhams.Where(u => u.MaPN.Equals(maPN)).ToList();
//                            try
//                            {
//                                db.tbl_SanPhams.DeleteAllOnSubmit(listSP);
//                                db.SubmitChanges();
//                            }
//                            catch
//                            {
//                                foreach (var change in db.GetChangeSet().Deletes)
//                                {
//                                    db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, change);
//                                }
//                            }

//                            db.tbl_PhieuNhaps.DeleteOnSubmit(pn);
//                            db.SubmitChanges();

//                            rs.ErrCode = EnumErrCode.Success;
//                            HopDong_ctrl.UpdateStatus(pn.MaHD);
//                        }
//                        else
//                        {
//                            rs.ErrCode = EnumErrCode.Empty;
//                        }
//                    }
//                    else
//                    {
//                        rs.ErrCode = EnumErrCode.AlreadyExist;
//                    }
                    
//                }
//                else
//                {
//                    rs.ErrCode = EnumErrCode.PermissionDenied;
//                    rs.ErrDes = Constants.MSG_Permission_Denied;
//                }
//            }
//            catch (Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }

//        public API_Result<List<tbl_PhieuNhap>> GetList(string loginCode, string MaHD)
//        {
//            API_Result<List<tbl_PhieuNhap>> rs = new API_Result<List<tbl_PhieuNhap>>();
//            try
//            {
//                List<tbl_PhieuNhap> listPN = db.tbl_PhieuNhaps.Where(pn => pn.MaHD.Equals(MaHD)).OrderBy(u => u.NgayNhap).ToList();
//                rs.ErrCode = EnumErrCode.Success;
//                rs.Data = listPN;
//                rs.RecordCount = listPN.Count();
//                if (listPN.Count() == 0)
//                {
//                    rs.ErrCode = EnumErrCode.DoesNotExist;
//                }
//            }
//            catch(Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }
//        public API_Result<List<PhieuNhap_ett>> GetAllList(string loginCode, string MaHD)
//        {
//            API_Result<List<PhieuNhap_ett>> rs = new API_Result<List<PhieuNhap_ett>>();
//            try
//            {
//                var preResult = GetList(loginCode, MaHD);
//                rs.ErrCode = preResult.ErrCode;
//                rs.ErrDes = preResult.ErrDes;
//                rs.RecordCount = preResult.RecordCount;
//                if(rs.ErrCode == EnumErrCode.Success)
//                {
//                    List<PhieuNhap_ett> listPN = new List<PhieuNhap_ett>();
//                    foreach(var item in preResult.Data)
//                    {
//                        listPN.Add(new PhieuNhap_ett(item));
//                    }

//                    rs.Data = listPN;
//                }
//            }
//            catch(Exception ex)
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
//                    IQueryable<tbl_PhieuXuat> qrs = db.tbl_PhieuXuats;

//                    var list = qrs.ToList();

//                    List<ListCombobox_ett<string>> listCB = new List<ListCombobox_ett<string>>();
//                    foreach (var item in list)
//                    {
//                        listCB.Add(new ListCombobox_ett<string>(item.MaPX, item.MaPX + " - " + item.MaCH));
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

//        public API_Result<List<PhieuNhap_ett>> SearchPaging(string loginCode, int status = -1, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
//        {
//            API_Result<List<PhieuNhap_ett>> rs = new API_Result<List<PhieuNhap_ett>>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    if(db.tbl_PhieuNhaps.Count() == 0)
//                    {
//                        rs.ErrCode = EnumErrCode.DoesNotExist;

//                        return rs;
//                    }

//                    IQueryable<tbl_PhieuNhap> qrs = null;
//                    IQueryable<tbl_PhieuNhap> qr = null;

//                    switch (searchType)
//                    {
//                        case EnumSearchType.All:
//                            qrs = db.tbl_PhieuNhaps;
//                            break;
//                        case EnumSearchType.ID:
//                            qrs = db.tbl_PhieuNhaps.Where(u => u.MaPN.Equals(searchValue));
//                            break;
//                        case EnumSearchType.ParentID:
//                            qrs = db.tbl_PhieuNhaps.Where(u => u.MaHD.Equals(searchValue));
//                            break;
//                        case EnumSearchType.Name:
//                            qrs = db.tbl_PhieuNhaps.Where(u => u.NguoiGiao.Contains(searchValue));
//                            break;
//                        case EnumSearchType.ListAll:
//                            qrs = db.tbl_PhieuNhaps;
//                            break;
//                    }

//                    if (status != -1)
//                    {
//                        //
//                    }

//                    tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
//                    if (Authentication.IsNCC(curUser))
//                    {
//                        qrs = (from q in qrs
//                               join hd in db.tbl_HopDongs on q.MaHD equals hd.MaHD
//                               join ncc in db.tbl_NhaCungCaps on hd.MaNCC equals ncc.MaNCC
//                               where ncc.DaiDien.Equals(curUser.MaTK)
//                               select q
//                               );
//                    }

//                    if (qrs.Count() > 0)
//                    {
//                        if (isDescending)
//                        {
//                            switch (orderBy)
//                            {
//                                case EnumOrderBy.Newest:
//                                    qrs = qrs.OrderByDescending(u => u.NgayNhap);
//                                    break;
//                                case EnumOrderBy.ID:
//                                    qrs = qrs.OrderByDescending(u => u.MaHD);
//                                    break;
//                                case EnumOrderBy.Name:
//                                    qrs = qrs.OrderByDescending(u => u.NguoiGiao);
//                                    break;
//                            }
//                        }
//                        else
//                        {
//                            switch (orderBy)
//                            {
//                                case EnumOrderBy.Newest:
//                                    qrs = qrs.OrderBy(u => u.NgayNhap);
//                                    break;
//                                case EnumOrderBy.ID:
//                                    qrs = qrs.OrderBy(u => u.MaHD);
//                                    break;
//                                case EnumOrderBy.Name:
//                                    qrs = qrs.OrderBy(u => u.NguoiGiao);
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
//                        List<tbl_PhieuNhap> tempListPN = qr.ToList();
//                        List<PhieuNhap_ett> listPN = new List<PhieuNhap_ett>();
//                        foreach(var item in tempListPN)
//                        {
//                            listPN.Add(new PhieuNhap_ett(item));
//                        }

//                        rs.Data = listPN;
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
//                tbl_PhieuNhap obj = db.tbl_PhieuNhaps.FirstOrDefault();

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

//        public API_Result<PhieuNhap_ett> Get(string loginCode, string id)
//        {
//            API_Result<PhieuNhap_ett> rs = new API_Result<PhieuNhap_ett>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    rs.ErrCode = EnumErrCode.Success;
//                    rs.Data = new PhieuNhap_ett(id);
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

//    }
//}