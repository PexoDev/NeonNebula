using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaygunScript : MonoBehaviour {
    public bool isPlayerOne;
    GameObject Player;
    GameController GCS;
	void Start () {
        GCS = GameObject.FindGameObjectWithTag("GameScripts").GetComponent<GameController>();
        if (isPlayerOne)
            Player = GCS.Player1;
        else
            Player = GCS.Player2;
    }
	void Update () {
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + 7f, Player.transform.position.z);
	}
}
