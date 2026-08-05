using CryptoExchange.Net.SharedApis;

namespace LBank.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Shared interface for Spot socket API usage
    /// </summary>
    public interface ILBankSocketClientSpotApiShared :
        IBalanceSocketClient,
        IKlineSocketClient,
        ITradeSocketClient,
        IOrderBookSocketClient,
        ITickerSocketClient,
        ISpotOrderSocketClient
    {
    }
}
