using CryptoExchange.Net;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.TokenManagement;
using LBank.Net.Clients.MessageHandlers;
using LBank.Net.Enums;
using LBank.Net.Interfaces.Clients.SpotApi;
using LBank.Net.Objects.Internal;
using LBank.Net.Objects.Models;
using LBank.Net.Objects.Options;
using LBank.Net.Objects.Sockets.Subscriptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace LBank.Net.Clients.SpotApi
{
    /// <summary>
    /// Client providing access to the LBank Spot websocket Api
    /// </summary>
    internal partial class LBankSocketClientSpotApi : SocketApiClient<LBankEnvironment, LBankAuthenticationProvider, LBankCredentials>, ILBankSocketClientSpotApi
    {
        #region fields
        protected override ErrorMapping ErrorMapping => LBankErrors.Errors;

        private readonly ILoggerFactory? _loggerFactory;
        private LBankRestClient? _tokenClient;
        internal TokenManager TokenManager { get; }
        private LBankRestClient TokenClient
        {
            get
            {
                if (_tokenClient == null)
                {
                    _tokenClient = new LBankRestClient(null, _loggerFactory, Options.Create(new LBankRestOptions
                    {
                        ApiCredentials = ApiCredentials,
                        Environment = ClientOptions.Environment,
                        Proxy = ClientOptions.Proxy,
                        OutputOriginalData = ClientOptions.OutputOriginalData
                    }));
                }

                return _tokenClient;
            }
        }

        private readonly LBankSocketOptions _clientOptions;
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal LBankSocketClientSpotApi(ILoggerFactory? loggerFactory, LBankSocketOptions options) :
            base(loggerFactory, LBankExchange.Metadata.Id, options.Environment.SocketClientAddress!, options, options.SpotOptions)
        {
            _loggerFactory = loggerFactory;
            _clientOptions = options;

            RateLimiter = LBankExchange.RateLimiter.RestApi;

            AddSystemSubscription(new LBankPingSubscription(_logger));

            // Server doesn't respond consistently to ping frames
            // KeepAliveInterval = TimeSpan.Zero;

            TokenManager = new TokenManager(
                LBankExchange.Metadata.Id,
                loggerFactory,
                TimeSpan.FromMinutes(30),
                TimeSpan.FromMinutes(60),
                startToken: StartListenKeyAsync,
                keepAliveToken: KeepAliveListenKeyAsync,
                stopToken: StopListenKeyAsync);
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(LBankExchange._serializerContext);
        /// <inheritdoc />
        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new LBankSocketSpotMessageHandler();

        /// <inheritdoc />
        protected override LBankAuthenticationProvider CreateAuthenticationProvider(LBankCredentials credentials)
            => new LBankAuthenticationProvider(credentials);

        private string GetConnectionAddress(bool useV3IsConfigured)
        {
            if (useV3IsConfigured && _clientOptions.UseV3)
                return ClientOptions.Environment.SocketClientV3Address.AppendPath("/old-wss/ccws/ws/V3") + "/";
            else
                return ClientOptions.Environment.SocketClientAddress.AppendPath("ws/V2/") + "/";
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<LBankTradeUpdate>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, LBankTradeUpdateMessage>((receiveTime, originalData, data) =>
            {
                UpdateTimeOffset(data.TimestampUtc);

                onMessage(
                    new DataEvent<LBankTradeUpdate>(LBankExchange.Metadata.Id, data.Trade, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Topic)
                        .WithSymbol(data.Symbol)
                        .WithDataTimestamp(data.TimestampUtc, GetTimeOffset())
                    );
            });

            var subscription = new LBankSubscription<LBankTradeUpdateMessage>(_logger, "trade", symbol, internalHandler, false);
            return await SubscribeAsync(GetConnectionAddress(true), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, StreamKlineInterval interval, Action<DataEvent<LBankKlineUpdate>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, LBankKlineUpdateMessage>((receiveTime, originalData, data) =>
            {
                UpdateTimeOffset(data.TimestampUtc);

                onMessage(
                    new DataEvent<LBankKlineUpdate>(LBankExchange.Metadata.Id, data.Kline, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Topic)
                        .WithSymbol(data.Symbol)
                        .WithDataTimestamp(data.TimestampUtc, GetTimeOffset())
                    );
            });

            var subscription = new LBankSubscription<LBankKlineUpdateMessage>(_logger, "kbar", symbol, internalHandler, false, interval);
            return await SubscribeAsync(GetConnectionAddress(true), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<LBankOrderBookUpdate>> onMessage, CancellationToken ct = default)
        {
            depth.ValidateIntValues(nameof(depth), 10, 50, 100);

            var internalHandler = new Action<DateTime, string?, LBankOrderBookUpdateMessage>((receiveTime, originalData, data) =>
            {
                UpdateTimeOffset(data.TimestampUtc);

                onMessage(
                    new DataEvent<LBankOrderBookUpdate>(LBankExchange.Metadata.Id, data.OrderBook, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Topic)
                        .WithSymbol(data.Symbol)
                        .WithDataTimestamp(data.TimestampUtc, GetTimeOffset())
                    );
            });

            var subscription = new LBankSubscription<LBankOrderBookUpdateMessage>(_logger, "depth", symbol, internalHandler, false, depth: depth);
            return await SubscribeAsync(GetConnectionAddress(true), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<LBankTickerUpdate>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, LBankTickerUpdateMessage>((receiveTime, originalData, data) =>
            {
                UpdateTimeOffset(data.TimestampUtc);

                onMessage(
                    new DataEvent<LBankTickerUpdate>(LBankExchange.Metadata.Id, data.Ticker, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Topic)
                        .WithSymbol(data.Symbol)
                        .WithDataTimestamp(data.TimestampUtc, GetTimeOffset())
                    );
            });

            var subscription = new LBankSubscription<LBankTickerUpdateMessage>(_logger, "tick", symbol, internalHandler, false);
            return await SubscribeAsync(GetConnectionAddress(true), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(string? listenKey, Action<DataEvent<LBankOrderUpdate>> onMessage, CancellationToken ct = default)
        {
            if (listenKey == null && !Authenticated)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, new NoApiCredentialsError());

            TokenLease? lease = null;
            if (listenKey == null)
            {
                var leaseResult = await TokenManager.AcquireAsync(new TokenScope(
                    LBankExchange.Metadata.Id,
                    EnvironmentName,
                    "Spot",
                    ApiCredentials!.Credential!.Key), ct).ConfigureAwait(false);
                if (!leaseResult.Success)
                    return WebSocketResult.Fail<UpdateSubscription>(Exchange, leaseResult.Error);

                lease = leaseResult.Data;
            }

            var lk = listenKey ?? lease!.Token.Token;
            var internalHandler = new Action<DateTime, string?, LBankOrderUpdateMessage>((receiveTime, originalData, data) =>
            {
                UpdateTimeOffset(data.TimestampUtc);

                onMessage(
                    new DataEvent<LBankOrderUpdate>(LBankExchange.Metadata.Id, data.Order, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Topic)
                        .WithSymbol(data.Symbol)
                        .WithDataTimestamp(data.TimestampUtc, GetTimeOffset())
                    );
            });

            var subscription = new LBankSubscription<LBankOrderUpdateMessage>(_logger, "orderUpdate", null, internalHandler, false, listenKey: lk)
            {
                TokenLease = lease
            };
            return await SubscribeAsync(GetConnectionAddress(false), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(string? listenKey, Action<DataEvent<LBankBalanceUpdate>> onMessage, CancellationToken ct = default)
        {
            if (listenKey == null && !Authenticated)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, new NoApiCredentialsError());

            TokenLease? lease = null;
            if (listenKey == null)
            {
                var leaseResult = await TokenManager.AcquireAsync(new TokenScope(
                    LBankExchange.Metadata.Id,
                    EnvironmentName,
                    "Spot",
                    ApiCredentials!.Credential!.Key), ct).ConfigureAwait(false);
                if (!leaseResult.Success)
                    return WebSocketResult.Fail<UpdateSubscription>(Exchange, leaseResult.Error);
            
                lease = leaseResult.Data;
            }

            var lk = listenKey ?? lease!.Token.Token;
            var internalHandler = new Action<DateTime, string?, LBankBalanceUpdateMessage>((receiveTime, originalData, data) =>
            {
                UpdateTimeOffset(data.TimestampUtc);

                onMessage(
                    new DataEvent<LBankBalanceUpdate>(LBankExchange.Metadata.Id, data.Balance, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Topic)
                        .WithSymbol(data.Symbol)
                        .WithDataTimestamp(data.TimestampUtc, GetTimeOffset())
                    );
            });

            var subscription = new LBankSubscription<LBankBalanceUpdateMessage>(_logger, "assetUpdate", null, internalHandler, false, listenKey: lk)
            {
                TokenLease = lease
            };
            return await SubscribeAsync(GetConnectionAddress(false), subscription, ct).ConfigureAwait(false);
        }

        private async Task<CallResult<string>> StartListenKeyAsync(TokenScope tokenScope, CancellationToken ct)
        {
            var result = await TokenClient.SpotApi.Account.StartUserStreamAsync(ct).ConfigureAwait(false);
            if (!result.Success)
                return CallResult.Fail<string>(result.Error);

            return CallResult.Ok(result.Data);
        }

        private async Task<CallResult> KeepAliveListenKeyAsync(TokenInfo token, CancellationToken ct)
        {
            var result = await TokenClient.SpotApi.Account.KeepAliveUserStreamAsync(token.Token, ct).ConfigureAwait(false);
            if (!result.Success)
                return CallResult.Fail<string>(result.Error);

            return CallResult.Ok();
        }

        private async Task<CallResult> StopListenKeyAsync(TokenInfo token, CancellationToken ct)
        {
            var result = await TokenClient.SpotApi.Account.StopUserStreamAsync(token.Token, ct).ConfigureAwait(false);
            if (!result.Success)
                return CallResult.Fail<string>(result.Error);

            return CallResult.Ok();
        }

        /// <inheritdoc />
        public ILBankSocketClientSpotApiShared SharedClient => this;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => LBankExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);
    }
}
