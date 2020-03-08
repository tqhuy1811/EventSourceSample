using System;

namespace EventSource.Boilerplate
{
	public class CreditSatisfiesFreeCallAllowanceOffer : IDomainEvent
	{
		public CreditSatisfiesFreeCallAllowanceOffer(
			Guid aggrgateId,
			DateTime time,
			Minutes freeMinutes)
		{
			Id = aggrgateId;
			OfferSatisfied = time;
			FreeMinutes = freeMinutes;
		}

		public DateTime OfferSatisfied { get; private set; }
		public Minutes FreeMinutes { get; private set; }
		public Guid Id { get; }
	}
}