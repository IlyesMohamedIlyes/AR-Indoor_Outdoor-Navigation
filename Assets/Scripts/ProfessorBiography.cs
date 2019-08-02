using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfessorBiography : MonoBehaviour {

	// Those with public are needed for the visualization of the button.
	public Image _professorImage;
	public Text _professorName;
	public char NameFirstLetter
	{
		get
		{ 
			return _professorName.text [0];
		}
	}

	public int _professorId;
	public Text _professorField;
	public Text _professorGrade;
	public Text _professorOfficeNumber;


	public void SetProfessorBiography (int professorId, string professorName, string professorField, string professorGrade, int professorOfficeNum, Sprite image)
	{
		_professorId = professorId;
		_professorName.text = professorName;
		_professorField.text = "Domaine " + professorField;
		_professorGrade.text = "Grade " + professorGrade;
		_professorOfficeNumber.text = "Bureau " + professorOfficeNum;
		_professorImage.sprite = image;
	}
		
}
