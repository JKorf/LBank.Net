# LBank.Net examples

Each file is an independent console program. Install `LBank.Net` in a console project and copy one program to `Program.cs`.

| Program | Workflow |
|---|---|
| [01-spot-quickstart.cs](01-spot-quickstart.cs) | Public data, balances, symbol rules, books, and candles |
| [02-spot-trading.cs](02-spot-trading.cs) | Live order, lookup, paginated open orders, and cancellation |
| [03-websocket.cs](03-websocket.cs) | Public/private subscriptions and cleanup |
| [04-di-order-book.cs](04-di-order-book.cs) | DI, Shared API V2 ticker/symbol catalog, and managed local book |
| [05-error-handling.cs](05-error-handling.cs) | Results and bounded public-read retries |

Replace API key/secret placeholders only for intended private operations. The trading program submits a live order when executed; cancellation can race with fills. Read [operational considerations](../../.claude/skills/lbank-net/references/safety.md) first. See [usage snippets](../../.claude/skills/lbank-net/references/usage.md) for focused patterns.

`AiExampleCompileTests` compiles these programs without running exchange operations.
