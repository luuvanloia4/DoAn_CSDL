//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using API.Models;
//using API.Models.Entity;
//using API.Shareds;

//namespace API.Ctrls
//{
//    public class Notify_ctrl
//    {
//        private dbQuanLyKhoDataContext db = new dbQuanLyKhoDataContext();

//        public bool Create(string maTK, string title, string content)
//        {
//            try
//            {
//                tbl_Message newObj = new tbl_Message();
//                newObj.MaTK = maTK;
//                newObj.Title = title;
//                newObj.Content = content;
//                newObj.IsDelete = false;
//                newObj.IsSeen = false;
//                newObj.Created = DateTime.Now;

//                db.tbl_Messages.InsertOnSubmit(newObj);
//                db.SubmitChanges();

//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        public
//
//        <bool> Create(string loginCode, tbl_Message obj)
//        {
//            API_Result<bool> rs = new API_Result<bool>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode))
//                {
//                    tbl_Message newObj = new tbl_Message();
//                    newObj.MaTK = obj.MaTK;
//                    newObj.Title = obj.Title;
//                    newObj.Content = obj.Content;
//                    newObj.IsDelete = false;
//                    newObj.IsSeen = false;
//                    newObj.Created = DateTime.Now;

//                    try
//                    {
//                        db.tbl_Messages.InsertOnSubmit(newObj);
//                        db.SubmitChanges();

//                        rs.ErrCode = EnumErrCode.Success;
//                        rs.Data = true;
//                    }
//                    catch(Exception ex)
//                    {
//                        rs.ErrCode = EnumErrCode.Error;
//                        rs.ErrDes = ex.Message;
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

//        public API_Result<List<tbl_Message>> GetStatus(string loginCode)
//        {
//            int msgCount = 5;
//            API_Result<List<tbl_Message>> rs = new API_Result<List<tbl_Message>>();
//            try
//            {
//                API_Result<List<tbl_Message>> preResult = GetList(loginCode);
//                rs.ErrCode = preResult.ErrCode;
//                rs.ErrDes = preResult.ErrDes;
//                if(preResult.ErrCode == EnumErrCode.Success)
//                {
//                    rs.RecordCount = preResult.Data.Where(u => u.IsSeen.Equals(false)).Count();
//                    rs.Data = preResult.Data.Take(msgCount).ToList();
//                }
//            }
//            catch(Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }

//        public API_Result<List<tbl_Message>> GetList(string loginCode)
//        {
//            API_Result<List<tbl_Message>> rs = new API_Result<List<tbl_Message>>();
//            try
//            {
//                tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
//                if(curUser != null)
//                {
//                    List<tbl_Message> list = db.tbl_Messages.Where(m => m.MaTK.Equals(curUser.MaTK) && m.IsDelete.Equals(false)).OrderByDescending(m => m.Created).ToList<tbl_Message>();
//                    rs.Data = list;
//                    rs.RecordCount = list.Count();
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

//        public API_Result<List<tbl_Message>> LoadMore(string loginCode, int size, int curCount)
//        {
//            API_Result<List<tbl_Message>> rs = new API_Result<List<tbl_Message>>();
//            try
//            {
//                tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
//                if (curUser != null)
//                {
//                    List<tbl_Message> listMsg = db.tbl_Messages.Where(u => u.MaTK.Equals(curUser.MaTK)).OrderByDescending(u => u.Created).Skip(curCount).Take(size).ToList();

//                    rs.ErrCode = EnumErrCode.Success;
//                    rs.Data = listMsg;
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

//        public API_Result<tbl_Message> Get(string loginCode, string id)
//        {
//            API_Result<tbl_Message> rs = new API_Result<tbl_Message>();
//            try
//            {
//                tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
//                if (curUser != null)
//                {
//                    tbl_Message obj = db.tbl_Messages.Where(m => m.ID.Equals(id)).FirstOrDefault();
//                    if(obj != null)
//                    {
//                        if(obj.MaTK == curUser.MaTK)
//                        {
//                            //Seen nhung k rep
//                            obj.IsSeen = true;
//                            try
//                            {
//                                db.SubmitChanges();
//                            }
//                            catch
//                            {
//                                //
//                            }
//                        }

//                        rs.Data = obj;
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
//            catch(Exception ex)
//            {
//                rs.ErrCode = EnumErrCode.Error;
//                rs.ErrDes = ex.Message;
//            }

//            return rs;
//        }

//        public API_Result<string> Delete(string loginCode, int[] listID)
//        {
//            API_Result<string> rs = new API_Result<string>();
//            try
//            {
//                if (Authentication.CheckLogin(loginCode)){
//                    int delSuccessCount = 0;
//                    int delFailCount = 0;
                    
//                    foreach(var id in listID)
//                    {
//                        tbl_Message delObj = db.tbl_Messages.Where(m => m.ID.Equals(id) && m.IsDelete.Equals(false)).FirstOrDefault();
                        
//                        try
//                        {
//                            delObj.IsDelete = true;
//                            db.SubmitChanges();
//                            delSuccessCount++;
//                        }
//                        catch
//                        {
//                            foreach (var change in db.GetChangeSet().Updates)
//                            {
//                                db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, change);
//                            }
//                            delFailCount++;
//                        }
//                    }

//                    rs.ErrCode = EnumErrCode.Success;
//                    rs.Data = "Xóa thành công " + delSuccessCount.ToString() + " trên tổng số" + (delSuccessCount + delFailCount).ToString() + " bản ghi!";
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

//        public API_Result<string> DeleteAll(string loginCode)
//        {
//            API_Result<string> rs = new API_Result<string>();
//            try
//            {
//                tbl_TaiKhoan curUser = Authentication.GetUser(loginCode).Data;
//                if (curUser != null)
//                {
//                    List<tbl_Message> listMsg = db.tbl_Messages.Where(m => m.MaTK.Equals(curUser.MaTK)).ToList();
//                    db.tbl_Messages.DeleteAllOnSubmit(listMsg);
//                    db.SubmitChanges();
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
//    }
//}