namespace ETClient.Models
{
	public class Position
	{
		public double Latitude { get; }

		public double Longitude { get; }

		public Position(double latitude, double longitude)
		{
			Latitude = 0;
			Longitude = 0;
		}
	}
}
