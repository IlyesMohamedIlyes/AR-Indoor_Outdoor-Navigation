using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForTextContent : MonoBehaviour {

	public Text _textStep;
	public Text _textImmediate;
	public Text _XZDest;

	private static ForTextContent _instance;
	public static ForTextContent Instance 
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

		_textStep.text = "Awake";
	}


}
