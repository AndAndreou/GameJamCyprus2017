using EazyTools.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityMine : MonoBehaviour {

    public GameObject proximityMine;
    public GameObject minespawnPoint;
    
    public float cooldownTime = 10; //How often can the powerup be used
    public bool powerUpEnabed = true; //Is the power up available for use

    public AudioClip proximityMineFX;

    float currentTime;
    float powerUpTimePassed;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UsePowerUp()
    {
        //spawn a bomb
        if (powerUpEnabed)
        {
            Instantiate(proximityMine, minespawnPoint.transform.position, Quaternion.identity);            
            powerUpEnabed = false;
            powerUpTimePassed = 0;

            SoundManager.PlaySound(proximityMineFX, 1f, false, this.transform);

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
        //when the shield regenerates
        if (powerUpTimePassed >= cooldownTime)
        {
            powerUpTimePassed = 0;
            powerUpEnabed = true;
            CancelInvoke();
        }
    }

    
}
