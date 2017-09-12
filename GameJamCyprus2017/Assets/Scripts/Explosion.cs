using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    
    public GameObject mineExplosion;
    
    public float damage = 100f;
    public float secondsToArmMine = 3f;
    public bool armed = false;
    public AudioClip explosionFX;
    public AudioSource audioSource;

    // Use this for initialization
    void Start () {
        StartCoroutine(Spawn());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(secondsToArmMine);
        armed = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("object entered");
        if (other.tag == "Player" && armed)
        {
            //Debug.Log("object was a player");
            Explode();
            other.gameObject.GetComponent<RobotController>().ApplyDamage(damage);            
            Destroy(gameObject);
        }
    }

    public void Explode()
    {
        GameObject explosion = Instantiate(mineExplosion, gameObject.transform.position, Quaternion.identity) as GameObject;
        audioSource = GetComponent<AudioSource>();
        AudioSource.PlayClipAtPoint(explosionFX, gameObject.transform.position);
        ParticleSystem explosionParts = explosion.GetComponent<ParticleSystem>();
        float totalDuration = explosionParts.duration + explosionParts.startLifetime;
        Destroy(explosion, totalDuration);
    }


}
