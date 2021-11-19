//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using API.Ctrls;
//using API.Models;
//using API.Models.Entity;
//using API.Shareds;

//namespace API.Ctrls
//{
//    public class PhieuXuat_ctrl
//    {
//        private dbQuanLyKhoDataContext db = new dbQuanLyKhoDataContext();

//        public string IncMaPX(string id)
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

//            return "PX" + STT.ToString();
//        }

//        public
//
//
//        <bool> Create(string loginCode, tbl_PhieuXuat obj)
//        {
//            API_Result<bool> rs = new API_Result<bool>();
//            try
//            {
//                tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
//                if (curUser != null)
//                {
//                    tbl_CuaHang ch = db.tbl_CuaHangs.Where(u => u.MaCH.Equals(obj.MaCH)).FirstOrDefault();
//                    if (curUser.PhanQuyen == 1 || (curUser.PhanQuyen == 2 && curUser.MaTK == ch.DaiDien) || curUser.PhanQuyen == 4)
//                    {
//                        //Admin hoac NCC
//                        string id = "PX0";
//                        var lastRecord = db.tbl_PhieuXuats.OrderByDescending(u => u.NgayLap).FirstOrDefault();
//                        if (lastRecord != null)
//                        {
//                            id = lastRecord.MaPX;
//                        }

//                        tbl_PhieuXuat newObj = new tbl_PhieuXuat();
//                        newObj.MaPX = IncMaPX(id);
//                        newObj.MaCH = obj.MaCH;
//                        newObj.MaTK = obj.MaTK;
//                        newObj.NgayLap = DateTime.Now;
//                        newObj.TrangThai = 0;

//                        try
//                        {
//                            db.tbl_PhieuXuats.InsertOnSubmit(newObj);
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

//        public API_Result<bool> Edit(string loginCode, tbl_PhieuXuat obj)
//        {
//            API_Result<bool> rs = new API_Result<bool>();
//            try
//            {
//                tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
//                if (curUser != null)
//                {
//                    tbl_PhieuXuat editObj = db.tbl_PhieuXuats.Where(u => u.MaPX.Equals(obj.MaPX)).FirstOrDefault();
//                    if (editObj != null)
//                    {
//                        tbl_CuaHang ch = db.tbl_CuaHangs.Where(c => c.MaCH.Equals(obj.MaCH)).FirstOrDefault();
//                        if (curUser.PhanQuyen == 1 || (curUser.PhanQuyen == 2 && curUser.MaTK.Equals(ch.DaiDien)) || curUser.PhanQuyen == 4)
//                        {
//                            //Admin hoac NCC
//                            editObj.MaCH = obj.MaCH;
//                            editObj.MaTK = obj.MaTK;
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

//        //public API_Result<List<tbl_PhieuXuat>> SearchPaging(string loginCode, int status = -1, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
//        //{
//        //    API_Result<List<tbl_PhieuXuat>> rs = new API_Result<List<tbl_PhieuXuat>>();
//        //    try
//        //    {
//        //        if (Authentication.CheckLogin(loginCode))
//        //        {
//        //            IQueryable<tbl_PhieuXuat> qrs = null;
//        //            IQueryable<tbl_PhieuXuat> qr = null;

//        //            switch (searchType)
//        //            {
//        //                case EnumSearchType.All:
//        //                    qrs = db.tbl_PhieuXuats.Where(u => u.IsDelete == null || u.IsDelete == false);
//        //                    break;
//        //                case EnumSearchType.ID:
//        //                    qrs = db.tbl_PhieuXuats.Where(u => u.MaNCC.Equals(searchValue) && (u.IsDelete == null || u.IsDelete == false));
//        //                    break;
//        //                case EnumSearchType.Name:
//        //                    qrs = db.tbl_PhieuXuats.Where(u => u.TenNCC.Contains(searchValue) && (u.IsDelete == null || u.IsDelete == false));
//        //                    break;
//        //                case EnumSearchType.Phone:
//        //                    qrs = db.tbl_PhieuXuats.Where(u => u.SDT.Equals(searchValue) && (u.IsDelete == null || u.IsDelete == false));
//        //                    break;
//        //                case EnumSearchType.ListAll:
//        //                    qrs = db.tbl_PhieuXuats;
//        //                    break;
//        //            }

//        //            if (status != -1)
//        //            {
//        //                //
//        //            }

