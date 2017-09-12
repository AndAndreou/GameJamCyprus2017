using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingGrenadeManager : MonoBehaviour {

	public float fireRateSeconds; // delay of fire
	public float speed;
	public float damage;
	public Transform bulletSpawnPoint;
	public GameObject bulletPrefab;
	private bool bCanShoot = true;

	public float flyTimeInSeconds;
	public float bombTimeInSeconds;

    private int playerNumber;
    // Use this for initialization
    void Start () {
        playerNumber = this.GetComponent<RobotController>().playerNumber;
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire" + playerNumber))
		{
			spawnBullet ();
		}
	}

	public void spawnBullet() {
		if (bCanShoot) {
			bCanShoot = false;

			GameObject clone;
			clone = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity) as GameObject;
			GrenadeManager gm = clone.GetComponent<GrenadeManager> ();
			gm.flyTimeInSeconds = flyTimeInSeconds;
			gm.bombTimeInSeconds = bombTimeInSeconds;
			Rigidbody rb = clone.GetComponent<Rigidbody> ();
			rb.velocity = transform.TransformDirection(Vector3.forward * speed);

			Invoke ("enableShooting", fireRateSeconds);
		}
	}

	public void enableShooting() { // Don't change this method's name
		bCanShoot = true;
	}

	public void disableShooting() {
		bCanShoot = false;
	}
}
