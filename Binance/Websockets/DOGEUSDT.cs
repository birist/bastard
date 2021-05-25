using System;
using System.Collections.Generic;
using System.Text;
using Binance.Net;

namespace Binance.Websockets
{
    public class DOGEUSDT
    {
        private BinanceSocketClient client;

        public DOGEUSDT()
        {
            client = new BinanceSocketClient();
        }

        public BinanceSocketClient Client { get => client; set => client = value; }

        public void Subscribe()
        {
            try
            {
                // TODO - Debug this shit. Is the default Websocket URL correct? Is this going out of scope before it can do anything useful?
                client.Spot.SubscribeToBookTickerUpdates("DOGEUSDT", data => {
                    // Handle data
                    Console.WriteLine("Symbol: " + data.Symbol);
                    Console.WriteLine("Best Bid Price: " + data.BestBidPrice);
                    Console.WriteLine("Best Bid Quantity: " + data.BestBidQuantity);
                    Console.WriteLine("Best Ask Price : " + data.BestAskPrice);
                    Console.WriteLine("Best Ask Quantity: " + data.BestAskQuantity);
                });

            }
            catch(Exception e)
            {
                Console.WriteLine("Exception while subscribing to DOGEUSDT: " + e);
            }
        }

        public void Unsubscribe()
        {
            client.UnsubscribeAll();
        }
    }
}
