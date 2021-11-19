using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIs.Models
{
    public enum EnumSearchType
    {
        All = 0,
        ID,
        Name,
        UserName,
        ParentID,
        Phone,
        ListAll,
        Permission
    }
}