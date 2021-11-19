//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using API.Abstracts;
//using API.Models;
//using API.Models.Entity;
//using API.Shareds;

//namespace API.Ctrls
//{
//    public class HeThong_ctrl : Controller_abs<tbl_ThongTinHT>
//    {
//        private dbQuanLyKhoDataContext db = new dbQuanLyKhoDataContext();

//        protected override  tbl_ThongTinHT BindObject(tbl_ThongTinHT newObj, tbl_ThongTinHT obj, EnumBindMode bindMode)
//        {
//            if(bindMode == EnumBindMode.Insert)
//            {
//                tbl_ThongTinHT bindObj = new tbl_ThongTinHT();
//                bindObj.TenHT = obj.TenHT;
//                bindObj.DiaChi = obj.DiaChi;
//                bindObj.SDT = obj.SDT;
//                bindObj.STK = obj.STK;
//                bindObj.NganHang = obj.NganHang;
//                bindObj.MaTK = obj.MaTK;

//                return bindObj;
//            }
//            else
//            {
//                newObj.TenHT = obj.TenHT;
//                newObj.DiaChi = obj.DiaChi;
//                newObj.SDT = obj.SDT;
//                newObj.STK = obj.STK;
//                newObj.NganHang = obj.NganHang;
//                newObj.MaTK = obj.MaTK;

//                return newObj;
//            }
//        }

//        protected override bool AddObject(tbl_ThongTinHT obj)
//        {
//            db.tbl_ThongTinHTs.InsertOnSubmit(obj);
//            db.SubmitChanges();
//            return true;
//        }

//        protected override tbl_ThongTinHT GetObject(tbl_ThongTinHT obj)
//        {
//            return db.tbl_ThongTinHTs.Where(u => u.MaHT.Equals(obj.MaHT)).FirstOrDefault();
//        }

//        protected override tbl_ThongTinHT GetObject(string id)
//        {
//            return db.tbl_ThongTinHTs.Where(u => u.MaHT.Equals(id)).FirstOrDefault();
//        }

//        protected override bool DeleteObject(tbl_ThongTinHT obj)
//        {
//            try
//            {
//                tbl_ThongTinHT delObj = GetObject(obj);

//                db.tbl_ThongTinHTs.DeleteOnSubmit(delObj);
//                db.SubmitChanges();
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        public
//        <tbl_ThongTinHT> Get(string loginCode)
//        {
//            API_Result<tbl_ThongTinHT> rs = new API_Result<tbl_ThongTinHT>();
//            try
//            {
//                tbl_ThongTinHT obj = db.tbl_ThongTinHTs.OrderByDescending(h => h.MaHT).FirstOrDefault();
//                rs.Data = obj;
//                rs.ErrCode = EnumErrCode.Success;
//            }
//            catch(Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }
    
//        public API_Result<HeThong_ett> GetAll(string loginCode)
//        {
//            API_Result<HeThong_ett> rs = new API_Result<HeThong_ett>();
//            try
//            {
//                rs.Data = new HeThong_ett();
//                rs.ErrCode = EnumErrCode.Success;
//            }
//            catch(Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }

//        public API_Result<HeThong_ett> GetAll(string loginCode, string id)
//        {
//            API_Result<HeThong_ett> rs = new API_Result<HeThong_ett>();
//            try
//            {
//                rs.Data = new HeThong_ett(id);
//                rs.ErrCode = EnumErrCode.Success;
//            }
//            catch (Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }

//        public API_Result<bool> Reset(string loginCode)
//        {
//            API_Result<bool> rs = new API_Result<bool>();
//            try
//            {
//                if (Authentication.IsAdmin(loginCode))
//                {
//                    //db.ExecuteCommand("delete tbl_Login");
//                    db.ExecuteCommand("delete tbl_NhaCungCap");
//                    db.ExecuteCommand("delete tbl_HopDong");
//                    db.ExecuteCommand("delete tbl_ThongTinHT");
//                    db.ExecuteCommand("delete tbl_ChiTietHD");
//                    db.ExecuteCommand("delete tbl_PhieuNhap");
//                    db.ExecuteCommand("delete tbl_ChiTietPN");
//                    db.ExecuteCommand("delete tbl_LoaiSanPham");
//                    db.ExecuteCommand("delete tbl_SanPham");
//                    db.ExecuteCommand("delete tbl_PhieuXuat");
//                    db.ExecuteCommand("delete tbl_ChiTietPX");
//                    db.ExecuteCommand("delete tbl_CuaHang");
//                    db.ExecuteCommand("delete tbl_Message");

//                    var listAccount = db.tbl_TaiKhoans.Where(u => !u.TenTK.Equals("admin")).ToList();
//                    try
//                    {
//                        db.tbl_TaiKhoans.DeleteAllOnSubmit(listAccount);
//                        db.SubmitChanges();
//                    }
//                    catch
//                    {
//                        //
//                    }

//                    rs.ErrCode = EnumErrCode.Success;
//                    rs.Data = true;
//                }
//                else
//                {
//                    rs.ErrCode = EnumErrCode.PermissionDenied;
//                    rs.ErrDes = Constants.MSG_Permission_Denied;
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