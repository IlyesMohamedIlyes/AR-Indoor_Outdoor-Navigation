namespace Mapbox.Examples
{
	using UnityEngine;
	using UnityEngine.UI;
	using System;
	using Mapbox.Unity.Location;
	using Mapbox.Unity.Map;


	public class ImmediatePositionWithLocationProvider : MonoBehaviour
	{
		
		bool _kalmanSet = false;
		KalmanFilter _kalmanFilter;

			[SerializeField]
		private AbstractMap _map;
		public GameObject _cameraParent; // player

		bool _isInitialized;

		ILocationProvider _locationProvider;
		ILocationProvider LocationProvider
		{
			get
			{
				if (_locationProvider == null)
				{
					_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
				}

				return _locationProvider;
			}
		}

		Vector3 _targetPosition;

		void Start()
		{
			_map.OnInitialized += () => _isInitialized = true;
		}
			
		void LateUpdate()
		{
			if (_isInitialized)
			{
				_cameraParent.transform.position = _map.GeoToWorldPosition (LocationProvider.CurrentLocation.LatitudeLongitude) + new Vector3 (0f, 5.22f, 0f);
			
			}
			
	/*		try
			{
			if (!_kalmanSet) 
			{
				_kalmanSet = true;
				_kalmanFilter = new KalmanFilter (1f);
				_kalmanFilter.SetState (GPSManager.Instance.DeviceCurrentLocation.LatitudeLongitude, GPSManager.Instance.DeviceCurrentLocation.Accuracy,
					(long)GPSManager.Instance.DeviceCurrentLocation.Timestamp);
			}

			_kalmanFilter.Process (GPSManager.Instance.DeviceCurrentLocation.LatitudeLongitude.x,GPSManager.Instance.DeviceCurrentLocation.LatitudeLongitude.y,
				GPSManager.Instance.DeviceCurrentLocation.Accuracy, (long)GPSManager.Instance.DeviceCurrentLocation.Timestamp);

				ForTextContent.Instance._textImmediate.text = _kalmanFilter.LatitudeLongitudeKalman.x + ","+_kalmanFilter.LatitudeLongitudeKalman.y;
			}
			catch (Exception e)
			{
				ForTextContent.Instance._textImmediate.text = e.ToString ();
			}
			try
			{
				transform.localPosition = Convertors.GeoToWorldGlobePosition (_kalmanFilter.LatitudeLongitudeKalman) - GPSManager.Instance.Accurancy;
				ForTextContent.Instance._textStep.text  = transform.position.ToString();
			}
			catch (Exception e) {
				ForTextContent.Instance._textStep.text += e.ToString ();
			}*/
	}



	}
}