using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using UnityEngine.SceneManagement;
using UIManaging;

public enum SceneName
{
	ARDIN_Indoor,
	ARDIN_Outdoor,
	ProcessingOutDoorNavigation
}



public class SceneIndicator : MonoBehaviour {


	bool _done;

	public GameObject _mondeMap;

	public GameObject _toolsManager;

	public Text _topMessage;
	public Text _bottomMessage;
	public GameObject _image;
	public GameObject _button;


	bool _isClose;

	static SceneIndicator _instance;
	public static SceneIndicator Instance
	{
		get
		{ 
			return _instance;
		}
	}

	private Vector2d _deviceLocation;
	Vector2d DeviceLocation 
	{
		get
		{
			return DeviceLocationProvider.Instance.CurrentLocation.LatitudeLongitude;
		}
	}

	private SceneName _sceneState;
	public SceneName SceneState
	{
		get
		{ 
			return _sceneState;
		}
	}


	void Awake ()
	{
		_instance = this;
	}
		

	public void OnStateChanged (SceneName scene, double distance)
	{
			switch (scene) 
			{

				case SceneName.ProcessingOutDoorNavigation: 

					LoadingPanelController.Instance.PanelMode (LoadingPanelMode.ProcessingOutDoorNavigation);
					break;	

			case SceneName.ARDIN_Indoor:
				_sceneState = SceneName.ARDIN_Indoor;
				_topMessage.text = "Trouvez un point de synchronisation";
				_image.SetActive (true);
				_image.GetComponent <Image> ().sprite = Resources.Load <Sprite> ("In this format");
				_bottomMessage.text = "";
				_button.SetActive (false);
					
				_toolsManager.GetComponent <PedometerU.Tests.StepCounter> ().enabled = true;
				_toolsManager.GetComponent <Mapbox.Examples.ImmediatePositionWithLocationProvider> ().enabled = false;

				_mondeMap.SetActive (false);

					break;

				case SceneName.ARDIN_Outdoor: 
					
					_sceneState = SceneName.ARDIN_Outdoor;
					_topMessage.text = "Cliquez pour vous naviguer jusqu'au département informatique USTHB";
					_image.SetActive (false);
					_bottomMessage.text = "Vous êtes à " + distance + "m loin du département ";
					_button.SetActive (true);
					_toolsManager.GetComponent <PedometerU.Tests.StepCounter> ().enabled = false;
					_toolsManager.GetComponent <Mapbox.Examples.ImmediatePositionWithLocationProvider> ().enabled = true;

					break;
			}

	}
		

	public void isCloseToDepartment (double distance)
	{
		// distance is in meters
		_isClose = distance < 70;

		if (_isClose)
			OnStateChanged (SceneName.ARDIN_Indoor, distance);
		else
			OnStateChanged (SceneName.ARDIN_Outdoor, distance);
	}




		
	
}
