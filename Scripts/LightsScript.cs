using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsScript : MonoBehaviour {

    //Public

    //Privates
    Light Light;
    void Start ()
    {
        Light = gameObject.GetComponent<Light>();
        StartCoroutine(Flickering());
    }
   
    IEnumerator Flickering()
    {
        for (;;)
        {
            for (int i = 0; i < 21; i++)
            {

                    Light.range -= 0.1f;
                    yield return new WaitForSeconds(0.05f);
            }

            for (int i = 0; i < 21; i++)
            {
                    Light.range += 0.1f;
                    yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
