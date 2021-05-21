using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ConsoleApp1
{
    public static class BitMart
    {
        //public static class SystemStatus
        //{
        //
        //}
        //public static class FundingAccount
        //{
        //
        //}
        public static class SpotTrading
        {
            public static class PublicAPI
            {
                //getCurrency
                //getSymbols
                //getSymbolDetails
                //getTicker
                //getKlineSteps
                //getKline
                
                public static Depth getDepth(string symbol)
                {
                    //return getDepth(symbol, "8", 50);

                    string jsonString;
                    string URL = "https://api-cloud.bitmart.com/spot/v1/symbols/book?symbol=" + symbol;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        jsonString = reader.ReadToEnd();
                    }
                    return JsonSerializer.Deserialize<Depth>(jsonString);
                }
                public static Depth getDepth(string symbol, string precision)
                {
                    return getDepth(symbol, precision, 50);
                }
                public static Depth getDepth(string symbol, int size)
                {
                    return getDepth(symbol, "8", size);
                }
                public static Depth getDepth(string symbol, string precision, int size)
                {
                    string jsonString;
                    string URL = "https://api-cloud.bitmart.com/spot/v1/symbols/book?symbol=" + symbol + "&precision=" + precision + "&size=" + size;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        jsonString = reader.ReadToEnd();
                    }
                    return JsonSerializer.Deserialize<Depth>(jsonString);
                }

                //getTrades
            }
           
        }

    }


}
