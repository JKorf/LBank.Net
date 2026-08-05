using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using LBank.Net.Enums;
using LBank.Net.Interfaces.Clients.SpotApi;
using LBank.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LBank.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class LBankRestClientSpotApiAccount : ILBankRestClientSpotApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly LBankRestClientSpotApi _baseClient;

        internal LBankRestClientSpotApiAccount(LBankRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Api Key Info

        /// <inheritdoc />
        public async Task<HttpResult<LBankApiKey>> GetApiKeyInfoAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/supplement/api_Restrictions.do", LBankExchange.RateLimiter.RestApi, 1, true);
            var result = await _baseClient.SendAsync<LBankApiKey>(request, null, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get User Assets

        /// <inheritdoc />
        public async Task<HttpResult<LBankBalance[]>> GetUserAssetsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/supplement/user_info.do", LBankExchange.RateLimiter.RestApi, 1, true);
            var result = await _baseClient.SendAsync<LBankBalance[]>(request, null, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Withdraw

        /// <inheritdoc />
        public async Task<HttpResult<LBankWithdrawResult>> WithdrawAsync(
            string address,
            string asset,
            decimal quantity,
            decimal fee,
            string? network = null,
            string? memo = null,
            string? notes = null,
            string? name = null,
            string? clientOrderId = null,
            bool? internalTransfer = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("address", address);
            parameters.Add("coin", asset);
            parameters.Add("amount", quantity);
            parameters.Add("fee", fee);
            parameters.Add("networkName", network);
            parameters.Add("memo", memo);
            parameters.Add("mark", notes);
            parameters.Add("name", name);
            parameters.Add("withdrawOrderId", clientOrderId);
            parameters.Add("type", internalTransfer);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/spot/wallet/withdraw.do", LBankExchange.RateLimiter.RestApi, 1, true);
            var result = await _baseClient.SendAsync<LBankWithdrawResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Deposit History

        /// <inheritdoc />
        public async Task<HttpResult<LBankDepositPage>> GetDepositHistoryAsync(
            string? asset = null,
            DepositStatus? status = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("coin", asset);
            parameters.Add("status", status);
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/spot/wallet/deposit_history.do", LBankExchange.RateLimiter.RestApi, 1, true);
            var result = await _baseClient.SendAsync<LBankDepositPage>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Withdraw History

        /// <inheritdoc />
        public async Task<HttpResult<LBankWithdrawalPage>> GetWithdrawHistoryAsync(
            string? asset = null,
            WithdrawStatus? status = null,
            string? clientOrderId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("coin", asset);
            parameters.Add("status", status);
            parameters.Add("withdrawOrderId", clientOrderId);
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/spot/wallet/withdraws.do", LBankExchange.RateLimiter.RestApi, 1, true);
            var result = await _baseClient.SendAsync<LBankWithdrawalPage>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Deposit Address

        /// <inheritdoc />
        public async Task<HttpResult<LBankDepositAddress>> GetDepositAddressAsync(
            string asset,
            string? network = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("coin", asset);
            parameters.Add("networkName", network);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/supplement/get_deposit_address.do", LBankExchange.RateLimiter.RestApi, 1, true);
            var result = await _baseClient.SendAsync<LBankDepositAddress>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Asset Details

        /// <inheritdoc />
        public async Task<HttpResult<Dictionary<string, LBankAssetDetails>>> GetAssetDetailsAsync(string? asset = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("coin", asset);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/supplement/asset_detail.do", LBankExchange.RateLimiter.RestApi, 1, true);
            var result = await _baseClient.SendAsync<Dictionary<string, LBankAssetDetails>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Trade Fee

        /// <inheritdoc />
        public async Task<HttpResult<LBankTradeFee[]>> GetTradeFeeAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(LBankExchange._parameterSerializationSettings);
            parameters.Add("category", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/supplement/customer_trade_fee.do", LBankExchange.RateLimiter.RestApi, 1, true);
            var result = await _baseClient.SendAsync<LBankTradeFee[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Account Info

        /// <inheritdoc />
        public async Task<HttpResult<LBankAccountInfo>> GetAccountInfoAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/supplement/user_info_account.do", LBankExchange.RateLimiter.RestApi, 1, true);
            var result = await _baseClient.SendAsync<LBankAccountInfo>(request, null, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Start User Stream
        /// <inheritdoc />
        public async Task<HttpResult<string>> StartUserStreamAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/subscribe/get_key.do", LBankExchange.RateLimiter.RestApi, 1, true);
            return await _baseClient.SendAsync<string>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Keep Alive User Stream

        /// <inheritdoc />
        public async Task<HttpResult> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default)
        {
            listenKey.ValidateNotNull(nameof(listenKey));

            var parameters = new Parameters(LBankExchange._parameterSerializationSettings)
            {
                { "subscribeKey", listenKey }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/subscribe/refresh_key.do", LBankExchange.RateLimiter.RestApi, 1, true);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Stop User Stream
        /// <inheritdoc />
        public async Task<HttpResult> StopUserStreamAsync(string listenKey, CancellationToken ct = default)
        {
            listenKey.ValidateNotNull(nameof(listenKey));

            var parameters = new Parameters(LBankExchange._parameterSerializationSettings)
            {
                { "subscribeKey", listenKey }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/subscribe/destroy_key.do", LBankExchange.RateLimiter.RestApi, 1, true);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

    }
}
