using System;
using System.Threading.Tasks;

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

		public async Task Add(PayAsYouGoAccount payAsYouGoAccount)
		{
			var streamName = StreamNameFor(payAsYouGoAccount.Id);
			await _eventStore.CreateNewStreamAsync(streamName, payAsYouGoAccount
				.Changes);
		}

		public async Task Save(PayAsYouGoAccount payAsYouGoAccount)
		{
			var streamName = StreamNameFor(payAsYouGoAccount.Id);
			await _eventStore.AppendEventsToStreamAsync(streamName,
				payAsYouGoAccount.Changes);
		}

		public async Task<PayAsYouGoAccount> FindBy(Guid id)
		{
			var streamName = StreamNameFor(id);
			var fromEventNumber = 0;
			var toEventNumber = int.MaxValue;
			//handling snapshot
			var snapshot = await _eventStore
				.GetLatestSnapshotAsync<PayAsYouGoAccountSnapshot>(
					streamName
				);

			if (snapshot != null)
			{
				// load only events after snapshot
				fromEventNumber = snapshot.Version + 1;
			}

			var payAsYouGoAccount = snapshot != null
				? new PayAsYouGoAccount(snapshot)
				: new PayAsYouGoAccount();
			var stream = await _eventStore.GetStreamAsync(streamName,
				fromEventNumber, toEventNumber);

			foreach (var @event in stream)
			{
				payAsYouGoAccount.Apply(@event);
			}

			return payAsYouGoAccount;
		}

		private string StreamNameFor(Guid id)
		{
			// stream per aggregate
			return $"{typeof(PayAsYouGoAccount).Name}-{id}";
		}
	}
}