using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighscoreManager : MonoBehaviour {

	public GameObject appServicesController;
	public Button getHighscoresButton;

	// Use this for initialization
	void Start () {
		Invoke ("LoadHighscores", 2f);
		//getHighscoresButton.gameObject.SetActive (false);

	}

    private void Update()
    {
        if (Input.GetButton("PowerUpFirst1"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void LoadHighscores() {
		appServicesController.GetComponent<HighscoresDemo2> ().GetTopHighscores ();
	}

}
