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

		public override void Apply(IDomainEvent changes)
		{
			When((dynamic) changes);
			Version = Version++;
		}

		public void Topup(Money credit)
		{
			Causes(new CreditAdded());
		}

		public void Causes(IDomainEvent @event)
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