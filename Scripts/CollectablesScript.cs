using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesScript : MonoBehaviour
{
    public int CollectableID = 0; //0Star;1SpeedUpgrade;2WeaponUpgrade;3Shield;4Missiles
    GameController GCS;
    void Start()
    {
        GCS = GameObject.FindGameObjectWithTag("GameScripts").GetComponent<GameController>();
    }

    void FixedUpdate()
    {
        if (!GCS.GamePaused)
        {
            transform.Rotate(new Vector3(0f, transform.rotation.y + 1f, 0f));
            transform.position=(new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerScript Player = collision.GetComponent<PlayerScript>();
            switch (CollectableID)
            {
                case 0:
                    GCS.StarCollected();
                    GCS.Score += 150;
                    break;
                case 1:
                    Player.SpeedUpgrade();
                    break;
                case 2:
                    Player.WeaponUpgrade();
                    break;
                case 3:
                    Player.Shield();
                    break;
                case 4:
                    Player.Missiles();
                    break;
                default:
                    GCS.Score += 400;
                    break;
            }
            GCS.Score += 100;
            Destroy(gameObject);
        }
    }
}