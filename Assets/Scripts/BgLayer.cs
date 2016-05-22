using UnityEngine;
using System.Collections;

public class BgLayer : MonoBehaviour {

	public float speed;
	public int zOrder;

	private Vector3 start;
	private Vector3 offset;

	void Start () {
		start = new Vector3(620,313,zOrder);
		offset = new Vector3(speed/2f,0,0);
	}

	// Update is called once per frame
	void Update () {
		if (GameManager.instance.playerRunning) {
			transform.position -= offset;
			if (transform.position.x <= -620) {
				transform.localPosition = start;
			}
		}
	}
}
