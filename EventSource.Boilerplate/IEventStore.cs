using System.Collections.Generic;

namespace EventSource.Boilerplate
{
	public interface IEventStore
	{
		void CreateStreamName(string streamName, List<IDomainEvent> events);
		void AppendEventsToStream(string streamName, List<IDomainEvent> events);
		List<IDomainEvent> GetStream(string streamName, int fromEventNumber, int toEventNumber);
		T GetLatestSnapshot<T>(string streamName);
		void AddSnapshot<T>(string streamName, T snapshot);
	}
}