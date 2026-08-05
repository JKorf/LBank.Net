using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LBank.Net
{
    internal class LBankAuthenticationProvider : AuthenticationProvider<LBankCredentials>
    {
        public override string Key => ApiCredentials.Credential.Key;


        public LBankAuthenticationProvider(LBankCredentials credentials) : base(credentials)
        {
        }


        public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration requestConfig)
        {
            if (!requestConfig.RequestDefinition.Authenticated)
                return;

            var timestamp = GetMillisecondTimestampLong(apiClient);
            var echoStr = ExchangeHelpers.RandomString(35);
            var signatureMethod = ApiCredentials.Credential is HMACCredential ? "HmacSHA256" : "RSA";

            var requestParameters = requestConfig.GetPositionParameters();
            var signParameters = new SortedDictionary<string, object>(requestParameters);
            signParameters.Add("api_key", Key);
            signParameters.Add("echostr", echoStr);
            signParameters.Add("timestamp", timestamp);
            signParameters.Add("signature_method", signatureMethod);    

            var signStr = signParameters.ToFormData();
            var md5 = SignMD5(signStr).ToUpperInvariant();
            string signature;
            if (ApiCredentials.Credential is HMACCredential hm)
                signature = SignHMACSHA256(hm, Encoding.UTF8.GetBytes(md5)).ToLower();
            else if (ApiCredentials.Credential is RSACredential rsa)
                signature = SignRSASHA256(rsa, Encoding.UTF8.GetBytes(md5), SignOutputType.Base64);
            else
                throw new InvalidOperationException("Invalid credential type");

            requestConfig.Headers ??= new Dictionary<string, string>();
            requestConfig.Headers.Add("timestamp", timestamp.ToString());
            requestConfig.Headers.Add("echostr", echoStr);
            requestConfig.Headers.Add("signature_method", signatureMethod);

            requestParameters.Add("api_key", Key);
            requestParameters.Add("sign", signature);
        }
    }
}
