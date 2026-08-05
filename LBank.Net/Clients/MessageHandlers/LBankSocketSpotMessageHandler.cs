using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using LBank.Net.Objects.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace LBank.Net.Clients.MessageHandlers
{
    internal class LBankSocketSpotMessageHandler : JsonSocketMessageHandler
    {
        private readonly static HashSet<string> _idValues = ["kbar", "trade", "depth", "tick"];
        public override JsonSerializerOptions Options { get; } = LBankExchange._serializerContext;

        public LBankSocketSpotMessageHandler()
        {
            AddTopicMapping<LBankTradeUpdateMessage>(x => x.Symbol);
            AddTopicMapping<LBankKlineUpdateMessage>(x => x.Symbol + x.Kline.Interval);
            AddTopicMapping<LBankOrderBookUpdateMessage>(x => x.Symbol + x.Count);
            AddTopicMapping<LBankTickerUpdateMessage>(x => x.Symbol);
        }

        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [

            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("ping"),
                ],
                StaticIdentifier = "ping",
                ForceIfFound = true
            },
            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("status"),
                ],
                StaticIdentifier = "error",
            },

            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("type"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("type")!,
            },
        ];

        protected override string? GetTypeIdentifierNonJson(ReadOnlySpan<byte> data, WebSocketMessageType? webSocketMessageType) 
        {
            var split = Encoding.UTF8.GetString(data.ToArray()).Split(["_"], StringSplitOptions.RemoveEmptyEntries);
            var idIndex = 0;
            for(var i = 1; i < split.Length; i++)
            {
                if (_idValues.Contains(split[i]))
                {
                    idIndex = i;
                    break;
                }
            }

            return string.Join("_", split.Skip(idIndex));
        }
    }
}
