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

    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface ILBankRestClientSpotSharedApi :
        IGetAssetEndpoint,
        IGetBalancesEndpoint,
        IGetBookTickerEndpoint,
        IGetDepositAddressesEndpoint,
        IGetDepositHistoryEndpoint,
        IGetFeesEndpoint,
        IGetKlinesEndpoint,
        IGetOrderBookEndpoint,
        IGetRecentTradesEndpoint,
        IGetWithdrawalHistoryEndpoint,
        IWithdrawEndpoint,
        IGetSpotSymbolsEndpoint,
        IGetSpotTickerEndpoint,
        IGetAllSpotTickersEndpoint,
        IPlaceSpotOrderEndpoint,
        IGetSpotOrderEndpoint,
        IGetOpenSpotOrdersEndpoint,
        IGetClosedSpotOrdersEndpoint,
        IGetSpotOrderTradesEndpoint,
        IGetSpotUserTradeHistoryEndpoint,
        ICancelSpotOrderEndpoint,
        IGetSpotOrderByClientOrderIdEndpoint,
        ICancelSpotOrderByClientOrderIdEndpoint
    {
    }
}
