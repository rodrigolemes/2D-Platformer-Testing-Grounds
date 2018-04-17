using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public Player player;
	public PlatformController[] movingPlatforms;
	
	[HideInInspector]
	public Vector3 playerPos;
	private Vector3[] movingPlatformsPos; 

	void Start () {
		movingPlatformsPos = new Vector3[movingPlatforms.Length];
		playerPos = player.transform.position;
		for (int i=0; i<movingPlatforms.Length; i++) {
			movingPlatformsPos[i].x = movingPlatforms[i].transform.position.x;
			movingPlatformsPos[i].y = movingPlatforms[i].transform.position.y;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)) {
			ResetPlayer();
		}
		if (Input.GetKeyDown(KeyCode.F)) {
			ResetPlatforms();
		}

		//if (player.transform.position.x > 68 && player.transform.position.x < 71 && player.transform.position.y > 4.5f) {
		//	playerPos = player.transform.position;
		//}
	}

	void ResetPlayer () {
		player.transform.position = playerPos;
	}

	void ResetPlatforms () {
		for (int i = 0; i < movingPlatforms.Length; i++) {
			movingPlatforms[i].transform.position = new Vector3(movingPlatformsPos[i].x, movingPlatformsPos[i].y);
		}
	}

}
