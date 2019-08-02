using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectRotator : MonoBehaviour {

	[SerializeField]
	AnimationCurve _curve;

	[SerializeField]
	/// <summary>
	/// The speed with which the gameobject rotates around its  Y axis
	/// </summary>
	private float rotationSpeed;
	
	// Update is called once per frame
	void Update ()
	{
		transform.Rotate(new Vector3(transform.eulerAngles.x,Time.deltaTime*rotationSpeed,transform.eulerAngles.z));
	}
}
