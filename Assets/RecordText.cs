using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RecordText : MonoBehaviour {
	// Use this for initialization
	void Start () {
		int record = PlayerPrefs.GetInt("record", 0);
		Debug.Log(record);
		if(record != 0) {
			GetComponent<Text>().text = ((float)record / 1000f).ToString();
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
