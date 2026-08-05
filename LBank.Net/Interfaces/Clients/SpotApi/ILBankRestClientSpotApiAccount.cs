using CryptoExchange.Net.Objects;
using LBank.Net.Enums;
using LBank.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LBank.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// LBank Spot account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
    /// </summary>
    public interface ILBankRestClientSpotApiAccount
    {
        /// <summary>
        /// Get API key info
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#query-user-api-key-permissions" /><br />
        /// Endpoint:<br />
        /// POST /v2/supplement/api_Restrictions.do<br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankApiKey>> GetApiKeyInfoAsync(CancellationToken ct = default);

        /// <summary>
        /// Get balances and asset networks
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#get-all-coins-information" /><br />
        /// Endpoint:<br />
        /// POST /v2/supplement/user_info.do<br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankBalance[]>> GetUserAssetsAsync(CancellationToken ct = default);

        /// <summary>
        /// Withdraw asset
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#withdrawal" /><br />
        /// Endpoint:<br />
        /// POST /v2/spot/wallet/withdraw.do<br />
        /// </para>
        /// </summary>
        /// <param name="address">["<c>address</c>"] Target address</param>
        /// <param name="asset">["<c>coin</c>"] The asset, for example `eth`</param>
        /// <param name="quantity">["<c>amount</c>"] Quantity</param>
        /// <param name="fee">["<c>fee</c>"] Fee</param>
        /// <param name="network">["<c>networkName</c>"] Network to use</param>
        /// <param name="memo">["<c>memo</c>"] Memo</param>
        /// <param name="notes">["<c>mark</c>"] Withdrawal notes</param>
        /// <param name="name">["<c>name</c>"] Target name, will be added to address book if provided</param>
        /// <param name="clientOrderId">["<c>withdrawOrderId</c>"] Client id</param>
        /// <param name="internalTransfer">["<c>type</c>"] True for internal tranfer</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankWithdrawResult>> WithdrawAsync(
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
            CancellationToken ct = default);

        /// <summary>
        /// Get deposit history
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#get-recharge-history" /><br />
        /// Endpoint:<br />
        /// POST /v2/spot/wallet/deposit_history.do<br />
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>coin</c>"] Filter by asset, for example `eth`</param>
        /// <param name="status">["<c>status</c>"] Filter by status</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankDepositPage>> GetDepositHistoryAsync(
            string? asset = null,
            DepositStatus? status = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get withdrawal history
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#get-withdrawal-history" /><br />
        /// Endpoint:<br />
        /// POST /v2/spot/wallet/withdraws.do<br />
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>coin</c>"] Filter by asset, for example `eth`</param>
        /// <param name="status">["<c>status</c>"] Filter by status</param>
        /// <param name="clientOrderId">["<c>withdrawOrderId</c>"] Filter by client id</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankWithdrawalPage>> GetWithdrawHistoryAsync(
            string? asset = null,
            WithdrawStatus? status = null,
            string? clientOrderId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get deposit address for an asset
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#the-user-obtains-the-deposit-address" /><br />
        /// Endpoint:<br />
        /// POST /v2/supplement/get_deposit_address.do<br />
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>coin</c>"] The asset, for example `eth`</param>
        /// <param name="network">["<c>networkName</c>"] Network name</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankDepositAddress>> GetDepositAddressAsync(
            string asset,
            string? network = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get asset details
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#list-asset-details" /><br />
        /// Endpoint:<br />
        /// POST /v2/supplement/asset_detail.do<br />
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>coin</c>"] Filter by asset, for example `ETH`</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<Dictionary<string, LBankAssetDetails>>> GetAssetDetailsAsync(string? asset = null, CancellationToken ct = default);

        /// <summary>
        /// Get trade fee
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#transaction-fee-rate-query" /><br />
        /// Endpoint:<br />
        /// POST /v2/supplement/customer_trade_fee.do<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>category</c>"] Filter by symbol, for example `ETHUSDT`</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankTradeFee[]>> GetTradeFeeAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get account info and balances
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/#account-information" /><br />
        /// Endpoint:<br />
        /// POST /v2/supplement/user_info_account.do<br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<LBankAccountInfo>> GetAccountInfoAsync(CancellationToken ct = default);

        /// <summary>
        /// Start a user stream and get the listen key
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/index.html#create-subscribekey" /><br />
        /// Endpoint:<br />
        /// POST /v2/subscribe/get_key.do<br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<string>> StartUserStreamAsync(CancellationToken ct = default);

        /// <summary>
        /// Extend the lifetime of an existing user stream
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/index.html#extend-the-validity-of-subscribekey" /><br />
        /// Endpoint:<br />
        /// POST /v2/subscribe/refresh_key.do<br />
        /// </para>
        /// </summary>
        /// <param name="listenKey">The listen key to keep alive</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default);

        /// <summary>
        /// Stop an existing user stream
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.lbank.com/docs/index.html#close-subscribekey" /><br />
        /// Endpoint:<br />
        /// POST /v2/subscribe/destroy_key.do<br />
        /// </para>
        /// </summary>
        /// <param name="listenKey">The listen key to stop</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult> StopUserStreamAsync(string listenKey, CancellationToken ct = default);
    }
}
