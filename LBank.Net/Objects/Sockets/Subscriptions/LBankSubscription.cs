using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using LBank.Net.Enums;
using Microsoft.Extensions.Logging;
using System;

namespace LBank.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class LBankSubscription<T> : Subscription
    {
        private readonly Action<DateTime, string?, T> _handler;
        private readonly string _topic;
        private readonly string? _symbol;
        private readonly string? _lk;
        private readonly StreamKlineInterval? _interval;
        private readonly int? _depth;

        /// <summary>
        /// ctor
        /// </summary>
        public LBankSubscription(ILogger logger, string topic, string? symbol, Action<DateTime, string?, T> handler, bool auth, StreamKlineInterval? interval = null, int? depth = null, string? listenKey = null)
            : base(logger, auth)
        {
            _handler = handler;
            _topic = topic;
            _symbol = symbol;
            _interval = interval;
            _depth = depth;
            _lk = listenKey;

            MessageRouter = MessageRouter.CreateForEvent<T>(_topic, symbol + interval + depth, DoHandleMessage);
        }

        /// <inheritdoc />
        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new LBankQuery(new Internal.LBankSubscribeMessage
            {
                Action = "subscribe",
                Topic = _topic,
                Symbol = _symbol ?? "all",
                Interval = _interval,
                Depth = _depth,
                SubscribeKey = _lk ?? TokenLease?.Token.Token
            }, false);
        }

        /// <inheritdoc />
        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            return new LBankQuery(new Internal.LBankSubscribeMessage
            {
                Action = "unsubscribe",
                Topic = _topic,
                Symbol = _symbol ?? "all",
                Interval = _interval,
                Depth = _depth,
                SubscribeKey = _lk ?? TokenLease?.Token.Token
            }, false);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, T message)
        {
            _handler.Invoke(receiveTime, originalData, message);
            return CallResult.Ok();
        }
    }
}
