using System.Collections.Generic;

namespace EventSource.Boilerplate
{
	public interface IEventStore
	{
		public void CreateStreamName(string streamName, List<DomainEvent> events);
		public void AppendEventsToStream(string streamName, List<DomainEvent> events);
		public List<DomainEvent> GetStream(string streamName, int fromEventNumber, int toEventNumber);
		public T GetLatestSnapshot<T>(string streamName); 
	}
}