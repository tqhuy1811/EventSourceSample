namespace EventSource.Boilerplate
{
	public class PayAsYouGoInclusiveMinutesOffer
	{
		private Money spendThreshold;

		public PayAsYouGoInclusiveMinutesOffer()
		{
			spendThreshold = new Money(10m);
			FreeMinutes = new Minutes(90);
		}

		public bool IsSatisfiedBy(Money credit)
		{
			return credit.IsGreaterThanOrEqualTo(spendThreshold);
		}

		public Minutes FreeMinutes { get; private set; }
	}
}