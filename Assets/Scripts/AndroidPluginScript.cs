using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

// THIS PLUGIN IS TO OPEN THE SERVICE LOCATION SETTING ON ANDROID IF THE USER DIDN'T ENABLE GPS.

public class AndroidPluginScript : MonoBehaviour {

	AndroidJavaClass plugin;


	void Start ()
	{
		plugin = new AndroidJavaClass ("zitaxproduction.com.unityplugin.PluginClass");
	}


	public void OpenGPSSettings ()
	{
		try
		{
			plugin.Call ("OpenGPSSettings", null);
		}
		catch (Exception e)
		{
				
		}
	}

}
