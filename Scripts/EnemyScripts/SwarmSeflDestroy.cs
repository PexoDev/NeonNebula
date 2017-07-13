using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmSeflDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(DestroySelf());
	}
	
	IEnumerator DestroySelf()
    {
        for (;;)
        {
            if (!gameObject.GetComponentInChildren<Rigidbody2D>())
                Destroy(gameObject);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
