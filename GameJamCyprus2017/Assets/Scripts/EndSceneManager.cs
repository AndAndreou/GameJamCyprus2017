using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneManager : MonoBehaviour {

    public Transform botPoint;
    public Text playerName;

    GameObject winner;
    // Use this for initialization
    void Start () {
        winner = GameObject.FindGameObjectWithTag(GameRepository.playerTag);
        winner.transform.position = botPoint.transform.position;
        playerName.text = winner.name;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire1"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
