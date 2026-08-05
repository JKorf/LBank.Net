using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using LBank.Net.Objects.Internal;
using Microsoft.Extensions.Logging;
using System;

namespace LBank.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class LBankPingSubscription : SystemSubscription
    {
        public LBankPingSubscription(ILogger logger) : base(logger, false)
        {
            MessageRouter = MessageRouter.CreateForEvent<LBankPing>("ping", HandlePing);
        }

        private CallResult? HandlePing(SocketConnection connection, DateTime time, string? arg3, LBankPing message)
        {
            _ = connection.SendAsync(ExchangeHelpers.NextId(), new LBankPong { Action = "pong", Ping = message.Ping}, 1);
            return CallResult.Ok();
        }
    }
}
