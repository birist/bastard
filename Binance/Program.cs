using System;

namespace Binance
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Binance Main");

            DOGE doge = new DOGE();
            doge.StartWebsocket();
        }
    }
}
