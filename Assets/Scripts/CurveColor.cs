using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurveColor : MonoBehaviour {


	[SerializeField]
	AnimationCurve _curve;

	// Update is called once per frame
	void Update () {

		var t = _curve.Evaluate(Time.time);
		GetComponent <Image> ().color = Color.Lerp(Color.clear, Color.white, t);
	}
}
