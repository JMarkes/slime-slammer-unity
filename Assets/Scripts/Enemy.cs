using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : Character {

	public int STR;
	public int DEF;
	public int MDF;
	public int startingTurnCounter;
	public int turnCounter;
	public int experience;

	[HideInInspector]
	public float movementSpeed = 4f;

	private Color32 white = new Color32(255,255,255,255);
	private Color32 red = new Color32(255,0,0,255);

	private Text enemyTurnCounterText;
	private Slider enemyHpBar;

	private Player player;
	private Rigidbody2D rb2D;
	private SpriteRenderer sprite;
	private ParticleSystem enemyPS;

	private Vector3 originPos = new Vector3(105, 184, -8);
	private Vector3 closePlayerPos = new Vector3(0, 184, -8);
	private Vector3 awayPlayerPos = new Vector3(145, 184, -8);

	private bool enraged = false;
	
	protected override void Awake() {
		base.Awake();

		enemyTurnCounterText = GameObject.Find("EnemyTurnCounter").GetComponent<Text>();

		enemyHpBar = GameObject.Find("EnemyHPBar").GetComponent<Slider>();
		enemyHpBar.maxValue = MAXHP;

		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		rb2D = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
		enemyPS = GameObject.Find("EnemyPS").GetComponent<ParticleSystem>();

		UpdateUI();
	}

	public void Move (Vector3 end, float speed) {
		StartCoroutine(MoveCoroutine(end, speed));
	}

	private IEnumerator MoveCoroutine(Vector3 end, float speed) {
		while (true) {
			transform.position = Vector3.MoveTowards(transform.position, end, speed);
			
			// If the object has arrived, stop the coroutine
			if (transform.position == end) {
				if (GameManager.instance.playerRunning) {
					GameManager.instance.playerRunning = false;
					player.PlayIdleAnim();
				}
				yield break;
			}
			
			// Otherwise, continue next frame
			yield return null;
		}
	}

	public void CheckTurn() {
		if(HP <= 0) return;

		turnCounter--;

		if (turnCounter <= 0) {
			turnCounter = startingTurnCounter;

			StartCoroutine(AttackAnim(originPos, closePlayerPos, 15));
		} else {
			GameManager.instance.charAnimPlaying = false;
		}

		UpdateUI();
	}

	private IEnumerator AttackAnim (Vector3 start, Vector3 end, float speed) {
		// Move towards the player
		yield return StartCoroutine(MoveCoroutine(end, speed));

		// Inflict damage
		player.ReceiveDamage(CalculateDamage());
		UpdateUI();

		// Move back to origin position
		yield return StartCoroutine(MoveCoroutine(start, speed));

		GameManager.instance.charAnimPlaying = false;
	}

	private IEnumerator HitAnim (Vector3 start, Vector3 end, float speed) {
		// Move away from the player
		yield return StartCoroutine(MoveCoroutine(end, speed));
		
		// Move back to origin position
		yield return StartCoroutine(MoveCoroutine(start, speed));
	}

	public void ReceiveDamage(int nTiles, int damage, int attackType) {
		StartCoroutine(ReceiveDamageCoroutine(nTiles, damage, attackType));
	}

	IEnumerator ReceiveDamageCoroutine(int nTiles, int damage, int attackType) {
		while (nTiles > 0) {
			// Play appropriate animation and sound effect
			if (attackType == 0) {
				yield return StartCoroutine(player.PlayAttackAnim());
				SoundManager.instance.PlaySingle(player.attackAction);
			} else {
				SoundManager.instance.PlaySingle(player.magicAction);
				yield return StartCoroutine(player.PlayMagicAnim());
			}

			// Pop up damage text
			int rand = Random.Range(30,46);
			GameManager.instance.InstantiateRisingLabel(new Vector3 (rand,300,0), damage.ToString(), 35, white);

			// Flash enemy red
			StartCoroutine(GameManager.instance.FlashSprite(sprite, Color.red));

			// Play enemy hit animation
			yield return StartCoroutine(HitAnim(originPos, awayPlayerPos, 25));
			
			// Decrease number of remaining attacks
			nTiles--;

			HP -= damage;
			if (HP <= 0) {
				HP = 0;
				Kill();
				nTiles = 0; // Skip following attacks
				GameManager.instance.charAnimPlaying = false;
			}
			UpdateUI();
		}
		yield return new WaitForSeconds(0.5f);
		CheckTurn();
	}
	
	public void Kill() {
		SoundManager.instance.PlaySingle(player.enemyKill);
		GameManager.instance.enemyDead = true;
		player.IncrementMonstersSlain();
		player.GetExp(experience);
		StartCoroutine(KillCoroutine());
	}

	IEnumerator KillCoroutine() {
		while(transform.position.y < 500) {
			transform.Translate(15,20,0, Space.World);
			rb2D.MoveRotation(rb2D.rotation - 40);
			yield return null;
		}

		Destroy (gameObject);
	}

	private int CalculateDamage() {
		int damage = (int) Mathf.Round(1 + (STR * 3 - player.GetArmorRank()));
		return Mathf.Max(damage,1);
	}

	public void BuffEnemy(int nTiles) {
		SoundManager.instance.PlaySingle(player.enraged);
		enemyPS.Play();
		GameManager.instance.InstantiateRisingLabel(new Vector3 (43,300,0), "ENRAGED!!", 30, red);
		STR += (int) Mathf.Round(STR * 0.2f * nTiles);
		DEF += (int) Mathf.Round(DEF * 0.2f * nTiles);
		MDF += (int) Mathf.Round(MDF * 0.2f * nTiles);
		experience += (int) Mathf.Round(experience * 0.2f * nTiles);
		enraged = true;
	}

	public void ApplyStats(int rank) {
		
		// Default stats in prefab object apply to rank 1
		// so this function is skipped
		if (rank == 1) return;

		rank--; // to make calculations easier and code cleaner

		MAXHP *= (2 * rank);      // times 2 per rank
		HP *= (2 * rank);         // times 2 per rank
		STR += (7 * rank);        // plus 7 per rank
		DEF += (7 * rank);        // plus 7 per rank
		MDF += (7 * rank);        // plus 7 per rank
		experience *= (2 * rank); // times 2 per rank

		enemyHpBar.maxValue = MAXHP;
		enemyHpBar.value = HP;
	}

	public void UpdateUI() {
		enemyTurnCounterText.text = turnCounter.ToString() + " " +
			(turnCounter == 1 ? "<size=20>TURN</size>" : "<size=20>TURNS</size>");
		enemyHpBar.value = HP;
	}
	
	void Update() {
		if (HP > 0) {
			if (turnCounter == 1) {
				float a = Mathf.PingPong (Time.time / 0.1f, 1.0f);
				enemyTurnCounterText.color = new Color(1,0,0,a);
			} else {
				enemyTurnCounterText.color = new Color(1,1,1,1);
			}
		}

		if (enraged) {
			float a = Mathf.PingPong (Time.time, 1.0f);
			sprite.color = new Color(1,a,a,1);
		}
	}
}
