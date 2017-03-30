using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CurrentTimeText : MonoBehaviour {
	// Use this for initialization
	System.DateTime start;
	void Start () {
		start = System.DateTime.Now;
	}

	// Update is called once per frame
	void Update () {
		GetComponent<Text>().text = ((System.DateTime.Now - start).TotalMilliseconds / 1000).ToString();
	}
}
