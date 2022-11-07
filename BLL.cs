using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDemo
{
    public class BLL
    {
        static string urlOrder = "/v4/order";
        static string urlOpen_order = "/v4/open-order";
        static string urlCancelBatchOrder = "/v4/batch-order";  
        static string uriBalance = "/v4/balance";


        public static string Order(string symbol, string side, string type, string timeInForce, string bizType, double price, double quantity)
        {

            StringBuilder data = new StringBuilder();


            data.Append("{\"symbol\":\"" + symbol + "\",");
            data.Append("\"side\":\"" + side + "\",");
            data.Append("\"type\":\"" + type + "\",");

            data.Append("\"timeInForce\":\"" + timeInForce + "\",");
            data.Append("\"bizType\":\"" + bizType + "\",");
            data.Append("\"price\":" + price + ",");
            data.Append("\"quantity\":" + quantity + "}");


            return HttpHelper.Post(urlOrder, data.ToString()).Json;
        }

        public static string GetOpenOrder()
        {
            return HttpHelper.Get(urlOpen_order, null).Json;
        }

        public static string GetBalance(string symbol)
        {
            string param = "currency="+symbol;

            return HttpHelper.Get(uriBalance, param).Json;

        }

        public static string DeleteOrder(string id)
        {
            string urlDelete = urlOrder + "/" + id;
            return HttpHelper.Delete(urlDelete).Json;
        }

        public static string CancelOrders(string orderIds)
        {

            string parm = "{\"orderIds\":" + orderIds + "}";

            return HttpHelper.Delete(urlCancelBatchOrder, parm).Json;

        }
    }
}
