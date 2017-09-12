using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingLaserManager : MonoBehaviour {

	public float fireRateSeconds; // delay of fire
	//public float speed;
	public float damage = 10;
    public float fireRate = 1;
    public float bulletSpeed = 5;
	public Transform bulletSpawnPoint;
	public GameObject bulletPrefab;
	private bool bCanShoot = true;

    private int playerNumber;
    private float nextFire;
    // Use this for initialization
    void Start () {
        playerNumber = this.GetComponent<RobotController>().playerNumber;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire" + playerNumber) &&(Time.time > nextFire))
		{
			spawnBullet();
		}
	}

	public void spawnBullet() {
        nextFire = Time.time + fireRate; //Set the next available time for firing a missile

        GameObject clone = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity) as GameObject;
        //clone.GetComponent<Rigidbody>().velocity = this.transform;

			//Invoke ("enableShooting", fireRateSeconds);
		
	}

	public void enableShooting() { // Don't change this method's name
		bCanShoot = true;
	}

	public void disableShooting() {
		bCanShoot = false;
	}
}
