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
        #region Subscribe Balances

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
    }
}
