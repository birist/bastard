using System;
using System.Collections.Generic;
using System.Text;

namespace Binance
{
    // NOTE: With the current level of complexity(or lack there of), this class does not offer much utility.
    //       Perhaps expand this to have specific Starts and Stops for different Symbol Websockets, etc. 
    //       This approach could help declutter Program.cs. Plus, we can create multiple Doge objects that are used for their own purposes in Program.cs, like getting price, or creating orders. 
    public class DOGE
    {
        private Websockets.DOGEUSDT dogeSocket;

        // Quick Test: Automatically subscribe to DOGEUSDT Socket and start outputting prices
        public void StartWebsocket()
        {
            dogeSocket = new Websockets.DOGEUSDT();
            dogeSocket.Subscribe();
        }

        public void StopWebsocket()
        {
            dogeSocket.Unsubscribe();
        }

    }
}
