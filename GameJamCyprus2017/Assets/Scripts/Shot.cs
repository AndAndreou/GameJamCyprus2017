using EazyTools.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {

    public float shotSpeed;
    public float shotDamage;
    //public GameObject mainProjectile;
    public ParticleSystem mainParticleSystem;
    //public GameObject sparkParticle;
    RobotController robotController;

    [HideInInspector]
    public Transform parentTransform;

    public string playerShotBy;
    private PlayerCustomizationData playerData = null;

    public GameObject player;

    public AudioClip shotFX;
    // Use this for initialization
    void Start () {

        SoundManager.PlaySound(shotFX, 1f, false, this.transform);

        robotController = new RobotController();
        Vector3 fireAhead = this.transform.forward;
        GetComponent<Rigidbody>().velocity = fireAhead * shotSpeed;
        //mainProjectile.SetActive(true);

        playerData = GameObject.FindObjectOfType<PlayerCustomizationData>();

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        //Dead damage or not
        if (collision.transform.tag == "Player" && collision.transform != parentTransform)
        {
            
            // If other hovercraft is hit
            Debug.Log("Shot hit a player");
            //if the other player has shield power up
            if (collision.transform.GetComponent<Shield>())
            {   //if the shield was not activated, deal damage
                if ((!collision.transform.GetComponent<Shield>().powerUpActivated))
                {
                    this.GetComponent<MeshRenderer>().enabled = false;
                    collision.transform.GetComponent<RobotController>().ApplyDamage(shotDamage);
                    FindAndInscreaseScore(shotDamage);
                    Destroy(this.GetComponent<Rigidbody>());
                    Destroy(this.gameObject);
                }
            }
            else //if the other player does not have shield power up in this game
            {
                collision.transform.GetComponent<RobotController>().ApplyDamage(shotDamage);
                FindAndInscreaseScore(shotDamage);
                this.GetComponent<MeshRenderer>().enabled = false;
                Destroy(this.GetComponent<Rigidbody>());
                Destroy(this.gameObject);
            }

        }
        else
        {
            //this.GetComponent<MeshRenderer>().enabled = false;
            //Destroy(this.GetComponent<Rigidbody>());
            //Destroy(this.gameObject);
        }


        if (collision.transform != parentTransform)
        {
            // Destroy the shot gameobject
            //sparkParticle.SetActive(true);
            //sparkParticle.GetComponent<ParticleSystem>().Play();
            this.GetComponent<MeshRenderer>().enabled = false;
            Destroy(this.GetComponent<Rigidbody>());
            Destroy(this.gameObject);
        }

    }

    public void FindAndInscreaseScore(float damage)
    {
        player = GameObject.Find(playerShotBy);
        int tempNumber = player.GetComponent<RobotController>().playerNumber;
        playerData.players[tempNumber - 1].playerEntity.score += damage;
    }
}
