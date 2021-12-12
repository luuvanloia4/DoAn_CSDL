using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIs.Models.Entity
{
    public class DashBoard_ett
    {
        public int TongSoHD { get; set; }
        public int TongSoHDCho { get; set; }
        public int TongSoHDHoanThanh { get; set; }

        public int TongSoYC { get; set; }
        public int TongSoYCCho { get; set; }
        public int TongSoYCPheDuyet { get; set; }
        public int TongSoYCHoanThanh { get; set; }

        public int PhanQuyenID { get; set; }

        public DashBoard_ett()
        {
            TongSoHD = 0;
            TongSoHDCho = 0;
            TongSoHDHoanThanh = 0;

            TongSoYC = 0;
            TongSoYCCho = 0;
            TongSoYCHoanThanh = 0;
            TongSoYCPheDuyet = 0;

            PhanQuyenID = -1;
        }

        public DashBoard_ett(int tongSoHD, int tongSoHDCho, int tongSoHDHoanThanh, int tongSoYC, int tongSoYCCho, int tongSoYCPheDuyet, int tongSoYCHoanThanh, int phanQuyenID)
        {
            TongSoHD = tongSoHD;
            TongSoHDCho = tongSoHDCho;
            TongSoHDHoanThanh = tongSoHDHoanThanh;
            TongSoYC = tongSoYC;
            TongSoYCCho = tongSoYCCho;
            TongSoYCPheDuyet = tongSoYCPheDuyet;
            TongSoYCHoanThanh = tongSoYCHoanThanh;
            PhanQuyenID = phanQuyenID;
        }
    }
}