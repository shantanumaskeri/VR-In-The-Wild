﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Application_MobileInput : MonoBehaviour 
{

	// Use this for initialization
	private void Start () 
	{
		
	}
	
	// Update is called once per frame
	private void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			Application.Quit ();
		}
	}
}
