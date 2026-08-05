namespace LBank.Net.Objects
{
    /// <summary>
    /// Api addresses
    /// </summary>
    public class LBankApiAddresses
    {
        /// <summary>
        /// The address used by the LBankRestClient for the Spot API
        /// </summary>
        public string RestClientSpotAddress { get; set; } = "";
        /// <summary>
        /// The address used by the LBankSocketClient for the websocket API
        /// </summary>
        public string SocketClientSpotAddress { get; set; } = "";
        /// <summary>
        /// The address used by the LBankSocketClient for the websocket V3 API
        /// </summary>
        public string SocketClientV3SpotAddress { get; set; } = "";

        /// <summary>
        /// The default addresses to connect to the LBank API
        /// </summary>
        public static LBankApiAddresses Default = new LBankApiAddresses
        {
            RestClientSpotAddress = "https://api.lbank.info",
            SocketClientSpotAddress = "wss://www.lbkex.net",
            SocketClientV3SpotAddress = "wss://www.lbank.com"
        };
    }
}
