using System;
using System.Collections.Generic;

namespace EventSource.Boilerplate
{
	public abstract class EventSourceAggregate: Entity
	{
		public List<DomainEvent> Changes { get; private set; }
		public int Version { get; protected set; }

		public EventSourceAggregate()
		{
			Changes = new List<DomainEvent>();
		}

		public abstract void Apply(DomainEvent changes);
	}
}