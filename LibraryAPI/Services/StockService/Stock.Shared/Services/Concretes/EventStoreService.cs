using EventStore.Client;
using Stock.Shared.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Stock.Shared.Services.Concretes
{
    public class EventStoreService : IEventStoreService
    {
        EventStoreClientSettings getEventStoreClientSettings()
            => EventStoreClientSettings.Create("esdb://adming:changeit@localhost:2113?tls=false&tlsVerifyCert=false");
        EventStoreClient client { get => new EventStoreClient(getEventStoreClientSettings()); }

        public async Task appendToStreamAsync(string streamName, IEnumerable<EventData> eventData)
            => await client.AppendToStreamAsync(
                streamName: streamName,
                eventData: eventData,
                expectedState: StreamState.Any
                
                );
        public EventData generateEventData(object @event)
            => new(
                eventId: Uuid.NewUuid(),
                type: @event.GetType().Name,
                data: JsonSerializer.SerializeToUtf8Bytes(@event)
                );

        public async Task subscribeToStreamAsync(string streamName,
            Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> _eventAppeared)
            => await client.SubscribeToStreamAsync(
                    streamName: streamName,
                    start: FromStream.Start,
                    eventAppeared: _eventAppeared,
                    subscriptionDropped: (streamSubscription, subscriptionDroppedReason, Exception) => Console.WriteLine("disconnected!")
                );

        public async Task deleteAsync(string streamName = "stock-stream")
        {
            await client.DeleteAsync(streamName, StreamState.NoStream);
        }
    }
}
