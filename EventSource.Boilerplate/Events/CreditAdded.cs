using System;

namespace EventSource.Boilerplate.Events
{
	public class CreditAdded: IDomainEvent
	{
		public CreditAdded(
			Guid aggregateId,
			Money credit)
		{
			Id = aggregateId;
			Credit = credit;
		}

		public Guid Id { get; }
		public Money Credit { get; }
		
	}
}