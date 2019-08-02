using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCamScript : MonoBehaviour {

	public GameObject WebCameraPlane; 

	// Use this for initialization
	void Start () {

		WebCamTexture webCameraTexture = new WebCamTexture ();
		WebCameraPlane.GetComponent<MeshRenderer> ().material.mainTexture = webCameraTexture;
		webCameraTexture.Play ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
