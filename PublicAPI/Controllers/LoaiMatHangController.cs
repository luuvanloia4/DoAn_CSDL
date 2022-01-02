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
    public class LoaiMatHangController : ApiController
    {
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

                LoaiMatHang_wsv.LoaiMatHang_wsv lmh_wsv = new LoaiMatHang_wsv.LoaiMatHang_wsv();
                var rs = lmh_wsv.GetListCombobox();

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

                Kho_wsv.Kho_wsv kho_wsv = new Kho_wsv.Kho_wsv();
                var rs = kho_wsv.GetByID(-1);

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
