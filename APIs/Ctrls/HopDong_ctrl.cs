//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web;
//using API.Models;
//using API.Models.Entity;
//using API.Shareds;

//namespace API.Ctrls
//{
//    public class HopDong_ctrl
//    {
//        private dbQuanLyKhoDataContext db = new dbQuanLyKhoDataContext();
//        private Notify_ctrl ntf_ctrl = new Notify_ctrl();
//        private TaiKhoan_ctrl tk_ctrl = new TaiKhoan_ctrl();

//        string IncMaHD(string maHD)
//        {
//            string tempStr = maHD.Substring(2);
//            int STT;
//            try
//            {
//                STT = int.Parse(tempStr);
//            }
//            catch
//            {
//                STT = 0;
//            }
//            STT++;

//            return "HD" + STT.ToString();
//        }
//        //Thao tác với dữ liệu
//        public static float UpdateStatus(string maHD)
//        {
//            dbQuanLyKhoDataContext db = new dbQuanLyKhoDataContext();
//            try
//            {
//                int slHienTai = 0;
//                int slTong = 0;

//                List<tbl_ChiTietHD> listCTHD = db.tbl_ChiTietHDs.Where(u => u.MaHD.Equals(maHD) && (u.IsDelete == null || u.IsDelete == false)).ToList();
//                foreach (var cthd in listCTHD)
//                {
//                    int tongSL;
//                    try
//                    {
//                        tongSL = (from pn in db.tbl_PhieuNhaps
//                                  join ctpn in db.tbl_ChiTietPNs on pn.MaPN equals ctpn.MaPN
//                                  where pn.MaHD.Equals(maHD) && ctpn.MaLSP.Equals(cthd.MaLSP)
//                                  select ctpn).Sum(u => u.SL);
//                    }
//                    catch
//                    {
//                        tongSL = 0;
//                    }
//                    try
//                    {
//                        cthd.SLHienTai = tongSL;
//                        db.SubmitChanges();
//                    }
//                    catch (Exception ex)
//                    {
//                        foreach (var change in db.GetChangeSet().Updates)
//                        {
//                            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, change);
//                        }
//                    }

//                    slHienTai += cthd.SLHienTai;
//                    slTong += cthd.SLTong;
//                }

//                if(slTong == 0)
//                {
//                    return 0;
//                }

//                tbl_HopDong hopDong = db.tbl_HopDongs.Where(hd => hd.MaHD.Equals(maHD)).FirstOrDefault();
//                if (slHienTai == slTong)
//                {
//                    hopDong.TrangThai = 1;
//                }
//                else if(hopDong.NgayLap.AddMonths(3) < DateTime.Now)
//                {
//                    hopDong.TrangThai = 2;
//                }
//                else
//                {
//                    hopDong.TrangThai = 0;
//                }

//                try
//                {
//                    db.SubmitChanges();
//                }
//                catch
//                {
//                    //
//                }


//                return (float)Math.Round(((double)slHienTai / slTong) * 100, 2);
//            }
//            catch
//            {
//                return 0;
//            }
//        }

//        //Trang thai: 0 - vua lap, 1 - dang giao, 2 - hoan thanh
//        public
//        <bool> Create(string loginCode, tbl_HopDong obj)
//        {
//            API_Result<bool> rs = new API_Result<bool>();
//            try
//            {
//                tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
//                if (curUser != null)
//                {
//                    if (curUser.PhanQuyen == 1 || curUser.PhanQuyen == 4)
//                    {
//                        //Admin
//                        string id = "HD0";
//                        var lastRecord = db.tbl_HopDongs.OrderByDescending(u => u.NgayLap).FirstOrDefault();
//                        if (lastRecord != null)
//                        {
//                            id = lastRecord.MaHD;
//                        }

