using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSource.Boilerplate
{
	public interface IEventStore
	{
		Task CreateNewStreamAsync(string streamName,
			List<IDomainEvent> events);

		Task AppendEventsToStreamAsync(string streamName,
			List<IDomainEvent> events,
			int? expectedVersion = null);

		Task<List<IDomainEvent>> GetStreamAsync(string streamName,
			int fromVersion,
			int toVersion);

		Task<T> GetLatestSnapshotAsync<T>(string streamName) where T : class;

		Task AddSnapshotAsync<T>(string streamName,
			T snapshot);
	}
}