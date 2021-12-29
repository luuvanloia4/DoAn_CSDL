using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIs.Models.Entity
{
    public class GroupID_ett
    {
        public int HeThongID { get; set; }
        public int NhaCungCapID { get; set; }
        public int CuaHangID { get; set; }

        public GroupID_ett()
        {
            //
        }

        public GroupID_ett(int heThongID, int nhaCungCapID, int cuaHangID)
        {
            HeThongID = heThongID;
            NhaCungCapID = nhaCungCapID;
            CuaHangID = cuaHangID;
        }
    }
}