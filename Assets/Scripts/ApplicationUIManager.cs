namespace UIManaging
{
	using System.Collections;
	using System.Collections.Generic;
	using System;
	using UnityEngine;
	using UnityEngine.UI;

	public enum ApplicationState
	{
		Loading_GPSPanel,
		SyncPoint_Calibration,
		Destination_Selection,
		AR_Navigation
	}

	public enum DestinationPanelState
	{
		RDCPanel,
		FFPanel
	}

	public class ApplicationUIManager : MonoBehaviour
	{

		public GameObject _forNavigation;

		private static ApplicationUIManager _instance;
		public static ApplicationUIManager Instance
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

		[SerializeField]
		private GameObject _loadingPanelGPS;

		[SerializeField]
		private GameObject _syncPointCalibrationUI;

		[SerializeField]
		private GameObject _destinationSelectionUI;

		[SerializeField]
		private GameObject _arNavigationUI;

		[SerializeField]
		private GameObject _RDCPanel;
		public bool RDCPanelState
		{
			get
			{ 
				return _RDCPanel.activeSelf;
			}
		}

		[SerializeField]
		private GameObject _FirstFloorPanel;
		public bool FFPanelState
		{
			get
			{ 
				return _FirstFloorPanel.activeSelf;
			}
		}


		private ApplicationState _applicationState;
		public ApplicationState CurrentState
		{
			get
			{
				return _applicationState;
			}
		}

		private DestinationPanelState _destState;
		public DestinationPanelState DestinationState
		{
			get 
			{
				return _destState;
			}
		}

		[SerializeField]
		private FixedLocationPointUI _destinationSelectionUIManager;

		[SerializeField]
		private GameObject _backButton;



		// Any actions that need to be triggered on Application State change. 
		public event Action<ApplicationState> StateChanged = delegate { };

		// Use this for initialization
		void Awake()
		{
			if (Instance != null)
			{
				DestroyImmediate(gameObject);
				return;
			}
			Instance = this;
			DontDestroyOnLoad(gameObject);
			/*
			_applicationState = ApplicationState.Loading_GPSPanel;
			_loadingPanelGPS.SetActive (true);
*/
			if (_syncPointCalibrationUI != null)
			{
			//	_syncPointCalibrationUI.SetActive (false);
				_applicationState = ApplicationState.SyncPoint_Calibration;
				_syncPointCalibrationUI.SetActive (true);
			}

			if (_destinationSelectionUI != null)
			{
				_destinationSelectionUI.SetActive(false);
			}

			if (_backButton != null)
			{
				_backButton.SetActive (false);
			}
				
		}
	
		public void changin ()
		{
			_applicationState = ApplicationState.Destination_Selection;
			_syncPointCalibrationUI.SetActive (false);
			_destinationSelectionUI.SetActive(true);
			_backButton.SetActive(true);
			StateChanged(_applicationState);

		}	

		IEnumerator ChargingPanel (DestinationPanelState stateSelected)
		{
			if (_loadingPanelGPS.activeSelf == false) 
				yield break;

			_loadingPanelGPS.GetComponent <LoadingPanelController> ().PanelMode (LoadingPanelMode.ForDestination);

			yield return new WaitForSecondsRealtime (3f);

			_loadingPanelGPS.SetActive (false);

			switch (stateSelected) 
			{
				case DestinationPanelState.FFPanel:
					_destState = DestinationPanelState.FFPanel;	
					_FirstFloorPanel.SetActive (true);
					_RDCPanel.SetActive (false);
					SQLiteDB_DestinationPoints.Instance.FromDB_To_ProfessorsPanel ("SELECT * " + " FROM Professeurs ");
					break;

				case DestinationPanelState.RDCPanel:
					_destState = DestinationPanelState.RDCPanel;
					_FirstFloorPanel.SetActive (false);
					_RDCPanel.SetActive (true);
					break;

			}

		}

		public void OnStateChanged(ApplicationState currentState, DestinationPanelState stateSelected)
		{
			switch (currentState)
			{
			case ApplicationState.Loading_GPSPanel:
				
					_applicationState = ApplicationState.SyncPoint_Calibration;
					_loadingPanelGPS.SetActive (false);
					_syncPointCalibrationUI.SetActive (true);
					_destinationSelectionUI.SetActive (false);
					break;
				
			case ApplicationState.SyncPoint_Calibration:
				
				_applicationState = ApplicationState.Destination_Selection;
				_loadingPanelGPS.SetActive (true);
				_syncPointCalibrationUI.SetActive (false);

				StartCoroutine (ChargingPanel (stateSelected));

				_destinationSelectionUI.SetActive (true);
				_backButton.SetActive (true);
				this.gameObject.GetComponent <AudioSource> ().Play ();

				Handheld.Vibrate ();
					

					break;

			case ApplicationState.Destination_Selection:
				_applicationState = ApplicationState.AR_Navigation;
				_loadingPanelGPS.SetActive (false);

				_destinationSelectionUI.SetActive (false); 
				_syncPointCalibrationUI.SetActive (false);
				_backButton.SetActive (true);
				_arNavigationUI.SetActive (true);
				Handheld.Vibrate ();
					
				break;

			default:
				break;
			}

			//Notify subscribers application state changed. 
			StateChanged(_applicationState);
		}

		public void OnBackButtonPressed()
		{
			switch (_applicationState)
			{
				
				case ApplicationState.Destination_Selection:
					_applicationState = ApplicationState.SyncPoint_Calibration;
					_syncPointCalibrationUI.SetActive (true);
					_destinationSelectionUI.SetActive (false);
					_RDCPanel.SetActive (false);
					_FirstFloorPanel.SetActive (false);
					_backButton.SetActive(false);
					break;

			case ApplicationState.AR_Navigation:
				_applicationState = ApplicationState.SyncPoint_Calibration;

				_arNavigationUI.SetActive (false);
					_syncPointCalibrationUI.SetActive (true);
					_destinationSelectionUI.SetActive (false);
					_RDCPanel.SetActive (false);
					_FirstFloorPanel.SetActive (false);
					Mapbox.Examples.SpawnOnMap.Instance.ClearSpawnedObjects ();
					_backButton.SetActive(false);
					break;

				default:
					break;
			}

			//Notify subscribers application state changed. 
			StateChanged(_applicationState);
		}

		public void AddToDestinationPointUI(int id, string panelName, string label, string type, Action<int> callback)
		{
			_destinationSelectionUIManager.RegisterUI(id, panelName, label, callback, type);
		}
			

		// Update is called once per frame
		void Update()
		{

		}
	}
}
