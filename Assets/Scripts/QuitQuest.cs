using UnityEngine;
using System.Collections;

public class QuitQuest : MonoBehaviour {

	public void onClick() {
		SoundManager.instance.PlaySingle(GameManager.instance.tapSound);
		Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		StartCoroutine(player.QuitQuest());
	}
}
