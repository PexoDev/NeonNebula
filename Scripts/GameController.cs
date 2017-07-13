using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    
    //Publics
    public bool GameOver = false, GamePaused = false, BossDestroyable = false;
    public GameObject Player1,Player2, EnemyPrefab, GameScripts, PlayerExplosion, MissileExplosion, EnemyMissileExplosion;
    public GameObject[] EnemyAmmunition, EnemyExplosions, Enemies, Collectables;
    public int Score=0, StarsCollected = 0;
    public Camera Camera;
    public int Difficulty = 0;
    public int PlayersAlive = 1;
    //Privates
    GameObject Boss;
    Coroutine BossMovement;
    UIController UCS;
    HighestScoreScript HSS;
    PlayerScript Players1Script, Players2Script;
    Renderer BlackScreenRenderer;
    bool RandomSpawning = true, AsteroidSpawning = true, GameStopped = false, BossSpawned = false;

    public void KillPlayer(bool isPlayerOne)
    {
        PlayersAlive--;
        if (PlayersAlive < 1)
            StartCoroutine(GameOverFunction());
        if (!isPlayerOne)
            Player1 = Player2;
        else
            Player2 = Player1;
    }
    void Start () {
        UCS = GameScripts.GetComponent<UIController>();
        HSS = GameScripts.GetComponent<HighestScoreScript>();
        BlackScreenRenderer = UCS.BlackScreen.GetComponent<Renderer>();
        Players1Script = Player1.GetComponent<PlayerScript>();
        Players2Script = Player2.GetComponent<PlayerScript>();
        ParticleLimits();
        UCS.StarsCountTextChange(0);
        StartCoroutine("StartGame");
        StartCoroutine("ScoreChange");
    }
	void Update () {   
        if (!GameOver&&!GameStopped)
            if (Input.GetKeyDown(KeyCode.Escape))
                UCS.PauseMenuFunction(GamePaused);
	}
    public IEnumerator GameOverFunction()
    {
        GameOver = true;
        StartCoroutine(UCS.GameOverMenuFunction(HSS.SaveHighestScore(Score)));
        yield return new WaitForSeconds(0.1f);
    }
    public void DestroyBoss()
    {
        StartCoroutine(KillBoss());
    }
    IEnumerator KillBoss()
    {
        BossDestroyable = false;
        StopCoroutine(BossMovement);
        Destroy(Boss);
        Vector3 Distance = new Vector3(Player1.transform.position.x-5, Player1.transform.position.y + 7f, 0f); 
        Vector3 Distance2 = new Vector3(Player2.transform.position.x + 5, Player2.transform.position.y + 7f, 0f);
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < 201; i++)
        { 
            Player1.transform.position = Player1.transform.position - Distance / 200;
            if(PlayersAlive>1)
                Player2.transform.position = Player2.transform.position - Distance2 / 200;
            Camera.orthographicSize -= 0.025f;
            yield return new WaitForSeconds(0.005f);
       }
        Players1Script.MaximumMovementHorizontal = new Vector2(25, -25);
        Players1Script.MaximumMovementVertical = new Vector2(3, -15);
        Players2Script.MaximumMovementHorizontal = new Vector2(25, -25);
        Players2Script.MaximumMovementVertical = new Vector2(3, -15);
        BossSpawned = false;
        UCS.StarsCountTextChange(StarsCollected);
        Difficulty++;
        StopGame(false);
        yield return new WaitForSeconds(0.5f);
        RandomSpawning = true;
    }
    IEnumerator SpawnBoss()
    {
        RandomSpawning = false;
        StarsCollected = 0;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }
        GamePaused = true;
        GameStopped = true;
        Boss = Instantiate(Enemies[5], new Vector3(0, 35f, 0f), Quaternion.identity);
        Vector3 Distance = new Vector3 (Player1.transform.position.x-5f,Player1.transform.position.y + 14f,0f);
        Vector3 Distance2 = new Vector3(Player2.transform.position.x+5f, Player2.transform.position.y + 14f, 0f);
        for (int i = 0; i < 201; i++)
        {
            Boss.transform.position = new Vector3(0f, Boss.transform.position.y - 0.125f, 0f);
            Player1.transform.position = Player1.transform.position - Distance / 200;
            if (PlayersAlive > 1)
                Player2.transform.position = Player2.transform.position - Distance2 / 200;
            Camera.orthographicSize += 0.025f;
            yield return new WaitForSeconds(0.005f);
        }
        Players1Script.MaximumMovementHorizontal = new Vector2(31,-31);
        Players1Script.MaximumMovementVertical = new Vector2(-5,-20);
        Players2Script.MaximumMovementHorizontal = new Vector2(31, -31);
        Players2Script.MaximumMovementVertical = new Vector2(-5, -20);
        yield return new WaitForSeconds(0.5f);
        BossMovement = StartCoroutine(Boss.GetComponent<BossScript>().Movement());
        BossSpawned = true;
        GamePaused = false;
        GameStopped = false;
    }
    public GameObject DropCollectable(float Percentage, Vector3 SpawnPosition)
    {
        
        float rand = Random.Range(0f, 1f);
        if (rand < Percentage*1.2)
            return Instantiate(SpawnCollectable(), SpawnPosition, Quaternion.identity);
        else
            return null;
    }
    public void StarCollected()
    {
        StarsCollected++;
        UCS.StarsCountTextChange(StarsCollected);
        if (StarsCollected > 3)
            StartCoroutine(SpawnBoss());
    }
    public void StopGame(bool Stop)
    {
        if (Stop)
        {
            GameStopped = true;
            GamePaused = true;
        }
        else
        {
            GameStopped = false;
            GamePaused = false;
        }
    }
    GameObject SpawnEnemy(int ID)                       /*0Asteroid;1Cruiser;2Spinner;3Reflecter;4Swarmer*/
    {
        GameObject TMP;
        if (BossSpawned)
         TMP = Instantiate(Enemies[ID], new Vector3((Random.Range(-10f, 10f)), 22f, 0), Quaternion.identity);
        else
         TMP = Instantiate(Enemies[ID], new Vector3((Random.Range(-10f, 10f)), 15f, 0), Quaternion.identity);

        TMP.transform.SetParent(gameObject.transform);
        return TMP;
    }
    GameObject SpawnCollectable(int ID = -1)            /*0StarKey;1WeaponSpeedUpgrade;2WeaponUpgrade;3Shield;4Missiles*/
    {
        if (ID != -1)
            return Collectables[ID];
        else
        {
            int rand;
            if (Score > 1000+StarsCollected*1500 && !BossSpawned)
            rand = (int)Random.Range(0f, 5f);
            else
            rand = (int)Random.Range(1f, 5f);

            switch (rand)
            {
                case 0:
                    return Collectables[0];
                case 1:
                    return Collectables[1];
                case 2:
                    return Collectables[2];
                case 3:
                    return Collectables[3];
                case 4:
                    return Collectables[4];
                default:
                    return Collectables[0];

            }
        }
    }
    IEnumerator StartGame()
    {
        
            UCS.BlackScreen.SetActive(true);
            while (BlackScreenRenderer.material.color.a > 0)
            {
                BlackScreenRenderer.material.color = new Color(0, 0, 0, BlackScreenRenderer.material.color.a - 0.05f);
                yield return new WaitForSeconds(0.01f);
            }
            UCS.BlackScreen.SetActive(false);
            UCS.MainText("3");

            for (int i = 0; i < 11; i++)
                if (!GamePaused)
                    yield return new WaitForSeconds(0.1f);
                else
                {
                    yield return new WaitForEndOfFrame();
                    i--;
                }
            UCS.MainText("2");
            for (int i = 0; i < 11; i++)
                if (!GamePaused)
                    yield return new WaitForSeconds(0.1f);
                else
                {
                    yield return new WaitForEndOfFrame();
                    i--;
                }
        UCS.MainText("1");
            for (int i = 0; i < 11; i++)
                if (!GamePaused)
                    yield return new WaitForSeconds(0.1f);
                else
                {
                    yield return new WaitForEndOfFrame();
                    i--;
                }
        UCS.MainText("");
            StartCoroutine("EnemySpawner");
            StartCoroutine("AsteroidSpawner");
            for (int i = 0; i < 41; i++)
                if (!GamePaused)
                    yield return new WaitForSeconds(0.1f);
                else
                {
                    yield return new WaitForEndOfFrame();
                    i--;
                }
        StartCoroutine("EnemySpawner");
    }
    IEnumerator EnemySpawner()
    {
        float WaitTime = 8f;
        for (;!GameOver;)
        {
            if (GamePaused || !RandomSpawning)
                yield return new WaitForEndOfFrame();
            else
            {
                SpawnEnemy((int)Random.Range(1f, 5f));
                for (int i = 0; i < 101; i++)
                    if (GamePaused)
                    {
                      i--;
                      yield return new WaitForEndOfFrame();
                    }
                    else
                        yield return new WaitForSeconds(WaitTime / 100);
                if (WaitTime > 4f)
                    WaitTime -= 0.05f;
            }
        }
        yield return new WaitForEndOfFrame();
    }
    IEnumerator AsteroidSpawner()
    {
        float WaitTime = 6.5f;
        for (; !GameOver;)
        {
            if (GamePaused || !AsteroidSpawning)
                yield return new WaitForEndOfFrame();
            else
            {

                GameObject tmp = SpawnEnemy(0);
                AsteroidScript tmpasteroid = tmp.GetComponent<AsteroidScript>();
                switch ((int)Random.Range(1f, 101f))
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                        tmp.transform.localScale = new Vector3(1f, 1f, 1f);
                            tmpasteroid.HP = 4;
                            tmpasteroid.Score = 50;
                            tmpasteroid.ExplosionScale = 2f;
                            break;
                    case 11:
                            tmp.transform.localScale = new Vector3(2f, 2f, 2f);
                            tmpasteroid.HP = 6;
                            tmpasteroid.Score = 250;
                            tmpasteroid.ExplosionScale = 3f;
                            break;
                    default:
                        break;
                }
                for(int i=0;i<101;i++)
                    if(GamePaused)
                    {
                        i--;
                        yield return new WaitForEndOfFrame();
                    }
                else
            yield return new WaitForSeconds(WaitTime/100);
                if (WaitTime > 2f)
                    WaitTime -= 0.05f;
            }
        }
        yield return new WaitForEndOfFrame();
    }
    IEnumerator ScoreChange()
    {
        for (;!GameOver;)
        {
            UCS.ScoreTextChange(Score.ToString());
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(0.2f);
    }
    void ParticleLimits()
    {
            ParticleSystem[] PartExpl = new ParticleSystem[EnemyExplosions.Length - 1];
            ParticleSystem[] PartExpl2 = new ParticleSystem[EnemyExplosions.Length - 1];
            ParticleSystem[] PartExpl3 = new ParticleSystem[8];
            ParticleSystem.MainModule[] MainExpl = new ParticleSystem.MainModule[EnemyExplosions.Length - 1];
            ParticleSystem.MainModule[] MainExpl2 = new ParticleSystem.MainModule[EnemyExplosions.Length - 1];
            ParticleSystem.MainModule[] MainExpl3 = new ParticleSystem.MainModule[8]; //0Player;1Missile;2RaygunA;3RaygunB;5Asteroid;4EnemyMissile
            for (int i = 1; i < PartExpl.Length+1; i++)
                {
                    PartExpl[i-1] = EnemyExplosions[i].GetComponent<ParticleSystem>();
                    PartExpl2[i-1] = EnemyExplosions[i].transform.GetChild(0).GetComponent<ParticleSystem>();
                }
            for (int i = 0; i < MainExpl.Length; i++)
                {
                    MainExpl[i] = PartExpl[i].main;
                    MainExpl2[i] = PartExpl2[i].main;
                }

        PartExpl3[0] = PlayerExplosion.GetComponent<ParticleSystem>();
        PartExpl3[1] = MissileExplosion.GetComponent<ParticleSystem>();
        PartExpl3[2] = Players1Script.RaygunPrefab.transform.GetChild(0).GetComponent<ParticleSystem>(); 
        PartExpl3[3] = Players1Script.RaygunPrefab.transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystem>();
        PartExpl3[4] = EnemyMissileExplosion.GetComponent<ParticleSystem>();
        PartExpl3[5] = EnemyExplosions[0].GetComponent<ParticleSystem>();
        PartExpl3[6] = Players2Script.RaygunPrefab.transform.GetChild(0).GetComponent<ParticleSystem>();
        PartExpl3[7] = Players2Script.RaygunPrefab.transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystem>();

        MainExpl3[0] = PartExpl3[0].main;
        MainExpl3[1] = PartExpl3[1].main;
        MainExpl3[2] = PartExpl3[2].main;
        MainExpl3[3] = PartExpl3[3].main;
        MainExpl3[4] = PartExpl3[4].main;
        MainExpl3[5] = PartExpl3[5].main;
        MainExpl3[6] = PartExpl3[6].main;
        MainExpl3[7] = PartExpl3[7].main;

        switch (GlobalSettings.GraphicsQuality)
            {
                case 0:
                for (int i = 0; i < PartExpl.Length; i++)
                {
                    MainExpl[i].maxParticles = 400;
                    MainExpl2[i].maxParticles = 40;
                }
                for (int i = 0; i < 4; i++)
                    MainExpl3[i].maxParticles = 400;
                MainExpl3[4].maxParticles = 40;
                MainExpl3[5].maxParticles = 40;
                MainExpl3[6].maxParticles = 400;
                MainExpl3[7].maxParticles = 40;
                break;
                case 1:
                    for (int i = 0; i < PartExpl.Length; i++)
                    {
                    MainExpl[i].maxParticles = 100;
                    MainExpl2[i].maxParticles = 15;
                    }
                    for (int i = 0; i < 5; i++)
                    MainExpl3[i].maxParticles = 100;
                    MainExpl3[5].maxParticles = 10;
                    MainExpl3[6].maxParticles = 100;
                    MainExpl3[7].maxParticles = 10;
                break;
                case 2:
                    for (int i = 0; i < MainExpl.Length; i++)
                    {
                    MainExpl[i].maxParticles = 10;
                    MainExpl2[i].maxParticles = 1;
                    }
                    for (int i = 0; i < 5; i++)
                    MainExpl3[i].maxParticles = 30;
                    MainExpl3[5].maxParticles = 5;
                    MainExpl3[6].maxParticles = 30;
                    MainExpl3[7].maxParticles = 5;
                break;
            }
    }
}
