namespace Mapbox.Utils
{
	using System;
	using UnityEngine;
	using System.Collections;


	public class Convertors : MonoBehaviour {


		public static float Radius 
		{
			get
			{ 
				return 6378.137f;
			}
		}

		public static Vector3 GeoToWorldGlobePosition(double lat, double lon)
		{
			double xPos = (Radius) * Math.Cos(Mathf.Deg2Rad * lat) * Math.Cos(Mathf.Deg2Rad * lon);
			double zPos = (Radius) * Math.Cos(Mathf.Deg2Rad * lat) * Math.Sin(Mathf.Deg2Rad * lon);


			// turn yPos to 0  // For set it on the ground.
			double yPos = (Radius) * Math.Sin(Mathf.Deg2Rad * lat);

			//This XYZ are in KM, so multiply per 1000 to get it in meters.
			return new Vector3((float)xPos * 1000, (float)yPos*1000, (float)zPos * 1000);
		}

		public static Vector3 Vector3ToXZ (Vector2d latLong)
		{
			var vec = GeoToWorldGlobePosition (latLong.x, latLong.y);
			return new Vector3 (vec.x, 1f, vec.z);
		}

		public static Vector3 GeoToWorldGlobePosition(Vector2d latLong)
		{
			return Vector3ToXZ (latLong);
		}

		public static Vector2d GeoFromGlobePosition(Vector3 point, float radius)
		{
			float latitude = Mathf.Asin(point.y / radius);
			float longitude = Mathf.Atan2(point.z, point.x);
			return new Vector2d(latitude * Mathf.Rad2Deg, longitude * Mathf.Rad2Deg);
		}

		// Another way to convert the LatLong coordinates which I tested. But don't need it.
		/*
		public static Vector3 GeoStringToWorldGlobePosition (string lat, string lon)
		{
			double xpos = 0.0, zpos = 0.0;

		
			if (lat.Split ('.') [1].StartsWith ("715"))
				xpos = (Double.Parse (lat.Split ('.') [1].Substring (3, 3)) - 1000) / 4;
			else
				xpos = Double.Parse (lat.Split ('.') [1].Substring (3, 3)) / 4;


			if (lon.Split ('.') [1].StartsWith ("184"))
				zpos = (Double.Parse (lon.Split ('.') [1].Substring (3, 3)) - 1000) / 5;
			else
				zpos = Double.Parse (lon.Split ('.') [1].Substring (3, 3)) / 5;


			/*	Debug.Log ("FROM " + lat + "WHICH IS "+ lat.Split ('.') [1].Substring (3, 3) + " TO " + xpos);
			Debug.Log ("FROM " + lon + "WHICH IS "+ lon.Split ('.') [1].Substring (3, 3) + " TO " + zpos);
			return GeoToWorldGlobePosition (xpos, zpos);
		}
*/

	}
}