using CryptoExchange.Net.Objects.Errors;

namespace LBank.Net
{
    internal static class LBankErrors
    {
        public static ErrorMapping Errors { get; } = new ErrorMapping(
            [
                new ErrorInfo(ErrorType.SystemError, false, "Server error", "10000", "10017"),

                new ErrorInfo(ErrorType.MissingParameter, false, "Parameter can not be null", "10001"),

                new ErrorInfo(ErrorType.InvalidParameter, false, "Parameter validation failed", "10002", "10003"),

                new ErrorInfo(ErrorType.RateLimitRequest, false, "Too many requests", "10004"),

                new ErrorInfo(ErrorType.Unauthorized, false, "Unknown API key", "10005"),
                new ErrorInfo(ErrorType.Unauthorized, false, "Invalid credentials", "10007"),
                new ErrorInfo(ErrorType.Unauthorized, false, "Missing permission or IP whitelist", "10022", "10067"),

                new ErrorInfo(ErrorType.UnavailableSymbol, false, "Symbol not available or allowed", "10008", "10024"),

                new ErrorInfo(ErrorType.InvalidQuantity, false, "Quantity too small", "10010", "10013", "10014", "10016", "10020", "10126"),
                new ErrorInfo(ErrorType.InvalidQuantity, false, "Quantity too large", "10121", "10128"),
                new ErrorInfo(ErrorType.InvalidPrice, false, "Limit price precision invalid", "10021"),
                new ErrorInfo(ErrorType.InvalidPrice, false, "Order price invalid", "10108", "10122", "10123"),

                new ErrorInfo(ErrorType.UnknownOrder, false, "Uknown order", "10032"),

                new ErrorInfo(ErrorType.DuplicateClientOrderId, false, "Duplicate client order id", "10036"),

            ]
            );
    }
}
