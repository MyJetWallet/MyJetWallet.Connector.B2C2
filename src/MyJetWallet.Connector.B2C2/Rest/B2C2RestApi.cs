using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyJetWallet.Connector.B2C2.Rest.Enums;
using MyJetWallet.Connector.B2C2.Rest.Models;
using MyJetWallet.Connector.B2C2.Rest.Util;

namespace MyJetWallet.Connector.B2C2.Rest
{
    public class B2C2RestApi
    {
        private const string Url = "https://api.uat.b2c2.net/";

        private readonly Client _client;

        private readonly HttpClient _httpClient;

        public B2C2RestApi(Client client)
        {
            _client = client;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(Url),
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        #region Util

        private async Task<B2C2Response> CallAsync(HttpMethod method, string endpoint, string body = null)
        {
            var request = new HttpRequestMessage(method, endpoint);

            if (body != null) request.Content = new StringContent(body, Encoding.UTF8, "application/json");

            request.Headers.Add("Authorization", "Token " + _client.ApiKey);

            var response = await _httpClient.SendAsync(request).ConfigureAwait(true);

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return new B2C2Response(response.IsSuccessStatusCode, result);
        }

        #endregion

        #region Account

        public async Task<B2C2Result<Balances>> GetAccountBalance()
        {
            var apiPath = "balance/";
            var result = await CallAsync(HttpMethod.Get, apiPath);
            return new B2C2Result<Balances>
            {
                Success = result.Success,
                Result = result.Success
                    ? new Balances(JsonSerializer.Deserialize<Dictionary<string, string>>(result.body))
                    : null,
                Error = result.Success ? null : JsonSerializer.Deserialize<B2C2Errors>(result.body)
            };
        }

        public async Task<B2C2Result<AccountInfo>> GetAccountInfo()
        {
            var apiPath = "account_info/";
            var result = await CallAsync(HttpMethod.Get, apiPath);
            return new B2C2Result<AccountInfo>
            {
                Success = result.Success,
                Result = result.Success
                    ? new AccountInfo(JsonSerializer.Deserialize<Dictionary<string, string>>(result.body))
                    : null,
                Error = result.Success ? null : JsonSerializer.Deserialize<B2C2Errors>(result.body)
            };
        }

        #endregion

        #region Orders

        public async Task<B2C2Result<Order>> PlaceMarketOrder(string clientOrderId,
            string instrument, OrderSide side, decimal quantity)
        {
            var apiPath = "order/";

            var body =
                $"{{\"client_order_id\": \"{clientOrderId}\"," +
                $"\"instrument\": \"{instrument}\"," +
                "\"order_type\": \"MKT\"," +
                $"\"side\": \"{side}\"," +
                $"\"valid_until\": \"{DateTime.Now.AddMinutes(5):yyyy-MM-ddTHH:mm:ss}\"," +
                $"\"quantity\": \"{quantity}\"}}";

            var result = await CallAsync(HttpMethod.Post, apiPath, body);
            return new B2C2Result<Order>
            {
                Success = result.Success,
                Result = result.Success ? JsonSerializer.Deserialize<Order>(result.body) : null,
                Error = result.Success ? null : JsonSerializer.Deserialize<B2C2Errors>(result.body)
            };
        }

        public async Task<B2C2Result<List<Order>>> GetOrders(string orderId)
        {
            var apiPath = $"order/{orderId}/";

            var result = await CallAsync(HttpMethod.Get, apiPath);
            return new B2C2Result<List<Order>>
            {
                Success = result.Success,
                Result = result.Success ? JsonSerializer.Deserialize<List<Order>>(result.body) : null,
                Error = result.Success ? null : JsonSerializer.Deserialize<B2C2Errors>(result.body)
            };
        }

        #endregion

        #region MarketData

        public async Task<B2C2Result<Dictionary<string, Currency>>> GetCurrenciesList()
        {
            var apiPath = "currency/";
            var result = await CallAsync(HttpMethod.Get, apiPath);
            return new B2C2Result<Dictionary<string, Currency>>
            {
                Success = result.Success,
                Result = result.Success ? JsonSerializer.Deserialize<Dictionary<string, Currency>>(result.body) : null,
                Error = result.Success ? null : JsonSerializer.Deserialize<B2C2Errors>(result.body)
            };
        }

        public async Task<B2C2Result<List<Instrument>>> GetInstrumentsList()
        {
            var apiPath = "instruments/";
            var result = await CallAsync(HttpMethod.Get, apiPath);
            return new B2C2Result<List<Instrument>>
            {
                Success = result.Success,
                Result = result.Success ? JsonSerializer.Deserialize<List<Instrument>>(result.body) : null,
                Error = result.Success ? null : JsonSerializer.Deserialize<B2C2Errors>(result.body)
            };
        }

        #endregion
    }
}