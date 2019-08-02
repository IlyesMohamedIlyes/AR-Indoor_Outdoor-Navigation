using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAround : MonoBehaviour {

	public GameObject g;

	void Start ()
	{
		transform.RotateAround (new Vector3 (72.66285f, -337.351f, 246.686f), new Vector3 (72.66285f, -337.351f, 251.686f), 360f);
	}
}
