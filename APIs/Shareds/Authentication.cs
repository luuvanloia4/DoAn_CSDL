using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using APIs.Models;

namespace APIs.Shareds
{
    public class Authentication
    {
        //Phan quyen
        public const int qSuperAdmin = 0;
        public const int qAdmin = 1;
        public const int qNhanVien = 2;
        public const int qCuaHang = 3;
        public const int qNCC = 4;

        //
        public static string EncodeMD5(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hashCode = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder strBuilder = new StringBuilder();
            foreach (byte b in hashCode)
            {
                strBuilder.Append(b.ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public static API_Result<string> Login(string userName, string password)
        {
            API_Result<string> rs = new API_Result<string>();
            try
            {
                DatabaseDataContext db = new DatabaseDataContext();
                password = EncodeMD5(password);
                tbl_TaiKhoan user = db.tbl_TaiKhoans.Where(u => u.UserName.Equals(userName) && u.Pass.Equals(password) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
                if (user != null)
                {
                    //Tạo phiên đăng nhập mới
                    try
                    {
                        user.Token = Guid.NewGuid().ToString();
                        db.SubmitChanges();

                        rs.ErrCode = EnumErrCode.Success;
                        rs.Data = user.Token;
                        rs.ErrDes = "Đăng nhập thành công!";
                    }
                    catch (Exception ex)
                    {
                        rs.ErrCode = EnumErrCode.Error;
                        rs.ErrDes = "Lỗi: không tạo được phiên đăng nhập \nChi tiết: " + ex.Message;
                    }
                }
                else
                {
                    rs.ErrCode = EnumErrCode.Fail;
                    rs.ErrDes = "Thông tin tài khoản hoặc mật khẩu không chính xác!";
                }
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public static bool CheckLogin(string loginCode)
        {
            DatabaseDataContext db = new DatabaseDataContext();
            view_TaiKhoan curUser = db.view_TaiKhoans.Where(u => u.Token.Equals(loginCode)).FirstOrDefault();

            return curUser != null;
        }

        public static API_Result<view_TaiKhoan> GetUser(string loginCode)
        {
            API_Result<view_TaiKhoan> rs = new API_Result<view_TaiKhoan>();
            try
            {
                DatabaseDataContext db = new DatabaseDataContext();
                view_TaiKhoan curUser = db.view_TaiKhoans.Where(u => u.Token.Equals(loginCode) && (u.IsDelete == null || u.IsDelete == false)).FirstOrDefault();
                if(curUser != null)
                {
                    rs.ErrCode = EnumErrCode.Success;
                    rs.Data = curUser;
                }
                else
                {
                    rs.ErrCode = EnumErrCode.Empty;
                    rs.ErrDes = "Phiên đăng nhập đã kết thúc!";
                }
            }
            catch(Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }
    
        public static bool IsAdmin(string loginCode)
        {
            view_TaiKhoan curUser = GetUser(loginCode).Data;

            return IsAdmin(curUser);
        }

        public static bool IsAdmin(view_TaiKhoan curUser)
        {
            return curUser.PhanQuyenID == qAdmin || curUser.PhanQuyenID == qSuperAdmin;
        }

        public static bool IsNCC(string loginCode)
        {
            view_TaiKhoan curUser = GetUser(loginCode).Data;

            return IsNCC(curUser);
        }

        public static bool IsNCC(view_TaiKhoan curUser)
        {
            return curUser.PhanQuyenID == qNCC;
        }

        public static bool IsCuaHang(string loginCode)
        {
            view_TaiKhoan curUser = GetUser(loginCode).Data;

            return IsCuaHang(curUser);
        }

        public static bool IsCuaHang(view_TaiKhoan curUser)
        {
            return curUser.PhanQuyenID == qCuaHang;
        }

        //Extra phan quyen ...
    }
}