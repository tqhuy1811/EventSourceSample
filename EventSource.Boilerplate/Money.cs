namespace EventSource.Boilerplate
{
	public class Money
	{
		public decimal Amount { get; set; }

		public Money(decimal amount)
		{
			Amount = amount;
		}
		
		public bool IsGreaterThanOrEqualTo(Money money)
		{
			return this.Amount >= money.Amount;
		}
		public Money Add(Money money)
		{
			return new Money(this.Amount + money.Amount);
		}
		
		public Money Subtract(Money money)
		{
			return new Money(this.Amount - money.Amount);
		}

		
		public Money MultiplyBy(int number)
		{
			return new Money(this.Amount * number);
		}
	}
}