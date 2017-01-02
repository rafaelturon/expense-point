using System;
using System.Collections.Generic;
using System.Linq;
using Blockchain.Investments.Core.Repositories;
using CQRSlite.Events;

namespace Blockchain.Investments.Core.WriteModel
{
    // TODO: change inmemory storage to Mongo
    public class MongoEventStore : IEventStore
    {
        private readonly IEventPublisher _publisher;
        private readonly Dictionary<Guid, List<IEvent>> _inMemoryDb = new Dictionary<Guid, List<IEvent>>();
        private readonly IRepository _repo;
        public MongoEventStore(IEventPublisher publisher, IRepository repo)
        {
            _publisher = publisher;
            _repo = repo;
            _repo.Initialize("EventStore");
        }

        public void Save<T>(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                List<IEvent> list;
                _inMemoryDb.TryGetValue(@event.Id, out list);
                if (list == null)
                {
                    list = new List<IEvent>();
                    _inMemoryDb.Add(@event.Id, list);
                }
                list.Add(@event);
                _publisher.Publish(@event);
            }
        }

        public IEnumerable<IEvent> Get<T>(Guid aggregateId, int fromVersion)
        {
            List<IEvent> events;
            _inMemoryDb.TryGetValue(aggregateId, out events);
            return events?.Where(x => x.Version > fromVersion) ?? new List<IEvent>();
        }
    }
}
