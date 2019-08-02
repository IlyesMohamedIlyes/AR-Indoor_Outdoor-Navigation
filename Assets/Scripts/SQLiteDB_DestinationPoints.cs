using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System;
using System.IO;


/* Methods lines :
 
 * Awake () : Line 73
 * OpenDataBase () : Line 
 * CloseDataBase () : Line
 * FromDB_To_ProfessorsPanel (string sqlQuery) : Line 148
 * InstantiateReadyButton (GameObject buttonToInstantiate, IDataReader informations) : Line 124
 * DestroyingButtonInPanel () : Line 140
 * FromDB_To_GeneralPopup (ChildrenProfessorBioButton forSqlQuery) : Line 189
 * LoadSynchronisationPoints (string type) : Line 257
 
*/



public class SQLiteDB_DestinationPoints : MonoBehaviour {


	public GameObject _buttonPrefabProfessor;
	public GameObject _separationBar;
	public GameObject _contentPanel;
	public GameObject GeneralInformationsBioPanel;

	char _nameFirstLetter = '-';
	List <GameObject> professorsList = new List <GameObject> ();

	string conn;  //path

	IDbConnection dbconn;

	bool _isDestinationPointsCompleted = false;
	public bool IsDestinationPointsCompleted
	{
		get
		{ 
			return _isDestinationPointsCompleted;
		}
		private set
		{ 
			_isDestinationPointsCompleted = value;
		}
	}

	bool _isSynchronisationPointsCompleted = false;
	public bool IsSynchronisationPointsCompleted
	{
		get
		{ 
			return _isSynchronisationPointsCompleted;
		}
		private set
		{
			_isSynchronisationPointsCompleted = value;
		}
	}


	static SQLiteDB_DestinationPoints _instance;
	public static SQLiteDB_DestinationPoints Instance
	{
		get
		{
			return _instance;
		}
		set
		{ 
			_instance = value;
		}
	}

	void Awake ()
	{
		Instance = this;
	
		try
		{
		if (Application.platform != RuntimePlatform.Android)
			conn = Application.dataPath + "/StreamingAssets/MesDonnees.db"; //Path to database. File extention be .db not .s3db !!

		else
		{
				conn = Application.persistentDataPath + "/" + "MesDonnees.db";
				/*
			if(!File.Exists(conn))
			{*/
				//text.text += "\n" + Application.streamingAssetsPath + "/" + "MesDonnees.db";
				WWW load = new WWW (Application.streamingAssetsPath + "/" + "MesDonnees.db");

				while (!load.isDone) {}
				
				//	text.text += "load done";
				
				File.WriteAllBytes (conn, load.bytes);
				//	text.text += "Wrote done";
		}

		//Open connection to the database in functions.  
		
		OpenDataBase ();
		LoadPoints ("sync-point");
		LoadPoints ("dest-point");
		
		}catch (Exception e)
		{
			//text.text += e.ToString ();
		}
	}


	void OpenDataBase ()
	{
		dbconn = (IDbConnection)new SqliteConnection ("URI=file:" + conn);
		dbconn.Open ();
	}

	void CloseDataBase ()
	{
		dbconn.Close ();
	}

	void InstantiateReadyButton (GameObject buttonToInstantiate, IDataReader reader)
	{
		var button = Instantiate (buttonToInstantiate);
		button.name = reader.GetString (1) + " " + reader.GetString (2);

		Sprite ensImage = Resources.Load <Sprite> ("Professors Photos/" + reader.GetInt32 (0));
		if (ensImage == null)
			ensImage = Resources.Load <Sprite> ("Professors Photos/" + "default");	
		
		button.GetComponent <ProfessorBiography> ().SetProfessorBiography (reader.GetInt32 (0), //id
			reader.GetString (1) + " " + reader.GetString (2), // <Nom> <Prénom>,
			reader.GetString (3), // <Domaine> (field),
			reader.GetString (4), // <Grade>,
			reader.GetInt32 (5), // <Numero Bureau>,
			ensImage // <Prof Image>.
		);
			
		professorsList.Add (button);
	}

