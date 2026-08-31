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
    internal class LBankSocketClientSpotSharedApi :
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

        #region Balance client
        public SubscribeBalanceOptions SubscribeBalanceOptions { get; } = new SubscribeBalanceOptions(_exchangeName, true);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(SubscribeBalancesRequest request, Action<DataEvent<SharedBalance[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeBalanceOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await _api.SubscribeToBalanceUpdatesAsync(
                null,
                update => handler(update.ToType<SharedBalance[]>(new[] {
                    new SharedBalance(
                        SupportedTradingModes,
                        update.Data.Asset,
                        update.Data.Free,
                        update.Data.Free + update.Data.Frozen) })),
                ct: ct).ConfigureAwait(false);
            return result;
        }
        #endregion

        #region Kline client
        public SubscribeKlineOptions SubscribeKlineOptions { get; } = new SubscribeKlineOptions(_exchangeName, false,
            SharedKlineInterval.OneMinute,
            SharedKlineInterval.FiveMinutes,
            SharedKlineInterval.FifteenMinutes,
            SharedKlineInterval.ThirtyMinutes,
            SharedKlineInterval.OneHour,
            SharedKlineInterval.FourHours,
            SharedKlineInterval.TwelveHours,
            SharedKlineInterval.OneDay,
            SharedKlineInterval.OneWeek,
            SharedKlineInterval.OneMonth)
        {
        };
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(SubscribeKlineRequest request, Action<DataEvent<SharedKline>> handler, CancellationToken ct)
        {
            var interval = (Enums.StreamKlineInterval)request.Interval;

            var validationError = SubscribeKlineOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbol = request.SymbolName(FormatSymbol);
            var result = await _api.SubscribeToKlineUpdatesAsync(symbol, interval, update => handler(update.ToType(
                new SharedKline(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Symbol),
                    update.Symbol!,
                    update.Data.OpenTimeUtc,
                    update.Data.ClosePrice,
                    update.Data.HighPrice,
                    update.Data.LowPrice,
                    update.Data.OpenPrice,
                    new SharedOrderQuantity(update.Data.Volume, update.Data.Turnover)))), ct).ConfigureAwait(false);
            return result;
        }
        #endregion

        #region Trade client

        public SubscribeTradeOptions SubscribeTradeOptions { get; } = new SubscribeTradeOptions(_exchangeName, false);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(SubscribeTradeRequest request, Action<DataEvent<SharedTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbols = request.SymbolName(FormatSymbol);
            var result = await _api.SubscribeToTradeUpdatesAsync(symbols, update => handler(update.ToType<SharedTrade[]>([
                new SharedTrade(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Symbol),
                    update.Symbol!,
                    new SharedOrderQuantity(update.Data.Quantity, update.Data.QuoteQuantity),
                    update.Data.Price,
                    update.Data.TimestampUtc)
                {
                    Side = update.Data.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell
                }])), ct).ConfigureAwait(false);
            return result;
        }
        #endregion

        #region Order Book client
        public SubscribeOrderBookOptions SubscribeOrderBookOptions { get; } = new SubscribeOrderBookOptions(_exchangeName, false, new[] { 10, 50, 100 });
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(SubscribeOrderBookRequest request, Action<DataEvent<SharedOrderBook>> handler, CancellationToken ct)
        {
            var validationError = SubscribeOrderBookOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbol = request.SymbolName(FormatSymbol);
            var result = await _api.SubscribeToOrderBookUpdatesAsync(symbol, request.Limit ?? 50, update => handler(
                update.ToType(
                    new SharedOrderBook(SharedQuantityType.BaseAsset, null, update.Data.Asks, update.Data.Bids))), ct).ConfigureAwait(false);
            return result;
        }
        #endregion

        #region Ticker client
        async Task<WebSocketResult<UpdateSubscription>> ISubscribeTickerOperation.SubscribeToTickerUpdatesAsync(SubscribeTickerRequest request, Action<DataEvent<SharedTicker>> handler, CancellationToken ct)
            => await SubscribeToTickerUpdatesAsync(request, x => handler(x.ToType<SharedTicker>(x.Data)), ct).ConfigureAwait(false);

        public SubscribeTickerOptions SubscribeTickerOptions { get; } = new SubscribeTickerOptions(_exchangeName);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(SubscribeTickerRequest request, Action<DataEvent<SharedSpotTicker>> handler, CancellationToken ct)
        {
            var validationError = SubscribeTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbol = request.SymbolName(FormatSymbol);
            var result = await _api.SubscribeToTickerUpdatesAsync(symbol, update => handler(update.ToType(
                new SharedSpotTicker(
                    request.Symbol,
                    symbol,
                    update.Data.LastPrice,
                    update.Data.HighPrice,
                    update.Data.LowPrice,
                    new SharedOrderQuantity(update.Data.Volume, update.Data.Turnover),
                    update.Data.PriceChangePercentage)
                {
                })), ct).ConfigureAwait(false);
            return result;
        }
        #endregion

        #region Spot Order client

        async Task<WebSocketResult<UpdateSubscription>> ISpotOrderSocketClient.SubscribeToSpotOrderUpdatesAsync(SubscribeSpotOrderRequest request, Action<DataEvent<SharedSpotOrder[]>> handler, CancellationToken ct)
            => await SubscribeToSpotOrderUpdatesAsync(request, x => handler(x.ToType<SharedSpotOrder[]>(x.Data)), ct).ConfigureAwait(false);

        public SubscribeSpotOrderOptions SubscribeSpotOrderOptions { get; } = new SubscribeSpotOrderOptions(_exchangeName, true);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToSpotOrderUpdatesAsync(SubscribeSpotOrderRequest request, Action<DataEvent<SharedSpotOrderUpdate[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await _api.SubscribeToOrderUpdatesAsync(
                null,
                update =>
                {
                    var (side, type) = ParseOrderType(update.Data.Type);
                    var symbol = ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Symbol);
                    handler(update.ToType<SharedSpotOrderUpdate[]>(new[] {
                        new SharedSpotOrderUpdate(
                                symbol,
                                update.Data.Symbol!,
                                update.Data.OrderId!,
                                type,
                                side,
                                ParseOrderStatus(update.Data.Status),
                                null)
                            {
                                OrderPrice = update.Data.Price > 0 ? update.Data.Price : null,
                                OrderQuantity = ParseQuantity(update.Data),
                                QuantityFilled = new SharedOrderQuantity(update.Data.AccumulativeQuantity, update.Data.QuoteQuantityFilled),
                                AveragePrice = update.Data.AveragePrice == 0 ? null : update.Data.AveragePrice,
                                UpdateTime = update.Data.UpdateTime,
                                TimeInForce = ParseTimeInFormce(update.Data.Type),
                                LastTrade = update.Data.TradeId == null ? null : new SharedUserTrade(
                                    symbol,
                                    update.Data.Symbol,
                                    update.Data.OrderId,
                                    update.Data.TradeId,
                                    side,
                                    new SharedOrderQuantity(update.Data.Quantity!.Value),
                                    update.Data.Price!.Value,
                                    update.Data.UpdateTime
                                    )
                            }
                        }));
                },
                ct: ct).ConfigureAwait(false);
            return result;
        }

        private SharedOrderQuantity ParseQuantity(LBankOrderUpdate order)
        {
            if (order.OrderQuantity == 0)
                return new SharedOrderQuantity();

            if (order.Type == OrderType.BuyMarket)
                return new SharedOrderQuantity(null, order.OrderQuantity);

            return new SharedOrderQuantity(order.OrderQuantity);
        }

        private (SharedOrderSide side, SharedOrderType type) ParseOrderType(OrderType type)
            => type switch
            {
                OrderType.BuyLimit => (SharedOrderSide.Buy, SharedOrderType.Limit),
                OrderType.SellLimit => (SharedOrderSide.Sell, SharedOrderType.Limit),
                OrderType.BuyMarket => (SharedOrderSide.Buy, SharedOrderType.Market),
                OrderType.SellMarket => (SharedOrderSide.Sell, SharedOrderType.Market),
                OrderType.BuyMaker => (SharedOrderSide.Buy, SharedOrderType.LimitMaker),
                OrderType.SellMaker => (SharedOrderSide.Sell, SharedOrderType.LimitMaker),
                _ => (SharedOrderSide.Sell, SharedOrderType.Limit)
            };

        private SharedTimeInForce? ParseTimeInFormce(OrderType type)
        {
            if (type == OrderType.BuyFok || type == OrderType.SellFok)
                return SharedTimeInForce.FillOrKill;

            if (type == OrderType.BuyIoc || type == OrderType.SellIoc)
                return SharedTimeInForce.ImmediateOrCancel;

            return null;
        }

        private SharedOrderStatus ParseOrderStatus(OrderStatus status)
        {
            if (status == Enums.OrderStatus.Canceled || status == Enums.OrderStatus.PartiallyCanceled)
                return SharedOrderStatus.Canceled;
            if (status == Enums.OrderStatus.Open || status == Enums.OrderStatus.PartiallyFilled)
                return SharedOrderStatus.Open;
            if (status == OrderStatus.Filled)
                return SharedOrderStatus.Filled;

            return SharedOrderStatus.Unknown;
        }
        #endregion
    }
}
