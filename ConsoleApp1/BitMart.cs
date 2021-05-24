using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Globalization;
using System.Security.Cryptography;
using RestSharp;

namespace ConsoleApp1
{
    /// <summary>
    /// BitMart API Driver for use with BitMart trading
    /// <para><see href="https://developer-pro.bitmart.com/"/></para>
    /// </summary>
    public static class BitMart
    {
        /// <summary>
        /// Auth clas containing AccessKey, Memo and SecretKey parameters for use with BitMart API secure signed requests
        /// </summary>
        public class Auth
        {
            public string AccessKey;
            string SecretKey;
            string Memo;

            public Auth(string accesskey, string memo, string secretkey)
            {
                AccessKey = accesskey;
                Memo = memo;
                SecretKey = secretkey;
            }

            /// <summary>
            /// generates signed hash value using TIMESTAMP, QUERYSTRING and the AUTH.SECRETKEY
            /// </summary>
            /// <returns>returns signed hash value as string</returns>
            public string generateSigned(string timestamp, string queryString)
            {
                string message = timestamp + "#" + Memo + "#" + queryString;

                UTF8Encoding encoding = new UTF8Encoding();
                byte[] keyBytes = encoding.GetBytes(SecretKey);
                byte[] messageBytes = encoding.GetBytes(message);
                HMACSHA256 cryptographer = new HMACSHA256(keyBytes);

                byte[] hashBytes = cryptographer.ComputeHash(messageBytes);

                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// Sends post request with signed authentication to BitMarts API
        /// </summary>
        /// <returns>Returns (string) server response to order request</returns>
        public static string sendSignedPostRequest(string URL, Auth auth, string body)
        {
            string timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            RestClient client = new RestClient(URL);

            client.Timeout = -1;

            RestRequest request = new RestRequest(Method.POST);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("X-BM-KEY", auth.AccessKey);
            request.AddHeader("X-BM-SIGN", auth.generateSigned(timestamp, body));
            request.AddHeader("X-BM-TIMESTAMP", timestamp);
            request.AddJsonBody(body);

            IRestResponse response = client.Execute(request);
            
            return response.Content;
        }

        public static string sendSignedGetRequest(string URL, Auth auth, string body)
        {
            string timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            RestClient client = new RestClient(URL);

            client.Timeout = -1;

            RestRequest request = new RestRequest(Method.GET);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("X-BM-KEY", auth.AccessKey);
            request.AddHeader("X-BM-SIGN", auth.generateSigned(timestamp, body));
            request.AddHeader("X-BM-TIMESTAMP", timestamp);

            IRestResponse response = client.Execute(request);

            return response.Content;
        }
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

            public static class PrivateAPI
            {
                //TODO:Make this return either a JSON object or an order ID. It would be useful to be able to directly use the order ID to reference if it has been completed or not.
                /// <summary>
                /// Place BitMart Order to buy SIZE amount of SYMBOL at current market value
                /// <para><see href="https://developer-pro.bitmart.com/en/spot/order/submit_order.html"/></para>
                /// </summary>
                /// <returns>Returns (string) server response to order request</returns>
                public static string placeMarketOrderBuy(Auth auth, string symbol, string size)
                {
                    string body = "{\"" +
                        "symbol\":\"" + symbol + "\",\"" +
                        "side\":\"buy\",\"" +
                        "type\":\"market\",\"" +
                        "size\":\"" + size + "\"}";

                    return sendSignedPostRequest("https://api-cloud.bitmart.com/spot/v1/submit_order", auth, body);
                }

                /// <summary>
                /// Place BitMart Order to sell SIZE amount of SYMBOL at current market value
                /// <para><see href="https://developer-pro.bitmart.com/en/spot/order/submit_order.html"/></para>
                /// </summary>
                /// <returns>Returns (string) server response to order request</returns>
                public static string placeMarketOrderSell(Auth auth, string symbol, string notional)
                {
                    string body = "{\"" +
                        "symbol\":\"" + symbol + "\",\"" +
                        "side\":\"buy\",\"" +
                        "type\":\"market\",\"" +
                        "size\":\"" + notional + "\"}";

                    return sendSignedPostRequest("https://api-cloud.bitmart.com/spot/v1/submit_order", auth, body);
                }

                /// <summary>
                /// Place BitMart Order to buy SIZE amount of SYMBOL at PRICE value
                /// <para><see href="https://developer-pro.bitmart.com/en/spot/order/submit_order.html"/></para>
                /// </summary>
                /// <returns>Returns (string) server response to order request</returns>
                public static string placeLimitOrderBuy(Auth auth, string symbol, string size, string price)
                {
                    string body = "{\"" +
                        "symbol\":\"" + symbol + "\",\"" +
                        "side\":\"buy\",\"" +
                        "type\":\"limit\",\"" +
                        "size\":\"" + size + "\",\"" +
                        "price\":\"" + price + "\"}";

                    return sendSignedPostRequest("https://api-cloud.bitmart.com/spot/v1/submit_order", auth, body);
                }

                /// <summary>
                /// Place BitMart Order to sell SIZE amount of SYMBOL at PRICE value
                /// <para><see href="https://developer-pro.bitmart.com/en/spot/order/submit_order.html"/></para>
                /// </summary>
                /// <returns>Returns (string) server response to order request</returns>
                public static string placeLimitOrderSell(Auth auth, string symbol, string size, string price)
                {
                    string body = "{\"" +
                        "symbol\":\"" + symbol + "\",\"" +
                        "side\":\"sell\",\"" +
                        "type\":\"limit\",\"" +
                        "size\":\"" + size + "\",\"" +
                        "price\":\"" + price + "\"}";

                    return sendSignedPostRequest("https://api-cloud.bitmart.com/spot/v1/submit_order", auth, body);
                }

                /// <summary>
                /// send BitMart request to cancel order ORDER_ID of currency SYMBOL
                /// <para><see href="https://developer-pro.bitmart.com/en/spot/order/cancel_order.html"/></para>
                /// </summary>
                /// <returns>Returns (string) server response to request</returns>
                public static string cancelOrder(Auth auth, string symbol, long order_id)
                {
                    string body = "{\"" +
                       "symbol\":\"" + symbol + "\",\"" +
                       "order_id\":" + order_id + "}";

                    return sendSignedPostRequest("https://api-cloud.bitmart.com/spot/v2/cancel_order", auth, body);
                }

                /// <summary>
                /// send BitMart request to cancel all buy orders of currency SYMBOL
                /// <para><see href="https://developer-pro.bitmart.com/en/spot/order/cancel_orders.html"/></para>
                /// </summary>
                /// <returns>Returns (string) server response to request</returns>
                public static string cancelAllBuyOrders(Auth auth, string symbol)
                {
                    string body = "{\"" +
                       "symbol\":\"" + symbol + "\",\"" +
                       "side\":\"buy\"}";

                    return sendSignedPostRequest("https://api-cloud.bitmart.com/spot/v1/cancel_orders", auth, body);
                }

                /// <summary>
                /// send BitMart request to cancel all sell orders of currency SYMBOL
                /// <para><see href="https://developer-pro.bitmart.com/en/spot/order/cancel_orders.html"/></para>
                /// </summary>
                /// <returns>Returns (string) server response to request</returns>
                public static string cancelAllSellOrders(Auth auth, string symbol)
                {
                    string body = "{\"" +
                       "symbol\":\"" + symbol + "\",\"" +
                       "side\":\"sell\"}";

                    return sendSignedPostRequest("https://api-cloud.bitmart.com/spot/v1/cancel_orders", auth, body);
                }

                /// <summary>
                /// send BitMart request to get order details for order ORDER_ID of currency SYMBOL
                /// <para><see href="https://developer-pro.bitmart.com/en/spot/order/order_detail.html"/></para>
                /// </summary>
                /// <returns>Returns (string) server response to request</returns>
                public static string getOrderDetails(Auth auth, string symbol, long order_id)
                {
                    string body =
                       "symbol=" + symbol +
                       "&order_id=" + order_id;

                    return sendSignedGetRequest("https://api-cloud.bitmart.com/spot/v1/order_detail" + "?" + body, auth, body);
                }

                //TODO: break this into multiple functions based on symbols.
                /// <summary>
                /// send BitMart request to get order history for currency SYMBOL of type STATUS on page OFFSET with LIMIT number of orders per pagedetails for order ORDER_ID of currency SYMBOL
                /// <para>STATUS IDICATORS:</para>
                ///<para>1 = Order failure</para>
                ///<para>2 = Placing order</para>
                ///<para>3 = Order failure, Freeze failure</para>
                ///<para>4 = Order success, Pending for fulfilment</para>
                ///<para>5 = Partially filled</para>
                ///<para>6 = Fully filled</para>
                ///<para>7 = Canceling</para>
                ///<para>8 = Canceled</para>
                ///<para>9 = Outstanding (4 and 5)</para>
                ///<para>10 = 6 and 8</para>
                /// <para><see href="https://developer-pro.bitmart.com/en/spot/order/orders.html"/></para>
                /// </summary>
                /// <returns>Returns (string) server response to request</returns>
                public static string getUserOrders(Auth auth, string symbol, int offset, int limit, string status)
                {
                    string body = 
                        "symbol=" + symbol + 
                        "&status=" + status + 
                        "&offset=" + offset + 
                        "&limit=" + limit;

                    return sendSignedGetRequest("https://api-cloud.bitmart.com/spot/v1/orders" + "?" + body, auth, body);
                }

                public static void getUserTrades()
                {

                }

            }
            public static class PublicAPI
            {
                //TODO:getCurrency
                //TODO:getSymbols
                //TODO:getSymbolDetails
                //TODO:getTicker
                //TODO:getKlineSteps
                //TODO:getKline
                
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

        public static class GenerateBMSecret
        {
            public static string Generate()
            {
                string secretKey = "Your Secret Key";
                string memo = "Your Memo";
                string queryString = "{\"symbol\":\"SAFEMOON_USDT\",\"side\":\"buy\",\"type\":\"limit\",\"size\":\"10\",\"price\":\"0.00000520\"}";
                string memoPlusQueryString = "#" + memo + "#" + queryString;
                long unixSeconds = DateTimeOffset.Now.ToUnixTimeSeconds();
                string unixSecondsString = unixSeconds.ToString() + "000";
                string secretArg1 = unixSecondsString + memoPlusQueryString;

                string X_BM_SIGN = HmacSha256Digest(secretArg1, secretKey);

                string[] lines = { unixSecondsString, X_BM_SIGN };
                OutputToFile(lines);

                Console.WriteLine("Secret Key: " + secretKey);
                Console.WriteLine("Memo: " + memo);
                Console.WriteLine("queryString: " + queryString);
                Console.WriteLine("# + Memo + # + queryString: " + memoPlusQueryString);
                Console.WriteLine("Unix Timestamp in Seconds: " + unixSeconds);
                Console.WriteLine("Unix Timestamp String in Seconds: " + unixSecondsString);
                Console.WriteLine("Secret Argument #1: " + secretArg1);
                Console.WriteLine("\nX-BM-SIGN: " + X_BM_SIGN);

                return "";
            }

            public static void OutputToFile(string[] lines)
            {
                using (StreamWriter outputFile = new StreamWriter("Your File Path\\TimeStampAndSecret.txt"))
                {
                    foreach (string line in lines)
                        outputFile.WriteLine(line);
                }
            }
           
            // Helper Functions - Currently unused
            private static byte[] StringEncode(string text)
            {
                var encoding = new UTF8Encoding();
                return encoding.GetBytes(text);
        }

            private static string HashEncode(byte[] hash)
            {
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }

            private static byte[] HexDecode(string hex)
            {
                var bytes = new byte[hex.Length / 2];
                for (int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber);
                }
                return bytes;
            }
        };

        public static string HmacSha256Digest(this string message, string secret)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] keyBytes = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            HMACSHA256 cryptographer = new HMACSHA256(keyBytes);

            byte[] bytes = cryptographer.ComputeHash(messageBytes);

            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }


}
