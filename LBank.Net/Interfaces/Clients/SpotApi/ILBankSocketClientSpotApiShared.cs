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

    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface ILBankSocketClientSpotSharedApi :
        ISubscribeBalancesOperation,
        ISubscribeKlinesOperation,
        ISubscribeTradesOperation,
        ISubscribeOrderBookOperation,
        ISubscribeTickerOperation,
        ISubscribeSpotOrdersOperation
    { }
}
