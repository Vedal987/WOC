﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayVersion : MonoBehaviour {
	public string Name;
	void Update () {
		this.GetComponent<Text> ().text = Name + " " + Application.version;
	}
}
