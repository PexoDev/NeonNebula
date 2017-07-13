using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueStarAmmunitionScript : MonoBehaviour {

    Rigidbody2D rb;
    GameController GCS;
    Vector3 Distance;
	void Start () {
        GCS = GameObject.FindGameObjectWithTag("GameScripts").GetComponent<GameController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (Random.Range(0f, 1f) > 0.5f)
            Distance = (GCS.Player1.transform.position - transform.position).normalized;
        else
            Distance = (GCS.Player2.transform.position - transform.position).normalized;

        StartCoroutine(Movement());
        gameObject.transform.SetParent(GCS.gameObject.transform);
    }

    IEnumerator Movement()
    {
        for (int i = 0; i < 401; i++)
        {

            if (!GCS.GamePaused)
            {
                rb.MovePosition(transform.position + Distance / 3);
                transform.Rotate(0f, 0f, 10f);
            }
            else
            {
                i--;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
