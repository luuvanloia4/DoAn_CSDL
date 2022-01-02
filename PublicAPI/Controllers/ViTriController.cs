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
    public class VitriController : ApiController
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

                int khoID = SharedFunction.ParseID(formData.Get("KhoID"));
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

                ViTri_wsv.ViTri_wsv vt_wsv = new ViTri_wsv.ViTri_wsv();
                var rs = vt_wsv.SearchPaging(khoID, startTime, endTime, searchValue, (ViTri_wsv.EnumSearchType)searchType, curPage, pageSize, (ViTri_wsv.EnumOrderBy)orderBy, isDes);

                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        //[HttpPost]
        //public async Task<HttpResponseMessage> GetDetail()
        //{
        //    ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }

        //    string root = HttpContext.Current.Server.MapPath("~/App_Data");
        //    var provider = new MultipartFormDataStreamProvider(root);

        //    try
        //    {
        //        await Request.Content.ReadAsMultipartAsync(provider);
        //        var formData = provider.FormData;

        //        //
        //        int id = SharedFunction.ParseID(formData.Get("ID"));
        //        //

        //        Kho_wsv.Kho_wsv kho_wsv = new Kho_wsv.Kho_wsv();
        //        var rs = kho_wsv.GetByID(id);

        //        return Request.CreateResponse(HttpStatusCode.OK, rs);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
        //    }
        //}

        //[HttpPost]
        //public async Task<HttpResponseMessage> GetListCombobox()
        //{
        //    ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }

        //    string root = HttpContext.Current.Server.MapPath("~/App_Data");
        //    var provider = new MultipartFormDataStreamProvider(root);

        //    try
        //    {
        //        await Request.Content.ReadAsMultipartAsync(provider);
        //        var formData = provider.FormData;

        //        int htID = SharedFunction.ParseID(formData.Get("HeThongID"));

        //        Kho_wsv.Kho_wsv kho_wsv = new Kho_wsv.Kho_wsv();
        //        var rs = kho_wsv.GetListCombobox(htID);

        //        return Request.CreateResponse(HttpStatusCode.OK, rs);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
        //    }
        //}

        ////Delete
        //[HttpPost]
        //public async Task<HttpResponseMessage> Delete()
        //{
        //    ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }

        //    string root = HttpContext.Current.Server.MapPath("~/App_Data");
        //    var provider = new MultipartFormDataStreamProvider(root);

        //    try
        //    {
        //        await Request.Content.ReadAsMultipartAsync(provider);
        //        var formData = provider.FormData;

        //        string loginCode = formData.Get("Token");
        //        //
        //        int id = SharedFunction.ParseID(formData.Get("ID"));
        //        //

        //        Kho_wsv.Kho_wsv kho_wsv = new Kho_wsv.Kho_wsv();
        //        var rs = kho_wsv.Delete(loginCode, id);

        //        return Request.CreateResponse(HttpStatusCode.OK, rs);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
        //    }
        //}

        //[HttpPost]
        //public async Task<HttpResponseMessage> DeleteList()
        //{
        //    ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }

        //    string root = HttpContext.Current.Server.MapPath("~/App_Data");
        //    var provider = new MultipartFormDataStreamProvider(root);

        //    try
        //    {
        //        await Request.Content.ReadAsMultipartAsync(provider);
        //        var formData = provider.FormData;

        //        string loginCode = formData.Get("Token");
        //        //
        //        string jsonListID = formData.Get("ListID");
        //        List<int> listID = JsonConvert.DeserializeObject<List<int>>(jsonListID);
        //        //

        //        Kho_wsv.Kho_wsv kho_wsv = new Kho_wsv.Kho_wsv();
        //        var rs = kho_wsv.DeleteList(loginCode, listID.ToArray());

        //        return Request.CreateResponse(HttpStatusCode.OK, rs);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
        //    }
        //}

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

                ViTri_wsv.ViTri_wsv vt_wsv = new ViTri_wsv.ViTri_wsv();
                var rs = vt_wsv.GetByID(-1);

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
