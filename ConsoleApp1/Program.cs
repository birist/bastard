using RestSharp;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ConsoleApp1
{
    class Program
    {

        static double USD = 15;
        static double taxes;
        static double CURRENCY = 0;
        static double sellAmount;
        static double buyAmount;
        static bool waitingOnBuy = false;
        static bool waitingOnSell = false;
        static string stock;
        static BitMart.Auth auth = new BitMart.Auth("3b3c027dabd3bf62e28a28bbe3b9e942b6234df0", "APIKEY", "c1c96d2596059a4641389d95664c5ea9e9b4cea4b72279a7c3231836f3cb1160");
        static long activeOrder;

        static void Main(string[] args)
        {
            //string response = BitMart.SpotTrading.PrivateAPI.placeLimitOrderSell(auth, "SAFEMOON_USDT", "714286", "0.000007");

            //OrderResponse order = JsonSerializer.Deserialize<OrderResponse>(response);

            //Console.WriteLine(order.data.order_id);
            //double balls = 0.00000700 - 0.00000001;

            //string amount = String.Format("{0:0.00000000}", balls);

            //double amount = 0.00000700;
            //string size = (USD / amount).ToString();
            //string size2 = amount.ToString("F8");
            //Console.WriteLine(size);
            //Console.WriteLine(size2);
            //int rounddown = Convert.ToInt32(Math.Floor((USD / 1.1025) / amount));
            //Console.WriteLine(rounddown);


            //BitMart.SpotTrading.PrivateAPI.placeLimitOrderSell(auth, "SAFEMOON_USDT", "714286", "0.000007"));
            //Console.WriteLine(BitMart.SpotTrading.PrivateAPI.cancelAllSellOrders(auth, "SAFEMOON_USDT"));
            //Console.WriteLine(BitMart.SpotTrading.PrivateAPI.getUserOrders(auth, "SAFEMOON_USDT", 1, 1, "8"));
            //Console.WriteLine(BitMart.SpotTrading.PrivateAPI.getOrderDetails(auth, "SAFEMOON_USDT", 6388141558));

            //Console.SetWindowSize(40, 10);
            //Console.SetBufferSize(40, 10);
            Console.WriteLine("Enter stock:");
            stock = Console.ReadLine();
            
            Depth depth = BitMart.SpotTrading.PublicAPI.getDepth(stock);
            sellAmount = double.Parse(depth.data.sells[0].price);
            buyAmount = double.Parse(depth.data.buys[0].price);
            

            buyOrder(buyAmount);
            waitingOnBuy = true;
            
            while(true)
            {
                depth = BitMart.SpotTrading.PublicAPI.getDepth(stock);
                Console.WriteLine("High:" + depth.data.sells[0].price);
                Console.WriteLine("Low:" + depth.data.buys[0].price);
                Console.WriteLine("------");
                if (waitingOnBuy == false && waitingOnSell == false)
                {
                    if (USD == 0)
                    {
                        sellAmount = double.Parse(depth.data.sells[0].price);
                        if (sellAmount < (buyAmount * 1.0027))
                            sellAmount = buyAmount * 1.0027;
                        sellOrder(sellAmount);
                        Console.WriteLine("Sell Order Made For " + sellAmount.ToString("F6"));
                        waitingOnSell = true;
                    }
                    if (CURRENCY == 0)
                    {
                        buyAmount = double.Parse(depth.data.buys[0].price);
                        if (buyAmount > (sellAmount / 1.0027))
                            buyAmount = sellAmount / 1.0027;

                        buyOrder(buyAmount);
                        Console.WriteLine("Buy Order Made For " + buyAmount.ToString("F6"));
                        waitingOnBuy = true;
                    }
                }
            
                double currentValue = double.Parse(getCurrentValue());
                Console.WriteLine("Current Value:" + currentValue);
                
                if (isOrderFufilled())
                {
                    if (waitingOnBuy == true)
                    {
                        CURRENCY = Convert.ToInt32(Math.Floor((USD / 1.0025) / buyAmount));
                        USD = 0;
                        waitingOnBuy = false;
                    }
                    if (waitingOnSell == true)
                    {
                        double gainsTax = ((CURRENCY * sellAmount) - (CURRENCY * buyAmount)) * 0.24;
                        taxes += gainsTax;
                        USD = USD + (CURRENCY * (double)sellAmount) - gainsTax;
                        CURRENCY = 0;
                        waitingOnSell = false;
                    }
                }
            
            
                if (waitingOnBuy == true)
                {
                    Console.WriteLine("Buy Order: " + buyAmount.ToString("F8"));
                    if (buyAmount < (double.Parse(depth.data.buys[0].price)))
                    {
                        if (buyAmount > (sellAmount / 1.0027))
                        {
                            buyAmount = sellAmount / 1.0027;
                        }
                        else
                        {
                            Console.WriteLine("Order Retracted!");
                            BitMart.SpotTrading.PrivateAPI.cancelAllBuyOrders(auth, stock);
                            buyAmount = double.Parse(depth.data.buys[0].price);
                            Console.WriteLine("New Buy Order: " + buyAmount.ToString("F6"));
                            buyOrder(buyAmount);
                        }
                    }
                }
            
                if (waitingOnSell == true)
                {
                    Console.WriteLine("Sell Order: " + sellAmount.ToString("F6"));
            
                    if (sellAmount > (double.Parse(depth.data.sells[0].price)))
                    {
                        if (sellAmount < (buyAmount * 1.0027))
                        {
                            sellAmount = buyAmount * 1.0027;
                        }
                        else
                        {
                            Console.WriteLine("Order Retracted!");
                            BitMart.SpotTrading.PrivateAPI.cancelAllSellOrders(auth, stock);
                            sellAmount = double.Parse(depth.data.sells[0].price);
                            Console.WriteLine("New Sell Order: " + sellAmount.ToString("F6"));
                            sellOrder(sellAmount);
                        }
                    }
                }
            
                Console.WriteLine("-------");
                Console.WriteLine("USD: $" + USD);
                Console.WriteLine(stock + ": " + CURRENCY);
                Console.WriteLine("Taxes: $" + taxes);
            
                System.Threading.Thread.Sleep(100);
            }
        }

        static void buyOrder(double amount)
        {
            int orderAmount = Convert.ToInt32(Math.Floor((USD / 1.0025) / amount));
            string size = orderAmount.ToString();
            string price = amount.ToString("F6");
            string response = BitMart.SpotTrading.PrivateAPI.placeLimitOrderBuy(auth, stock, size, price);
            OrderResponse order = JsonSerializer.Deserialize<OrderResponse>(response);
            activeOrder = order.data.order_id;
        }
        static void sellOrder(double amount)
        {
            string price = amount.ToString("F8");

            string response = BitMart.SpotTrading.PrivateAPI.placeLimitOrderSell(auth, stock, CURRENCY.ToString(), price);

            OrderResponse order = JsonSerializer.Deserialize<OrderResponse>(response);
            activeOrder = order.data.order_id;
        }

        static string getCurrentValue()
        {
            string jsonString;
            string URL = "https://api-cloud.bitmart.com/spot/v1/ticker?symbol=" + stock;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }
            Ticker ticker = JsonSerializer.Deserialize<Ticker>(jsonString);
            return ticker.data.tickers[0].last_price;
        }


        static bool isOrderFufilled()
        {
            string response = BitMart.SpotTrading.PrivateAPI.getOrderDetails(auth, stock, activeOrder);
        
            OrderDetailResponse activeOrderDetail = JsonSerializer.Deserialize<OrderDetailResponse>(response);

            string status = activeOrderDetail.data.status;

            if (string.Compare(status, "6") == 0)
            {
                Console.WriteLine("ORDER FUFILLED!");
                return true;
            }  
            else
                return false;
        }
    }
}
