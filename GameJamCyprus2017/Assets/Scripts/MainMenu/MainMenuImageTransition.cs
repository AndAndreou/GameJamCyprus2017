using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuImageTransition : MonoBehaviour {

	public Image backgroundWithoutButtons;
	public Image backgroundForButtons;
	public float showStartScreenTime;
	public float transitionTime;
	private bool bIgnoreTimescale = false;

	// Use this for initialization
	void Start () {
		Invoke ("HideBackgroudnWithoutButtons", showStartScreenTime);
		backgroundForButtons.canvasRenderer.SetAlpha( 0.0f );
		backgroundForButtons.CrossFadeAlpha(1f, showStartScreenTime + transitionTime, bIgnoreTimescale);
	}

	void HideBackgroudnWithoutButtons() {
		backgroundWithoutButtons.CrossFadeAlpha(0f, transitionTime, bIgnoreTimescale);
	}
}
