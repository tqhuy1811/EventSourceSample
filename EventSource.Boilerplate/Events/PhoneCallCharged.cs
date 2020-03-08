using System;

namespace EventSource.Boilerplate.Events
{
	public class PhoneCallCharged: IDomainEvent
	{
		public PhoneCallCharged(
			Guid aggregateId,
			PhoneCall phoneCall,
			Money costOfCall,
			Minutes coveredByAllowance)
		{
			Id = aggregateId;
			PhoneCall = phoneCall;
			CostOfCall = costOfCall;
			CoveredByAllowance = coveredByAllowance;
		}

		public Guid Id { get;  }
		
		public PhoneCall PhoneCall { get; }

		public Money CostOfCall { get; }

		public Minutes CoveredByAllowance { get; }
	}
}