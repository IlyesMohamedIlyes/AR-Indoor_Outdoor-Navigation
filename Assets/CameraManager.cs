using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Examples;
using Mapbox.Unity.Location;

enum CameraMode
{
	IndoorNavigation,
	OutdoorNavigation
}
	

public class CameraManager : MonoBehaviour {

	public GameObject _cameraParent;
	public Camera _arCamera;
	public bool _modeIndoor;




	private static CameraManager _instance;
	public static CameraManager Instance
	{
		get
		{ 
			return _instance;
		}
	}



	void Awake ()
	{
		_instance = this;

		_cameraParent.transform.position = _arCamera.transform.position;
		_cameraParent.transform.rotation = _arCamera.transform.rotation;
		_arCamera.transform.SetParent (_cameraParent.transform);
	}
		

	[SerializeField]
	bool _rotateZ;

	public void ControlPosCamera (Vector3 syncP)
	{
		if (_modeIndoor)
		{
			PedometerU.Tests.StepCounter.Instance.LastStepPosition = syncP;
		}

		_cameraParent.transform.position = syncP;
	}


	Vector3 _targetRotation;

	public void ControlRotCameraWithEuler (Quaternion target)
	{
		_cameraParent.transform.rotation = target;
	}


	public void ControlRotCameraWithTarget (Transform target)
	{
		//	target.position = new Vector3 (-pos.x, pos.y, -pos.z);
		_cameraParent.transform.LookAt (target);

	}

}
