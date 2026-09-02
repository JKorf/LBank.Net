using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using LBank.Net.Enums;
using LBank.Net.Interfaces.Clients.SpotApi;
using LBank.Net.Objects.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LBank.Net.Clients.SpotApi
{
    internal partial class LBankSocketClientSpotSharedApi :
        SharedApiBase,
        ILBankSocketClientSpotApiShared,
        ILBankSocketClientSpotSharedApi
    {
        private readonly LBankSocketClientSpotApi _api;

        private const string _topicId = "LBankSpot";
        private const string _exchangeName = "LBank";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(LBankExchange.Metadata, this);

        public LBankSocketClientSpotSharedApi(LBankSocketClientSpotApi api)
            : base(
                  api.Exchange,
                  [TradingMode.Spot],
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                SubscribeBalanceOptions,
                SubscribeKlineOptions,
                SubscribeTradeOptions,
                SubscribeOrderBookOptions,
                SubscribeTickerOptions,
                SubscribeSpotOrderOptions
                );
        }

    }
}
