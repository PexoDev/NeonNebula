using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMainScript : MonoBehaviour
{
	int HP = 10;
	GameController GCS;

	void Start ()
	{
		GCS = GameObject.FindGameObjectWithTag ("GameScripts").GetComponent<GameController> ();
		HP += GCS.Difficulty * 3;
	}

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (GCS.BossDestroyable) {
			if (collision.tag == "PlayerAmmunition")
				HP--;
			if (collision.tag == "Raygun" || collision.tag == "MissileExplosion")
				HP -= 5;
			if (collision.tag == "PlayerShield")
				HP = 0;

			if (HP < 1)
				StartCoroutine (DestroyBoss ());
		}
	}

	IEnumerator DestroyBoss ()
	{
		GCS.StopGame (true);
		for (int i = 0; i < 6; i++) {
			Instantiate (GCS.EnemyExplosions [(int)Random.Range (1f, 5f)], new Vector3 (transform.position.x + Random.Range (-5f, 5f), transform.position.y + Random.Range (-5f, 5f), 0f), Quaternion.identity);
			yield return new WaitForSeconds (0.5f);
		}
		GCS.Score += 2500 + 1000 * GCS.Difficulty;
		GCS.DestroyBoss ();
	}
}
