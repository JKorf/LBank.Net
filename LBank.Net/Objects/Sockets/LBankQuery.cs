using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using System;
using LBank.Net.Objects.Internal;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace LBank.Net.Objects.Sockets
{
    internal class LBankQuery : Query<LBankError>
    {
        public LBankQuery(LBankSubscribeMessage request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            var confirmationString = request.Topic + "_" + request.Symbol;
            if (request.Interval != null)
                confirmationString += "_" + EnumConverter.GetString(request.Interval.Value);
            else if (request.Depth != null)
                confirmationString += $"_{request.Depth}_null";

            MessageRouter = MessageRouter.Create(
                MessageRoute.CreateForQuery<LBankError>("error", HandleError),
                MessageRoute.CreateForQuery<string, LBankError>(confirmationString, HandleConfirmation)
                );

            // For V2 there is no confirmation message
            if (request.Action == "unsubscribe")
            {
                ExpectsResponse = false;
            }
            else
            {
                // Wait up to 2 seconds for an error. If data comes in before the query will also succeed
                TimeoutBehavior = TimeoutBehavior.Succeed;
                RequestTimeout = TimeSpan.FromSeconds(2);
            }
        }

        public CallResult<LBankError> HandleError(SocketConnection connection, DateTime receiveTime, string? originalData, LBankError message)
        {
            return CallResult.Fail<LBankError>(new ServerError(ErrorInfo.Unknown with { Message = message.Message }), originalData);
        }

        public CallResult<LBankError> HandleConfirmation(SocketConnection connection, DateTime receiveTime, string? originalData, string message)
        {
            return CallResult.Ok<LBankError>(new LBankError(), originalData);
        }
    }
}
