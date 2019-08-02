using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;


public class ForwardProfessorsInput : MonoBehaviour {

	InputField _inputField;

	private string _searchPer;
	public string SearchPer
	{
		get
		{ 
			return _searchPer;
		}
		set
		{ 
			_searchPer = value;
		}
	}

	void Awake ()
	{
		_inputField = GetComponent<InputField>();

		_inputField.onValueChanged.AddListener (HandleUserInput);

	}
		

	public void HandleUserInput(string searchString)
	{	
		if (!string.IsNullOrEmpty (searchString))
		{
			string sqlQuery = null;

			switch (SearchPer)
			{
				case "name" :

								
					sqlQuery = "SELECT * " + " FROM Professeurs " + "WHERE Nom LIKE '" + searchString + "%'"; // Don't forget the => ' <= .

					SQLiteDB_DestinationPoints.Instance.FromDB_To_ProfessorsPanel (sqlQuery);

				break;

				case "desknumber": 
				
//					    if (Regex.IsMatch (searchString, @"^-?\d+$"))     /* This managed in SearchManager Script. Verify if the string is all integer */
					  
						sqlQuery = "SELECT * " + " FROM Professeurs " + "WHERE NumeroBureau = " + searchString; // Don't forget the => ' <= .

						SQLiteDB_DestinationPoints.Instance.FromDB_To_ProfessorsPanel (sqlQuery);
//					  
				break;

				default :
				break;
			}

		}else 
		{
			SQLiteDB_DestinationPoints.Instance.FromDB_To_ProfessorsPanel ("SELECT * " + " FROM Professeurs ");
		}

	}

}
