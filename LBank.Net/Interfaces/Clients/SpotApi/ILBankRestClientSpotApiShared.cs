using CryptoExchange.Net.SharedApis;

namespace LBank.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Shared interface for Spot rest API usage
    /// </summary>
    public interface ILBankRestClientSpotApiShared :
        IAssetsRestClient,
        IBalanceRestClient,
        IBookTickerRestClient,
        IDepositRestClient,
        IFeeRestClient,
        IKlineRestClient,
        IOrderBookRestClient,
        IRecentTradeRestClient,
        IWithdrawalRestClient,
        IWithdrawRestClient,
        ISpotSymbolRestClient,
        ISpotTickerRestClient,
        ISpotOrderRestClient,
        ISpotOrderClientIdRestClient
    {
    }
}
