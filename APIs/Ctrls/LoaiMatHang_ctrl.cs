using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APIs.Models;
using APIs.Models.Entity;
using APIs.Models.Form;
using APIs.Shareds;

namespace APIs.Ctrls
{
    public class LoaiMatHang_ctrl
    {
        private string tableName = "loại mặt hàng";

        private DatabaseDataContext db = new DatabaseDataContext();

        public API_Result<bool> IsEmpty()
        {
            API_Result<bool> rs = new API_Result<bool>();
            try
            {
                rs.Data = db.view_LoaiMatHangs.Where(u => u.IsDelete == null || u.IsDelete == false).Count() == 0;
                rs.ErrCode = EnumErrCode.Success;
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
                IQueryable<view_LoaiMatHang> qrs = db.view_LoaiMatHangs.Where(u => u.IsDelete == null || u.IsDelete == false);

                List<ListCombobox_ett<int>> listCB = new List<ListCombobox_ett<int>>();
                foreach(var item in qrs.ToList())
                {
                    listCB.Add(new ListCombobox_ett<int>(item.ID, item.Ten));
                }

                rs.ErrCode = EnumErrCode.Success;
                rs.Data = listCB;
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