using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace PublicAPI.Shared
{
    public static class SharedFunction
    {
        public static string[] ListImageAcceptType = new string[] { ".png", ".jpg", ".jpeg", ".gif" };

        public static int ParseInt(string str)
        {
            try
            {
                int value = int.Parse(str);
                return value;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        public static int ParseID(string str)
        {
            try
            {
                int value = int.Parse(str);
                return value;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        public static bool ParseBool(string str)
        {
            try
            {
                bool value = bool.Parse(str);
                return value;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public static DateTime ParseDate(string date)
        {
            DateTime result;
            try
            {
                result = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            catch
            {
                result = DateTime.ParseExact("2020-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            return result;
        }

        public static DateTime ParseDateTime(string dateTime)
        {
            DateTime result;
            try
            {
                result = DateTime.ParseExact(dateTime, "HH:mm:ss yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            catch
            {
                result = DateTime.ParseExact("00:00:00 2020-01-01", "HH:mm:ss yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            return result;
        }

        public static void ParseDualTime(string StartTime, string EndTime, ref DateTime startTime, ref DateTime endTime)
        {
            try
            {
                startTime = DateTime.ParseExact(StartTime, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            catch
            {
                startTime = DateTime.ParseExact("2020-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            try
            {
                endTime = DateTime.ParseExact(EndTime, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            catch
            {
                endTime = DateTime.Now;
            }

            endTime = endTime.AddDays(1);
        }

        public static HttpPostedFileBase GetFileByName(string fileName, HttpFileCollectionBase listUploadFile, string[] filterFileType)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string fileType = Path.GetExtension(fileName);
                bool accept = (filterFileType.Length > 0) ? filterFileType.Contains(fileType) : true;
                if (accept)
                {
                    for (int i = 0; i < listUploadFile.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(listUploadFile[i].FileName) && fileName.Contains(listUploadFile[i].FileName))
                        {
                            return listUploadFile[i];
                        }
                    }
                }
            }

            return null;
        }

        public static string SaveFile(HttpPostedFileBase file, string fullPath, string fileType)
        {
            try
            {
                string fileName = file.FileName.Split('.')[0] + Guid.NewGuid().ToString() + fileType;
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                file.SaveAs(fullPath + fileName);
                return fileName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}