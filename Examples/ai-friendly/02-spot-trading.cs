// 02-spot-trading.cs
//
// Demonstrates: LBank Spot order placement, lookup, open orders, and cancellation.
//
// Setup: dotnet add package JKorf.LBank.Net
//
// WARNING: This example submits a real order when used with live credentials.

using LBank.Net;
using LBank.Net.Clients;
using LBank.Net.Enums;

var client = new LBankRestClient(options =>
{
    options.ApiCredentials = new LBankCredentials("API_KEY", "API_SECRET");
});

const string symbol = "eth_usdt";
var clientOrderId = $"ai-example-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

// LBank combines side and execution behavior in OrderType.
var order = await client.SpotApi.Trading.PlaceOrderAsync(
    symbol: symbol,
    orderType: OrderType.BuyLimit,
    quantity: 0.01m,
    price: 1m,
    clientOrderId: clientOrderId);

if (!order.Success)
{
    Console.WriteLine($"Order rejected: {order.Error}");
    return;
}

Console.WriteLine($"Placed order {order.Data.OrderId} ({order.Data.ClientOrderId})");

var orderInfo = await client.SpotApi.Trading.GetOrderAsync(
    symbol,
    orderId: order.Data.OrderId);

if (orderInfo.Success)
{
    Console.WriteLine(
        $"Order status={orderInfo.Data.Status}, original={orderInfo.Data.OriginalQuantity}, " +
        $"executed={orderInfo.Data.ExecutedQuantity}");
}

// Open-order and order-history endpoints are paginated.
var openOrders = await client.SpotApi.Trading.GetOpenOrdersAsync(
    symbol,
    page: 1,
    pageSize: 50);

if (openOrders.Success)
    Console.WriteLine($"Open orders on {symbol}: {openOrders.Data.Orders.Length}");

var cancel = await client.SpotApi.Trading.CancelOrderAsync(
    symbol,
    orderId: order.Data.OrderId);

Console.WriteLine(cancel.Success
    ? $"Canceled order {cancel.Data.OrderId}"
    : $"Cancel failed: {cancel.Error}");
