using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDiscScript : MonoBehaviour {

    public float TravelDistance = 0.35f;

    Rigidbody2D rb;
    float defaultx;
    GameController GCS;
    void Start() {
        GCS = GameObject.FindGameObjectWithTag("GameScripts").GetComponent<GameController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        defaultx = transform.position.x;
        gameObject.transform.SetParent(GCS.gameObject.transform);
    }
    void Update()
    {
        if (!GCS.GamePaused)
            rb.MovePosition(new Vector2(defaultx, rb.transform.position.y + TravelDistance));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameObject.tag=="PlayerAmmunition")
            if (collision.tag!= "PlayerShield" && collision.tag != "Player" && collision.tag != "Reflecter" && collision.tag != "PlayerAmmunition" && collision.tag!="Collectable" && collision.tag != "EnemyAmmunition" && collision.tag != "MissileExplosion" && collision.tag != "Missile")
                Destroy(gameObject);
        if (gameObject.tag == "EnemyAmmunition")
            if (collision.tag == "Player")
                Destroy(gameObject);
    }
}
