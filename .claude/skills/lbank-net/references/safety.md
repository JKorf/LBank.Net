# LBank operational considerations

## Credentials and permissions

HMAC uses key and secret. RSA XML uses `new LBankCredentials().WithRSAXml(key, privateKey)`; PEM uses `WithRSAPem` on netstandard2.1 or supported modern .NET targets and is unavailable on netstandard2.0. Keep private material in application secret storage and select the authentication type configured on the exchange API key. Trading permissions do not imply withdrawal permissions. The upstream order API may not be enabled for every account; surface permission errors instead of changing signing schemes or retrying blindly.

## Orders and cancellation

`PlaceOrderAsync` submits a live order; there is no test-order method on the current interface. `OrderType` encodes side and behavior. Market-buy quantity is quote-asset value; market sells and limit orders have different sizing semantics. Validate symbol rules, precision, minimum quantity, and funds for the selected type before sending.

A limit order can execute immediately; a low example price is not a safety guarantee. Cancellation can race with fills. Inspect order status and executed quantity/trades to determine remaining exposure. Use one of exchange order ID or client order ID for lookup/cancel. Retain unique client IDs to correlate uncertain requests. Cancel-all affects the selected symbol's open orders; do not infer that no funds were spent from a successful cancellation.

The current native trading interface has no batch placement workflow or futures position-mode controls. Inspect each result independently when application code submits multiple orders; success of one does not describe the others.

## Withdrawals

Withdrawals move funds. Match the address to the intended network and preserve required memo/tag, fee, quantity, notes, name, client ID, and internal-transfer setting. Read current asset/network information and withdrawal history where needed. A successful submission is not proof of final delivery. Reconcile an uncertain withdrawal using its client ID and history before considering resubmission.

## Private streams and cached clients

Private order/balance subscriptions accept a listen key. Passing null with credentials lets the library acquire and maintain it; do not add a second keepalive loop. When explicitly managing a key through REST start/keepalive/stop methods, own that lifecycle and avoid stopping a key still used by another subscription. Public v3/v2 selection is separate from authenticated listen-key ownership. Clear cached per-user clients when credentials change.

## Uncertain submissions and retries

A timeout or connection loss can leave an order or withdrawal accepted but unconfirmed. `Error.IsTransient` alone does not justify repeating it. Query orders by retained identifiers, inspect fills/open orders, or reconcile withdrawal history before deciding whether to resubmit; allow for status propagation delays. Bounded backoff is suitable for transient public reads. Keep a generic read-retry helper away from account-changing requests.
