using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartQuest : MonoBehaviour {

	private Player player;

	public void onClick() {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

		SoundManager.instance.PlaySingle(GameManager.instance.tapSound);

		StartCoroutine(QuestOn());
	}

	private IEnumerator QuestOn() {
		// Fadeout music
		StartCoroutine(GameManager.instance.FadeOutMusic());

		// Player walking
		player.gameObject.GetComponent<Animator>().SetTrigger("playerRun");
		StartCoroutine(player.MoveCoroutine(new Vector3(500, 250, -8), 2));

		yield return StartCoroutine(GameManager.instance.FadeOut(0.5f));		

		// Start Battle Scene
		GameManager.instance.StartQuest();
	}
}
