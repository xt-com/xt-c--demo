using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ApiDemo
{
    public class ApiReturnInfo
    {

        private bool success = true;

        /// <summary>
        /// 
        /// </summary>
        private HttpStatusCode statusCode = HttpStatusCode.OK;

        /// <summary>
        ///  success:true  msg is empty
        /// 
        /// </summary>
        private string msg = "";
        private string json;
        private string u;

        private long timStamp;
     
        /// <summary>
        /// 
        /// </summary>
        public bool Success { get => success; set => success = value; }

        public HttpStatusCode StatusCode { get => statusCode; set => statusCode = value; }

        public string Msg { get => msg; set => msg = value; }

        /// <summary>
        /// api url
        /// </summary>
        public string U { get => u; set => u = value; }

        /// <summary>
        /// the json of the  response 
        /// </summary>
        public string Json { get => json; set => json = value; }

        /// <summary>
        /// request
        /// 
        /// </summary>
        public long TimStamp { get => timStamp; set => timStamp = value; }

        

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            sb.Append("\"u\":\"" + u + "\",");
            sb.Append("\"d\":" + timStamp + ",");
            sb.Append("\"s\":" + (int)statusCode );
            sb.Append("}");
            return sb.ToString();
        }


    }
}
