using System;

namespace EventSource.Boilerplate
{
	public class PhoneCall
	{
		public PhoneCall(
			PhoneNumber numberDialled,
			DateTime startTime,
			Minutes minutes)
		{
			StartTime = startTime;
			Minutes = minutes;
			NumberDialled = numberDialled;
		}

		public DateTime StartTime { get; }
		public Minutes Minutes { get; }
		public PhoneNumber NumberDialled { get; }
	}
}