using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class PreferenceManager : MonoBehaviour {

	[SerializeField]
	private WaitForSeconds _wait;

	public VideoPlayer _testVideo;

	public GameObject _flashLight;
	public GameObject _screenShot;
	public GameObject _chooseGuide;
	public GameObject _showAttributions;
	public GameObject _contribute;


	public List <GameObject> _attributions = new List <GameObject> ();

	public GameObject _panelGuides;
	public GameObject _panelAttributions;

	void Start ()
	{
		_wait = new WaitForSeconds (0.1f);
		_showAttributions.GetComponent <Button> ().onClick.AddListener (Show);
		_contribute.GetComponent <Button> ().onClick.AddListener (OpenUrl);
		_chooseGuide.GetComponent <Button> ().onClick.AddListener (Guide);
	}

	void OpenUrl ()
	{
		Application.OpenURL ("https://github.com/ZitaxProduct/AR-Indoor-Navigation");
	}

	void Show ()
	{
		_panelAttributions.SetActive (!_panelAttributions.activeSelf);
	}

	void Guide ()
	{
		_testVideo.Play ();
	}

	public void OnClick ()
	{
		StartCoroutine ("Enable");
	}

	IEnumerator Enable ()
	{
		_screenShot.SetActive (!_screenShot.activeSelf);
		_chooseGuide.SetActive (!_chooseGuide.activeSelf);
		yield return _wait;
		_flashLight.SetActive (!_flashLight.activeSelf);
		_showAttributions.SetActive (!_showAttributions.activeSelf);
		yield return _wait;
		_contribute.SetActive (!_contribute.activeSelf);
	}

}