//                        tbl_HopDong newObj = new tbl_HopDong();
//                        newObj.MaHD = IncMaHD(id);
//                        newObj.MaNCC = obj.MaNCC;
//                        newObj.MaTK = obj.MaTK;
//                        newObj.NgayLap = DateTime.Now;
//                        newObj.TrangThai = 0;
//                        newObj.IsDelete = false;
                        

//                        try
//                        {
//                            db.tbl_HopDongs.InsertOnSubmit(newObj);
//                            db.SubmitChanges();

//                            rs.ErrCode = EnumErrCode.Success;
//                            rs.Data = true;
//                            rs.ErrDes = newObj.MaHD;
                            
//                            try
//                            {
//                                string maTK = (from ncc in db.tbl_NhaCungCaps
//                                               join tk in db.tbl_TaiKhoans on ncc.DaiDien equals tk.MaTK
//                                               where ncc.MaNCC.Equals(newObj.MaNCC)
//                                               select tk).FirstOrDefault().MaTK;
//                                ntf_ctrl.Create(maTK,
//                                    "Hợp đồng " + newObj.MaHD + " đã được tạo!",
//                                    "Hợp đồng " + newObj.MaHD + " đã được tạo vào " + newObj.NgayLap.ToString("HH:mm dd/MM/yyyy") + ".\n Xem chi tiết <a href=\"/HopDong/Detail/" + newObj.MaHD + "\">Tại đây</a>!");
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

//        public API_Result<bool> Edit(string loginCode, tbl_HopDong obj)
//        {
//            API_Result<bool> rs = new API_Result<bool>();
//            try
//            {
//                tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
//                if (curUser != null)
//                {
//                    tbl_HopDong editObj = db.tbl_HopDongs.Where(u => u.MaHD.Equals(obj.MaHD)).FirstOrDefault();
//                    if (editObj != null)
//                    {
//                        if (curUser.PhanQuyen == 1 || curUser.PhanQuyen == 4)
//                        {
//                            //Admin
//                            editObj.MaNCC = obj.MaNCC;
//                            editObj.MaTK = obj.MaTK;
//                            editObj.NgayLap = obj.NgayLap;
//                            editObj.TrangThai = obj.TrangThai;
//                            editObj.IsDelete = obj.IsDelete;

//                            try
//                            {
//                                db.SubmitChanges();

//                                rs.ErrCode = EnumErrCode.Success;
//                                rs.Data = true;

//                                try
//                                {
//                                    string maTK = (from ncc in db.tbl_NhaCungCaps
//                                                   join tk in db.tbl_TaiKhoans on ncc.DaiDien equals tk.MaTK
//                                                   where ncc.MaNCC.Equals(editObj.MaNCC)
//                                                   select tk).FirstOrDefault().MaTK;
//                                    ntf_ctrl.Create(maTK,
//                                        "Hợp đồng " + editObj.MaHD + " đã được cập nhật thông tin!",
//                                        "Hợp đồng " + editObj.MaHD + " đã cập nhật vào " + DateTime.Now.ToString("HH:mm dd/MM/yyyy") + ".\n Xem chi tiết <a href=\"/HopDong/Detail/" + editObj.MaHD + "\">Tại đây</a>!");
//                                }
//                                catch
//                                {
//                                    //Không tạo được thông báo => kệ
//                                }
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
//        public API_Result<string> Delete(string loginCode, string hdID)
//        {
//            API_Result<string> rs = new API_Result<string>();
//            try
//            {
//                tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
//                if (curUser != null)
//                {
//                    if (curUser.PhanQuyen == 1 || curUser.PhanQuyen == 4)
//                    {
//                        tbl_HopDong delObj = db.tbl_HopDongs.Where(u => u.MaHD.Equals(hdID) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
//                        if(delObj.TrangThai != 1)
//                        {
//                            try
//                            {
//                                delObj.IsDelete = true;
//                                db.SubmitChanges();

