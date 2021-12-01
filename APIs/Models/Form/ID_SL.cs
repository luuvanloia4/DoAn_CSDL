using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIs.Models.Form
{
    public class ID_SL
    {
        public int ID { get; set; }
        public int SL { get; set; }

        public ID_SL()
        {
            //
        }

        public ID_SL(int id, int sl)
        {
            ID = id;
            SL = sl;
        }
    }
}