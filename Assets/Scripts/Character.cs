using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Character : MonoBehaviour {

	public string charName;
	public int MAXHP;
	public int HP;

	protected virtual void Awake() {

	}
}
