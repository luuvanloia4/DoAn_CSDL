using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APIs.Models;
using APIs.Models.Entity;
using APIs.Shareds;

namespace APIs.Ctrls
{
    public class Menu_ctrl
    {
        private DatabaseDataContext db = new DatabaseDataContext();

        public API_Result<List<view_Menu>> GetListMenu(string loginCode)
        {
            API_Result<List<view_Menu>> rs = new API_Result<List<view_Menu>>();
            try
            {
                view_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
                if(curUser != null)
                {
                    List<view_Menu> listMenu = db.view_Menus.Where(u => u.PhanQuyen.Equals(curUser.PhanQuyenID)).ToList();

                    rs.ErrCode = EnumErrCode.Success;
                    rs.Data = listMenu;
                }
                else
                {
                    rs.ErrCode = EnumErrCode.NotYetLogin;
                }
            }
            catch(Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDes = ex.Message;
            }

            return rs;
        }

        public static int ConvertID(string strID)
        {
            string tempStr = strID.Trim();
            int id;
            try
            {
                id = int.Parse(tempStr);
            }
            catch
            {
                id = 0;
            }

            return id;
        }
    }
}