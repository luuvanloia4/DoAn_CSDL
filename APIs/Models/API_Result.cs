using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIs.Models
{
    public class API_Result<T>
    {
        public T Data { get; set; }
        public EnumErrCode ErrCode { get; set; }
        public string ErrDes { get; set; }
        public int RecordCount { get; set; }
        public int PageCount { get; set; }

        //
        public API_Result()
        {
            ErrCode = EnumErrCode.Error;
            ErrDes = string.Empty;
            RecordCount = 0;
            PageCount = 0;
        }
    }
}