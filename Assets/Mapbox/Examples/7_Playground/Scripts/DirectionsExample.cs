//-----------------------------------------------------------------------
// <copyright file="DirectionsExample.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Mapbox.Examples.Playground
{
	using Mapbox.Unity.Location;
	using Mapbox.Unity;
	using System;
	using System.Collections;
	using UnityEngine;
	using UnityEngine.UI;
	using Mapbox.Directions;
	using Mapbox.Utils;
	using System.Collections.Generic;


	public class DirectionsExample : MonoBehaviour
	{
		bool _firstuse = true;
		
		[SerializeField]
		private Vector2d _startLocation;
		public Vector2d StartLocation
		{
			set
			{ 
				_startLocation = value;
			}
		}

		[SerializeField]
		private Vector2d _endLocation;
		public Vector2d EndLocation
		{
			set
			{ 
				_endLocation = value;
			}
		}

		Directions _directions;

	
		Vector2d[] _coordinates = new Vector2d[2];

		List <Vector2d> _points = new List<Vector2d> ();
		public List <Vector2d> GeoPoints
		{
			get
			{ 
				return _points;
			}
		}

		DirectionResource _directionResource;

		double _distance;

		DirectionsExample _instance;
		public DirectionsExample Instance
		{
			get
			{ 
				return _instance;
			}
		}

		public GameObject _mondeMap;


		ILocationProvider _locationProvider;
		public ILocationProvider LocationProvider
		{
			private get
			{
				if (_locationProvider == null)
				{
					_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
				}

				return _locationProvider;
			}
			set
			{
				if (_locationProvider != null)
				{
					_locationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;

				}
				_locationProvider = value;
				_locationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
			}
		}

		void Awake ()
		{
			_instance = this;
		}

		void Start()
		{
			_directions = MapboxAccess.Instance.Directions;

			_endLocation = new Vector2d (36.71580103, 3.18511784); //Initiliazing to RDC_ENTER

#if UNITY_EDITOR	
			// to test on Editor
			_endLocation = new Vector2d (36.723009, 3.150947); 
#endif

			// Can we make routing profiles an enum? 
			_directionResource = new DirectionResource(_coordinates, RoutingProfile.Driving);
			_directionResource.Steps = true;

			LocationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;

			StartCoroutine ("CalculateDistance");

		}

		void OnDestroy()
		{
			if (LocationProvider != null)
			{
				LocationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;
			}
		}

		void LocationProvider_OnLocationUpdated(Location location)
		{
			_startLocation = location.LatitudeLongitude;
		}

		public void Navigate ()
		{

			_mondeMap.SetActive (true);

			StartCoroutine (Route ());
		}

		IEnumerator CalculateDistance ()
		{
			// to wait for the Location update event.
			yield return new WaitUntil (() => (_startLocation.x != 0.0 && _endLocation.x != 0.0));

			//while (true)
			//{
				var distance = Vector2d.Distance (_startLocation, _endLocation);
				SceneIndicator.Instance._bottomMessage.text = "Vous êtes à " + distance + "m loin du département ";

				if (_firstuse)
				{
					_firstuse = false;
					SceneIndicator.Instance.isCloseToDepartment (distance);
				}
				yield return new WaitForSeconds (0.75f);
			//}

		}

		/// <summary>
		/// Ensure both respective coordinates are not zero then Route.
		/// </summary>
		IEnumerator Route ()
		{
			// to wait for the Location update event.
			yield return new WaitUntil (() => (_startLocation.x != 0.0 && _endLocation.x != 0.0));
				
			Debug.Log ("done");

			_directionResource.Coordinates = new Vector2d[] {_startLocation, _endLocation};
			_directions.Query(_directionResource, HandleDirectionsResponse);
		}

		/// <summary>
		/// Log directions response to UI.
		/// </summary>
		/// <param name="res">Res.</param>

		void HandleDirectionsResponse(DirectionsResponse res)
		{
			
			var controlDistance = Vector2d.Distance (_startLocation, _endLocation);
			if (controlDistance < 4000f) // 4Km // To avoid bug due to complicate route extractions for long ditances.
			{
				SceneIndicator.Instance.OnStateChanged (SceneName.ProcessingOutDoorNavigation, 0.0);

				foreach (var a in res.Routes) 
				{
					
					foreach (var b in a.Legs) 
					{
						foreach (var c in b.Steps) 
						{
							Debug.Log (c.Maneuver.Location);
							_points.Add (c.Maneuver.Location);
						}
					}
				}

				_mondeMap.SetActive (false);
				SpawnOnMap.Instance.CalculatePath (_points);
			}

		}
	}
}
