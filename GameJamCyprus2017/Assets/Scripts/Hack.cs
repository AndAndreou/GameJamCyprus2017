using EazyTools.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hack : MonoBehaviour {

    public float cooldownTime = 10; //How often can the powerup be used
    public bool powerUpEnabed = true; //Is the power up available for use
    public bool empAffected = false;


    public AudioClip hackFX;

    float currentTime;
    float powerUpTimePassed;
    RobotController robotController;
    // Use this for initialization
    void Start () {
        robotController = GetComponent<RobotController>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UsePowerUp()
    {
        if (powerUpEnabed)
        {
            float activationMoment = Time.time;
            powerUpEnabed = false;
            powerUpTimePassed = 0;
            Debug.Log("hack");
            CheckForOtherPowerUp();

            SoundManager.PlaySound(hackFX, 1f, false, this.transform);

            //Starting in 0.01 seconds, check the availability of the power up every second
            InvokeRepeating("CheckPowerUpAvailability", 0.01f, 1);
        }

    }
    void CheckPowerUpAvailability()
    {
        powerUpTimePassed++;
        //when the shield regenerates
        if (powerUpTimePassed >= cooldownTime)
        {
            powerUpTimePassed = 0;
            powerUpEnabed = true;
            CancelInvoke();
        }

    }


    public void reActivateHack()
    {
        powerUpEnabed = false;
        powerUpTimePassed = 0;
        InvokeRepeating("CheckPowerUpAvailability", 0.01f, 1);
    }

    public void CheckForOtherPowerUp()
    {
        if (robotController.GetComponent<Shield>()){ robotController.GetComponent<Shield>().powerUpEnabed = true; }
        if (robotController.GetComponent<ProximityMine>()) { robotController.GetComponent<ProximityMine>().powerUpEnabed = true; }
        if (robotController.GetComponent<EMP>()) { robotController.GetComponent<EMP>().powerUpEnabed = true; }
    }


}
