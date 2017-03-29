using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRepeat : MonoBehaviour {

	// Use this for initialization
	public GameObject template;
	public GameObject dimensionObject;
	public GameObject container;

	public GameObject following;

	private List<GameObject> slices = new List<GameObject>();

	void Start () {
		int i;

		float width = dimensionObject.GetComponent<SpriteRenderer>().bounds.size.x;

		float startX = following.transform.position.x - 15;
		float endX = following.transform.position.x + 15;

		for(i = 0; i < (endX - startX) / width; i++) {
			Vector3 position = new Vector3(startX + width * i, template.transform.position.y, template.transform.position.z);
			GameObject slice = Instantiate(template, position, template.transform.rotation);
			slice.transform.parent = container.transform;
			slices.Add(slice);
		}
	}
	void Update () {
		foreach(GameObject slice in slices) {
			Vector3 position = slice.transform.position;
			position.x -= following.transform.position.x;

			slice.transform.position = position;
		}
	}
}
