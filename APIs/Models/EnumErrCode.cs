using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIs.Models
{
    public enum EnumErrCode
    {
        Error,
        Fail,
        Success,
        Empty,
        NotYetLogin,
        ExistMultiOfUnique,
        DiffrentPass,
        AlreadyExist,
        InvalidEndDate,
        FailUploadImage,
        SuccessWithEmptyTokenFirebase,
        PermissionDenied,
        FailAddNotification,
        DoesNotExist,
        ValidateRequiment,
        SuccessWithFailSomething,
        NotHaveQuotaToRollback
    }
}