//                                rs.ErrCode = EnumErrCode.Success;
//                                rs.ErrDes = "Xóa thành công bản ghi!";
//                            }
//                            catch (Exception ex)
//                            {
//                                foreach (var change in db.GetChangeSet().Updates)
//                                {
//                                    db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, change);
//                                }

//                                rs.ErrCode = EnumErrCode.Error;
//                                rs.ErrDes = ex.Message;
//                            }
//                        }
//                        else
//                        {
//                            rs.ErrCode = EnumErrCode.AlreadyExist;
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
        
//        //Lấy dữ liệu
//        public API_Result<List<ListCombobox_ett<string>>> GetListCombobox(string loginCode)
//        {
//            API_Result<List<ListCombobox_ett<string>>> rs = new API_Result<List<ListCombobox_ett<string>>>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    IQueryable<tbl_HopDong> qrs = db.tbl_HopDongs.Where(u => u.IsDelete == null || u.IsDelete == false);

//                    var list = qrs.ToList();

//                    List<ListCombobox_ett<string>> listCB = new List<ListCombobox_ett<string>>();
//                    foreach (var item in list)
//                    {
//                        listCB.Add(new ListCombobox_ett<string>(item.MaHD, item.MaHD + " - " + item.MaNCC));
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

//        public API_Result<List<tbl_HopDong>> SearchPaging(string loginCode, DateTime startTime, DateTime endTime, int status = -1, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
//        {
//            API_Result<List<tbl_HopDong>> rs = new API_Result<List<tbl_HopDong>>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    if(db.tbl_HopDongs.Count() == 0)
//                    {
//                        rs.ErrCode = EnumErrCode.DoesNotExist;

//                        return rs;
//                    }

//                    IQueryable<tbl_HopDong> qrs = null;
//                    IQueryable<tbl_HopDong> qr = null;

//                    switch (searchType)
//                    {
//                        case EnumSearchType.All:
//                            qrs = db.tbl_HopDongs.Where(u => u.IsDelete == null || u.IsDelete == false);
//                            break;
//                        case EnumSearchType.ID:
//                            qrs = db.tbl_HopDongs.Where(u => u.MaHD.Equals(searchValue) && (u.IsDelete == null || u.IsDelete == false));
//                            break;
//                        case EnumSearchType.Name:
//                            qrs = db.tbl_HopDongs.Where(u => u.MaNCC.Equals(searchValue) && (u.IsDelete == null || u.IsDelete == false));
//                            break;
//                        case EnumSearchType.ParentID:
//                            qrs = db.tbl_HopDongs.Where(u => u.MaTK.Equals(searchValue) && (u.IsDelete == null || u.IsDelete == false));
//                            break;
//                        case EnumSearchType.ListAll:
//                            qrs = db.tbl_HopDongs;
//                            break;
//                    }

//                    if (Authentication.IsNCC(loginCode))
//                    {

//                        tbl_NhaCungCap ncc = db.tbl_NhaCungCaps.Where(u => u.DaiDien.Equals(Authentication.GetUser(loginCode).Data.MaTK)).First();
//                        qrs = qrs.Where(u => u.MaNCC.Equals(ncc.MaNCC));
//                    }

//                    //
//                    qrs = qrs.Where(u => startTime <= u.NgayLap && u.NgayLap <= endTime);

//                    if (status != -1)
//                    {
//                        qrs = qrs.Where(u => u.TrangThai.Equals(status));
//                    }

//                    if (qrs.Count() > 0)
//                    {
//                        if (isDescending)
//                        {
//                            switch (orderBy)
//                            {
//                                case EnumOrderBy.Newest:
//                                    qrs = qrs.OrderByDescending(u => u.NgayLap);
//                                    break;
//                                case EnumOrderBy.ID:
//                                    qrs = qrs.OrderByDescending(u => u.MaHD);
//                                    break;
//                                case EnumOrderBy.Name:
//                                    qrs = qrs.OrderByDescending(u => u.MaNCC);
//                                    break;
//                            }
//                        }
//                        else
//                        {
//                            switch (orderBy)
//                            {
//                                case EnumOrderBy.Newest:
//                                    qrs = qrs.OrderBy(u => u.NgayLap);
//                                    break;
//                                case EnumOrderBy.ID:
//                                    qrs = qrs.OrderBy(u => u.MaHD);
//                                    break;
//                                case EnumOrderBy.Name:
//                                    qrs = qrs.OrderBy(u => u.MaNCC);
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

