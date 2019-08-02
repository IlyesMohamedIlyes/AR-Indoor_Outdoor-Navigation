using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyClip : MonoBehaviour {

	Animation _anime;

	int _number;

	void Start ()
	{
		_anime = GetComponent <Animation> ();

		StartCoroutine ("PlayAnime");
	}

	IEnumerator PlayAnime ()
	{
		while (true) 
		{
			var pos = transform.position;

			_number = Random.Range (1, 3);
			_anime.clip = _anime.GetClip ("Clip" + _number); 
			_anime.Play ();

			yield return new WaitForSeconds (1f);
		}
	}
}
