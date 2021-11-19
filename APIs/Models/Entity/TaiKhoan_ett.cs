using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APIs.Shareds;

namespace APIs.Models.Entity
{
    public class TaiKhoan_ett
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string HoTen { get; set; }
        public string Img { get; set; }
        public DateTime NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
        public string ChucVu { get; set; }
        public int PhanQuyenID { get; set; }
        public string PhanQuyen { get; set; }
        public DateTime NgayTao { get; set; }
        public bool IsDelete { get; set; }


        public TaiKhoan_ett()
        {
            //
        }

        //Table
        private void BindObject(tbl_TaiKhoan obj)
        {
            this.ID = obj.ID;
            this.UserName = obj.UserName;
            this.Img = obj.Img;
            this.HoTen = obj.HoTen;
            this.NgaySinh = obj.NgaySinh;
            this.DiaChi = obj.DiaChi;
            this.SDT = obj.SDT;
            this.ChucVu = obj.ChucVu;
            this.PhanQuyenID = obj.PhanQuyenID;
            switch (obj.PhanQuyenID)
            {
                case Authentication.qSuperAdmin:
                    this.PhanQuyen = "Quản trị hệ thống";
                    break;
                case Authentication.qAdmin:
                    this.PhanQuyen = "Quản lý kho";
                    break;
                case Authentication.qCuaHang:
                    this.PhanQuyen = "Đại diện cửa hàng";
                    break;
                case Authentication.qNCC:
                    this.PhanQuyen = "Đại diện nhà cung cấp";
                    break;
                case Authentication.qNhanVien:
                    this.PhanQuyen = "Nhân viên kho";
                    break;
                default:
                    this.PhanQuyen = "Chưa phân quyền";
                    break;
            }
            this.NgayTao = obj.NgayTao;
            this.IsDelete = obj.IsDelete;
        }

        public TaiKhoan_ett(tbl_TaiKhoan obj)
        {
            BindObject(obj);
        }

        //View
        private void BindObject(view_TaiKhoan obj)
        {
            this.ID = obj.ID;
            this.UserName = obj.UserName;
            this.Img = obj.Img;
            this.HoTen = obj.HoTen;
            this.NgaySinh = obj.NgaySinh;
            this.DiaChi = obj.DiaChi;
            this.SDT = obj.SDT;
            this.ChucVu = obj.ChucVu;
            this.PhanQuyenID = obj.PhanQuyenID;
            switch (obj.PhanQuyenID)
            {
                case Authentication.qSuperAdmin:
                    this.PhanQuyen = "Quản trị hệ thống";
                    break;
                case Authentication.qAdmin:
                    this.PhanQuyen = "Quản lý kho";
                    break;
                case Authentication.qCuaHang:
                    this.PhanQuyen = "Đại diện cửa hàng";
                    break;
                case Authentication.qNCC:
                    this.PhanQuyen = "Đại diện nhà cung cấp";
                    break;
                case Authentication.qNhanVien:
                    this.PhanQuyen = "Nhân viên kho";
                    break;
                default:
                    this.PhanQuyen = "Chưa phân quyền";
                    break;
            }
            this.NgayTao = obj.NgayTao;
            this.IsDelete = obj.IsDelete;
        }

        public TaiKhoan_ett(view_TaiKhoan obj)
        {
            BindObject(obj);
        }


        public TaiKhoan_ett(int id)
        {
            DatabaseDataContext db = new DatabaseDataContext();
            var obj = db.tbl_TaiKhoans.Where(u => u.ID.Equals(id)).FirstOrDefault();
            if (obj != null)
            {
                BindObject(obj);
            }
        }
    }
}