//        public API_Result<List<HopDong_ett>> SearchAllPaging(string loginCode, DateTime startTime, DateTime endTime, int status = -1, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
//        {
//            API_Result<List<HopDong_ett>> rs = new API_Result<List<HopDong_ett>>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    var preResult = SearchPaging(loginCode, startTime, endTime, status, searchValue, searchType, curPage, pageSize, orderBy, isDescending);
//                    rs.ErrCode = preResult.ErrCode;
//                    rs.ErrDes = preResult.ErrDes;
//                    rs.RecordCount = preResult.RecordCount;
//                    rs.PageCount = preResult.PageCount;

                    
//                    if(preResult.ErrCode == EnumErrCode.Success)
//                    {
//                        List<HopDong_ett> list = new List<HopDong_ett>();
//                        foreach(var item in preResult.Data)
//                        {
//                            list.Add(new HopDong_ett(item));
//                        }
//                        rs.Data = list;
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

//        public API_Result<bool> IsEmpty(string loginCode)
//        {
//            API_Result<bool> rs = new API_Result<bool>();
//            try
//            {
//                tbl_HopDong obj = db.tbl_HopDongs.Where(n => n.IsDelete == null || n.IsDelete == false).FirstOrDefault();

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

//        public API_Result<tbl_HopDong> Get(string loginCode, string id)
//        {
//            API_Result<tbl_HopDong> rs = new API_Result<tbl_HopDong>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    rs.Data = db.tbl_HopDongs.Where(u => u.MaHD.Equals(id)).FirstOrDefault();
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

//        public API_Result<HopDong_ett> GetAll(string loginCode, string id)
//        {
//            API_Result<HopDong_ett> rs = new API_Result<HopDong_ett>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    rs.Data = new HopDong_ett(id);
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

//        public API_Result<List<ChiTietHD_ett>> GetListChiTiet(string loginCode, string id)
//        {
//            API_Result<List<ChiTietHD_ett>> rs = new API_Result<List<ChiTietHD_ett>>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    var list = db.tbl_ChiTietHDs.Where(u => u.MaHD.Equals(id) && (u.IsDelete == null || u.IsDelete == false)).ToList();
//                    if(list.Count() > 0)
//                    {
//                        UpdateStatus(id);

//                        List<ChiTietHD_ett> listCT = new List<ChiTietHD_ett>();
//                        foreach(var item in list)
//                        {
//                            listCT.Add(new ChiTietHD_ett(item.MaHD, item.MaLSP));
//                        }

//                        rs.ErrCode = EnumErrCode.Success;
//                        rs.Data = listCT;
//                        rs.RecordCount = listCT.Count();
//                    }
//                    else
//                    {
//                        rs.ErrCode = EnumErrCode.Empty;
//                        rs.ErrDes = "Hợp đồng chưa có sản phẩm nào!";
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
//        public API_Result<float> GetPercents(string loginCode, string id)
//        {
//            API_Result<float> rs = new API_Result<float>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    tbl_HopDong hopDong = db.tbl_HopDongs.Where(hd => hd.MaHD.Equals(id)).FirstOrDefault();
//                    int tongSL = 0;
//                    foreach (var item in db.tbl_ChiTietHDs.Where(cthd => cthd.MaHD.Equals(id) && (cthd.IsDelete == null || cthd.IsDelete == false)).ToList<tbl_ChiTietHD>())
//                    {
//                        tongSL += item.SLTong;
//                    }
//                    int curSL = 0;
//                    foreach (var item in (from pn in db.tbl_PhieuNhaps
//                                          join ctpn in db.tbl_ChiTietPNs on pn.MaPN equals ctpn.MaPN
//                                          where pn.MaHD.Equals(id)
//                                          select ctpn).ToList<tbl_ChiTietPN>())
//                    {
//                        curSL += item.SL;
//                    }
//                    if (curSL == 0)
//                    {
//                        rs.Data = 0;
//                    }
//                    else
//                    {
//                        float result = (float)curSL / tongSL;
//                        result *= 100;
//                        result = (float)Math.Round(result, 2);
//                        rs.Data = result;
//                    }

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
//                rs.Data = 0;
//            }

