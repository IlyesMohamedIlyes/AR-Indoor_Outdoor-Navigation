using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public enum LoadingPanelMode 
{
	ForGPSDisable,
	ForGPSInitialize,
	ForGPSFailed,
	ProcessingOutDoorNavigation,
	ForDestination
}

public class LoadingPanelController : MonoBehaviour
{

	WaitForSeconds _wait;
	bool _isDone = false;
	[SerializeField]
	GameObject _content;

	// This value of the loading panel's color.
	public Color UpdateContent
	{
		set
		{ 
				_content.GetComponent <Image> ().color = value;
				 
		}
	}

	[SerializeField]
	Text _text;
	public string	UpdateText
	{
		set
		{ 
			_text.text = value;
		}
	}

	[SerializeField]
	AnimationCurve _curve;


	private static LoadingPanelController _instance;
	public static LoadingPanelController Instance
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

	private LoadingPanelMode _loadingPanelState;
	public LoadingPanelMode LoadingPanelCurrentState
	{
		get
		{ 
			return _loadingPanelState;
		}
	}

	public void PanelMode (LoadingPanelMode currentState)
	{
		float c;
		switch (currentState) 
		{
			case LoadingPanelMode.ForGPSDisable:
				c = (float)(45.0 / 255.0);
				UpdateContent = new Color (c, c, c, 255 / 255);
				UpdateText = "PLEASE ENABLE GPS";
				break;

			case LoadingPanelMode.ForGPSInitialize: 
				c = (float)(209.0 / 255.0);
				_loadingPanelState = LoadingPanelMode.ForGPSInitialize;
				UpdateContent = new Color (1f , 1f, 1f, c);
				UpdateText = "INITIALIZING..";
				break;
			
			case LoadingPanelMode.ForGPSFailed : 
				c = (float)(209.0 / 255.0);
				_loadingPanelState = LoadingPanelMode.ForGPSFailed;
				UpdateContent = new Color (1f , 1f, 1f, c);
				UpdateText = "FAILED TO INITIALIZE";
				break;

			case LoadingPanelMode.ProcessingOutDoorNavigation:
				gameObject.SetActive (true);
				c = (float)(209.0 / 255.0);
				_loadingPanelState = LoadingPanelMode.ProcessingOutDoorNavigation;
				UpdateContent = new Color (1f , 1f, 1f, c);
				UpdateText = "PROCESSING..";
				break;

			case LoadingPanelMode.ForDestination: 
				c = (float)(209.0 / 255.0);
				_loadingPanelState = LoadingPanelMode.ForDestination;
				UpdateContent = new Color (1f , 1f, 1f,c);
				UpdateText = "Synchronizing your position..";
				break;
		}
	}

	void Awake()
	{
		_instance = this;	

		_loadingPanelState = LoadingPanelMode.ForGPSDisable;
		PanelMode (LoadingPanelMode.ForGPSDisable);
	}


	void Update()
	{
		var t = _curve.Evaluate(Time.time);
		_text.color = Color.Lerp(Color.clear, Color.white, t);

#if UNITY_EDITOR

		if (!_isDone)
		{
			_isDone = true;
			UIManaging.ApplicationUIManager.Instance.OnStateChanged (UIManaging.ApplicationState.Loading_GPSPanel, UIManaging.DestinationPanelState.FFPanel);
		}
		return;
#endif	

		if (!Input.location.isEnabledByUser)
		{
			gameObject.SetActive (true);
			PanelMode (LoadingPanelMode.ForGPSDisable);
		}

		if (Input.location.status == LocationServiceStatus.Initializing)
		{
			gameObject.SetActive (true);
			PanelMode (LoadingPanelMode.ForGPSInitialize);
		}

		if (Input.location.status == LocationServiceStatus.Failed)
		{
			PanelMode (LoadingPanelMode.ForGPSFailed);
		}

		if (Mapbox.Unity.Location.DeviceLocationProvider.Instance.IsGPSEnable && !_isDone)
		{
			_isDone = true;
			UIManaging.ApplicationUIManager.Instance.OnStateChanged (UIManaging.ApplicationState.Loading_GPSPanel, UIManaging.DestinationPanelState.FFPanel);
		}

	}
		
}
