using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeStat : MonoBehaviour {

	public void onClick(Button button) {

		if (button.name == "WeaponRankUpButton") {
			GameManager.instance.UpgradeStat(0);
		}
		else if (button.name == "MagicRankUpButton") {
			GameManager.instance.UpgradeStat(1);
		}
		else if (button.name == "ArmorRankUpButton") {
			GameManager.instance.UpgradeStat(2);
		}
	}
}
