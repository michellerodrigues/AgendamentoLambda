using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SagaApiLambda
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class BodyJson
    {
    }

    public class Path
    {
    }

    public class Querystring
    {
        public string msgid { get; set; }
    }

    public class Header
    {
        public string accept { get; set; }
        [JsonProperty("accept-encoding")]
        public string AcceptEncoding { get; set; }
        [JsonProperty("accept-language")]
        public string AcceptLanguage { get; set; }
        [JsonProperty("cache-control")]
        public string CacheControl { get; set; }
        public string Host { get; set; }
        [JsonProperty("sec-ch-ua")]
        public string SecChUa { get; set; }
        [JsonProperty("sec-ch-ua-mobile")]
        public string SecChUaMobile { get; set; }
        [JsonProperty("sec-fetch-dest")]
        public string SecFetchDest { get; set; }
        [JsonProperty("sec-fetch-mode")]
        public string SecFetchMode { get; set; }
        [JsonProperty("sec-fetch-site")]
        public string SecFetchSite { get; set; }
        [JsonProperty("sec-fetch-user")]
        public string SecFetchUser { get; set; }
        [JsonProperty("upgrade-insecure-requests")]
        public string UpgradeInsecureRequests { get; set; }
        [JsonProperty("User-Agent")]
        public string UserAgent { get; set; }
        [JsonProperty("X-Amzn-Trace-Id")]
        public string XAmznTraceId { get; set; }
        [JsonProperty("X-Forwarded-For")]
        public string XForwardedFor { get; set; }
        [JsonProperty("X-Forwarded-Port")]
        public string XForwardedPort { get; set; }
        [JsonProperty("X-Forwarded-Proto")]
        public string XForwardedProto { get; set; }
    }

    public class Params
    {
        public Path path { get; set; }
        public Querystring querystring { get; set; }
        public Header header { get; set; }
    }

    public class StageVariables
    {
    }

    public class Context
    {
        [JsonProperty("account-id")]
        public string AccountId { get; set; }
        [JsonProperty("api-id")]
        public string ApiId { get; set; }
        [JsonProperty("api-key")]
        public string ApiKey { get; set; }
        [JsonProperty("authorizer-principal-id")]
        public string AuthorizerPrincipalId { get; set; }
        public string caller { get; set; }
        [JsonProperty("cognito-authentication-provider")]
        public string CognitoAuthenticationProvider { get; set; }
        [JsonProperty("cognito-authentication-type")]
        public string CognitoAuthenticationType { get; set; }
        [JsonProperty("cognito-identity-id")]
        public string CognitoIdentityId { get; set; }
        [JsonProperty("cognito-identity-pool-id")]
        public string CognitoIdentityPoolId { get; set; }
        [JsonProperty("http-method")]
        public string HttpMethod { get; set; }
        public string stage { get; set; }
        [JsonProperty("source-ip")]
        public string SourceIp { get; set; }
        public string user { get; set; }
        [JsonProperty("user-agent")]
        public string UserAgent { get; set; }
        [JsonProperty("user-arn")]
        public string UserArn { get; set; }
        [JsonProperty("request-id")]
        public string RequestId { get; set; }
        [JsonProperty("resource-id")]
        public string ResourceId { get; set; }
        [JsonProperty("resource-path")]
        public string ResourcePath { get; set; }
    }

    public class LambdaRequestMessage
    {
        [JsonProperty("body-json")]
        public BodyJson BodyJson { get; set; }
        [JsonProperty("params")]
        public Params Parametros { get; set; }
        [JsonProperty("stage-variables")]
        public StageVariables StageVariables { get; set; }
        public Context context { get; set; }
    }
}
