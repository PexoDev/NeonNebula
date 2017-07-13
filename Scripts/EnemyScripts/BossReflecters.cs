using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossReflecters : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAmmunition")
        {
            collision.transform.Rotate(new Vector3(0, 0, 180));
            collision.GetComponent<LaserDiscScript>().TravelDistance = -0.35f;
            ParticleSystem tmp = collision.GetComponentInChildren<ParticleSystem>();
            ParticleSystem.MainModule tmp2 = tmp.main;
            tmp2.gravityModifier = 0.1f;
            collision.tag = "EnemyAmmunition";
        }
    }
}
