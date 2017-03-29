using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour {

	private float elapsedTime = 0;
	private class Cloud {
		private Vector3 position;
		private GameObject gameObject;
		private GameObject template;

		public Cloud(Vector3 position, GameObject template, GameObject sky) {
			this.position = position;
			this.template = template;

			this.gameObject = Instantiate(template);
			this.gameObject.transform.parent = sky.transform;
			this.gameObject.transform.position = position;
		}
		public void Remove() {
			Destroy(this.gameObject);
		}
	}
	private GameObject[] cloudTemplates;

	private List<Cloud> clouds = new List<Cloud>();
	public GameObject sky;

	void Start () {
		cloudTemplates = GameObject.FindGameObjectsWithTag("Cloud");

		int i;
		for(i = 0; i < 20; i++) {
			clouds.Add(new Cloud(
				new Vector3(Random.Range(-15, 40), Random.Range(3, 30), Random.Range(0, i % 4 == 0 ? 100 : 10)),
				cloudTemplates[(int) Mathf.Ceil(Random.Range(0, cloudTemplates.Length))],
				sky
			));
		}
	}
}
