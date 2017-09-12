using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour {

    public Animator fadeOutAnimator;

	// Use this for initialization
	void Start () {
		
	}
	
    public void DoFadeOut()
    {
        fadeOutAnimator.SetTrigger("DoFadeOut");
    }

    public void FadeOutHasBeFinish()
    {
        SceneManager.LoadScene("EndScene");
        RobotController[] players = GameObject.FindObjectsOfType<RobotController>();
        foreach(RobotController p in players)
        {
            if(p.isAlive == true)
            {
                DontDestroyOnLoad(p.gameObject);
                break;
            }
        }
    }
}
