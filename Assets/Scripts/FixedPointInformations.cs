using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;
using System;
using Mapbox.Unity.Location;


public class FixedPointInformations : IFixedLocation {

	private int _locationId;
	public int LocationId
	{
		get
		{
			return _locationId;
		}
	}
		
	private string _locationName;
	public string LocationName
	{
		get
		{
			return _locationName;
		}
	}

	private string _locationType;
	public string LocationType
	{
		get
		{
			return _locationType;
		}
	}

	protected Vector2d _currentLocation;
	public Vector2d CurrentLocation
	{
		get
		{
			return _currentLocation;
		}
	}

	private Vector3 _locationXZ;
	public Vector3 LocationXZ
	{
		get
		{
			return _locationXZ;
		}
		set
		{ 
			_locationXZ = value;
		}
	}
	//Constructor
	public FixedPointInformations (int id, string name, string type, Vector2d latitudeLongitude)
	{
		_locationId = id;
		_locationName = name;
		_locationType = type;
		_currentLocation = latitudeLongitude;

		LocationXZ = Convertors.GeoToWorldGlobePosition (latitudeLongitude) - DeviceLocationProvider.Instance.Origine;


	}

		
}
