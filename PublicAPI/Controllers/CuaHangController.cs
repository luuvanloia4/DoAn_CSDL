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
    public class CuaHangController : ApiController
    {
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
                //
                string strStartTime = formData.Get("StartTime");
                string strEndTime = formData.Get("EndTime");
                DateTime startTime = DateTime.Now;
                DateTime endTime = DateTime.Now;
                SharedFunction.ParseDualTime(strStartTime, strEndTime, ref startTime, ref endTime);

                string searchValue = formData.Get("SearchValue");
                int searchType = SharedFunction.ParseInt(formData.Get("SearchType"));
                int curPage = SharedFunction.ParseInt(formData.Get("CurPage"));
                int pageSize = SharedFunction.ParseID(formData.Get("PageSize"));
                int orderBy = SharedFunction.ParseInt(formData.Get("OrderBy"));
                bool isDes = SharedFunction.ParseBool(formData.Get("IsDescending"));
                //

                CuaHang_wsv.CuaHang_wsv ch_wsv = new CuaHang_wsv.CuaHang_wsv();
                var rs = ch_wsv.SearchPaging(loginCode, htID, startTime, endTime, searchValue, (CuaHang_wsv.EnumSearchType)searchType, curPage, pageSize, (CuaHang_wsv.EnumOrderBy)orderBy, isDes);

                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetDetail()
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

                //
                int id = SharedFunction.ParseID(formData.Get("ID"));
                //

                CuaHang_wsv.CuaHang_wsv ch_wsv = new CuaHang_wsv.CuaHang_wsv();
                var rs = ch_wsv.GetByID(id);

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

                int htID = SharedFunction.ParseID(formData.Get("HeThongID"));

                CuaHang_wsv.CuaHang_wsv ch_wsv = new CuaHang_wsv.CuaHang_wsv();
                var rs = ch_wsv.GetListCombobox(htID);

                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

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

                CuaHang_wsv.CuaHang_wsv ch_wsv = new CuaHang_wsv.CuaHang_wsv();
                var rs = ch_wsv.Delete(loginCode, id);

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
                List<int> listID = JsonConvert.DeserializeObject<List<int>>(jsonListID);
                //

                CuaHang_wsv.CuaHang_wsv ch_wsv = new CuaHang_wsv.CuaHang_wsv();
                var rs = ch_wsv.DeleteList(loginCode, listID.ToArray());

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

                CuaHang_wsv.CuaHang_wsv ch_wsv = new CuaHang_wsv.CuaHang_wsv();
                var rs = ch_wsv.GetByID(-1);

                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
        #endregion
    }
}
