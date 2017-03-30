using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerFuture : MonoBehaviour {

	public GameObject plane;
	public GameObject futures;

	bool locked = false;

	public int numGhosts;
	public int startStep;
	public int steps = 50;


	void Update () {

		if(!Application.isPlaying) {
			locked = false;
		}

		if(locked) {
			return;
		}

		if(Application.isPlaying) {
			locked = true;
		}

		plane = GameObject.FindGameObjectWithTag("Player");
		futures = GameObject.FindGameObjectWithTag("Future");

		GameObject[] ghostObjects = GameObject.FindGameObjectsWithTag("Ghost");

		foreach (GameObject gameObject in ghostObjects) {
			DestroyImmediate(gameObject);
		}

		if(!Application.isEditor) {
			return;
		}


		int i = 0;
		int o = 0;
		for(i = 0; i < numGhosts; i++) {

			GameObject created = Instantiate(plane);
			created.tag = "Ghost";
			created.name = plane.name;

			PlayerBehaviour planeBehaviour = plane.GetComponent<PlayerBehaviour>();

			Vector3 position = plane.transform.position;
			position.z = 0.01f;

			created.transform.position = position;
			created.transform.parent = futures.transform;

			SpriteRenderer spRend = created.GetComponent<SpriteRenderer>();
			Color col = spRend.color;
			col.a = 0.5f;
			spRend.color = col;

			PlayerBehaviour behaviour = created.GetComponent<PlayerBehaviour>();
			created.transform.eulerAngles = new Vector3(0, 0, plane.transform.eulerAngles.z);

			behaviour.ghost = true;

			for(o = 0; o < startStep + steps; o++) {
				behaviour.DoUpdate(Time.fixedDeltaTime);
			}

			plane = created;
		}
	}
}
