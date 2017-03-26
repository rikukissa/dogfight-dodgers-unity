using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRepeat : MonoBehaviour {

	// Use this for initialization
	public GameObject template;
	public GameObject container;

	void Start () {
		int i;
		for(i = 1; i < 55; i++) {
			GameObject slice = Instantiate(template, new Vector3(template.transform.position.x + 3 * i, template.transform.position.y, template.transform.position.z), template.transform.rotation);
			slice.transform.parent = container.transform;

		}
	}

	// Update is called once per frame
	void Update () {

	}
}