	void DestroyingButtonInPanel ()
	{
		//text.text += "destroy";
		Debug.Log ("Destroy");
		foreach (Transform child in  _contentPanel.transform)
			Destroy (child.gameObject);
	}

	public void FromDB_To_ProfessorsPanel (string sqlQuery)
	{
		if (professorsList.Count != 0)
			professorsList.Clear ();

		try
		{
			//text.text += "In SQLite Code";
			//text.text += "\n "+conn+"\n";

			/* Did this to prevent openning DataBase two times, by Awake Method and this one. 
			 * The DataBase will be re-openned till it get closed in LoadSynchronisationPoints method.
			*/

			if (IsDestinationPointsCompleted && IsSynchronisationPointsCompleted)
			{
				OpenDataBase ();
			}	

			using (IDbCommand dbcmd = dbconn.CreateCommand ())
			{
				dbcmd.CommandText = sqlQuery;

				using (IDataReader reader = dbcmd.ExecuteReader ())
				{

					DestroyingButtonInPanel ();
				
					while (reader.Read ())
					{
						InstantiateReadyButton (_buttonPrefabProfessor, reader);
					
					}// Fin While (reader.read)

					reader.Close ();

					dbcmd.Dispose ();
				}
			}

			// Sorting List per name.
			professorsList.Sort ((p1, p2) => p1.GetComponent <ProfessorBiography> ()._professorName.text.
				CompareTo (p2.GetComponent <ProfessorBiography> ()._professorName.text));
			
			int nombreOfSameLetter = 0; // this is used for a good representation of professors buttons to sort them by their name first letter.

			foreach (var button in professorsList)
			{
				// This if statements are used to prevent having a bar in the middle.
				if (professorsList.Count > 1)
				{	
					if (professorsList.Count == 2 && SearchManager.Instance.SearchMode.Equals ("name"))
					{
						button.transform.SetParent(_contentPanel.transform, false);
					}
					else
					{
						if (_nameFirstLetter != button.GetComponent <ProfessorBiography> ().NameFirstLetter)
						{
							_nameFirstLetter = button.GetComponent <ProfessorBiography> ().NameFirstLetter;

							_separationBar.GetComponentInChildren <Text> ().text = _nameFirstLetter.ToString ();

							if (nombreOfSameLetter != 0)
							{
								var numberOfEmptyObjects = 3 - nombreOfSameLetter % 3; // we've 3 buttons per line. 

								// this is like a return to line (saut de ligne)
								while (numberOfEmptyObjects > 0)
								{
									var gameO = new GameObject ();
									gameO.AddComponent <RectTransform> ();

									gameO.transform.SetParent (_contentPanel.transform, false);
									numberOfEmptyObjects--;
								}

								nombreOfSameLetter = 0;

							} // Fin if (nombreOfSameLetter != 0)

							var bar = Instantiate (_separationBar);
							bar.transform.SetParent (button.transform, false);
							bar.GetComponent <RectTransform> ().anchoredPosition = new Vector3 (162.6761f, -3.577461f, 0f);

						}
						
						nombreOfSameLetter++;

						button.transform.SetParent(_contentPanel.transform, false);
					}

				} // Fin if (professorsList.Count > 1)
				else
				{
					button.transform.SetParent(_contentPanel.transform, false);
				}
			
			} // Fin foreach (var button in professorsList)
					

					/* 
					 * Closing after the LoadSynchronizationPoints method is done, because it don't have openDataBase method, for a reason.
					*/
			if (IsDestinationPointsCompleted && IsSynchronisationPointsCompleted)
			{
				CloseDataBase ();
			}
			
		}
		catch (Exception e) 
		{
			Debug.Log (e.ToString ());
		}
	}

