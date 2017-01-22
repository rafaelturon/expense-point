using System;
using Blockchain.Investments.Core.Domain;
using CQRSlite.Events;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Blockchain.Investments.Core.ReadModel.Events
{
    public class TransactionCreated : IEvent 
	{
        public TransactionCreated() {}
        public TransactionCreated(Guid id, string userId, JournalEntry journalEntry) 
        {
            Id = id;
            AggregateId = id.ToString();
            UserId = userId;
            JournalEntry = journalEntry;
        }
        private ObjectId _objectId;
        [BsonId]
        public ObjectId ObjectId 
        {
            get
            {
                if (_objectId.ToString().Equals("000000000000000000000000"))
                    _objectId = ObjectId.GenerateNewId();

                return _objectId;
            }
            set
            {
                _objectId = value;
            }
        }
        public Guid Id
        {
            get
            {
                return new Guid(AggregateId);
            }
            set
            {
                AggregateId = value.ToString();
            }
        }
        public string AggregateId {get; set;}
        public string UserId { get; set; }
        public JournalEntry JournalEntry { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}