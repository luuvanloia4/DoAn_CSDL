using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIs.Models
{
    public class SearchResult
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }

        public SearchResult()
        {
            //Do nothing
        }

        public SearchResult(string Title, string Image, string Link)
        {
            this.Title = Title;
            this.Image = Image;
            this.Link = Link;
        }
    }
}