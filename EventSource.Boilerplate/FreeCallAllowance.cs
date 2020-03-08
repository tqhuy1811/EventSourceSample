using System;

namespace EventSource.Boilerplate
{
	public class FreeCallAllowance
	{
		public Minutes Allowance { get; private set; }
		public DateTime DateStarted { get; private set; }

		public FreeCallAllowance(Minutes allowance, DateTime dateStarted)
		{
			Allowance = allowance;
			DateStarted = dateStarted;
		}

		public void Subtract(Minutes minutes)
		{
			Allowance = Allowance.Subtract(minutes);
		}
        
		public Minutes MinutesWhichCanCover(PhoneCall phoneCall, IClock clock)
		{
			if (Allowance.IsGreaterOrEqualTo(phoneCall.Minutes))
			{
				return phoneCall.Minutes;
			}
			else
			{
				return Allowance;
			}
		}

		private bool StillValid(IClock clock)
		{
			return DateStarted.AddDays(30) > clock.Time();
		}
	}
}