using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	private int row;
	private int column;
	private int tileType;
	
	private Animator animator;
	private int totalMove = 0;
	
	void Awake () {
		GameManager.instance.GetBoardManager().AddTileToList(this);
		animator = GetComponent<Animator>();
	}

	public void Init (int row, int column, int tileType) {
		this.row = row;
		this.column = column;
		this.tileType = tileType;
	}

	public void Drop (int nTiles) {
		row += nTiles;
		totalMove += nTiles;
	}

	public void PopOut () {
		row = -1;
		column = -1;
		tileType = -1;
		animator.SetTrigger("pop");
	} 

	public void Move (float yDir) {
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2 (0, totalMove*yDir);
		StartCoroutine (MoveCoroutine (end));
		totalMove = 0;
	}

	private IEnumerator MoveCoroutine(Vector3 end) {
		float timeSinceStarted = 0f;
		while (true) {
			timeSinceStarted += Time.deltaTime;
			transform.position = Vector3.Lerp(transform.position, end, timeSinceStarted);
			
			// If the object has arrived, stop the coroutine
			if (transform.position == end) {
				yield break;
			}
			
			// Otherwise, continue next frame
			yield return null;
		}
	}
	
	public int GetRow() {
		return row;
	}

	public int GetColumn() {
		return column;
	}

	public int GetTileType() {
		return tileType;
	}

	void Update() {
		if (transform.localScale.x == 0) {
			Destroy(gameObject);
		}
	}
}
