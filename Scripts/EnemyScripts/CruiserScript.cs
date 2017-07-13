using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CruiserScript : MonoBehaviour
{

	GameController GCS;
	Rigidbody2D rb;
	GameObject Player;
	int HP = 6;

	void Start ()
	{
		GCS = GameObject.FindGameObjectWithTag ("GameScripts").GetComponent<GameController> ();
		rb = gameObject.GetComponent<Rigidbody2D> ();
		StartCoroutine (Shooting ());
		StartCoroutine (Movement ());
		HP += GCS.Difficulty * 2;

		if (Random.Range (0f, 1f) > 0.5f)
			Player = GCS.Player1;
		else
			Player = GCS.Player2;
	}

	IEnumerator Shooting ()
	{
		float WaitTime = 3f;
		for (;;) {
			if (GCS.GamePaused)
				yield return new WaitForEndOfFrame ();
			else {
				Instantiate (GCS.EnemyAmmunition [1], new Vector3 (transform.position.x, transform.position.y - 3.25f, 0f), Quaternion.identity);
				for (int i = 0; i < 101; i++)
					if (GCS.GamePaused) {
						i--;
						yield return new WaitForEndOfFrame ();
					} else
						yield return new WaitForSeconds (WaitTime / 100);
				if (WaitTime > 1.2f)
					WaitTime -= 0.05f;
			}
		}
	}

	IEnumerator Movement ()
	{
		for (;;) {
			if (GCS.GamePaused)
				yield return new WaitForEndOfFrame ();
			else {

				if (transform.position.y > 5) {
					for (int i = 0; i < 6; i++) {
						rb.MovePosition (new Vector2 (transform.position.x, transform.position.y - 0.08f));
						yield return new WaitForSeconds (0.005f);
					}
				} else {
					if (Mathf.Pow ((Player.transform.position.x - transform.position.x), 2) > 0.8) {
						if (Player.transform.position.x > transform.position.x) {
							rb.MovePosition (new Vector2 (transform.position.x + 0.04f, transform.position.y));
							yield return new WaitForSeconds (0.001f);
						} else if (Player.transform.position.x < transform.position.x) {
							rb.MovePosition (new Vector2 (transform.position.x - 0.04f, transform.position.y));
							yield return new WaitForSeconds (0.001f);
						}
					} else
						yield return new WaitForEndOfFrame ();
				}
			}
		}
	}

	private void OnTriggerEnter2D (Collider2D collision)
	{
        
		if (collision.tag == "PlayerAmmunition")
			HP--;

		if (collision.tag == "Raygun" || collision.tag == "MissileExplosion")
			HP -= 5;

		if (collision.tag == "Player")
			HP = 0;

		if (HP < 1)
			Die ();
	}

	public void Die ()
	{
		GCS.DropCollectable (0.4f, transform.position);
		Instantiate (GCS.EnemyExplosions [1], transform.position, Quaternion.identity);
		GCS.Score += 325;
		Destroy (gameObject);
	}
}
