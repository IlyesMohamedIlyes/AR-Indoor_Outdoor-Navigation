namespace Mapbox.Unity.Location
{
	using System;
	using Mapbox.Utils;


	public interface IFixedLocation
	{
		int LocationId { get; }
		string LocationName { get; }
		string LocationType { get; }
		Vector2d CurrentLocation { get; }
		//void SetLocation (int id, string name, string type, Vector2d latitudeLongitude);
	}
}