namespace UIManaging
{
	using System.Collections;
	using System.Collections.Generic;
	using System;
	using UnityEngine;

	public class FixedLocationPointUI : MonoBehaviour
	{
		// TODO : Make this an abstract class and create concrete implementations for manual/iBeacon stuff. 
		[SerializeField]
		public GameObject buttonPrefab;
		/*
		[SerializeField]
		public Transform tpPanel;

		[SerializeField]
		public Transform administrationPanel;

		[SerializeField]
		public Transform otherPanel;
*/
		public void RegisterUI(int id, string panelName, string label, Action<int> callback, string type = null)
		{
			if (buttonPrefab != null)
			{
				var syncButtonGO = Instantiate(buttonPrefab);
				syncButtonGO.transform.SetParent(GameObject.Find (panelName).transform, false);

				var syncButton = syncButtonGO.GetComponent<SyncLocationInteraction>();
				syncButton.Register(id, label, callback, type);

			}
		}
	}
}