//            return rs;
//        }
        
//        public API_Result<int[]> GetStatus(string loginCode)
//        {
//            API_Result<int[]> rs = new API_Result<int[]>();
//            int[] arrFail = { 0, 0, 0 };
//            try
//            {
//                tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
//                if(curUser != null)
//                {
//                    IQueryable<tbl_HopDong> tbl_HopDongs = db.tbl_HopDongs;

//                    if(curUser.PhanQuyen == 3)
//                    {
//                        tbl_NhaCungCap ncc = db.tbl_NhaCungCaps.Where(u => u.DaiDien.Equals(curUser.MaTK)).FirstOrDefault();
//                        tbl_HopDongs = tbl_HopDongs.Where(u => u.MaNCC.Equals(ncc.MaNCC));
//                    }

//                    int HD_1, HD_2, HD_3;
//                    HD_1 = tbl_HopDongs.Where(hd => hd.IsDelete == null || hd.IsDelete == false).Count();
//                    HD_2 = tbl_HopDongs.Where(hd => (hd.IsDelete == null || hd.IsDelete == false) && hd.TrangThai == 0).Count();
//                    HD_3 = tbl_HopDongs.Where(hd => (hd.IsDelete == null || hd.IsDelete == false) && hd.TrangThai == 1).Count();

//                    int[] arrSuccess = { HD_1, HD_2, HD_3 };
//                    rs.Data = arrSuccess;
//                    rs.ErrCode = EnumErrCode.Success;
//                }
//                else
//                {
//                    rs.Data = arrFail;
//                    rs.ErrCode = EnumErrCode.NotYetLogin;
//                }
//            }
//            catch(Exception ex)
//            {
//                rs.Data = arrFail;
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }
    
//        public API_Result<string> AddListChiTiet(string loginCode, List<tbl_ChiTietHD> listCTHD)
//        {
//            API_Result<string> rs = new API_Result<string>();
//            try
//            {
//                tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
//                if (curUser != null)
//                {
//                    if (curUser.PhanQuyen == 1 || curUser.PhanQuyen == 4)
//                    {
//                        DateTime now = DateTime.Now;
//                        foreach(var item in listCTHD)
//                        {
//                            tbl_ChiTietHD oldObj = db.tbl_ChiTietHDs.Where(u => u.MaHD.Equals(item.MaHD) && u.MaLSP.Equals(item.MaLSP)).FirstOrDefault();
//                            if(oldObj == null)
//                            {
//                                tbl_ChiTietHD newObj = new tbl_ChiTietHD();
//                                newObj.MaHD = item.MaHD;
//                                newObj.MaLSP = item.MaLSP;
//                                newObj.SLHienTai = 0;
//                                newObj.SLTong = item.SLTong;
//                                newObj.Update = now;
//                                newObj.IsDelete = false;

//                                try
//                                {
//                                    db.tbl_ChiTietHDs.InsertOnSubmit(newObj);
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
//                            else{
//                                oldObj.SLTong = item.SLTong;
//                                oldObj.SLHienTai = 0;
//                                oldObj.IsDelete = false;
//                                oldObj.Update = now;