//        //            if (qrs.Count() > 0)
//        //            {
//        //                if (isDescending)
//        //                {
//        //                    switch (orderBy)
//        //                    {
//        //                        case EnumOrderBy.Newest:
//        //                            qrs = qrs.OrderByDescending(u => u.Update);
//        //                            break;
//        //                        case EnumOrderBy.ID:
//        //                            qrs = qrs.OrderByDescending(u => u.MaNCC);
//        //                            break;
//        //                        case EnumOrderBy.Name:
//        //                            qrs = qrs.OrderByDescending(u => u.TenNCC);
//        //                            break;
//        //                    }
//        //                }
//        //                else
//        //                {
//        //                    switch (orderBy)
//        //                    {
//        //                        case EnumOrderBy.Newest:
//        //                            qrs = qrs.OrderBy(u => u.Update);
//        //                            break;
//        //                        case EnumOrderBy.ID:
//        //                            qrs = qrs.OrderBy(u => u.MaNCC);
//        //                            break;
//        //                        case EnumOrderBy.Name:
//        //                            qrs = qrs.OrderBy(u => u.TenNCC);
//        //                            break;
//        //                    }
//        //                }

//        //                rs.RecordCount = qrs.Count();
//        //                rs.PageCount = rs.RecordCount / pageSize;
//        //                if (rs.RecordCount % pageSize != 0)
//        //                {
//        //                    rs.PageCount++;
//        //                }
//        //                qr = qrs.Skip((curPage - 1) * pageSize).Take(pageSize);
//        //                rs.Data = qr.ToList();
//        //            }
//        //            else
//        //            {
//        //                rs.ErrCode = EnumErrCode.Empty;
//        //            }
//        //        }
//        //        else
//        //        {
//        //            rs.ErrCode = EnumErrCode.NotYetLogin;
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        rs.ErrCode = EnumErrCode.Error;
//        //        rs.ErrDes = ex.Message;
//        //    }

//        //    return rs;
//        //}

//        //public API_Result<List<PhieuXuat_ett>> SearchFullPaging(string loginCode, int status = -1, string searchValue = "", EnumSearchType searchType = EnumSearchType.All, int curPage = 1, int pageSize = 10, EnumOrderBy orderBy = EnumOrderBy.Newest, bool isDescending = false)
//        //{
//        //    API_Result<List<PhieuXuat_ett>> rs = new API_Result<List<PhieuXuat_ett>>();
            
//        //}

//        public API_Result<bool> IsEmpty(string loginCode)
//        {
//            API_Result<bool> rs = new API_Result<bool>();
//            try
//            {
//                tbl_PhieuXuat obj = db.tbl_PhieuXuats.FirstOrDefault();

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

//        public API_Result<PhieuXuat_ett> Get(string loginCode, string id)
//        {
//            API_Result<PhieuXuat_ett> rs = new API_Result<PhieuXuat_ett> ();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    rs.ErrCode = EnumErrCode.Success;
//                    rs.Data = new PhieuXuat_ett(id);
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

//        public API_Result<int[]> GetStatus(string loginCode)
//        {
//            API_Result<int[]> rs = new API_Result<int[]>();
//            int[] arrFail = { 0, 0, 0 };
//            try
//            {
//                tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;

//                IQueryable<tbl_PhieuXuat> tbl_PhieuXuats = db.tbl_PhieuXuats;
//                if(curUser.PhanQuyen == 2)
//                {
//                    tbl_CuaHang ch = db.tbl_CuaHangs.Where(c => c.DaiDien.Equals(curUser.MaTK)).FirstOrDefault();
//                    tbl_PhieuXuats = tbl_PhieuXuats.Where(px => px.MaCH.Equals(ch.MaCH));
//                }

//                int YC_1, YC_2, YC_3;
//                YC_1 = tbl_PhieuXuats.Count();
//                YC_2 = tbl_PhieuXuats.Where(px => px.TrangThai == 1).Count();
//                YC_3 = tbl_PhieuXuats.Where(px => px.TrangThai == 2).Count();

//                int[] arrSuccess = { YC_1, YC_2, YC_3 };

//                rs.Data = arrSuccess;
//                rs.ErrCode = EnumErrCode.Success;
//            }
//            catch(Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//                rs.Data = arrFail;
//            }

//            return rs;
//        }
//    }
//}