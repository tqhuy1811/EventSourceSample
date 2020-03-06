using System.Collections.Generic;

namespace EventSource.Boilerplate
{
	public abstract class EventSourceAggregate: Entity
	{
		public List<IDomainEvent> Changes { get; private set; }
		public int Version { get; protected set; }

		public EventSourceAggregate()
		{
			Changes = new List<IDomainEvent>();
		}

		public abstract void Apply(IDomainEvent changes);
	}
}