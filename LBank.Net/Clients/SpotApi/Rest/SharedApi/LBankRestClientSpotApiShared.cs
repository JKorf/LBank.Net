using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using LBank.Net.Enums;
using LBank.Net.Interfaces.Clients.SpotApi;
using LBank.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LBank.Net.Clients.SpotApi
{
    internal partial class LBankRestClientSpotSharedApi : 
        SharedApiBase,
        ILBankRestClientSpotApiShared,
        ILBankRestClientSpotSharedApi
    {
        private readonly LBankRestClientSpotApi _api;

        private const string _topicId = "LBankSpot";
        private const string _exchangeName = "LBank";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(LBankExchange.Metadata, this);

        private readonly static HashSet<string> _knownFiats = ["USD", "EUR", "BRL"];

        public LBankRestClientSpotSharedApi(LBankRestClientSpotApi api)
            : base(
                  SharedTransport.Rest,
                  api.Exchange,
                  [TradingMode.Spot],
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                GetAssetOptions,
                GetAllSpotTickersOptions,
                GetAssetOptions,
                GetBalancesOptions,
                GetBookTickerOptions,
                GetDepositAddressesOptions,
                GetDepositHistoryOptions,
                GetFeeOptions,
                GetKlinesOptions,
                GetOrderBookOptions,
                GetRecentTradesOptions,
                GetWithdrawalHistoryOptions,
                WithdrawOptions,
                GetSpotSymbolsOptions,
                GetSpotTickerOptions,
                GetAllSpotTickersOptions,
                PlaceSpotOrderOptions,
                GetSpotOrderOptions,
                GetOpenSpotOrdersOptions,
                GetClosedSpotOrdersOptions,
                GetSpotOrderTradesOptions,
                GetSpotUserTradeHistoryOptions,
                CancelSpotOrderOptions,
                GetSpotOrderByClientOrderIdOptions,
                CancelSpotOrderByClientOrderIdOptions
                );
        }

    }
}
