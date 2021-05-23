using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Globalization;
using System.Security.Cryptography;

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
