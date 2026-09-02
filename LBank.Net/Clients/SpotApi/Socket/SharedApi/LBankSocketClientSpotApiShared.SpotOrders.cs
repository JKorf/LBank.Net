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
    internal partial class LBankSocketClientSpotSharedApi
    {
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
