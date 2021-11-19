using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIs.Shareds
{
    public class Constants
    {
        //Alias
        public const string tbl_TaiKhoan = "tai-khoan";
        public const string tbl_HeThong = "he-thong";
        public const string tbl_Kho = "kho";
        public const string tbl_NhaCungCap = "nha-cung-cap";
        public const string tbl_MatHang = "mat-hang";
        public const string tbl_PhieuNhap = "phieu-nhap";
        public const string tbl_CuaHang = "cua-hang";
        public const string tbl_PhieuXuat = "phieu-xuat";
        public const string tbl_HopDong = "hop-dong";

        //Message
        public static string MSG_Insert_Success = "Thêm mới dữ liệu {0} thành công!";
        public static string MSG_Insert_Fail = "Thêm mới dữ liệu {0} thất bại!";
        public static string MSG_Already_Exist = "Bản ghi {0} đã tồn tại!";
        public static string MSG_Update_Success = "Cập nhật dữ liệu {0} thành công!";
        public static string MSG_Update_Fail = "Cập nhật dữ liệu {0} thất bại!";
        public static string MSG_Object_Empty = "Bản ghi có mã {0} không tồn tại!";
        public static string MSG_Delete_Success = "Xóa dữ liệu {0} thành công!";
        public static string MSG_Delete_Fail = "Xóa dữ liệu {0} thất bại!";
        public static string MSG_Search_Success = "Tìm kiếm dữ liệu {0} thành công!";
        public static string MSG_Search_Fail = "Tìm kiếm dữ liệu {0} thất bại!";
        public static string MSG_Search_Empty = "Không tìm thấy dữ liệu {0} nào thỏa mãn điều kiện!";
        public static string MSG_GetList_Success = "Lấy danh sách thành công!";
        public static string MSG_GetList_Fail = "Lấy danh sách thất bại!";
        public static string MSG_GetList_Empty = "Lấy danh sách rỗng!";
        public static string MSG_ChangePass_Success = "Đổi mật khẩu thành công!";
        public static string MSG_ChangePass_Fail = "Đổi mật khẩu thất bại!";
        public static string MSG_Data_Empty = "Không được bỏ trống dữ liệu!";
        public static string MSG_Connection_Fail = "Mất kết nối với máy chủ!";
        public static string MSG_Something_Wrong = "Có lỗi xảy ra trong quá trình {0}, vui lòng thử lại sau ít phút!";
        public static string MSG_Permission_Denied = "Người dùng hiện tại không có đủ quyền thực hiện hành động, vui lòng liên hệ admin của hệ thống để biết thêm chi tiết!";
        public static string MSG_Non_Function = "Chức năng đang trong quá trình bảo trì!";
        public static string MSG_Not_Login = "Phiên đăng nhập đã kết thúc!";
    }
}