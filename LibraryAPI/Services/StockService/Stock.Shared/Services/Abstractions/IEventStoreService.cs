using EventStore.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Shared.Services.Abstractions
{
    public interface IEventStoreService
    {
        Task appendToStreamAsync(string streamName, IEnumerable<EventData> eventData);
        EventData generateEventData(object @event);
        Task subscribeToStreamAsync(string streamName, Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> eventApperaed);
        Task deleteAsync(string streamName = "stock-stream");
    }
}
