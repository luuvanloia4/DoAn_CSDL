using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_CSDL.Shared
{
    public class Constants
    {
        //Session
        public const string LoginCode_SessionName = "LoginCode";
        public const string UserID_SessionName = "UseID";
        public const string UserRole_SessionName = "UserRole";
        public const string ListSP_SessionName = "ListSP";

        //File location
        //Default file:
        public const string DefaultImage = "/Data/System/Images/img_error.png";
        public const string DefaultAvatar = "/Data/System/Images/default_image.jpg";

        //Upload
        public const string RootPath = "/Data/";
        public const string UploadRootPath = RootPath + "Upload/";
        public const string ImageUploadPath = UploadRootPath + "Images/";
        public const string UserImageUploadPath = ImageUploadPath + "User/";
        public const string SystemImageUploadPath = ImageUploadPath + "System/";
        public const string ProductImageUploadPath = ImageUploadPath + "Product/";
        public const string BannerImageUploadPath = ImageUploadPath + "Banner/";

        //Export
        //...
    }
}