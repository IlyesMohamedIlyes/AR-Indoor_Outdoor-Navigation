using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

// For Destination Points.
public class SyncLocationInteraction : MonoBehaviour
{
	int _locationId;

	public Button _syncButton;
	public Text _syncLocationText;
	public Image _syncLocationImage;

//	public Sprite _syncPointIcon;
	public Sprite _administrationIcon;
	public Sprite _wcIcon;
	public Sprite _bibliothequeIcon;
	public Sprite _mcIcon;
	public Sprite _doctorantPlaceIcon;


	public event Action<int> OnSyncLocationInteraction = delegate { };

	void Awake()
	{
		_syncButton.onClick.AddListener(SyncLocation);
	}


	public static Color hexToColor()
	{
/* **** 
 * Other method to get random colors
 * *****
		hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
		hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
		byte a = 255, r, g, b;//assume fully visible unless specified in hex

		byte r = byte.Parse(hex.Substring (0, 1), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring (2, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.TryParse(hex.Substring (4, 2), System.Globalization.NumberStyles.HexNumber);
		//Only use alpha if the string has enough characters
		if (hex.Length == 12)
		{
			a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
		}

		return new Color32(r, g, b, a);
*/

		return new Color (UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1.0f);
	}

	public void Register(int id, string label, Action<int> callback, string type = null)
	{
		//_syncButton.onClick.AddListener(SyncLocation);
		OnSyncLocationInteraction += callback;


		_locationId = id;
		_syncLocationText.text = label;

		if (type != null)
		{
			if (type == "ADM")
			{
				var iconColor = hexToColor();
				_syncLocationImage.color = iconColor;
			}

			if (type == "TP")
			{
				var iconColor = hexToColor();
				_syncLocationImage.color = iconColor;
			}
				
			if (type == "Bibliotheque")
			{
				var iconColar = hexToColor ();
				_syncLocationImage.color = iconColar;

				// Using this when Icon bib uploaded
				//_syncLocationImage.sprite = _bibliothequeIcon;
			}

			if (type == "Sanitaire") 
			{
				_syncLocationImage.sprite = _wcIcon;
			}

			if (type == "SalleDoctorants")
			{
				//A refaire with an icon
				//_syncLocationImage.sprite = _doctorantPlaceIcon
				var iconColar = hexToColor ();
				_syncLocationImage.color = iconColar;
			}

			if (type == "MicroClub") 
			{
				_syncLocationImage.sprite = _mcIcon;
			}
		}
	}

	private void SyncLocation()
	{
		Debug.Log("Button OnClick In SyncLocation "+ _locationId);
		// This will call 'OnDestRequested' Method of DestinationPointProvider Class. the Method is in line 131

		OnSyncLocationInteraction(_locationId);
	}
}
