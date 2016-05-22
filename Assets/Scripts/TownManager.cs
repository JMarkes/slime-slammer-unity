using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TownManager : MonoBehaviour {

	private const int STAT_MAX_RANK = 99;

	private Button weaponRankUpButton;
	private Button magicRankUpButton;
	private Button armorRankUpButton;

	private Text weaponRankUpInfo;
	private Text magicRankUpInfo;
	private Text armorRankUpInfo;

	private Text weaponRankUpDesc;
	private Text magicRankUpDesc;
	private Text armorRankUpDesc;

	private int weaponRank;
	private int magicRank;
	private int armorRank;

	private Player player;

	public void Init () {
		weaponRankUpButton = GameObject.Find("WeaponRankUpButton").GetComponent<Button>();
		magicRankUpButton = GameObject.Find("MagicRankUpButton").GetComponent<Button>();
		armorRankUpButton = GameObject.Find("ArmorRankUpButton").GetComponent<Button>();

		weaponRankUpInfo = GameObject.Find("WeaponRankUpInfo").GetComponent<Text>();
		magicRankUpInfo = GameObject.Find("MagicRankUpInfo").GetComponent<Text>();
		armorRankUpInfo = GameObject.Find("ArmorRankUpInfo").GetComponent<Text>();

		weaponRankUpDesc = GameObject.Find("WeaponRankUpDescription").GetComponent<Text>();
		magicRankUpDesc = GameObject.Find("MagicRankUpDescription").GetComponent<Text>();
		armorRankUpDesc = GameObject.Find("ArmorRankUpDescription").GetComponent<Text>();

		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); 
	}

	private void GetStats() {
		weaponRank = player.GetWeaponRank();
		magicRank = player.GetMagicRank();
		armorRank = player.GetArmorRank();
	}

	private void EnableUpgradeButtons() {
		weaponRankUpButton.interactable = player.CanUpgrade(0);
		magicRankUpButton.interactable = player.CanUpgrade(1);
		armorRankUpButton.interactable = player.CanUpgrade(2);
	}

	private void GetConditions(int statRank, Text statRankUpInfo) {
		if (statRank == STAT_MAX_RANK) {
			statRankUpInfo.text = "<size=30>MAX RANK</size>";
		} else {
			bool hasLevel = player.GetLevel() >= (statRank + 1);
			bool hasGold = player.GetGold() >= (statRank * 100); 

			statRankUpInfo.text = "<size=30>Upgrade</size>\n";

			if (!hasLevel) {
				statRankUpInfo.text += "<color=red>";
			}

			statRankUpInfo.text += "Hero Lv. " + (statRank + 1);

			if (!hasLevel) {
				statRankUpInfo.text += "</color>";
			}

			statRankUpInfo.text += " + ";

			if (!hasGold) {
				statRankUpInfo.text += "<color=red>";
			}

			statRankUpInfo.text += (statRank * 100) + " GOLD";

			if (!hasGold) {
				statRankUpInfo.text += "</color>";
			}
		}
	}
	
	public void UpdateUI() {

		player.UpdateUI();

		GetStats();

		EnableUpgradeButtons();

		GetConditions(weaponRank, weaponRankUpInfo);
		GetConditions(magicRank, magicRankUpInfo);
		GetConditions(armorRank, armorRankUpInfo);

		weaponRankUpDesc.text = "Weapon\n" + "<size=26>" + "Rank " + weaponRank + "</size>"; 
		magicRankUpDesc.text = "Magic\n" + "<size=26>" + "Rank " + magicRank + "</size>"; 
		armorRankUpDesc.text = "Armor\n" + "<size=26>" + "Rank " + armorRank + "</size>";
	}
}
