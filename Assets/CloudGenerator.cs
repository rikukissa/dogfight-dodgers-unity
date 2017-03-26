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
	public Camera camera;
	public GameObject sky;

	// Use this for initialization
	void Start () {
		cloudTemplates = GameObject.FindGameObjectsWithTag("Cloud");

		int i;
		for(i = 0; i < 100; i++) {
			clouds.Add(new Cloud(
				new Vector3(Random.Range(-20, 100), Random.Range(0, 100), Random.Range(0, 10)),
				cloudTemplates[(int) Mathf.Ceil(Random.Range(0, cloudTemplates.Length))],
				sky
			));
		}

		// Camera.GetAllCameras(cameras);
	}
	float Mod(float x, float m) {
		return (x % m + m) % m;
	}
	// Update is called once per frame
	void Update () {
		// elapsedTime += Time.deltaTime;

		// Random.InitState(Random.Range(0, 100));


		// foreach (Camera camera in cameras) {
		// 	camera.worldToCameraMatrix.
		// }

		// int i = 0;

		// for(i = 0; i < 10; i++) {
		// 	float movement = elapsedTime * Random.value;

		// 	//

		// 	float depth = Mathf.Clamp(0.2f, 1, Random.value);

		// 	float x = Mod(Random.value * 100 - movement, 100);
		// 	float y = Mod(Random.value * 100 - movement, 100);

		// 	GameObject cloud = Instantiate(cloudTemplates[(int) Mathf.Ceil(Random.Range(0, cloudTemplates.Length))]);

		// 	if(x < cameras[0].transform.position.x || x > cameras[0].transform.position.x + cameras[0].aspect) {
		// 		return;
		// 	}

		// 	cloud.transform.position = new Vector3(x, y, depth);


		// }

	}
}