	//To update General professor Panel 
	public void FromDB_To_GeneralPopup (ProfessorBiography professorSelected)
	{	

		ProfessorBiography indexOnGeneral = GeneralInformationsBioPanel.GetComponent <ProfessorBiography> ();

		indexOnGeneral._professorId = professorSelected._professorId;
		indexOnGeneral._professorName.text = professorSelected._professorName.text;
		indexOnGeneral._professorField.text = professorSelected._professorField.text;
		indexOnGeneral._professorGrade.text = professorSelected._professorGrade.text;
		indexOnGeneral._professorOfficeNumber.text = professorSelected._professorOfficeNumber.text;
		indexOnGeneral._professorImage.sprite = professorSelected._professorImage.sprite;

	}

	private List <FixedPointInformations> _synchronisationPoints = new List<FixedPointInformations> ();
	public List <FixedPointInformations> SynchronisationPoints
	{
		get
		{ 
			if (_synchronisationPoints.Count == 0)
				LoadPoints ("sync-point");
			
			return _synchronisationPoints;
		}

	}

	private Dictionary <int, FixedPointInformations> _destinationPoints = new Dictionary<int, FixedPointInformations> ();
	public Dictionary <int, FixedPointInformations> DestinationPoints
	{
		get
		{ 
			if (_destinationPoints.Count == 0)
				LoadPoints ("dest-point");

			return _destinationPoints;
		}
	}

	void LoadPoints (string type)
	{
		try
		{
			using (IDbCommand dbcmd = dbconn.CreateCommand ())
			{
				dbcmd.CommandText = "SELECT * FROM PointsFixes WHERE type LIKE '" + type + "'";

				using (IDataReader reader = dbcmd.ExecuteReader ())
				{
					//text.text += "Cmd done";
					switch (type)
					{
						case "sync-point" : 
								while (reader.Read ())
								{
									var latlong = reader.GetString (3).Split (','); // Lat is index 0, long is index 1
										
									_synchronisationPoints.Add ( 
										new FixedPointInformations
								(reader.GetInt32 (0), reader.GetString (1), reader.GetString (2), new Mapbox.Utils.Vector2d (Double.Parse (latlong [0]), Double.Parse (latlong [1])))
									) // last  is the end of Add method of variable _synchronisationPoints. 
									;// Schéma : int id, string name, string type, Vector2d latitudeLongitude

									/*	_synchronisationPoints.Add ( 
											new FixedPointInformations
											(reader.GetInt32 (0), reader.GetString (1), reader.GetString (2), latlong[0],latlong[1])
										);*/
								
								} // Fin While

							IsSynchronisationPointsCompleted = true;
						break;

						case "dest-point" : 
								while (reader.Read ())
								{
									var latlong = reader.GetString (3).Split (','); // Lat is index 0, long is index 1

									_destinationPoints.Add (reader.GetInt32 (0),
										new FixedPointInformations
								(reader.GetInt32 (0), reader.GetString (1), reader.GetString (2), new Mapbox.Utils.Vector2d (Double.Parse (latlong [0]), Double.Parse (latlong [1])))
									) // last  is the end of Add method of variable _destinationPoints. 
									;// Schéma : int id, string name, string type, Vector2d latitudeLongitude
								
								
								/* _destinationPoints.Add (reader.GetInt32 (0),
								new FixedPointInformations
								(reader.GetInt32 (0), reader.GetString (1), reader.GetString (2), latlong [0], latlong [1])
								);*/
									
								} // Fin While

							IsDestinationPointsCompleted = true;
						break;
					}

					reader.Close ();

					dbcmd.Dispose ();

					if (IsDestinationPointsCompleted && IsSynchronisationPointsCompleted)
					{
						CloseDataBase ();
					}
				}
			}


		}
		catch (Exception e) 
		{
			Debug.Log (e.ToString ());
		}
	}


}	
