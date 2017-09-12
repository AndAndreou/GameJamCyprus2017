using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EazyTools.SoundManager;

public class EMP : MonoBehaviour {

    public float cooldownTime = 10; //How often can the powerup be used
    public bool powerUpEnabed = true; //Is the power up available for use
    public float empDuration = 10;//How long does the EMP stay activated
    public bool powerUpActivated = false; //Is the power up currently active
    public float erpRadious = 10;
    public bool empAffected = false;
    public GameObject empPulse;

    public AudioClip empFX;

    Rigidbody rigidbody;
    Transform transform;
    

    float currentTime;
    float powerUpTimePassed;
    GameObject[] playersArray;
    RobotController robotController;

    // Use this for initialization
    void Start () {
        currentTime = Time.time; //save the current game time
        rigidbody = GetComponentInParent<Rigidbody>();
        robotController = new RobotController();
    }

    // Update is called once per frame
    void Update()
    {
        transform = rigidbody.transform;        
    }

    public void UsePowerUp()
    {
        if (powerUpEnabed)
        {
            float activationMoment = Time.time;
            powerUpActivated = true; Debug.Log("Activated power up: EMP.");
            powerUpTimePassed = 0;
            powerUpEnabed = false;
            Vector3 raycastOrigin = gameObject.transform.position + GetComponentInParent<Rigidbody>().centerOfMass;
            // Cast a sphere wrapping character controller 10 meters forward
            // to see if it is about to hit anything.
            ExplodePulse();
            Collider[] hitColliders = Physics.OverlapSphere(raycastOrigin, erpRadious);
            //Debug.Log(raycastOrigin);
            int i = 0;
            while (i < hitColliders.Length)
            {
                //Debug.Log(hitColliders.Length);
                //do something for every collider found in the erp radious
                if ((hitColliders[i].tag == "Player")&&(hitColliders[i].transform != this.transform))
                {                    
                    //check if they player has "Hack" power up available
                    if (!hitColliders[i].gameObject.GetComponent<Hack>())
                    {

                        if (hitColliders[i].gameObject.GetComponent<EMP>()) {
                            hitColliders[i].gameObject.GetComponent<EMP>().reActivateHack();
                        }
                        if (hitColliders[i].gameObject.GetComponent<ProximityMine>())
                        {
                            hitColliders[i].gameObject.GetComponent<ProximityMine>().reActivateHack();
                        }
                        if (hitColliders[i].gameObject.GetComponent<Shield>())
                        {
                            hitColliders[i].gameObject.GetComponent<Shield>().reActivateHack();
                        }
                    }
                    else if(hitColliders[i].gameObject.GetComponent<Hack>().enabled)
                    {
                        hitColliders[i].gameObject.GetComponent<Hack>().powerUpEnabed = false;
                        hitColliders[i].gameObject.GetComponent<Hack>().reActivateHack();
                        Debug.Log("Found a hacker: " + hitColliders[i].name);
                    }
                }
                i++;
            }
            //Starting in 0.01 seconds, check the availability of the power up every second
            InvokeRepeating("CheckPowerUpAvailability", 0.01f, 1);
        }
    }

    public void reActivateHack()
    {
        powerUpEnabed = false;
        powerUpTimePassed = 0;
        InvokeRepeating("CheckPowerUpAvailability", 0.01f, 1);
    }

    void CheckPowerUpAvailability()
    {
        powerUpTimePassed++;
        //if the EMP duration has run out
        if (powerUpTimePassed >= empDuration)
        {
            powerUpActivated = false;
        }

        if (powerUpTimePassed >= cooldownTime)
        {
            //Set the player able to use power ups again
            //Find all player gameobjects and save them in an array
            playersArray = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in playersArray)
            {
                player.gameObject.GetComponent<RobotController>().canUsePowerUps = true;
            }
            powerUpTimePassed = 0;
            powerUpEnabed = true;
            CancelInvoke();
        }
    }

    public void ExplodePulse()
    {
        try
        {
            GameObject explosionPulse = Instantiate(empPulse, transform.position + new Vector3(0, 5, 0), Quaternion.identity) as GameObject;
            ParticleSystem explosionParts = explosionPulse.GetComponent<ParticleSystem>();
            float totalDuration = explosionParts.duration + explosionParts.startLifetime;
            Destroy(explosionPulse, 5);

            SoundManager.PlaySound(empFX, 1f, false, explosionPulse.transform);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

    }
}
