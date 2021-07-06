using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.Connector.B2C2.Rest;
using MyJetWallet.Connector.B2C2.Rest.Models;
using MyJetWallet.Connector.B2C2.WebSocket;
using MyJetWallet.Connector.B2C2.WebSocket.Models;

namespace TestApp
{
    internal class Program
    {
        private static readonly string API_KEY = "";

        private static async Task Main(string[] args)
        {
            // await TestRestApi();
            // await TestErrorsDeserialisation();
            await TestWebSocket();
        }

        private static async Task TestWebSocket()
        {
            using var loggerFactory =
                LoggerFactory.Create(builder =>
                    builder.AddSimpleConsole(options =>
                    {
                        options.IncludeScopes = true;
                        options.SingleLine = true;
                        options.TimestampFormat = "hh:mm:ss ";
                    }));

            await UseWsOrderBooks(loggerFactory);
        }

        private static async Task UseWsOrderBooks(ILoggerFactory loggerFactory)
        {
            var client = new B2C2WsOrderBooks(loggerFactory.CreateLogger<B2C2WsOrderBooks>(), API_KEY,
                new[] {new MarketProfile {name = "BTCUSD.SPOT", levels = new double[] {1, 2}}});

            client.Start();

            var log = true;

            client.ReceiveUpdates = book =>
            {
                if (log)
                    Console.WriteLine($"Receive updates for {book.Instrument}");

                log = false;

                return Task.CompletedTask;
            };

            var cmd = Console.ReadLine();
            while (cmd != "exit")
            {
                if (cmd == "count")
                {
                    var books = client.GetOrderBooks().Count;
                    Console.WriteLine($"Count books: {books}");
                }
                else if (cmd == "reset")
                {
                    client.Reset("BTCUSD.SPOT", new double[] {1, 2}).Wait();
                }
                else if (cmd == "time")
                {
                    var book = client.GetOrderBookById("BTCUSD.SPOT");

                    Console.WriteLine($"nw: {DateTimeOffset.UtcNow:O}");
                    Console.WriteLine($"t1: {book.Timestamp}");

                    client.Reset("BTCUSD.SPOT", new double[] {1, 2}).Wait();
                }
                else if (cmd == "sub")
                {
                    client.Subscribe("BTCUSD.SPOT", new double[] {1, 2}).Wait();
                }
                else if (cmd == "unsub")
                {
                    client.Unsubscribe("BTCUSD.SPOT").Wait();
                }
                else
                {
                    var orderBook = client.GetOrderBookById(cmd);

                    if (orderBook != null)
                        Console.WriteLine(JsonSerializer.Serialize(orderBook,
                            new JsonSerializerOptions {WriteIndented = true}));
                    else
                        Console.WriteLine("Not found");
                }

                cmd = Console.ReadLine();
            }
        }

        private static async Task TestRestApi()
        {
            var api = B2C2RestApiFactory.CreateClient(API_KEY);

            Console.WriteLine(" ====  Account Info ==== ");
            var accountInfo = await api.GetAccountInfo();
            Console.WriteLine(JsonSerializer.Serialize(accountInfo, new JsonSerializerOptions {WriteIndented = true}));
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine(" ====  Balances ==== ");
            var balances = await api.GetAccountBalance();
            Console.WriteLine(JsonSerializer.Serialize(balances, new JsonSerializerOptions {WriteIndented = true}));
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine(" ====  Instruments ==== ");
            var instruments = await api.GetInstrumentsList();
            Console.WriteLine(JsonSerializer.Serialize(instruments, new JsonSerializerOptions {WriteIndented = true}));
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine(" ====  Currencies ==== ");
            var currencies = await api.GetCurrenciesList();
            Console.WriteLine(JsonSerializer.Serialize(currencies, new JsonSerializerOptions {WriteIndented = true}));
            Console.WriteLine();
            Console.WriteLine();

            // Console.WriteLine(" ====  Execute Order ==== ");
            // var order = await api.PlaceMarketOrder("Order1", "BTCUSD.SPOT", OrderSide.buy, new decimal(0.01));
            // Console.WriteLine(JsonSerializer.Serialize(order, new JsonSerializerOptions() {WriteIndented = true}));
            // Console.WriteLine();
            // Console.WriteLine();

            Console.WriteLine(" ====  Get Orders ==== ");
            var orders = await api.GetOrders("Order1");
            Console.WriteLine(JsonSerializer.Serialize(orders, new JsonSerializerOptions {WriteIndented = true}));
            Console.WriteLine();
            Console.WriteLine();
        }

        private static async Task TestErrorsDeserialisation()
        {
            var errorsString =
                "{\"errors\":[{\"message\":\"Authentication credentials were not provided.\",\"code\":1100}]}";
            Console.WriteLine(errorsString);
            Console.WriteLine(JsonSerializer.Serialize(JsonSerializer.Deserialize<B2C2Errors>(errorsString),
                new JsonSerializerOptions {WriteIndented = true}));
            var errorString = "{\"message\":\"Authentication credentials were not provided.\",\"code\":1100}";
            Console.WriteLine(errorString);
            Console.WriteLine(JsonSerializer.Serialize(JsonSerializer.Deserialize<B2C2Error>(errorString),
                new JsonSerializerOptions {WriteIndented = true}));
            var errorListString = "[{\"message\":\"Authentication credentials were not provided.\",\"code\":1100}]";
            Console.WriteLine(errorListString);
            Console.WriteLine(JsonSerializer.Serialize(JsonSerializer.Deserialize<List<B2C2Error>>(errorListString),
                new JsonSerializerOptions {WriteIndented = true}));
        }
    }
}