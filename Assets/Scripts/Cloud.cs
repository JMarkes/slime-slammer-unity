using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

	private Vector3 offset;
	private Transform cloudHolder;

	void Start () {
		offset = new Vector3(Random.Range(2,5) * 0.02f, 0, 0);
		cloudHolder = GameObject.Find("CloudHolder").transform;
	}

	// Update is called once per frame
	void Update () {
		transform.position -= offset;
		if (transform.position.x <= -450) {
			int randCloud = Random.Range(1,4);
			int randYPos = Random.Range(375,426);
			GameObject cloud = Instantiate (Resources.Load("Prefabs/Background/Cloud" + randCloud), 
			                                new Vector3 (450, randYPos, -2), 
			                                Quaternion.identity) as GameObject;
			cloud.transform.SetParent(cloudHolder);
			Destroy(gameObject);
		}
	}
}
