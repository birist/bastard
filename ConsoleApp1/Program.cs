using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ConsoleApp1
{
    class Program
    {
        static double USD = 500;
        static double taxes;
        static double safeMoon = 0;
        static double lastAmount;
        static double sellAmount;
        static double buyAmount;
        static bool waitingOnBuy = false;
        static bool waitingOnSell = false;
        static string stock;
        static void Main(string[] args)
        {
            Console.SetWindowSize(40, 10);
            Console.SetBufferSize(40, 10);
            Console.WriteLine("Enter stock:");
            stock = Console.ReadLine();

            Depth depth = BitMart.SpotTrading.PublicAPI.getDepth(stock);
            sellAmount = double.Parse(depth.data.sells[0].price);
            buyAmount = double.Parse(depth.data.buys[0].price);

            lastAmount = double.Parse(getCurrentValue());
            Buy(buyAmount);

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
                        Console.WriteLine("Sell Order Made For " + sellAmount);
                        waitingOnSell = true;
                    }
                    if (safeMoon == 0)
                    {
                        buyAmount = double.Parse(depth.data.buys[0].price);
                        Console.WriteLine("Buy Order Made For " + buyAmount);
                        waitingOnBuy = true;
                    }
                }

                double currentValue = double.Parse(getCurrentValue());
                Console.WriteLine("Current Value:" + currentValue);

                if (waitingOnBuy == true)
                {
                    Console.WriteLine("Buy Order: " + buyAmount);
                    if (buyAmount < double.Parse(depth.data.buys[0].price))
                    {
                        Console.WriteLine("Order Retracted!");
                        Console.WriteLine("New Buy Order: " + buyAmount);
                        buyAmount = double.Parse(depth.data.buys[0].price);
                    }

                    if (currentValue <= buyAmount)
                    {
                        Buy(buyAmount);
                        waitingOnBuy = false;
                        Console.WriteLine("Buy Made At " + buyAmount);
                    }
                }

                if (waitingOnSell == true)
                {
                    Console.WriteLine("Sell Order: " + sellAmount);

                    if (sellAmount > double.Parse(depth.data.sells[0].price))
                    {
                        Console.WriteLine("Order Retracted!");
                        Console.WriteLine("New Sell Order: " + sellAmount);
                        sellAmount = double.Parse(depth.data.sells[0].price);
                    }

                    if (currentValue >= sellAmount)
                    {
                        Sell(sellAmount);
                        waitingOnSell = false;
                        Console.WriteLine("Sale Made At " + sellAmount);
                    }
                }

                Console.WriteLine("-------");
                Console.WriteLine("USD: $" + USD);
                Console.WriteLine(stock + ": " + safeMoon);
                Console.WriteLine("Taxes: $" + taxes);

                System.Threading.Thread.Sleep(1500);
            }
        }

        static void Buy(double amount)
        {
            safeMoon = USD / amount;
            USD = 0;
            lastAmount = amount;
        }
        static void Sell(double amount)
        {
            double gainsTax = ((safeMoon * sellAmount) - (safeMoon * buyAmount)) * 0.24;

            taxes += gainsTax;
            USD = ((double)safeMoon * (double) amount) - gainsTax;
            safeMoon = 0;
            lastAmount = amount;
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
    }
}
