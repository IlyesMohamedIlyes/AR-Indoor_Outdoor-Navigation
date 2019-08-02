using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* this script to remove. */

public class PrincipalPanelsManager : MonoBehaviour {

	public GameObject FFPanel;
	public GameObject RDCPanel;

	static PrincipalPanelsManager _instance;
	public static PrincipalPanelsManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new PrincipalPanelsManager();
			}
			return _instance;
		}
	}
		

	// False for First Floor. 			
	//True for Rez-De-Chaussé.
	public void OnStateChanging (bool RDC_or_FF)
	{
		if (!RDC_or_FF)
		{
			FFPanel.SetActive (true);
			RDCPanel.SetActive (false);
			SQLiteDB_DestinationPoints.Instance.FromDB_To_ProfessorsPanel ("SELECT * " + " FROM Professeurs ");

		} else 
		{
			FFPanel.SetActive (false);
			RDCPanel.SetActive (true);
		}
	}
}
