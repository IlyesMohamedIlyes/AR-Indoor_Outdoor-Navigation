using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;
using UIManaging;
using System;
using UnityEngine.UI;


public class DestinationPointsProvider : MonoBehaviour {

	bool _isComplete = false;

	static DestinationPointsProvider _instance;
	public static DestinationPointsProvider Instance
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

	void Awake ()
	{
		Instance = this;
		_isComplete = false;
	}

	void Update ()
	{
		if (SQLiteDB_DestinationPoints.Instance.IsDestinationPointsCompleted)
			if (ApplicationUIManager.Instance.RDCPanelState) //if True
				ProvideDestinationPoints ();
	}
		

	Dictionary <int, FixedPointInformations> _destinationPoints = new Dictionary<int, FixedPointInformations> ();
	public Dictionary <int, FixedPointInformations> DestinationPoints
	{
		get
		{ 
			if (_destinationPoints.Count == 0)
				_destinationPoints = SQLiteDB_DestinationPoints.Instance.DestinationPoints;

			return _destinationPoints;
		}
	}


	public void ProvideDestinationPoints ()
	{
		if (!_isComplete) 
		{
			_isComplete = true;

			List<int> salleTPIds = new List<int> ();
			List<int> salleAdminIds = new List<int> ();
			List<int> sanitaireIds = new List<int> ();
			int bibId = 0, microClubId = 0, salleDoctoId = 0;

			//ForTextContent.Instance.text.text += "\n In Update of DestinationPointLocationProvider line 185";

			foreach (var id in DestinationPoints.Keys) {
					
				Debug.Log ("sort destinations ID. this : " + id);

				var type = DestinationPoints [id].LocationName.Split ('-') [0];

				if (type == "ADM")
					salleAdminIds.Add (id);

				if (type == "TP")
					salleTPIds.Add (id);

				if (type== "Sanitaire")
					sanitaireIds.Add (id);

				if (type == "MicroClub")
					microClubId = id;

				if (type == "SalleDoctorants")
					salleDoctoId = id;

				if (type == "Bibliotheque")
					bibId = id;

					
			}

			string[] nomtype = null;

			foreach (var salleTPId in salleTPIds) {
				nomtype = DestinationPoints [salleTPId].LocationName.Split ('-');
				ApplicationUIManager.Instance.AddToDestinationPointUI (salleTPId, "SalleTPUIPanel", nomtype [1], nomtype [0], OnDestRequested);
			}

			foreach (var adminId in salleAdminIds) {
				nomtype = DestinationPoints [adminId].LocationName.Split ('-');
				ApplicationUIManager.Instance.AddToDestinationPointUI (adminId, "AdministrationUIPanel", nomtype [1], nomtype [0], OnDestRequested);
			}

			nomtype = DestinationPoints [bibId].LocationName.Split ('-');
			ApplicationUIManager.Instance.AddToDestinationPointUI (bibId, "OtherUIPanel", nomtype [0], nomtype [0], OnDestRequested);

			nomtype = DestinationPoints [salleDoctoId].LocationName.Split ('-');
			ApplicationUIManager.Instance.AddToDestinationPointUI (salleDoctoId,"OtherUIPanel", nomtype [1], nomtype [0], OnDestRequested);
				
			nomtype = DestinationPoints [microClubId].LocationName.Split ('-');
			ApplicationUIManager.Instance.AddToDestinationPointUI (microClubId,"OtherUIPanel", nomtype [1], nomtype [0], OnDestRequested);


			foreach (var wcId in sanitaireIds)
			{
				nomtype = DestinationPoints [wcId].LocationName.Split ('-');
				ApplicationUIManager.Instance.AddToDestinationPointUI (wcId, "OtherUIPanel", "W.C.", nomtype [0], OnDestRequested);
			}

		}
	}

		// This Method is called from an event OnClick in SyncLocation class, line 100.
		public void OnDestRequested(int id)
		{
			//Destination.
			Debug.Log("Pressed button In DestinationPointsProvider");

			try
			{
				PlayerControler.Instance.SetDestination(_destinationPoints[id].LocationXZ);
			}
			catch (Exception e) 
			{
				ForTextContent.Instance._textImmediate.text += "\n" + e.ToString ();
			}


			ApplicationUIManager.Instance._forNavigation.GetComponent <Text> ().text = "Salle destination : " + _destinationPoints [id].LocationName;
				
			// The second argument is a plus in this type of ApplicationState
			ApplicationUIManager.Instance.OnStateChanged(ApplicationState.Destination_Selection, DestinationPanelState.FFPanel);
		}
		
	}

