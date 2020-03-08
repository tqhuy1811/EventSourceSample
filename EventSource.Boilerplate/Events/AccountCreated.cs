using System;

namespace EventSource.Boilerplate.Events
{
	public class AccountCreated : IDomainEvent
	{
		public AccountCreated(Guid aggregateId, Money credit)
		{
			Id = aggregateId;
			Credit = credit;
		}
		public Guid Id { get; }
		public Money Credit { get; private set; }
	}
}