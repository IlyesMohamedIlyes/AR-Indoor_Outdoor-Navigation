namespace Mapbox.Examples
{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UIManaging;
using Mapbox.Unity.Location;



public class ImageTargetSynchPoint : MonoBehaviour, ITrackableEventHandler {


	public int _idSyncPoint;
	public Transform _target;


	private TrackableBehaviour mTrackableBehaviour;

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

	Quaternion _targetRotation;

	void LocationProvider_OnLocationUpdated(Location location)
	{
		if (location.IsHeadingUpdated)
		{
			var euler = Mapbox.Unity.Constants.Math.Vector3Zero;
		
				euler.y = location.Heading;
		
			_targetRotation = Quaternion.Euler(euler);

		}
	}


	// Use this for initialization
	void Start ()
	{
		
		LocationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
		
		mTrackableBehaviour = GetComponent <TrackableBehaviour> ();
		if (mTrackableBehaviour) 
		{
			mTrackableBehaviour.RegisterTrackableEventHandler (this);
		}	
	
	}



	public void OnTrackableStateChanged (TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
	{

		if (newStatus == TrackableBehaviour.Status.DETECTED ||
		    newStatus == TrackableBehaviour.Status.TRACKED ||
		    newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
		{
			if (ApplicationUIManager.Instance.CurrentState == ApplicationState.SyncPoint_Calibration) 
			{
				// Target found

				// This is for stopping positioning automaticlly the player by GPS
				// Camera.main is the gameobject which represent the player in this virtual environnement
			
				// Position the player manually, the order is absolute. don't change it.
				CameraManager.Instance.ControlPosCamera (SQLiteDB_DestinationPoints.Instance.SynchronisationPoints [_idSyncPoint - 1].LocationXZ);
				
				CameraManager.Instance.ControlRotCameraWithEuler (_targetRotation);

				if (_idSyncPoint <= 3)
					ApplicationUIManager.Instance.OnStateChanged (ApplicationState.SyncPoint_Calibration, DestinationPanelState.RDCPanel);
				else
					ApplicationUIManager.Instance.OnStateChanged (ApplicationState.SyncPoint_Calibration, DestinationPanelState.FFPanel);
			
	
			}

		}
	}
		
}
}