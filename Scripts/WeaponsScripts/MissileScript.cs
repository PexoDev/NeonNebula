using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour {
    public float MissileSpeed = 0.2f;
    public bool PlayersMissile = true;
    GameController GCS;
    float Defaultx;
	void Start () {
        GCS = GameObject.FindGameObjectWithTag("GameScripts").GetComponent<GameController>();
        Defaultx = transform.position.x;
        gameObject.transform.SetParent(GCS.gameObject.transform);
        StartCoroutine(AutoDestruct());
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!GCS.GamePaused)
            transform.position = new Vector3(Defaultx, transform.position.y + MissileSpeed, 0f);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(PlayersMissile)
        if (collision.tag != "Player" && collision.tag != "PlayerAmmunition" && collision.tag != "Ragun" && collision.tag !="PlayerShield" && collision.tag != "EnemyAmmunition")
        {
            Instantiate(GCS.MissileExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if(!PlayersMissile)
        {
            if (collision.tag == "Player" || collision.tag == "PlayerAmmunition" || collision.tag == "Ragun" || collision.tag == "PlayerShield")
            {
                Instantiate(GCS.EnemyMissileExplosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    IEnumerator AutoDestruct()
    {
        for (;;)
        {
            if (transform.position.y > 9 && PlayersMissile) 
            {
                Instantiate(GCS.MissileExplosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
                yield return new WaitForEndOfFrame();
            }
            else
                yield return new WaitForSeconds(0.1f);

            if(transform.position.y < -19 && !PlayersMissile)
            {
                Instantiate(GCS.EnemyMissileExplosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
                yield return new WaitForEndOfFrame();
            }
            else
                yield return new WaitForSeconds(0.1f);
        }
    }
}
