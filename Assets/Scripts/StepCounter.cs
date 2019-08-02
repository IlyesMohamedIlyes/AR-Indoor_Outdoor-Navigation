/* 
*   Pedometer
*   Copyright (c) 2017 Yusuf Olokoba
*/

namespace PedometerU.Tests {

    using UnityEngine;
    using UnityEngine.UI;
	using Mapbox.Unity.Location;
	using System;
	using UnityStandardAssets.Characters.FirstPerson;

    public class StepCounter : MonoBehaviour {


		const float StepsToMeters = 0.715f; 
		float _lastHeading;
        private Pedometer pedometer;

		GameObject _lastStepPosition;
		public Vector3 LastStepPosition
		{
			set
			{ 
				_lastStepPosition.transform.position = value;
			}
		}

		static StepCounter _instance;
		public static StepCounter Instance
		{
			get
			{ 
				return _instance;
			}
		}

		void Awake ()
		{
			_instance = this;
		}

        private void Start () 
		{
#if UNITY_ANDROID   
			// Create a new pedometer
            pedometer = new Pedometer(OnStep);

			_lastStepPosition = new GameObject ();
            
			_lastHeading = DeviceLocationProvider.Instance.CurrentLocation.Heading;

			// Reset UI
            OnStep(0, 0);
#endif
        }

        private void OnStep (int steps, double distance)
		{
#if UNITY_ANDROID

			if (!PlayerControler.Instance.IsPathSet)
				return;

			var loc = DeviceLocationProvider.Instance.CurrentLocation;

			ForTextContent.Instance._textStep.text = "another step " + steps;
			if (loc.IsHeadingUpdated)
			{
				_lastHeading = loc.Heading;

				ForTextContent.Instance._textStep.text += "Heading is " + _lastHeading;

				//PlayerControler.Instance.CameraParent.transform.rotation = Quaternion (0f, _lasHeading, 0f, 0f);
			}

			Vector3 player = CameraManager.Instance._cameraParent.transform.position;
			Vector3 target;

			float newx = (float) (StepsToMeters * Math.Cos (_lastHeading));
			float newz = (float) (StepsToMeters * Math.Sin (_lastHeading));
				
			target = new Vector3 (newx, 0f, newz);

			ForTextContent.Instance._textImmediate.text = CameraManager.Instance._cameraParent.transform.position + " devient ";
			CameraManager.Instance._cameraParent.transform.Translate (target);					
			ForTextContent.Instance._textImmediate.text += CameraManager.Instance._cameraParent.transform.position.ToString ();
#endif
        }

	
        private void OnDisable () {
            // Release the pedometer
            pedometer.Dispose();
            pedometer = null;
        }
    }
}