using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NicknameSettingManager : MonoBehaviour {

	public string[] nicknames;
	public InputField nicknameInput;

    public PlayerCustomizationData playerCustomizationData;

    private int currentNickname = 0;
	private int maxPlayers = 4;

	// Use this for initialization
	void Start () {
		ResetScreen ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)) {
			SetNickname ();
		}
	}

	public void ResetScreen () {
		nicknames = new string[maxPlayers];
		currentNickname = 0;
		UpdateUI ();
	}

	public void SetNickname () {
		string nickname = nicknameInput.text.ToUpper();
		// Validations
		if (nickname.Trim () == "") {
			UpdateUI ();
			return;
		}
		if (currentNickname < maxPlayers) {
			nicknames [currentNickname] = nickname;
			currentNickname++;
			if (currentNickname == maxPlayers) {
				MoveToNextScene ();
			}
			UpdateUI ();
		}
	}

	public void UpdateUI () {
		nicknameInput.placeholder.GetComponent<Text> ().text = string.Format ("Give player {0} nickname", currentNickname + 1);
		nicknameInput.text = ""; // To clear the previous nickname
		nicknameInput.Select(); // To focus on the input field
		nicknameInput.ActivateInputField();
	}

	private void MoveToNextScene() {
		Debug.Log ("Move to next scene");
        nicknameInput.gameObject.SetActive(false);
        playerCustomizationData.GetNicknamesFromNicknameManager();
        SceneManager.LoadScene("PlayerCustomization");
    }
}
