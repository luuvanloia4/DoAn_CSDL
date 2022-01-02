using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Http.Cors;
using PublicAPI.Shared;

namespace PublicAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TaiKhoanController : ApiController
    {
        [HttpPost]
        public async Task<HttpResponseMessage> Login()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);
                var formData = provider.FormData;

                string userName = formData.Get("UserName");
                string password = formData.Get("Password");

                TaiKhoan_wsv.TaiKhoan_wsv tk_wsv = new TaiKhoan_wsv.TaiKhoan_wsv();
                var rs = tk_wsv.Login(userName, password);

                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.ToString());
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> CheckLogin()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                var formData = provider.FormData;

                string loginCode = formData.Get("Token");

                TaiKhoan_wsv.TaiKhoan_wsv tk_wsv = new TaiKhoan_wsv.TaiKhoan_wsv();
                var rs = tk_wsv.CheckLogin(loginCode);

                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SearchPaging()
        {
            ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                var formData = provider.FormData;

                string loginCode = formData.Get("Token");
                int htID = SharedFunction.ParseID(formData.Get("HeThongID"));

                string strStartTime = formData.Get("StartTime");
                string strEndTime = formData.Get("EndTime");
                DateTime startTime = DateTime.Now;
                DateTime endTime = DateTime.Now;
                SharedFunction.ParseDualTime(strStartTime, strEndTime, ref startTime, ref endTime);

                int status = SharedFunction.ParseID(formData.Get("status"));

                string searchValue = formData.Get("SearchValue");
                int searchType = SharedFunction.ParseInt(formData.Get("SearchType"));
                int curPage = SharedFunction.ParseInt(formData.Get("CurPage"));
                int pageSize = SharedFunction.ParseID(formData.Get("PageSize"));
                int orderBy = SharedFunction.ParseInt(formData.Get("OrderBy"));
                bool isDes = SharedFunction.ParseBool(formData.Get("IsDescending"));
                //

                TaiKhoan_wsv.TaiKhoan_wsv tk_wsv = new TaiKhoan_wsv.TaiKhoan_wsv();
                var rs = tk_wsv.SearchPaging(loginCode, htID, startTime, endTime, status, searchValue, (TaiKhoan_wsv.EnumSearchType)searchType, curPage, pageSize, (TaiKhoan_wsv.EnumOrderBy)orderBy, isDes);

                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        //Create
        [HttpPost]
        public async Task<HttpResponseMessage> Create()
        {
            ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                var formData = provider.FormData;

                string loginCode = formData.Get("Token");
                int htID = SharedFunction.ParseID(formData.Get("HeThongID"));
                

                //
                string multiParam = formData.Get("MultiParam");
                //

                TaiKhoan_wsv.TaiKhoan_wsv tk_wsv = new TaiKhoan_wsv.TaiKhoan_wsv();

                var rs = tk_wsv.CheckLogin(loginCode);

                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        //Edit

        //Delete
        [HttpPost]
        public async Task<HttpResponseMessage> Delete()
        {
            ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                var formData = provider.FormData;

                string loginCode = formData.Get("Token");
                //
                int id = SharedFunction.ParseID(formData.Get("ID"));
                //

                TaiKhoan_wsv.TaiKhoan_wsv tk_wsv = new TaiKhoan_wsv.TaiKhoan_wsv();
                var rs = tk_wsv.Delete(loginCode, id);

                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> DeleteList()
        {
            ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                var formData = provider.FormData;

                string loginCode = formData.Get("Token");
                //
                string jsonListID = formData.Get("ListID");
                List<int> listID = new List<int>();
                try
                {
                    listID = JsonConvert.DeserializeObject<List<int>>(jsonListID);
                }
                catch(Exception ex)
                {
                    //Không parse đc thì chịu
                }
                //

                TaiKhoan_wsv.TaiKhoan_wsv tk_wsv = new TaiKhoan_wsv.TaiKhoan_wsv();
                var rs = tk_wsv.DeleteList(loginCode, listID.ToArray());

                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Detail()
        {
            ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                var formData = provider.FormData;

                string loginCode = formData.Get("Token");
                //
                int id = SharedFunction.ParseID(formData.Get("ID"));
                //

                TaiKhoan_wsv.TaiKhoan_wsv tk_wsv = new TaiKhoan_wsv.TaiKhoan_wsv();
                var rs = tk_wsv.GetByID(loginCode, id);

                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetListCombobox()
        {
            ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                var formData = provider.FormData;

                string loginCode = formData.Get("Token");
                int htID = SharedFunction.ParseID(formData.Get("HeThongID"));
                //
                string role = formData.Get("Role");
                //

                int roleID;
                switch (role)
                {
                    case "quanly":
                        roleID = 1;
                        break;
                    case "nhanvien":
                        roleID = 2;
                        break;
                    case "cuahang":
                        roleID = 3;
                        break;
                    case "nhacungcap":
                        roleID = 4;
                        break;
                    default:
                        roleID = 404;
                        break;
                }

                TaiKhoan_wsv.TaiKhoan_wsv tk_wsv = new TaiKhoan_wsv.TaiKhoan_wsv();

                var rs = tk_wsv.CheckLogin(loginCode);

                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        #region Sample action API
        [HttpPost]
        public async Task<HttpResponseMessage> Sample()
        {
            ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                var formData = provider.FormData;

                string loginCode = formData.Get("Token");
                //
                string multiParam = formData.Get("MultiParam");
                //

                TaiKhoan_wsv.TaiKhoan_wsv tk_wsv = new TaiKhoan_wsv.TaiKhoan_wsv();
                var rs = tk_wsv.CheckLogin(loginCode);

                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
        #endregion
    }
}