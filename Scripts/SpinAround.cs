using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAround : MonoBehaviour {
    public float RotateSpeed = -1f;
    GameController GCS;
	// Use this for initialization
	void Start () {
        GCS = GameObject.FindGameObjectWithTag("GameScripts").GetComponent<GameController>();

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!GCS.GamePaused)
            transform.Rotate(new Vector3(0f,0f,RotateSpeed));
    }
}
