namespace EventSource.Boilerplate
{
	public class PhoneNumber
	{
		public PhoneNumber(string phoneNumber)
		{
			Number = phoneNumber;
		}

		public string Number { get; set; }

		public bool IsUKLandlineOrMobile()
		{
			return Number.StartsWith("+44");
		}

		public bool IsInternational()
		{
			return !IsUKLandlineOrMobile();
		}
	}
}