//                                try
//                                {
//                                    db.SubmitChanges();
//                                }
//                                catch
//                                {
//                                    foreach(var change in db.GetChangeSet().Updates)
//                                    {
//                                        db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, change);
//                                    }
//                                }
//                            }
//                        }
//                        rs.ErrCode = EnumErrCode.Success;

//                        try
//                        {
//                            string maHD = listCTHD[0].MaHD;
//                            string maTK = (from hd in db.tbl_HopDongs
//                                           join ncc in db.tbl_NhaCungCaps on hd.MaNCC equals ncc.MaNCC
//                                           join tk in db.tbl_TaiKhoans on ncc.DaiDien equals tk.MaTK
//                                           where hd.MaHD.Equals(maHD)
//                                           select tk).FirstOrDefault().MaTK;
//                            ntf_ctrl.Create(maTK,
//                                "Hợp đồng " + maHD + " đã được cập nhật danh sách sản phẩm!",
//                                "Hợp đồng " + maHD + " đã cập nhật vào " + DateTime.Now.ToString("HH:mm dd/MM/yyyy") + ".\n Xem chi tiết <a href=\"/HopDong/Detail/" + maHD + "\">Tại đây</a>!");
//                        }
//                        catch
//                        {
//                            //Không tạo được thông báo => kệ
//                        }
//                    }
//                    else {
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
        
//        public API_Result<ChiTietHD_ett> GetChiTiet(string loginCode, string maHD, string maLSP)
//        {
//            API_Result<ChiTietHD_ett> rs = new API_Result<ChiTietHD_ett>();
//            try
//            {
//                rs.Data = new ChiTietHD_ett(maHD, maLSP);
//                rs.ErrCode = EnumErrCode.Success;
//            }
//            catch(Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }

//        public API_Result<bool> EditChiTiet(string loginCode, tbl_ChiTietHD obj)
//        {
//            API_Result<bool> rs = new API_Result<bool>();
//            try
//            {
//                tbl_HopDong hd = db.tbl_HopDongs.Where(u => u.MaHD.Equals(obj.MaHD)).FirstOrDefault();
//                if(hd == null || hd.TrangThai != 1)
//                {
//                    tbl_ChiTietHD oldObj = db.tbl_ChiTietHDs.Where(u => u.MaHD.Equals(obj.MaHD) && u.MaLSP.Equals(obj.MaLSP)).FirstOrDefault();
//                    oldObj.SLTong = obj.SLTong;
//                    //oldObj.SLHienTai = obj.SLHienTai;

//                    db.SubmitChanges();
//                    rs.ErrCode = EnumErrCode.Success;
//                    rs.Data = true;
//                }
//                else
//                {
//                    rs.ErrCode = EnumErrCode.AlreadyExist;
//                }
                
//            }
//            catch(Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }

//        public API_Result<bool> RemoveChiTiet(string loginCode, string maHD, string maLSP)
//        {
//            API_Result<bool> rs = new API_Result<bool>();
//            try
//            {
//                tbl_ChiTietHD obj = db.tbl_ChiTietHDs.Where(u => u.MaHD.Equals(maHD) && u.MaLSP.Equals(maLSP)).FirstOrDefault();
//                if(obj.SLHienTai == 0)
//                {
//                    db.tbl_ChiTietHDs.DeleteOnSubmit(obj);
//                    db.SubmitChanges();
//                    rs.ErrCode = EnumErrCode.Success;
//                    rs.Data = true;
//                }
//                else
//                {
//                    rs.ErrCode = EnumErrCode.AlreadyExist;
//                    rs.ErrDes = "Không thể xóa chi tiết hợp đồng đã tồn tại trong phiếu xuất!";
//                }
//            }
//            catch(Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }

//        public API_Result<float> GetHopDongStatus(string loginCode, string maHD)
//        {
//            API_Result<float> rs = new API_Result<float>();
//            try
//            {
//                rs.ErrCode = EnumErrCode.Success;
//                rs.Data = UpdateStatus(maHD);
//            }
//            catch(Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }

