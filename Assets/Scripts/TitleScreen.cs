using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleScreen : MonoBehaviour {

	public void onClick() {
		StartCoroutine(GameManager.instance.LoadTownScene());
	}
}
