using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EMPExplosion : MonoBehaviour {

    public GameObject empPulse;
    

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ExplodePulse()
    {
        try
        {
            GameObject explosionPulse = Instantiate(empPulse, transform.position, Quaternion.identity) as GameObject;
            ParticleSystem explosionParts = explosionPulse.GetComponent<ParticleSystem>();
            float totalDuration = explosionParts.duration + explosionParts.startLifetime;
            Destroy(explosionPulse, totalDuration);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
        
        //audioSource = GetComponent<AudioSource>();
        //AudioSource.PlayClipAtPoint(explosionFX, gameObject.transform.position);
        
        
    }
}
