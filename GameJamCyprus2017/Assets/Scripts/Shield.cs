using EazyTools.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    //public GameObject powerUp;
    public float cooldownTime = 10; //How often can the powerup be used
    public float shieldDuration = 3;//How long does the shield stay activated
    public bool powerUpEnabed = true; //Is the power up available for use
    public bool powerUpActivated = false; //Is the power up currently active

    float currentTime;
    float powerUpTimePassed;
    RobotController robotController;

    public AudioClip shieldFX;

    // Use this for initialization
    void Start () {
        currentTime = Time.time; //save the current game time
        robotController = new RobotController();
    }
	
	// Update is called once per frame
	void Update () {

    }


    public void UsePowerUp()
    {
        if (powerUpEnabed)
        {
            float activationMoment = Time.time;
            powerUpActivated = true; Debug.Log("Activated power up: Shield.");
            powerUpEnabed = false;
            powerUpTimePassed = 0;

            //Set the player to not receive damage
            robotController.canReceiveDmg = false;
            //TODO: Change the emmision map  =======

            SoundManager.PlaySound(shieldFX, 1f, false, this.transform);

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
        //if the shield duration has run out
        if (powerUpTimePassed >= shieldDuration)
        {
            powerUpActivated = false;
        }
        //when the shield regenerates
        if (powerUpTimePassed >= cooldownTime)
        {
            //Set the player able to receive damage
            robotController.canReceiveDmg = true;
            //TODO: Change the emmision map  =======
            powerUpTimePassed = 0;
            powerUpEnabed = true;
            CancelInvoke();
        }        
    }

}//End of monobehaviour
