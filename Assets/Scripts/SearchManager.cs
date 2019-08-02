using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchManager : MonoBehaviour {

	public GameObject _perNameToggleCheckMark;
	public GameObject _perNumberDesktopToggleCheckMark;
	public GameObject _userInputField;

	public string SearchMode
	{
		get
		{ 
			if (_perNameToggleCheckMark.activeSelf == true)
				return "name";

			return "desknumber";
		}
	}

	static SearchManager _instance;
	public static SearchManager Instance
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


	// Use this for initialization
	void Start () {

		// Don't know why it's inverse. But it's working. So i'm gonna let it as it is.

		_perNameToggleCheckMark.SetActive (false);
		_perNumberDesktopToggleCheckMark.SetActive (true);

		// Just for initialization
		OnChangingValues ();
	}

	public void OnChangingValues ()
	{
		
		if (_perNameToggleCheckMark.activeSelf == true)
		{
			// THE SEARCH IS PER *** Desktop Number ***

			_perNameToggleCheckMark.SetActive (false);
			_perNumberDesktopToggleCheckMark.SetActive (true);

			_userInputField.GetComponent <ForwardProfessorsInput> ().SearchPer = "desknumber";

			_userInputField.GetComponent <InputField> ().placeholder.GetComponent <Text> ().text = "Entrez le numéro de bureau du professeur";
			_userInputField.GetComponent <InputField> ().characterLimit = 3;
			_userInputField.GetComponent <InputField> ().contentType = InputField.ContentType.IntegerNumber;

			_userInputField.GetComponent <InputField> ().text = ""; // initialize the input field

			_userInputField.GetComponent <InputField> ().onEndEdit.AddListener(_userInputField.GetComponent <ForwardProfessorsInput> ().HandleUserInput);
			_userInputField.GetComponent <InputField> ().onValueChanged.RemoveListener (_userInputField.GetComponent <ForwardProfessorsInput> ().HandleUserInput);
		
		} else
		{
			// THE SEARCH IS PER *** NAME ***

			_perNameToggleCheckMark.SetActive (true);
			_perNumberDesktopToggleCheckMark.SetActive (false);

			_userInputField.GetComponent <ForwardProfessorsInput> ().SearchPer = "name";

			_userInputField.GetComponent <InputField> ().placeholder.GetComponent <Text> ().text = "Entrez le nom du professeur";
			_userInputField.GetComponent <InputField> ().characterLimit = 0;
			_userInputField.GetComponent <InputField> ().contentType = InputField.ContentType.Name;

			_userInputField.GetComponent <InputField> ().onEndEdit.RemoveListener(_userInputField.GetComponent <ForwardProfessorsInput> ().HandleUserInput);
			_userInputField.GetComponent <InputField> ().onValueChanged.AddListener (_userInputField.GetComponent <ForwardProfessorsInput> ().HandleUserInput);

			_userInputField.GetComponent <InputField> ().text = ""; // initialize the input field
		}
	
		SQLiteDB_DestinationPoints.Instance.FromDB_To_ProfessorsPanel ("SELECT * FROM Professors");
	}
}
