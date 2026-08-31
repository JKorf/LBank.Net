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
    internal class LBankRestClientSpotSharedApi : 
        SharedApiBase,
        ILBankRestClientSpotApiShared,
        ILBankRestClientSpotSharedApi
    {
        private readonly LBankRestClientSpotApi _api;

        private const string _topicId = "LBankSpot";
        private const string _exchangeName = "LBank";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(LBankExchange.Metadata, this);

        private readonly static HashSet<string> _knownFiats = ["USD", "EUR", "BRL"];

        public LBankRestClientSpotSharedApi(LBankRestClientSpotApi api)
            : base(
                  api.Exchange,
                  [TradingMode.Spot],
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                GetAssetOptions,
                GetAllSpotTickersOptions,
                GetAssetOptions,
                GetBalancesOptions,
                GetBookTickerOptions,
                GetDepositAddressesOptions,
                GetDepositHistoryOptions,
                GetFeeOptions,
                GetKlinesOptions,
                GetOrderBookOptions,
                GetRecentTradesOptions,
                GetWithdrawalHistoryOptions,
                WithdrawOptions,
                GetSpotSymbolsOptions,
                GetSpotTickerOptions,
                GetAllSpotTickersOptions,
                PlaceSpotOrderOptions,
                GetSpotOrderOptions,
                GetOpenSpotOrdersOptions,
                GetClosedSpotOrdersOptions,
                GetSpotOrderTradesOptions,
                GetSpotUserTradeHistoryOptions,
                CancelSpotOrderOptions,
                GetSpotOrderByClientOrderIdOptions,
                CancelSpotOrderByClientOrderIdOptions
                );
        }

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

        #region Balance client
        public GetBalancesOptions GetBalancesOptions { get; } = new GetBalancesOptions(_exchangeName, AccountTypeFilter.Spot);

        public async Task<HttpResult<SharedBalance[]>> GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
        {
            var validationError = GetBalancesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedBalance[]>(Exchange, validationError);

            var result = await _api.Account.GetAccountInfoAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedBalance[]>(result);

            return HttpResult.Ok(result, result.Data.Balances.Select(x =>
                new SharedBalance(
                    SupportedTradingModes,
                    x.Asset,
                    x.Free,
                    x.Free + x.Locked)).ToArray());
        }

        #endregion

        #region Book Ticker client

        public GetBookTickerOptions GetBookTickerOptions { get; } = new GetBookTickerOptions(_exchangeName, false);
        public async Task<HttpResult<SharedBookTicker>> GetBookTickerAsync(GetBookTickerRequest request, CancellationToken ct)
        {
            var validationError = GetBookTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedBookTicker>(Exchange, validationError);

            var resultTicker = await _api.ExchangeData.GetBookTickerAsync(request.Symbol!.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!resultTicker.Success)
                return HttpResult.Fail<SharedBookTicker>(resultTicker);

            var ticker = resultTicker.Data;
            return HttpResult.Ok(resultTicker, new SharedBookTicker(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, ticker.Symbol),
                ticker.Symbol,
                ticker.BestAskPrice,
                new SharedOrderQuantity(ticker.BestAskQuantity),
                ticker.BestBidPrice,
                new SharedOrderQuantity(ticker.BestBidQuantity)));
        }

        #endregion

        #region Deposit client

        public GetDepositAddressesOptions GetDepositAddressesOptions { get; } = new GetDepositAddressesOptions(_exchangeName, true);
        public async Task<HttpResult<SharedDepositAddress[]>> GetDepositAddressesAsync(GetDepositAddressesRequest request, CancellationToken ct)
        {
            var validationError = GetDepositAddressesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedDepositAddress[]>(Exchange, validationError);

            var depositAddresses = await _api.Account.GetDepositAddressAsync(request.Asset, request.Network).ConfigureAwait(false);
            if (!depositAddresses.Success)
                return HttpResult.Fail<SharedDepositAddress[]>(depositAddresses);

            return HttpResult.Ok<SharedDepositAddress[]>(depositAddresses, [new SharedDepositAddress(request.Asset, depositAddresses.Data.Address)
            {
                TagOrMemo = depositAddresses.Data.Memo,
                Network = request.Network
            }]);
        }

        Task<HttpResult<SharedDeposit[]>> IDepositRestClient.GetDepositsAsync(GetDepositsRequest request, PageRequest? pageRequest, CancellationToken ct)
            => GetDepositHistoryAsync(request, pageRequest, ct);
        GetDepositHistoryOptions IDepositRestClient.GetDepositsOptions => GetDepositHistoryOptions;

        public GetDepositHistoryOptions GetDepositHistoryOptions { get; } = new GetDepositHistoryOptions(_exchangeName, false, true, true, 1000)
        {
            MaxAge = TimeSpan.FromDays(83)
        };
        public async Task<HttpResult<SharedDeposit[]>> GetDepositHistoryAsync(GetDepositsRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetDepositHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedDeposit[]>(Exchange, validationError);

            // Determine page token
            int limit = request.Limit ?? 1000;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, maxPeriod: TimeSpan.FromDays(7));

            // Get data
            var result = await _api.Account.GetDepositHistoryAsync(
                request.Asset,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedDeposit[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                    () => Pagination.NextPageFromTime(pageParams, result.Data.Deposits.Min(x => x.InsertTime)),
                    result.Data.Deposits.Length,
                    result.Data.Deposits.Select(x => x.InsertTime),
                    request.StartTime,
                    request.EndTime ?? DateTime.UtcNow,
                    pageParams,
                    TimeSpan.FromDays(7),
                    TimeSpan.FromDays(83));

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data.Deposits, x => x.InsertTime, request.StartTime, request.EndTime, direction)
                .Select(x =>
                    new SharedDeposit(
                        x.Asset,
                        x.Quantity,
                        x.Status == DepositStatus.Success,
                        x.InsertTime,
                        ParseTransferStatus(x.Status))
                    {
                        Network = x.Network,
                        TransactionId = x.TransactionId
                    }).ToArray(), nextPageRequest);
        }

        private SharedTransferStatus ParseTransferStatus(DepositStatus status)
        {
            if (status == DepositStatus.Success)
                return SharedTransferStatus.Completed;
            if (status == DepositStatus.Failed
            || status == DepositStatus.Canceled)
            {
                return SharedTransferStatus.Failed;
            }
            if (status == DepositStatus.Applying)
            {
                return SharedTransferStatus.InProgress;
            }

            return SharedTransferStatus.Unknown;
        }

        #endregion

        #region Fee Client
        public GetFeeOptions GetFeeOptions { get; } = new GetFeeOptions(_exchangeName, true);

        public async Task<HttpResult<SharedFee>> GetFeesAsync(GetFeeRequest request, CancellationToken ct)
        {
            var validationError = GetFeeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFee>(Exchange, validationError);

            // Get data
            var result = await _api.Account.GetTradeFeeAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFee>(result);

            var feeInfo = result.Data.SingleOrDefault();
            if (feeInfo == null)
                return HttpResult.Fail<SharedFee>(Exchange, new ServerError(ErrorType.UnknownSymbol, $"No fee information found for symbol {request.Symbol}"));

            // Return
            return HttpResult.Ok(result, new SharedFee(feeInfo.MakerCommission * 100, feeInfo.TakerCommission * 100));
        }
        #endregion

        #region Kline client

        public GetKlinesOptions GetKlinesOptions { get; } = new GetKlinesOptions(_exchangeName, true, false, true, 2000, false,
            SharedKlineInterval.OneMinute,
            SharedKlineInterval.FiveMinutes,
            SharedKlineInterval.FifteenMinutes,
            SharedKlineInterval.ThirtyMinutes,
            SharedKlineInterval.OneHour,
            SharedKlineInterval.FourHours,
            SharedKlineInterval.EightHours,
            SharedKlineInterval.TwelveHours,
            SharedKlineInterval.OneDay,
            SharedKlineInterval.OneWeek,
            SharedKlineInterval.OneMonth);
        public async Task<HttpResult<SharedKline[]>> GetKlinesAsync(GetKlinesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var interval = (Enums.KlineInterval)request.Interval;

            var validationError = GetKlinesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedKline[]>(Exchange, validationError);

            int limit = request.Limit ?? 2000;
            var direction = DataDirection.Ascending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime ?? (DateTime.UtcNow.AddSeconds(-((limit - 1) * (int)interval))), request.EndTime ?? DateTime.UtcNow, pageRequest, false);

            // Get data
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await _api.ExchangeData.GetKlinesAsync(
                symbol,
                interval,
                pageParams.Limit,
                pageParams.StartTime!.Value,
                ct: ct
                ).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedKline[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => request.StartTime == null ? null : Pagination.NextPageFromTime(pageParams, result.Data.Max(x => x.OpenTime).AddSeconds((int)interval)),
                     result.Data.Length,
                     result.Data.Select(x => x.OpenTime),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.OpenTime, request.StartTime, request.EndTime, direction)
                    .Select(x =>
                        new SharedKline(
                            request.Symbol,
                            symbol,
                            x.OpenTime,
                            x.ClosePrice,
                            x.HighPrice,
                            x.LowPrice,
                            x.OpenPrice,
                            new SharedOrderQuantity(x.Volume)))
                    .ToArray(), nextPageRequest);
        }

        #endregion

        #region Order Book client
        public GetOrderBookOptions GetOrderBookOptions { get; } = new GetOrderBookOptions(_exchangeName, 1, 200, false);
        public async Task<HttpResult<SharedOrderBook>> GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
        {
            var validationError = GetOrderBookOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedOrderBook>(Exchange, validationError);

            var result = await _api.ExchangeData.GetOrderBookAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedOrderBook>(result);

            return HttpResult.Ok(result, new SharedOrderBook(SharedQuantityType.BaseAsset, null, result.Data.Asks, result.Data.Bids));
        }

        #endregion

        #region Recent Trade client
        public GetRecentTradesOptions GetRecentTradesOptions { get; } = new GetRecentTradesOptions(_exchangeName, 500, false);

        public async Task<HttpResult<SharedTrade[]>> GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
        {
            var validationError = GetRecentTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedTrade[]>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await _api.ExchangeData.GetTradesAsync(
                symbol,
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTrade[]>(result);

            return HttpResult.Ok(result, result.Data.Select(x =>
            new SharedTrade(request.Symbol, symbol, new SharedOrderQuantity(x.Quantity, x.QuoteQuantity), x.Price, x.Timestamp)
            {
                Side = x.IsBuyerMaker ? SharedOrderSide.Buy : SharedOrderSide.Sell
            }).ToArray());
        }

        #endregion

        #region Withdrawal client

        Task<HttpResult<SharedWithdrawal[]>> IWithdrawalRestClient.GetWithdrawalsAsync(GetWithdrawalsRequest request, PageRequest? pageRequest, CancellationToken ct)
            => GetWithdrawalHistoryAsync(request, pageRequest, ct);
        GetWithdrawalHistoryOptions IWithdrawalRestClient.GetWithdrawalsOptions => GetWithdrawalHistoryOptions;

        public GetWithdrawalHistoryOptions GetWithdrawalHistoryOptions { get; } = new GetWithdrawalHistoryOptions(_exchangeName, false, true, true, 1000)
        {
            MaxAge = TimeSpan.FromDays(83)
        };
        public async Task<HttpResult<SharedWithdrawal[]>> GetWithdrawalHistoryAsync(GetWithdrawalsRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetWithdrawalHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedWithdrawal[]>(Exchange, validationError);

            // Determine page token
            int limit = request.Limit ?? 1000;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, maxPeriod: TimeSpan.FromDays(7));

            // Get data
            var result = await _api.Account.GetWithdrawHistoryAsync(
                request.Asset,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedWithdrawal[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                    () => Pagination.NextPageFromTime(pageParams, result.Data.Withdrawals.Min(x => x.ApplyTime)),
                    result.Data.Withdrawals.Length,
                    result.Data.Withdrawals.Select(x => x.ApplyTime),
                    request.StartTime,
                    request.EndTime ?? DateTime.UtcNow,
                    pageParams,
                    TimeSpan.FromDays(7),
                    TimeSpan.FromDays(83));

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data.Withdrawals, x => x.ApplyTime, request.StartTime, request.EndTime, direction)
                .Select(x =>
                    new SharedWithdrawal(
                        x.FeeAsset,
                        x.Address ?? string.Empty,
                        x.Quantity,
                        x.Status == WithdrawStatus.Completed,
                        x.ApplyTime,
                        GetWithdrawalStatus(x))
                    {
                        Id = x.Id.ToString(),
                        Network = x.Network,
                        TransactionId = x.TransactionId,
                        Fee = x.Fee
                    })
                .ToArray(), nextPageRequest);
        }

        private SharedTransferStatus GetWithdrawalStatus(LBankWithdrawal x)
        {
            if (x.Status == WithdrawStatus.Applying)
            {
                return SharedTransferStatus.InProgress;
            }

            if (x.Status == WithdrawStatus.Completed)
                return SharedTransferStatus.Completed;

            if (x.Status == WithdrawStatus.Canceled
                || x.Status == WithdrawStatus.Failed)
            {
                return SharedTransferStatus.Failed;
            }

            return SharedTransferStatus.Unknown;
        }

        #endregion

        #region Withdraw client

        public WithdrawOptions WithdrawOptions { get; } = new WithdrawOptions(_exchangeName)
        {
            RequiredExchangeParameters = [
                new ParameterDescription("Fee", typeof(decimal), "The fee to use for the withdrawal", 0.001m)
                ]
        };

        public async Task<HttpResult<SharedId>> WithdrawAsync(WithdrawRequest request, CancellationToken ct)
        {
            var validationError = WithdrawOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var withdrawal = await _api.Account.WithdrawAsync(
                request.Address,
                request.Asset,
                request.Quantity,
                fee: request.GetParamValue<decimal>(_exchangeName, "Fee"),
                network: request.Network,
                memo: request.AddressTag,
                ct: ct).ConfigureAwait(false);
            if (!withdrawal.Success)
                return HttpResult.Fail<SharedId>(withdrawal);

            return HttpResult.Ok(withdrawal, new SharedId(withdrawal.Data.WithdrawId.ToString()));
        }

        #endregion

        #region Spot Symbol client
        public SharedSymbolCatalog? SpotSymbolCatalog => ExchangeSymbolCache.GetSymbolCatalog(_exchangeName, _topicId, _api.EnvironmentName, null);
        public GetSpotSymbolsOptions GetSpotSymbolsOptions { get; } = new GetSpotSymbolsOptions(_exchangeName, false);

        public async Task<HttpResult<SharedSpotSymbol[]>> GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = GetSpotSymbolsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotSymbol[]>(Exchange, validationError);

            var taskAvailable = _api.ExchangeData.GetAvailableSymbolsAsync(ct: ct);
            var taskSymbols = _api.ExchangeData.GetSymbolsAsync(ct: ct);
            await Task.WhenAll(taskAvailable, taskSymbols).ConfigureAwait(false);
            var resultAvailable = taskAvailable.Result;
            var resultSymbols = taskSymbols.Result;
            if (!resultAvailable.Success)
                return HttpResult.Fail<SharedSpotSymbol[]>(resultAvailable);
            if (!resultSymbols.Success)
                return HttpResult.Fail<SharedSpotSymbol[]>(resultSymbols);

            var resultData =
                 resultSymbols.Data
                .Select(x => ParseSymbol(x, resultAvailable.Data)!)
                .Where(x => x != null)
                .ToArray();

            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, _api.EnvironmentName, null, resultData);
            return HttpResult.Ok(resultSymbols, SharedUtils.ApplySymbolFilter(resultData, request));
        }

        private SharedSpotSymbol? ParseSymbol(LBankSymbol s, string[] availableSymbols)
        {
            var assets = s.Symbol.Split('_');
            if (assets.Length != 2)
                return null;

            var available = availableSymbols.Contains(s.Symbol);
            var result = new SharedSpotSymbol(assets[0], assets[1], s.Symbol, available)
            {
                PriceDecimals = s.PriceAccuracy,
                QuantityDecimals = s.QuantityAccuracy,
                QuantityStep = s.MinQuantityStep,
                MinTradeQuantity = s.MinOrderQuantity,
                DisplayName = s.Symbol
            };

            if (LibraryHelpers.IsCommodity(result.BaseAsset))
            {
                result.BaseAssetType = SharedAssetType.TradFi;
                result.BaseAssetSubType = SharedAssetSubType.Commodity;
            }
            else if (LibraryHelpers.IsEquity(result.BaseAsset, ["x", "on"], [])
                && result.BaseAsset.EndsWith("x") || result.BaseAsset.EndsWith("on"))
            {
                result.BaseAssetType = SharedAssetType.TradFi;
                result.BaseAssetSubType = SharedAssetSubType.Equity;
            }
            else
            {
                result.BaseAssetType = SharedAssetType.Crypto;
                if (LibraryHelpers.IsStableCoin(result.BaseAsset))
                    result.BaseAssetSubType = SharedAssetSubType.StableCoin;
            }

            if (_knownFiats.Contains(result.QuoteAsset))
            {
                result.QuoteAssetType = SharedAssetType.Fiat;
            }
            else
            {
                result.QuoteAssetType = SharedAssetType.Crypto;
                if (LibraryHelpers.IsStableCoin(result.QuoteAsset))
                    result.QuoteAssetSubType = SharedAssetSubType.StableCoin;
            }

            return result;
        }

        public async Task<ExchangeCallResult<SharedSymbol[]>> GetSpotSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetSpotSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<SharedSymbol[]>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<SharedSymbol[]>.Ok(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, _api.EnvironmentName, null, baseAsset));
        }

        public async Task<ExchangeCallResult<bool>> SupportsSpotSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode != TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Only Spot symbols allowed");

            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetSpotSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, _api.EnvironmentName, null, symbol));
        }

        public async Task<ExchangeCallResult<bool>> SupportsSpotSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetSpotSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, _api.EnvironmentName, null, symbolName));
        }
        #endregion

        #region Ticker client

        public GetSpotTickerOptions GetSpotTickerOptions { get; } = new GetSpotTickerOptions(_exchangeName);
        public async Task<HttpResult<SharedSpotTicker>> GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = GetSpotTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTicker>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await _api.ExchangeData.GetTickersAsync(symbol, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotTicker>(result);

            var ticker = result.Data.SingleOrDefault();
            if (ticker == null)
                return HttpResult.Fail<SharedSpotTicker>(Exchange, new ServerError(ErrorType.UnknownSymbol, $"No ticker found for symbol {request.Symbol}"));


            return HttpResult.Ok(result, new SharedSpotTicker(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, symbol),
                symbol,
                ticker.Ticker.LastPrice,
                ticker.Ticker.HighPrice,
                ticker.Ticker.LowPrice,
                new SharedOrderQuantity(ticker.Ticker.Volume, ticker.Ticker.Turnover),
                ticker.Ticker.PriceChangePercentage)
            {
            });
        }

        Task<HttpResult<SharedSpotTicker[]>> ISpotTickerRestClient.GetSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
            => GetAllSpotTickersAsync(request, ct);
        GetAllSpotTickersOptions ISpotTickerRestClient.GetSpotTickersOptions => GetAllSpotTickersOptions;

        public GetAllSpotTickersOptions GetAllSpotTickersOptions { get; } = new GetAllSpotTickersOptions(_exchangeName);
        public async Task<HttpResult<SharedSpotTicker[]>> GetAllSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = GetAllSpotTickersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTicker[]>(Exchange, validationError);

            var result = await _api.ExchangeData.GetTickersAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotTicker[]>(result);

            return HttpResult.Ok(result, result.Data.Select(x =>
                new SharedSpotTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                    x.Symbol,
                    x.Ticker.LastPrice,
                    x.Ticker.HighPrice,
                    x.Ticker.LowPrice,
                    new SharedOrderQuantity(x.Ticker.Volume, x.Ticker.Turnover),
                    x.Ticker.PriceChangePercentage)
                {
                }).ToArray());
        }

        #endregion

        #region Spot Order client

        public SharedFeeDeductionType SpotFeeDeductionType => SharedFeeDeductionType.DeductFromOutput;
        public SharedFeeAssetType SpotFeeAssetType => SharedFeeAssetType.QuoteAsset;
        public SharedOrderType[] SpotSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market, SharedOrderType.LimitMaker };
        public SharedTimeInForce[] SpotSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel, SharedTimeInForce.FillOrKill };

        public string GenerateClientOrderId() => ExchangeHelpers.RandomString(32);

        public SharedQuantitySupport SpotSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset,
                SharedQuantityType.QuoteAsset,
                SharedQuantityType.BaseAsset);

        public PlaceSpotOrderOptions PlaceSpotOrderOptions { get; } = new PlaceSpotOrderOptions(_exchangeName);
        public async Task<HttpResult<SharedId>> PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
        {
            var validationError = PlaceSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var orderType = DetermineOrderType(request);
            var result = await _api.Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                orderType,
                request.Quantity?.QuantityInBaseAsset ?? request.Quantity?.QuantityInQuoteAsset ?? 0,
                request.Price,
                clientOrderId: request.ClientOrderId,
                ct: ct).ConfigureAwait(false);

            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.OrderId));
        }

        private OrderType DetermineOrderType(PlaceSpotOrderRequest request)
        {
            if (request.OrderType == SharedOrderType.Market)
                return request.Side == SharedOrderSide.Buy ? OrderType.BuyMarket : OrderType.SellMarket;

            if (request.OrderType == SharedOrderType.LimitMaker)
                return request.Side == SharedOrderSide.Buy ? OrderType.BuyMaker : OrderType.SellMaker;

            if (request.TimeInForce == SharedTimeInForce.FillOrKill)
                return request.Side == SharedOrderSide.Buy ? OrderType.BuyFok : OrderType.SellFok;

            if (request.TimeInForce == SharedTimeInForce.ImmediateOrCancel)
                return request.Side == SharedOrderSide.Buy ? OrderType.BuyIoc : OrderType.SellIoc;

            return request.Side == SharedOrderSide.Buy ? OrderType.BuyLimit : OrderType.SellLimit;
        }

        public GetSpotOrderOptions GetSpotOrderOptions { get; } = new GetSpotOrderOptions(_exchangeName, true);
        public async Task<HttpResult<SharedSpotOrder>> GetSpotOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = GetSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder>(Exchange, validationError);

            var order = await _api.Trading.GetOrderAsync(request.Symbol!.GetSymbol(FormatSymbol), request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedSpotOrder>(order);

            var (side, type) = ParseOrderType(order.Data.Type);
            return HttpResult.Ok(order, new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, order.Data.Symbol),
                order.Data.Symbol,
                order.Data.OrderId,
                type,
                side,
                ParseOrderStatus(order.Data.Status),
                order.Data.Timestamp)
            {
                ClientOrderId = order.Data.ClientOrderId,
                OrderPrice = order.Data.Price,
                OrderQuantity = new SharedOrderQuantity(order.Data.OriginalQuantity, order.Data.OriginalQuoteOrderQuantity),
                QuantityFilled = new SharedOrderQuantity(order.Data.ExecutedQuantity, order.Data.CummulativeQuoteQuantity),
                TimeInForce = ParseTimeInForce(order.Data.Type),
                UpdateTime = order.Data.UpdateTime
            });
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

        public GetOpenSpotOrdersOptions GetOpenSpotOrdersOptions { get; } = new GetOpenSpotOrdersOptions(_exchangeName, true)
        {
            RequiredRequestParameters = [
                new ParameterDescription(nameof(GetOpenOrdersRequest.Symbol), typeof(SharedSymbol), "Symbol to request open orders for", "eth_usdt")
                ]
        };
        public async Task<HttpResult<SharedSpotOrder[]>> GetOpenSpotOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = GetOpenSpotOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder[]>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var order = await _api.Trading.GetOpenOrdersAsync(symbol: symbol, 1, 200, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedSpotOrder[]>(order);

            return HttpResult.Ok(order, order.Data.Orders.Select(x =>
            {
                var (side, type) = ParseOrderType(x.Type);
                return new SharedSpotOrder(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                    x.Symbol,
                    x.OrderId,
                    type,
                    side,
                    ParseOrderStatus(x.Status),
                    x.Timestamp)
                {
                    ClientOrderId = x.ClientOrderId,
                    OrderPrice = x.Price,
                    OrderQuantity = new SharedOrderQuantity(x.OriginalQuantity, x.OriginalQuoteOrderQuantity),
                    QuantityFilled = new SharedOrderQuantity(x.ExecutedQuantity, x.CummulativeQuoteQuantity),
                    TimeInForce = ParseTimeInForce(x.Type),
                    UpdateTime = x.UpdateTime
                };
            }).ToArray());
        }

        public GetSpotClosedOrdersOptions GetClosedSpotOrdersOptions { get; } = new GetSpotClosedOrdersOptions(_exchangeName, false, true, false, 200)
        {
            MaxAge = TimeSpan.FromDays(7)
        };
        public async Task<HttpResult<SharedSpotOrder[]>> GetClosedSpotOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetClosedSpotOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder[]>(Exchange, validationError);

            // Determine page token
            int limit = request.Limit ?? 200;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await _api.Trading.GetOrdersAsync(
                symbol: symbol,
                page: pageParams.Page ?? 1,
                pageSize: limit,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotOrder[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => Pagination.NextPageFromPage(pageParams),
                     result.Data.Orders.Length,
                     result.Data.Orders.Select(x => x.Timestamp),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams,
                     maxAge: TimeSpan.FromDays(7));

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data.Orders, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                .Select(x =>
                {
                    var (side, type) = ParseOrderType(x.Type);
                    return new SharedSpotOrder(
                        ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                        x.Symbol,
                        x.OrderId,
                        type,
                        side,
                        ParseOrderStatus(x.Status),
                        x.Timestamp)
                    {
                        ClientOrderId = x.ClientOrderId,
                        OrderPrice = x.Price,
                        OrderQuantity = new SharedOrderQuantity(x.OriginalQuantity, x.OriginalQuoteOrderQuantity),
                        QuantityFilled = new SharedOrderQuantity(x.ExecutedQuantity, x.CummulativeQuoteQuantity),
                        TimeInForce = ParseTimeInForce(x.Type),
                        UpdateTime = x.UpdateTime
                    };
                })
                .ToArray(), nextPageRequest);
        }

        public GetSpotOrderTradesOptions GetSpotOrderTradesOptions { get; } = new GetSpotOrderTradesOptions(_exchangeName, true)
        {
            RequestNotes = "API does not allow filtering by order id, so trades are only found if they;re in the 100 most recent trades"
        };
        public async Task<HttpResult<SharedUserTrade[]>> GetSpotOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var validationError = GetSpotOrderTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var order = await _api.Trading.GetUserTradesAsync(symbol: symbol, request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedUserTrade[]>(order);

            return HttpResult.Ok(order, order.Data.Select(x => new SharedUserTrade(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                x.Symbol,
                x.OrderId.ToString(),
                x.Id.ToString(),
                x.IsBuyer ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                new SharedOrderQuantity(x.Quantity, x.QuoteQuantity),
                x.Price,
                x.Timestamp)
            {
                Fee = x.Commission,
                Role = x.IsMaker ? SharedRole.Maker : SharedRole.Taker
            }).ToArray());
        }


        Task<HttpResult<SharedUserTrade[]>> ISpotOrderRestClient.GetSpotUserTradesAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
            => GetSpotUserTradeHistoryAsync(request, pageRequest, ct);
        GetSpotUserTradeHistoryOptions ISpotOrderRestClient.GetSpotUserTradesOptions => GetSpotUserTradeHistoryOptions;

        public GetSpotUserTradeHistoryOptions GetSpotUserTradeHistoryOptions { get; } = new GetSpotUserTradeHistoryOptions(_exchangeName, false, true, true, 100)
        {
        };
        public async Task<HttpResult<SharedUserTrade[]>> GetSpotUserTradeHistoryAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetSpotUserTradeHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            // Determine page token
            int limit = request.Limit ?? 100;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await _api.Trading.GetUserTradesAsync(
                symbol: symbol,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                limit: pageParams.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedUserTrade[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => Pagination.NextPageFromTime(pageParams, result.Data.Min(x => x.Timestamp)),
                     result.Data.Length,
                     result.Data.Select(x => x.Timestamp),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams,
                     maxAge: TimeSpan.FromDays(30));

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                .Select(x =>
                    new SharedUserTrade(
                        ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                        x.Symbol,
                        x.OrderId.ToString(),
                        x.Id.ToString(),
                        x.IsBuyer ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                        new SharedOrderQuantity(x.Quantity, x.QuoteQuantity),
                        x.Price,
                        x.Timestamp)
                    {
                        Fee = x.Commission,
                        Role = x.IsMaker ? SharedRole.Maker : SharedRole.Taker
                    })
                .ToArray(), nextPageRequest);
        }

        public CancelSpotOrderOptions CancelSpotOrderOptions { get; } = new CancelSpotOrderOptions(_exchangeName, true);
        public async Task<HttpResult<SharedId>> CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await _api.Trading.CancelOrderAsync(request.Symbol!.GetSymbol(FormatSymbol), request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(request.OrderId));
        }

        private SharedOrderStatus ParseOrderStatus(OrderStatus status)
        {
            if (status == Enums.OrderStatus.Canceled || status == Enums.OrderStatus.PartiallyCanceled)
                return SharedOrderStatus.Canceled;
            if (status == Enums.OrderStatus.Open || status == Enums.OrderStatus.PartiallyFilled || status == OrderStatus.Cancelling)
                return SharedOrderStatus.Open;
            if (status == OrderStatus.Filled)
                return SharedOrderStatus.Filled;

            return SharedOrderStatus.Unknown;
        }

        private SharedTimeInForce? ParseTimeInForce(OrderType type)
        {
            if (type == OrderType.BuyLimit || type == OrderType.SellLimit) return SharedTimeInForce.GoodTillCanceled;
            if (type == OrderType.SellIoc || type == OrderType.BuyIoc) return SharedTimeInForce.ImmediateOrCancel;
            if (type == OrderType.SellFok || type == OrderType.BuyFok) return SharedTimeInForce.FillOrKill;

            return null;
        }
        #endregion

        #region Spot Client Id Order Client

        public GetSpotOrderByClientOrderIdOptions GetSpotOrderByClientOrderIdOptions { get; } = new GetSpotOrderByClientOrderIdOptions(_exchangeName, true);
        public async Task<HttpResult<SharedSpotOrder>> GetSpotOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = GetSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder>(Exchange, validationError);

            var order = await _api.Trading.GetOrderAsync(request.Symbol!.GetSymbol(FormatSymbol), clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedSpotOrder>(order);

            var (side, type) = ParseOrderType(order.Data.Type);
            return HttpResult.Ok(order, new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, order.Data.Symbol),
                order.Data.Symbol,
                order.Data.OrderId,
                type,
                side,
                ParseOrderStatus(order.Data.Status),
                order.Data.Timestamp)
            {
                ClientOrderId = order.Data.ClientOrderId,
                OrderPrice = order.Data.Price,
                OrderQuantity = new SharedOrderQuantity(order.Data.OriginalQuantity, order.Data.OriginalQuoteOrderQuantity),
                QuantityFilled = new SharedOrderQuantity(order.Data.ExecutedQuantity, order.Data.CummulativeQuoteQuantity),
                TimeInForce = ParseTimeInForce(order.Data.Type),
                UpdateTime = order.Data.UpdateTime
            });
        }

        public CancelSpotOrderByClientOrderIdOptions CancelSpotOrderByClientOrderIdOptions { get; } = new CancelSpotOrderByClientOrderIdOptions(_exchangeName, true);
        public async Task<HttpResult<SharedId>> CancelSpotOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await _api.Trading.CancelOrderAsync(request.Symbol!.GetSymbol(FormatSymbol), clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(request.OrderId));
        }
        #endregion
    }
}
