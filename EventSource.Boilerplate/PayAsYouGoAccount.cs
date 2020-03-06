using System.Data.SqlTypes;
using EventSource.Boilerplate.Events;

namespace EventSource.Boilerplate
{
	public class PayAsYouGoAccount : EventSourceAggregate
	{
		private FreeCallAllowance _freeCallAllowance;
		private Money _credit;

		public PayAsYouGoAccount()
		{
		}

		public PayAsYouGoAccount(PayAsYouGoAccountSnapshot snapshot)
		{
			Version = snapshot.Version;
		}

		public override void Apply(DomainEvent changes)
		{
			When((dynamic) changes);
			Version = Version++;
		}

		public void Topup(Money credit)
		{
			Causes(new CreditAdded());
		}

		public void Causes(DomainEvent @event)
		{
			Changes.Add(@event);
			Apply(@event);
		}

		private void When(CreditAdded creditAdded)
		{
		}

		private void When(PhoneCallCharged phoneCallCharged)
		{
		}
	}
}