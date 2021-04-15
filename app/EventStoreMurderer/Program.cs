using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EventsFacade.Utilities;
using EventStore.Client;

namespace EventStoreMurderer
{
    class Program
    {
        private int _counter = 0;
        private static EventStoreClient _client;
        private const string BigStream = "PizzaOrders";
        private const string SmallStream = "Smol";

        record PizzaOrder(string OrdererName, string PizzaName, double Price, int Amount);

        static async Task MakeOrders(Action onFinish = null, string stream = BigStream, int count = 10)
        {
            var order = new PizzaOrder("Karol", "Tartufo", 30, 20);

            Stopwatch watch = new();
            watch.Start();
            for (int i = 0; i < count; i++)
            {
                var eventData = new EventData(Uuid.NewUuid(), nameof(PizzaOrder),
                    JsonSerializer.SerializeToUtf8Bytes(order));

                await _client.AppendToStreamAsync(stream, StreamState.Any, new[] { eventData });
            }

            watch.Stop();

            onFinish?.Invoke();
        }

        static async Task ReadOrders(Action onFinish = null, string stream = BigStream)
        {
            var random = new Random();

            var orders = _client.ReadStreamAsync(Direction.Forwards, stream, StreamPosition.Start);
            var ordersMaterialized = await orders.ToListAsync();
            // Console.WriteLine(ordersMaterialized.ElementAt(random.Next(0, ordersMaterialized.Count)).Event .Created);

            onFinish?.Invoke();
        }

        static async Task Main(string[] args)
        {
            _client = RegisterEventsModule.GetClient("esdb://localhost:2113?tls=false");

            var counter = 0;
            void OnRequestFinished() => Interlocked.Increment(ref counter);

            // await MakeOrders(stream: SmallStream);

            var sw = new Stopwatch();
            var repeats = 20;
            sw.Start();
            for (int i = 0; i < repeats; i++)
            {
                await ReadOrders();
            }
            sw.Stop();
            Console.WriteLine($"Big: { sw.ElapsedMilliseconds/repeats}");

            sw.Restart();
            for (int i = 0; i < repeats; i++)
            {
                await ReadOrders(null, SmallStream);
            }
            sw.Stop();
            Console.WriteLine($"Smol: { sw.ElapsedMilliseconds/repeats }");


            // for (int i = 0; i < 480; i++)
            // {
            //     await Task.Run((async () =>
            //     {
            //         await MakeOrders(OnRequestFinished);
            //     }));
            //
            //     for (int j = 0; j < 10; j++)
            //     {
            //         Task.Run((async () =>
            //         {
            //             await ReadOrders(OnRequestFinished);
            //         }));
            //     }
            //
            //     Thread.Sleep(900);
            //
            //     Console.WriteLine(counter);
            // }
        }
    }
}