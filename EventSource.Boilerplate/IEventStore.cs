using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSource.Boilerplate
{
	public interface IEventStore
	{
		void CreateStreamName(string streamName, List<DomainEvent> events);
		void AppendEventsToStream(string streamName, List<DomainEvent> events);
		List<DomainEvent> GetStream(string streamName, int fromEventNumber, int toEventNumber);
		T GetLatestSnapshot<T>(string streamName);
		void AddSnapshot<T>(string streamName, T snapshot);
	}
}