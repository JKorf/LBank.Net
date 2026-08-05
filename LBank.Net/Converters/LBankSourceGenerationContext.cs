using LBank.Net.Objects.Internal;
using LBank.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LBank.Net.Converters
{
    [JsonSerializable(typeof(LBankResponse<LBankSymbol[]>))]
    [JsonSerializable(typeof(LBankResponse<LBankAsset[]>))]
    [JsonSerializable(typeof(LBankResponse<DateTime>))]
    [JsonSerializable(typeof(LBankResponse<LBankOrderBook>))]
    [JsonSerializable(typeof(LBankResponse<LBankPrice[]>))]
    [JsonSerializable(typeof(LBankResponse<LBankBookTicker>))]
    [JsonSerializable(typeof(LBankResponse<LBankSymbolTicker[]>))]
    [JsonSerializable(typeof(LBankResponse<LBankTrade[]>))]
    [JsonSerializable(typeof(LBankResponse<LBankKline[]>))]
    [JsonSerializable(typeof(LBankResponse<LBankApiKey>))]
    [JsonSerializable(typeof(LBankResponse<LBankBalance[]>))]
    [JsonSerializable(typeof(LBankResponse<LBankDepositPage>))]
    [JsonSerializable(typeof(LBankResponse<LBankWithdrawResult>))]
    [JsonSerializable(typeof(LBankResponse<LBankWithdrawalPage>))]
    [JsonSerializable(typeof(LBankResponse<LBankDepositAddress>))]
    [JsonSerializable(typeof(LBankResponse<Dictionary<string, LBankAssetDetails>>))]
    [JsonSerializable(typeof(LBankResponse<LBankTradeFee[]>))]
    [JsonSerializable(typeof(LBankResponse<LBankOrderResult>))]
    [JsonSerializable(typeof(LBankResponse<LBankOrder>))]
    [JsonSerializable(typeof(LBankResponse<LBankOrder[]>))]
    [JsonSerializable(typeof(LBankResponse<LBankOrderPage>))]
    [JsonSerializable(typeof(LBankResponse<LBankAccountInfo>))]
    [JsonSerializable(typeof(LBankResponse<LBankUserTrade[]>))]
    [JsonSerializable(typeof(LBankResponse<string[]>))]
    [JsonSerializable(typeof(LBankResponse<string>))]
    [JsonSerializable(typeof(LBankResponse<LBankListenKey>))]
    [JsonSerializable(typeof(LBankResponse))]

    [JsonSerializable(typeof(LBankPing))]
    [JsonSerializable(typeof(LBankPong))]
    [JsonSerializable(typeof(LBankSubscribeMessage))]
    [JsonSerializable(typeof(LBankError))]
    [JsonSerializable(typeof(LBankTradeUpdateMessage))]
    [JsonSerializable(typeof(LBankKlineUpdateMessage))]
    [JsonSerializable(typeof(LBankOrderBookUpdateMessage))]
    [JsonSerializable(typeof(LBankTickerUpdateMessage))]
    [JsonSerializable(typeof(LBankOrderUpdateMessage))]
    [JsonSerializable(typeof(LBankBalanceUpdateMessage))]

    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(int?))]
    [JsonSerializable(typeof(int))]
    [JsonSerializable(typeof(long?))]
    [JsonSerializable(typeof(long))]
    [JsonSerializable(typeof(decimal))]
    [JsonSerializable(typeof(decimal?))]
    [JsonSerializable(typeof(DateTime))]
    [JsonSerializable(typeof(DateTime?))]
    internal partial class LBankSourceGenerationContext : JsonSerializerContext
    {
    }
}
