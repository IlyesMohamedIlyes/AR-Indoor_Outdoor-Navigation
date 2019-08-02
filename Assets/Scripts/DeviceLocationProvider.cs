namespace Mapbox.Unity.Location
{
	using UnityEngine.UI;
	using UnityEngine;
	using System.Collections;
	using Mapbox.Utils;
	using System;
	
	//GPS MANAGER
	public class DeviceLocationProvider : AbstractLocationProvider {

		public Text text;

		[SerializeField]
		float _desiredAccuracyInMeters = 1f;

		[SerializeField]
		float _updateDistanceInMeters = 1f;

		Coroutine _pollRoutine;

		double _lastLocationTimestamp;

		double _lastHeadingTimestamp;

		WaitForSeconds _wait;

		private bool _gpsEnabled = false;
		public bool IsGPSEnable
		{
			get
			{ 
				return _gpsEnabled;
			}
			private set
			{ 
				_gpsEnabled = value;
			}
		}

		public int maxWait;


		public Vector3 Origine
		{ 
			get
			{ 						
				return new Vector3 (5104885.43154424f, 0f, 284077.32300765446f);
			}
		}

		private Vector3 _deviceXZPosition;
		public Vector3 DeviceXZPosition
		{
			get
			{ 
				_deviceXZPosition = Convertors.GeoToWorldGlobePosition (CurrentLocation.LatitudeLongitude) - Origine;
			
				return _deviceXZPosition;
			}
				
		}

		private static DeviceLocationProvider _instance;
		public static DeviceLocationProvider Instance 
		{
			get
			{ 
					return _instance;
			}

			private set
			{ 
				_instance = value;
			}
		}



		public void Awake () {
	
			Instance = this;
			
			_wait = new WaitForSeconds(1f);


			// This is permanent
	//		IsGPSEnable = true;
		}
		void Start ()
		{
			if (_pollRoutine == null)
			{
				_pollRoutine = StartCoroutine(getLocation());
			}
		}

		//ATTENTION : Do not Instance any GameObject in this courotine. It will manage a NullReferenceException.
		// This ATTENTION is not for you if you already know that. Just move on.
		IEnumerator getLocation () {

#if UNITY_EDITOR
			yield return new WaitWhile (() => !UnityEditor.EditorApplication.isRemoteConnected);
#endif
		
				if (!Input.location.isEnabledByUser) {
					Debug.LogError ("DeviceLocationProvider: " + "Location is not enabled by user!");

					yield return new WaitWhile (() => !Input.location.isEnabledByUser);
				}

				Input.location.Start (_desiredAccuracyInMeters, _updateDistanceInMeters);
				Input.compass.enabled = true;

				yield return new WaitWhile (() =>  Input.location.status == LocationServiceStatus.Initializing);


				IsGPSEnable = true;
				#if UNITY_EDITOR
				// HACK: this is to prevent Android devices, connected through Unity Remote, 
				// from reporting a location of (0, 0), initially.
				yield return _wait;
				#endif
			
				while (true)
				{
					
				if (!Input.location.isEnabledByUser)
						yield return new WaitUntil (() => Input.location.isEnabledByUser);
				
					_currentLocation.IsHeadingUpdated = false;
					_currentLocation.IsLocationUpdated = false;


					var timestamp = Input.compass.timestamp;
					if (Input.compass.enabled && timestamp > _lastHeadingTimestamp) {
						var heading = Input.compass.trueHeading;
						_currentLocation.Heading = heading;
						_lastHeadingTimestamp = timestamp;

						_currentLocation.IsHeadingUpdated = true;
					}

					var lastData = Input.location.lastData;
					timestamp = lastData.timestamp;


					if (Input.location.status == LocationServiceStatus.Running && timestamp > _lastLocationTimestamp) {
						_currentLocation.LatitudeLongitude = new Vector2d (lastData.latitude, lastData.longitude);
					_currentLocation.Accuracy = (int)lastData.horizontalAccuracy;
						_currentLocation.Timestamp = timestamp;
						_lastLocationTimestamp = timestamp;

						_currentLocation.IsLocationUpdated = true;
					}
				
					
				if (_currentLocation.IsHeadingUpdated || _currentLocation.IsLocationUpdated) 
				{
					SendLocation (_currentLocation);
				}
					
				yield return null;
			}
		
		}




	}
}
