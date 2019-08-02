namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;
	using UnityEngine.AI;
	using UnityEngine.UI;
	using System.Collections;


	public class SpawnOnMap : MonoBehaviour
	{

		public Text _text;


		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		private float tileSpacing = 2;

		WaitForSeconds _wait;

		private List<Vector3> oldarrowList = new List<Vector3>();

		[SerializeField]
		List <Vector2d> _locations;
		public Vector2d AddLocation
		{
			set
			{ 
				_locations.Add(value);
			}
		}

		[SerializeField]
		float _spawnScale = 100f;

		bool _arrowsSet = false;
		bool _AllSet = false;

		[SerializeField]
		GameObject _markerPrefab;

		GameObject _arrowsParent;

		[SerializeField]
		GameObject _departmentSignPreb;

		List<GameObject> _spawnedObjects = new List<GameObject>();

		private static SpawnOnMap _instance;
		public static SpawnOnMap Instance
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

		void Start ()
		{
			_arrowsParent = new GameObject ("Arrows");
		}

		public void ClearSpawnedObjects ()
		{
			_AllSet = false;
			_arrowsSet = false;

			_spawnedObjects.Clear ();
		}
			
		void StartPath()
		{
			for (int i = 0; i < _locations.Count; i++)
			{
				var instance = Instantiate (_markerPrefab);
				instance.transform.localPosition = _map.GeoToWorldPosition (_locations [i]);
			
				instance.transform.localScale = Vector3.one * _spawnScale;

				instance.transform.SetParent (_arrowsParent.transform);

				_spawnedObjects.Add (instance);
				oldarrowList.Add (instance.transform.position);
			}

			DrawArrows (oldarrowList);		
		}

		private void Update()
		{		

			_departmentSignPreb.transform.position = _map.GeoToWorldPosition (new Vector2d (36.71580103, 3.18511784)) + new Vector3 (0f, 17f, 0f);

			if (!_AllSet)
				return;
			
			var count = _spawnedObjects.Count;
			if (!_arrowsSet) 
			{
				StartPath ();
			}
			else
			{
				for (int i = 0; i < count; i++)
				{
					var spawnedObject = _spawnedObjects [i];
					var location = _map.WorldToGeoPosition (spawnedObject.transform.position);
				
					spawnedObject.transform.localPosition = _map.GeoToWorldPosition (location) + new Vector3 (0f, 4f, 0f);
				}
			}

		}

		public void CalculatePath (List <Vector2d> coordinates)
		{
			if (coordinates.Count != 0) 
			{
				foreach (var s in coordinates)
					_locations.Add (s);

				_AllSet = true;
			}
			else
			{
				Debug.Log ("VIDE");
			}
		}

		private void DrawArrows (List <Vector3> path)
		{
			if (path.Count < 2)
				return;

			Quaternion planerot = Quaternion.identity;

			for (int i = 0; i < path.Count; i++)
			{
			
				float distance = 0;
				Vector3 offsetVector = Vector3.zero;

				if (i < path.Count - 1) 
				{
					offsetVector = path [i + 1] - path [i];
					planerot = Quaternion.LookRotation (offsetVector);
					distance = Vector3.Distance (path [i + 1], path [i]);

					if (distance < tileSpacing)
						continue;

					planerot = Quaternion.Euler (90, planerot.eulerAngles.y, planerot.eulerAngles.z);

					float newSpacing = 0;
					for (int j = 0; j < distance / tileSpacing; j++)
					{
						newSpacing += tileSpacing;
						var normalizedVector = offsetVector.normalized;
						var position = path [i] + newSpacing * normalizedVector;

						GameObject instance = Instantiate (_markerPrefab, position, planerot);
						instance.transform.localScale = Vector3.one * _spawnScale;
						instance.transform.SetParent (_arrowsParent.transform);

						_locations.Add (_map.WorldToGeoPosition (instance.transform.localPosition));
						_spawnedObjects.Add (instance);
					}
				}
				else 
				{
					GameObject instance = Instantiate (_markerPrefab, path [i], planerot);
					_locations.Add (_map.WorldToGeoPosition (instance.transform.localPosition));
					_spawnedObjects.Add (instance);
				}

			}

			_arrowsSet = true;
			//_text.text = "Arrows set";
			_map.gameObject.SetActive (false);
			UIManaging.ApplicationUIManager.Instance.OnStateChanged (UIManaging.ApplicationState.Destination_Selection, UIManaging.DestinationPanelState.FFPanel);

		}
	}
}