using System;

namespace EventSource.Boilerplate
{
	public interface IDomainEvent
	{
		public Guid Id { get; }
	}
}