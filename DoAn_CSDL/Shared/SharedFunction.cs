using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DoAn_CSDL.Shared
{
    public static class SharedFunction
    {
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
    }
}