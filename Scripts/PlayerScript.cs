using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerScript : MonoBehaviour
{

	//Public
	public bool isPlayerOne = true;
	public GameObject GameScripts, LaserDiscPrefab, RaygunPrefab, MissilePrefab, PlayerShield;
	public GameObject [] Cannons;
	public Rigidbody2D rb;
	public KeyCode ShootKey = KeyCode.Space, AltShootKey = KeyCode.LeftControl, Up = KeyCode.UpArrow, AltUp = KeyCode.W, Down = KeyCode.DownArrow, AltDown = KeyCode.S, Left = KeyCode.LeftArrow, AltLeft = KeyCode.A, Right = KeyCode.RightArrow, AltRight = KeyCode.D, MissileKey = KeyCode.Q, AltMissileKey = KeyCode.E;
	public Vector2 MaximumMovementVertical = new Vector2 (3f, -10f), MaximumMovementHorizontal = new Vector2 (13f, -13f);

    //Privates
    AudioSource PowerUp;
	Coroutine ActualMovement, ActualShooting;
	GameController GCS;
	UIController UICS;
	[SerializeField]
	int LastCannon = 0, WeaponUpgradeLevel = 0, SpeedUpgradeLevel = 0, MissilesCount = 1;
	float MovementSpeed = 0.4f, WeaponDelay = 0.4f;
	bool ShieldActive = false;

	void Start ()
	{
        rb = gameObject.GetComponent<Rigidbody2D> ();
		GCS = GameScripts.GetComponent<GameController> ();
		UICS = GameScripts.GetComponent<UIController> ();
		PowerUp = gameObject.GetComponent<AudioSource> ();
		UICS.CountMissiles (MissilesCount, isPlayerOne);
	}
    void Update ()
	{
		if (!GCS.GameOver) {
			if (!GCS.GamePaused) {

                if ((Input.GetKey (ShootKey) || Input.GetKey (AltShootKey)) && (ActualShooting == null))
                    switch (WeaponUpgradeLevel)
                    {
                        case 0:
                            ActualShooting = StartCoroutine(SingleShooting());
                            break;
                        case 1:
                            ActualShooting = StartCoroutine(DoubleShooting());
                            break;
                        case 2:
                            ActualShooting = StartCoroutine(TripleShooting());
                            break;
                        case 3:
                            ActualShooting = StartCoroutine(RaygunShooting());
                            break;
                        default:
                            ActualShooting = StartCoroutine(SingleShooting());
                            break;
                    }
                if ((Input.GetKeyDown(MissileKey) || Input.GetKeyDown(AltMissileKey)) && MissilesCount > 0)
                    StartCoroutine(ShootMissile());
            }
        } else {
			rb.simulated = false;
		}
	}

	void FixedUpdate ()
	{
		if (!GCS.GamePaused) {
			Movement ();
		}
	}

    void Movement ()
	{

        if ((Input.GetKey (Left) || Input.GetKey (AltLeft)) && transform.position.x > MaximumMovementHorizontal.y)
                if ((Input.GetKey (Up) || Input.GetKey (AltUp)) && transform.position.y < MaximumMovementVertical.x)
                    rb.MovePosition (new Vector2 (transform.position.x - MovementSpeed, transform.position.y + MovementSpeed));
                else {
                    if ((Input.GetKey (Down) || Input.GetKey (AltDown)) && transform.position.y > MaximumMovementVertical.y)
                        rb.MovePosition (new Vector2 (transform.position.x - MovementSpeed, transform.position.y - MovementSpeed));
                    else
                        rb.MovePosition (new Vector2 (transform.position.x - MovementSpeed, transform.position.y));
                }
                else {
                    if ((Input.GetKey (Right) || Input.GetKey (AltRight)) && transform.position.x < MaximumMovementHorizontal.x)
                    if ((Input.GetKey (Up) || Input.GetKey (AltUp)) && transform.position.y < MaximumMovementVertical.x)
                        rb.MovePosition (new Vector2 (transform.position.x + MovementSpeed, transform.position.y + MovementSpeed));
                    else {
                        if ((Input.GetKey (Down) || Input.GetKey (AltDown)) && transform.position.y > MaximumMovementVertical.y)
                            rb.MovePosition (new Vector2 (transform.position.x + MovementSpeed, transform.position.y - MovementSpeed));
                        else
                            rb.MovePosition (new Vector2 (transform.position.x + MovementSpeed, transform.position.y));
                    }
                    else {

                        if ((Input.GetKey (Up) || Input.GetKey (AltUp)) && transform.position.y < MaximumMovementVertical.x)
                            rb.MovePosition (new Vector2 (transform.position.x, transform.position.y + MovementSpeed));
                        else if ((Input.GetKey (Down) || Input.GetKey (AltDown)) && transform.position.y > MaximumMovementVertical.y)
                            rb.MovePosition (new Vector2 (transform.position.x, transform.position.y - MovementSpeed));
                    }
                }


    }

    IEnumerator SingleShooting ()
	{
		Instantiate (LaserDiscPrefab, Cannons [LastCannon].transform.position, Quaternion.identity);

		if (LastCannon == 1)
			LastCannon = 0;
		else if (LastCannon == 0)
			LastCannon = 1;

		yield return new WaitForSeconds (WeaponDelay);
		StopCoroutine (ActualShooting);
		ActualShooting = null;
	}

	IEnumerator DoubleShooting ()
	{
		Instantiate (LaserDiscPrefab, Cannons [0].transform.position, Quaternion.identity);
		Instantiate (LaserDiscPrefab, Cannons [1].transform.position, Quaternion.identity);
		yield return new WaitForSeconds (WeaponDelay);
		StopCoroutine (ActualShooting);
		ActualShooting = null;
	}

	IEnumerator TripleShooting ()
	{
		Instantiate (LaserDiscPrefab, Cannons [0].transform.position, Quaternion.identity);
		Instantiate (LaserDiscPrefab, Cannons [1].transform.position, Quaternion.identity);
		Instantiate (LaserDiscPrefab, Cannons [2].transform.position, Quaternion.identity);
		yield return new WaitForSeconds (WeaponDelay);
		StopCoroutine (ActualShooting);
		ActualShooting = null;
	}

	IEnumerator RaygunShooting ()
	{
		GameObject Shoot = Instantiate (RaygunPrefab, new Vector3 (transform.position.x, transform.position.y + 7f, transform.position.z), Quaternion.identity);
		yield return new WaitForSeconds (0.55f);
		Shoot.GetComponentInChildren<BoxCollider2D> ().enabled = true;
		yield return new WaitForSeconds (WeaponDelay);
		StopCoroutine (ActualShooting);
		ActualShooting = null;
	}

	IEnumerator RaygunStart ()
	{
		UICS.CountRaygun (5, isPlayerOne);
		for (int i = 0; i < 501; i++)
			if (!GCS.GamePaused) {
				yield return new WaitForSeconds (0.01f);
				if (i % 100 == 0)
					UICS.CountRaygun (5 - i / 100, isPlayerOne);
			} else {
				yield return new WaitForEndOfFrame ();
				i--;
			}
		if (WeaponUpgradeLevel >= 3)
			WeaponUpgradeLevel--;
	}

	IEnumerator ShootMissile ()
	{
		Instantiate (MissilePrefab, Cannons [2].transform.position, Quaternion.identity);
		MissilesCount--;
		UICS.CountMissiles (MissilesCount, isPlayerOne);
		yield return new WaitForSeconds (0.1f);
	}

	private void OnTriggerEnter2D (Collider2D collision)
	{
        if (collision.tag == "EnemyExplosion")
            if (ShieldActive)
            {
                collision.GetComponent<CircleCollider2D>().enabled = false;
                ShieldActive = false;
                PlayerShield.SetActive(false);
            }
            else
            {
                Instantiate(GCS.PlayerExplosion, transform.position, Quaternion.identity);
                GCS.KillPlayer(isPlayerOne);
                UICS.CountMissiles(0, isPlayerOne);
                UICS.CountRaygun(0, isPlayerOne);
                gameObject.SetActive(false);
            }

            if (collision.tag == "Asteroid" || collision.tag == "EnemyAmmunition" || collision.tag == "Spinner" || collision.tag == "Reflecter" || collision.tag == "Cruiser" || collision.tag == "EnemyPlasma" || collision.tag == "Swarmer" || collision.tag == "Boss")
               if (!ShieldActive)
               {
                   Instantiate(GCS.PlayerExplosion, transform.position, Quaternion.identity);
                   GCS.KillPlayer(isPlayerOne);
                   UICS.CountMissiles(0, isPlayerOne);
                   UICS.CountRaygun(0, isPlayerOne);
                   gameObject.SetActive(false);
               }
               else
               {
                   if (collision.tag != "Boss" && collision.tag != "Player")
                   {
                       Destroy(collision.gameObject);
                       ShieldActive = false;
                       PlayerShield.SetActive(false);
                   }
               }
	}

	public void WeaponUpgrade ()
	{
		PowerUp.Play ();
		WeaponUpgradeLevel++;
		if (WeaponUpgradeLevel >= 3) {
			if (WeaponUpgradeLevel > 3) {
				StopCoroutine ("RaygunStart");
				WeaponUpgradeLevel--;
			}
			StartCoroutine ("RaygunStart");
		} else
			WeaponDelay += 0.05f;
	}

	public void SpeedUpgrade ()
	{
		PowerUp.Play ();
		if (SpeedUpgradeLevel < 4) {
			MovementSpeed += 0.07f;
			SpeedUpgradeLevel++;
		} else
			GCS.Score += 100;
	}

	public void Shield ()
	{
		PowerUp.Play ();
		PlayerShield.SetActive (true);
		ShieldActive = true;
		GCS.Score += 200;
	}

	public void Missiles ()
	{
		PowerUp.Play ();
		if (MissilesCount < 5) {
			MissilesCount++;
			UICS.CountMissiles (MissilesCount, isPlayerOne);
		} else
			GCS.Score += 50;
	}
}
