using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDemo
{
   public class TimeHelper
    {
        private static DateTime BEGINNING_TIME = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DateTime Long2DateTime(long time)
        {
            return BEGINNING_TIME.AddMilliseconds(time).ToLocalTime();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static long DateTime2Long(DateTime dt)
        {
            return (long)(dt.ToUniversalTime() - BEGINNING_TIME).TotalMilliseconds;
        }
    }
}
