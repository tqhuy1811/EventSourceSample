using System;

namespace EventSource.Boilerplate
{
	public interface IPayAsYouGoAccountRepository
	{
		
	}

	public class PayAsYouGoRepository : IPayAsYouGoAccountRepository
	{
		private readonly IEventStore _eventStore;

		public PayAsYouGoRepository(IEventStore eventStore)
		{
			_eventStore = eventStore;
		}

		public void Add(PayAsYouGoAccount payAsYouGoAccount)
		{
			var streamName = StreamNameFor(payAsYouGoAccount.Id);
			_eventStore.CreateStreamName(streamName, payAsYouGoAccount.Changes);
		}

		public void Save(PayAsYouGoAccount payAsYouGoAccount)
		{
			var streamName = StreamNameFor(payAsYouGoAccount.Id);
			_eventStore.AppendEventsToStream(streamName, payAsYouGoAccount.Changes);
		}

		public PayAsYouGoAccount FindBy(Guid id)
		{
			var streamName = StreamNameFor(id);
			var fromEventNumber = 0;
			var toEventNumber = int.MaxValue;
			//handling snapshot
			var snapshot = _eventStore.GetLatestSnapshot<PayAsYouGoAccountSnapshot>(
				streamName
			);
			
			if (snapshot != null)
			{
				// load only events after snapshot
				fromEventNumber = snapshot.Version + 1;
			}

			var payAsYouGoAccount = snapshot != null ? new PayAsYouGoAccount(snapshot) : new PayAsYouGoAccount(); 
			var stream = _eventStore.GetStream(streamName, fromEventNumber, toEventNumber);
			
			foreach (var @event in stream)
			{
				payAsYouGoAccount.Apply(@event);
			}

			return payAsYouGoAccount;
		}

		private string StreamNameFor(Guid id)
		{
			
			return $"{typeof(PayAsYouGoAccount).Name}-{id}";
		}
	}
}