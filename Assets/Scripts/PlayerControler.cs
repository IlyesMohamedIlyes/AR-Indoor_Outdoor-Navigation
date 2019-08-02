using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mapbox.Examples;
using Mapbox.Utils;

public class PlayerControler : MonoBehaviour {


	public NavMeshAgent _agent;

	[SerializeField]
	float _positionFollowFactor;

	bool _isInitialized;
	bool _isPathSet = false;
	public bool IsPathSet
	{
		get
		{ 
			return _isPathSet;
		}
	}
	[SerializeField]
	private GameObject directionPrefab;

	[SerializeField]
	private float tileSpacing = 2;



	private List<GameObject> arrowList = new List<GameObject>();

	Vector3 _targetPosition;
	public Vector3 TargetPosition
	{
		get
		{ 
			return _targetPosition;
		}
	}

	float elapsed = 0.0f;

	NavMeshPath path;

	static PlayerControler _instance;
	public static PlayerControler Instance
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
	}

	void Start ()
	{
		path = new NavMeshPath();

	}

	public void SetDestination (Vector3 targetPoint)
	{
		_targetPosition = targetPoint;
		//_agent.SetDestination (targetPoint);
		NavMesh.CalculatePath (_agent.transform.position, targetPoint, NavMesh.AllAreas, path);
		DrawPath (path);
		_isPathSet = true;
	}

	void Update ()
	{
		elapsed += Time.deltaTime;
		if (elapsed > 1.0f && _isPathSet)
		{
			elapsed -= 1.0f;
			NavMesh.CalculatePath (_agent.transform.position, _targetPosition, NavMesh.AllAreas, path);
			DrawPath (path);
		}
	}

	void DrawPath(NavMeshPath navPath)
	{
		List<GameObject> arrows = arrowList;
		StartCoroutine(ClearArrows(arrows));
		arrowList.Clear();
		//If the path has 1 or no corners, there is no need to draw the line
		if (navPath.corners.Length < 2)
		{
			return;
		}

		// Set the array of positions to the amount of corners...
		//_line.positionCount = navPath.corners.Length;
		Quaternion planerot = Quaternion.identity;
		for (int i = 0; i < navPath.corners.Length; i++)
		{
			// Go through each corner and set that to the line renderer's position...
			//_line.SetPosition(i, navPath.corners[i]);
			float distance = 0;
			Vector3 offsetVector = Vector3.zero;
			if (i < navPath.corners.Length - 1)
			{
				//plane rotation calculation
				offsetVector = navPath.corners[i + 1] - navPath.corners[i];
				planerot = Quaternion.LookRotation(offsetVector);
				distance = Vector3.Distance(navPath.corners[i + 1], navPath.corners[i]);
				if (distance < tileSpacing)
					continue;

				planerot = Quaternion.Euler(90, planerot.eulerAngles.y, planerot.eulerAngles.z);

				//plane position calculation
				float newSpacing = 0;
				for (int j = 0; j < distance / tileSpacing; j++)
				{
					newSpacing += tileSpacing;
					var normalizedVector = offsetVector.normalized;
					var position = navPath.corners[i] + newSpacing * normalizedVector;
					GameObject go = Instantiate(directionPrefab, position, planerot);
					arrowList.Add(go);
				}
			}
			else
			{
				GameObject go = Instantiate(directionPrefab, navPath.corners[i], planerot);
				arrowList.Add(go);
			}
		}
			
	}

	private IEnumerator ClearArrows(List<GameObject> arrows)
	{
		if (arrowList.Count == 0)
			yield break;

		foreach (var arrow in arrows)
			Destroy(arrow);
	}


	/*if (Input.GetMouseButtonDown (0))
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit))
			{
				_agent.SetDestination (hit.point);
				NavMesh.CalculatePath(transform.position, hit.point, NavMesh.AllAreas, path);
				DrawPath(path);
				_isPathSet = true;
			}

	}*/
}
