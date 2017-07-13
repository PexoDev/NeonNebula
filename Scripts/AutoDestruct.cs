using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour {
    GameController GCS;
    public float TimeOfLiving = 1f;
	void Start () {
        GCS = GameObject.FindGameObjectWithTag("GameScripts").GetComponent<GameController>();
        StartCoroutine(DestroySelf());
    }
IEnumerator DestroySelf()
    {
        for (int i = 0; i < 201; i++)
            if (GCS.GamePaused)
            {
                i--;
                yield return new WaitForEndOfFrame();
            }
        else
        yield return new WaitForSeconds(TimeOfLiving/200);
        Destroy(gameObject);
    }
}
