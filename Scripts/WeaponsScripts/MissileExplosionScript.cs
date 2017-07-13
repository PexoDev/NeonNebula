using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplosionScript : MonoBehaviour {

    GameController GCS;
    void Start()
    {
        GCS = GameObject.FindGameObjectWithTag("GameScripts").GetComponent<GameController>();
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        for (int i = 0; i < 21; i++)
            if (GCS.GamePaused)
            {
                i--;
                yield return new WaitForEndOfFrame();
            }
            else
                yield return new WaitForSeconds(0.01f);

        gameObject.GetComponent<CircleCollider2D>().enabled = true;

        for (int i = 0; i < 11; i++)
            if (GCS.GamePaused)
            {
                i--;
                yield return new WaitForEndOfFrame();
            }
            else
                yield return new WaitForSeconds(0.01f);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }
}
