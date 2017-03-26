using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	Vector3 position;
	float throttle = 0;
	float vx = 0;
	float vy = 0;
	float angle = 0;

	// Use this for initialization
	void Start () {
		position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		// if(Input.GetButton("Throttle")) {
			throttle = 1;
		// } else {
		// 	throttle = 0;
		// }

		vx += throttle * 0.01f * Time.deltaTime;

		if(vx > 0) {
			vy += vx / 100 * Time.deltaTime;
			angle += vx * Time.deltaTime;
		}
		
		position.y += vy;
		position.x += vx * Time.deltaTime;
		
		transform.position = position;
		transform.eulerAngles = new Vector3(0, 0, angle);
	}
}