//        public API_Result<List<HopDong_ett>> ExportData(string loginCode, DateTime startTime, DateTime endTime, int status = -1, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
//        {
//            API_Result<List<HopDong_ett>> rs = new API_Result<List<HopDong_ett>>();

//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    if (db.tbl_HopDongs.Count() == 0)
//                    {
//                        rs.ErrCode = EnumErrCode.DoesNotExist;

//                        return rs;
//                    }

//                    IQueryable<tbl_HopDong> qrs = null;
//                    IQueryable<tbl_HopDong> qr = null;

//                    switch (searchType)
//                    {
//                        case EnumSearchType.All:
//                            qrs = db.tbl_HopDongs.Where(u => u.IsDelete == null || u.IsDelete == false);
//                            break;
//                        case EnumSearchType.ID:
//                            qrs = db.tbl_HopDongs.Where(u => u.MaHD.Equals(searchValue) && (u.IsDelete == null || u.IsDelete == false));
//                            break;
//                        case EnumSearchType.Name:
//                            qrs = db.tbl_HopDongs.Where(u => u.MaNCC.Equals(searchValue) && (u.IsDelete == null || u.IsDelete == false));
//                            break;
//                        case EnumSearchType.ParentID:
//                            qrs = db.tbl_HopDongs.Where(u => u.MaTK.Equals(searchValue) && (u.IsDelete == null || u.IsDelete == false));
//                            break;
//                        case EnumSearchType.ListAll:
//                            qrs = db.tbl_HopDongs;
//                            break;
//                    }

//                    //
//                    qrs = qrs.Where(u => startTime <= u.NgayLap && u.NgayLap <= endTime);

//                    if (Authentication.IsNCC(loginCode))
//                    {

//                        tbl_NhaCungCap ncc = db.tbl_NhaCungCaps.Where(u => u.DaiDien.Equals(Authentication.GetUser(loginCode).Data.MaTK)).First();
//                        qrs = qrs.Where(u => u.MaNCC.Equals(ncc.MaNCC));
//                    }

//                    if (status != -1)
//                    {
//                        qrs = qrs.Where(u => u.TrangThai.Equals(status));
//                    }

//                    if (qrs.Count() > 0)
//                    {
//                        if (isDescending)
//                        {
//                            switch (orderBy)
//                            {
//                                case EnumOrderBy.Newest:
//                                    qrs = qrs.OrderByDescending(u => u.NgayLap);
//                                    break;
//                                case EnumOrderBy.ID:
//                                    qrs = qrs.OrderByDescending(u => u.MaHD);
//                                    break;
//                                case EnumOrderBy.Name:
//                                    qrs = qrs.OrderByDescending(u => u.MaNCC);
//                                    break;
//                            }
//                        }
//                        else
//                        {
//                            switch (orderBy)
//                            {
//                                case EnumOrderBy.Newest:
//                                    qrs = qrs.OrderBy(u => u.NgayLap);
//                                    break;
//                                case EnumOrderBy.ID:
//                                    qrs = qrs.OrderBy(u => u.MaHD);
//                                    break;
//                                case EnumOrderBy.Name:
//                                    qrs = qrs.OrderBy(u => u.MaNCC);
//                                    break;
//                            }
//                        }

//                        rs.RecordCount = qrs.Count();
//                        rs.PageCount = 1;
//                        {
//                            rs.PageCount++;
//                        }
//                        qr = qrs; //.Skip((curPage - 1) * pageSize).Take(pageSize);
//                        List<tbl_HopDong> listTemp = qr.ToList();
//                        List<HopDong_ett> listHD = new List<HopDong_ett>();
//                        foreach (var item in listTemp)
//                        {
//                            listHD.Add(new HopDong_ett(item));
//                        }

//                        rs.Data = listHD;
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
//    }
//}