namespace EventSource.Boilerplate
{
	public class PhoneCallCosting
	{
		private Money PricePerMinute { get; set; }

		public PhoneCallCosting()
		{
			PricePerMinute = new Money(0.30m); 
		}

		public virtual Money DetermineCostOfCall(Minutes minutes)
		{
			return minutes.CostAt(PricePerMinute);
		}
	}
}