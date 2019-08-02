using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;


public class SpawnPrefab : MonoBehaviour
{
	
	void Start ()
	{
		var _prefabs = new GameObject ("Prefabs Holder");
		
		foreach (var dp in SQLiteDB_DestinationPoints.Instance.DestinationPoints)
		{
			GameObject c = GameObject.CreatePrimitive (PrimitiveType.Cube);
			c.name = dp.Value.LocationName;
			c.transform.position = dp.Value.LocationXZ;

			c.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);

			c.transform.SetParent (_prefabs.transform);
		}
		
		foreach (var dp in SQLiteDB_DestinationPoints.Instance.SynchronisationPoints)
		{
			GameObject c = GameObject.CreatePrimitive (PrimitiveType.Cube);
			c.name = dp.LocationName;
			c.transform.position = dp.LocationXZ;

			c.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);

			c.transform.SetParent (_prefabs.transform);
		}

	} 

}
