using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using LBank.Net.Enums;
using LBank.Net.Interfaces.Clients.SpotApi;
using LBank.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LBank.Net.Clients.SpotApi
{
    internal partial class LBankRestClientSpotSharedApi
    {
        #region Asset client

        public GetAssetOptions GetAssetOptions { get; } = new GetAssetOptions(_exchangeName, false);
        public async Task<HttpResult<SharedAsset>> GetAssetAsync(GetAssetRequest request, CancellationToken ct)
        {
            var validationError = GetAssetOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedAsset>(Exchange, validationError);

            var assets = await _api.ExchangeData.GetAssetsAsync(request.Asset, ct: ct).ConfigureAwait(false);
            if (!assets.Success)
                return HttpResult.Fail<SharedAsset>(assets);

            return HttpResult.Ok(assets, new SharedAsset(request.Asset)
            {
                Networks = assets.Data.Select(x => new SharedAssetNetwork(x.Network)
                {
                    WithdrawEnabled = x.CanWithdraw,
                    DepositEnabled = x.CanDeposit,                    
                    ContractAddress = x.ContractAddress,
                    MinWithdrawQuantity = x.AssetFee.MinWithdrawQuantity,
                    WithdrawFee = x.AssetFee.FeeQuantity
                }).ToArray()
            });
        }

        GetAllAssetsOptions IAssetsRestClient.GetAssetsOptions { get; } = new GetAllAssetsOptions(_exchangeName, false)
        {
            Supported = false,
            RequestNotes = "There is no way to retrieve the full list of supported assets in the LBank API"
        };
        Task<HttpResult<SharedAsset[]>> IAssetsRestClient.GetAssetsAsync(GetAssetsRequest request, CancellationToken ct)
        {
            return Task.FromResult(HttpResult.Fail<SharedAsset[]>(Exchange, new InvalidOperationError($"Method not available for {Exchange}")));
        }

        #endregion
    }
}
