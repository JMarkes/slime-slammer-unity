using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour {

	private Vector3 outOfScreen = new Vector3(0,1000,0);

	public void onClick(Button button) {

		if (GameManager.instance.disableMenu) {
			return;
		}

		GameManager.instance.inMenu = true;

		SoundManager.instance.PlaySingle(GameManager.instance.tapSound);

		// Leave Intro button
		if (button.name == "IMLetsGoButton") {
			Hide("IntroPanel");
			Hide("MenuPanel");
			GameManager.instance.inMenu = false;
		}

		// Info Button
		if (button.name == "InfoButton") {
			Show("MenuPanel");
			Show("InfoMenu");
			return;
		}

		///////////////
		// Info Menu //
		///////////////

		// Tutorial Button
		if (button.name == "IMTutorialButton") {
			Hide("InfoMenu");
			Show("TutorialMenu");
			return;
		}

		// Credits Button
		if (button.name == "IMCreditsButton") {
			Hide("InfoMenu");
			Show("CreditsMenu");
			return;
		}

		// Back Button
		if (button.name == "IMBackButton") {
			Hide("InfoMenu");
			Hide("MenuPanel");
			GameManager.instance.inMenu = false;
		}

		///////////////////
		// Tutorial Menu //
		///////////////////

		// Back Button
		if (button.name == "TMBackButton") {
			Hide("TutorialMenu");
			if (Application.loadedLevel == 0 || Application.loadedLevel == 1) {
				Show("InfoMenu");
			} else if (Application.loadedLevel == 2) {
				Show("InfoBattleMenu");
			}
		}

		//////////////////
		// Credits Menu //
		//////////////////

		// Back Button
		if (button.name == "CMBackButton") {
			Hide("CreditsMenu");
			Show("InfoMenu");
		}

		// Info Battle Button
		if (button.name == "InfoBattleButton") {
			Show("MenuPanel");
			Show("InfoBattleMenu");
			return;
		}

		//////////////////////
		// Info Battle Menu //
		//////////////////////

		// Tutorial Button
		if (button.name == "IBMTutorialButton") {
			if (Application.loadedLevel == 0 || Application.loadedLevel == 1) {
				Hide("InfoMenu");
			} else if (Application.loadedLevel == 2) {
				Hide("InfoBattleMenu");
			}
			Show("TutorialMenu");
			return;
		}
		
		// Quit Quest Button
		if (button.name == "IBMQuitQuestButton") {
			Hide("InfoBattleMenu");
			Show("ExitMenu");
		}

		// Back Button
		if (button.name == "IBMBackButton") {
			Hide("InfoBattleMenu");
			Hide("MenuPanel");
			StartCoroutine(LeaveMenu());
		}

		///////////////
		// Exit Menu //
		///////////////

		// Quit Quest Button (YES)
		if (button.name == "EMQuitQuestButton") {
			Hide("ExitMenu");
			Hide("MenuPanel");
			GameManager.instance.inMenu = false;
			Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
			StartCoroutine(GameManager.instance.FadeOutMusic());
			StartCoroutine(player.QuitQuest());
		}

		// Back Button (NO)
		if (button.name == "EMBackButton") {
			Hide("ExitMenu");
			Show("InfoBattleMenu");
		}
	}

	private IEnumerator LeaveMenu() {
		yield return new WaitForSeconds(0.5f);
		GameManager.instance.inMenu = false;
	}

	private void Show(string menuName) {
		GameObject menuObject = GameObject.Find(menuName);
		menuObject.transform.localPosition = Vector3.zero;
	}

	private void Hide(string menuName) {
		GameObject menuObject = GameObject.Find(menuName);
		menuObject.transform.localPosition = outOfScreen;
	}
}
