using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIs.Models.Entity
{
    public class ListCombobox_ett<T>
    {
        public T Data { get; set; }
        public string DisplayData { get; set; }

        public ListCombobox_ett()
        {
            //
        }

        public ListCombobox_ett(T data, string displayData)
        {
            this.Data = data;
            this.DisplayData = displayData;
        }
    }
}