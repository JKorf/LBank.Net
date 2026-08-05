using LBank.Net.Clients;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using LBank.Net.Objects.Options;
using Microsoft.Extensions.Options;

// REST
var restClient = new LBankRestClient();
var ticker = await restClient.SpotApi.ExchangeData.GetTickersAsync("eth_usdt");
if (!ticker.Success)
{
    Console.WriteLine($"Failed to get ticker: {ticker.Error}");
    return;
}

Console.WriteLine($"Rest client ticker price for eth_usdt: {ticker.Data.Single().Ticker.LastPrice}");

Console.WriteLine();
Console.WriteLine("Press enter to start websocket subscription");
Console.ReadLine();

// Websocket
// Optional, manually add logging
var logFactory = new LoggerFactory();
logFactory.AddProvider(new TraceLoggerProvider());

var socketClient = new LBankSocketClient(Options.Create(new LBankSocketOptions { }), logFactory);
var subscription = await socketClient.SpotApi.SubscribeToTickerUpdatesAsync("eth_usdt", update =>
{
    Console.WriteLine($"Websocket client ticker price for eth_usdt: {update.Data.LastPrice}");
});

if (!subscription.Success)
{
    Console.WriteLine($"Failed to subscribe to ticker updates: {subscription.Error}");
    return;
}

Console.ReadLine();
