using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	private bool hasSaved = false;
	public Player player;
	public GameManager GM;
	
	void Update () {
		if (player.transform.position.y > 5 && player.transform.position.x > 68 && !hasSaved) {
			GM.playerPos = new Vector3(70, 5.065f);
			hasSaved = true;
		}
	}
}
