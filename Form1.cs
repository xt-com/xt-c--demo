using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ApiDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          //  { "rc":0,"mc":"SUCCESS","ma":[],"result":{ "orderId":"158415965735324800"} }
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //{"rc":0,"mc":"SUCCESS","ma":[],"result":{"orderId":"158667522741901824"}}

            //   string s = BLL.Order("XT_USDT", "BUY", "LIMIT", "GTC", "SPOT", 3, 2);


            //{"rc":0,"mc":"SUCCESS","ma":[],
            //"result":[
            //{"symbol":"xt_usdt","orderId":"158667522741901824","clientOrderId":null,"baseCurrency":"xt","quoteCurrency":"usdt","side":"BUY","type":"LIMIT","timeInForce":"GTC","price":"3.0000","origQty":"2.00","origQuoteQty":"6.0000","executedQty":"0.00","leavingQty":"2.00","tradeBase":"0.00","tradeQuote":"0.0000","avgPrice":null,"fee":null,"feeCurrency":null,"closed":false,"state":"NEW","time":1667531285321,"updatedTime":1667531285445},
            //{"symbol":"xt_usdt","orderId":"158418031732034688","clientOrderId":null,"baseCurrency":"xt","quoteCurrency":"usdt","side":"BUY","type":"LIMIT","timeInForce":"GTC","price":"3.0000","origQty":"2.00","origQuoteQty":"6.0000","executedQty":"0.00","leavingQty":"2.00","tradeBase":"0.00","tradeQuote":"0.0000","avgPrice":null,"fee":null,"feeCurrency":null,"closed":false,"state":"NEW","time":1667471802029,"updatedTime":1667471802274},

            string s1 = BLL.GetOpenOrder();


            //  { "rc":0,"mc":"SUCCESS","ma":[],
            //  "result":{ "currency":"usdt","currencyId":11,"frozenAmount":"76.00000000",
            //  "availableAmount":"1137.04360000","totalAmount":"1213.04360000","convertBtcAmount":"0.05975669"} }

            // string s2 = BLL.GetBalance("usdt");




            // { "rc":0,"mc":"SUCCESS","ma":[],"result":{ "cancelId":"158670246808132109"} }
            // string s3 = BLL.DeleteOrder("158667522741901824");


           // { "rc":0,"mc":"SUCCESS","ma":[],"result":null}
          //  string s4 = BLL.CancelOrders("[156112903603534976,156125761536048256]");


        }
    }
}
