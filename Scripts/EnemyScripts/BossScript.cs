using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript: MonoBehaviour {
    public GameObject Shield;
    GameController GCS;
    Rigidbody2D rb;
    bool MovingLeft = false, MovingUp = false;
    float Y = 0;
    int ModulesAlive = 4;
    void Start()
    {
        GCS = GameObject.FindGameObjectWithTag("GameScripts").GetComponent<GameController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public void ModuleDestroyed()
    {
        ModulesAlive--;
        if (ModulesAlive < 1)
        { Shield.SetActive(false);
            GCS.BossDestroyable = true;
        }
    }

   
   public IEnumerator Movement()
    {
        for (;;)
        {
            if (!GCS.GamePaused)
            {

                if ((transform.position.x < -29 && MovingLeft) || (!MovingLeft && transform.position.x > 29))
                    MovingLeft = !MovingLeft;
                if ((transform.position.y < -5 && !MovingUp) || (transform.position.y >= 12 && MovingUp))

                    MovingUp = !MovingUp;
                if (MovingUp)
                    Y = transform.position.y + 0.05f;
                else
                    Y = transform.position.y - 0.05f;

                if (MovingLeft)
                    rb.MovePosition(new Vector2(transform.position.x - 0.1f, Y));
                else
                    rb.MovePosition(new Vector2(transform.position.x + 0.1f, Y));
                yield return new WaitForSeconds(0.01f);
            }
            else
                yield return new WaitForEndOfFrame();
        }
    }
  
}
