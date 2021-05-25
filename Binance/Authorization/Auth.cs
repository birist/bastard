using System.IO;
using Binance.Net;
using Binance.Net.Objects.Spot;
using CryptoExchange.Net.Authentication;

namespace Binance.Authorization
{
    class Auth
    {
        private string credentialsPath = Path.GetFullPath(@"Authorization\APICredentials.json");
        Auth()
        {
            using (var stream = File.OpenRead(credentialsPath))
            {
                BinanceClient.SetDefaultOptions(new BinanceClientOptions
                {
                    ApiCredentials = new ApiCredentials(stream)

                });
            }
        }
    }
}
