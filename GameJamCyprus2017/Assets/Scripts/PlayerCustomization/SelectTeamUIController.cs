//using EazyTools.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectTeamUIController : MonoBehaviour {

    [Header("GameObjects")]
    public GameObject loadingGO;
    public GameObject instructionsGO;

    [Header("Animators")]
    public Animator[] controllers = new Animator[4];

    [Header("UIComponent")]
    public Text[] numberOfControllers = new Text[4];

    [Header("Colors")]
    public Color readyColor;
    public Color unReadyColor;

    [Header("Audio")]
    public AudioClip backgroundMusic;

    private int[] teamOfEachController = new int[4];
    private bool[] playerReadyBasic = new bool[4];
	private bool[] playerReadyPowerUp1 = new bool[4];
	private bool[] playerReadyPowerUp2 = new bool[4];

    private float volume;

    private float[] debounceHorizontal = new float[4];
	private float[] debounceVertical = new float[4];
    private float repeat = 0.15f;  // reduce to speed up auto-repeat input

	public PlayerCustomizationData playerData = null;
	public GameObject[] playerCustomizationsUI;
	public PowerUpsUI powerUpsUI;
	public WeaponTypesUI weaponTypesUI;
	public PlayerGunsUI[] playerGunsUI;

    private bool callOtherScene;
    // Use this for initialization
    void Start () {
		playerData = GameObject.FindObjectOfType<PlayerCustomizationData> ();

        callOtherScene = false;

        volume = PlayerPrefs.GetFloat("Volume", 1f);
        //SoundManager.PlayMusic(backgroundMusic, volume, true, true, 2f, 1f);
    }
	
	// Update is called once per frame
	void Update () {

        if(callOtherScene == true)
        {
            return;
        }

        ReadInputs();
		UpdateUI ();
        CheckIfAllReadyOrGoBack();
    }

	private void UpdateUI() {
		int curIndex = 0;
		foreach (GameObject go in playerCustomizationsUI) {
			foreach (Transform t in go.transform) {
				if (t.name == "Title") {
					//t.GetComponent<Text> ().text = "Player " + (curIndex + 1);
					t.GetComponent<Text> ().text = playerData.players [curIndex].playerEntity.nickname;
				} else if (t.name == "WeaponTypeText") {
					t.GetComponent<Text> ().text = "WeaponType " + playerData.players [curIndex].weaponType.value;
				} else if (t.name == "WeaponTypeImage") {
					int weaponIndex = (int)playerData.players [curIndex].weaponType.value;
					//Debug.Log (weaponIndex);
					t.GetComponent<Image> ().sprite = weaponTypesUI.textures [weaponIndex];
					if (curIndex < playerGunsUI.Length) {
						for (int j = 0; j < playerGunsUI [curIndex].gunsLeftHand.Length; j++) {
							if (j == weaponIndex) {
								//Debug.Log ("Setting player: " + curIndex + " weapon: " + j + " value: " +
								//(int)(playerData.players [curIndex].weaponType.value) + " to true");
								playerGunsUI [curIndex].gunsLeftHand [j].SetActive (true);
								playerGunsUI [curIndex].gunsRightHand [j].SetActive (true);
							} else {
								//Debug.Log ("Setting player: " + curIndex + " weapon: " + j + " value: " +
								//(int)(playerData.players [curIndex].weaponType.value) + " to false");
								playerGunsUI [curIndex].gunsLeftHand [j].SetActive (false);
								playerGunsUI [curIndex].gunsRightHand [j].SetActive (false);
							}
						}
					}
				} else if (t.name == "PowerUpType1Text") {
					t.GetComponent<Text> ().text = "PowerUpType1 " + playerData.players [curIndex].powerUp1Type.value;
				} else if (t.name == "PowerUpType1Image") {
					t.GetComponent<Image> ().sprite = powerUpsUI.textures [(int) playerData.players [curIndex].powerUp1Type.value];
				} else if (t.name == "PowerUpType2Text") {
					t.GetComponent<Text> ().text = "PowerUpType2 " + playerData.players [curIndex].powerUp2Type.value;
				} else if (t.name == "PowerUpType2Image") {
					t.GetComponent<Image> ().sprite = powerUpsUI.textures [(int) playerData.players [curIndex].powerUp2Type.value];
				}
			}
			curIndex++;
		}
	}

    private void ReadInputs()
    {
        for(int i = 0; i < teamOfEachController.Length; i++)
        {

            //ready button
            bool readyInput = Input.GetButtonDown("Fire" + (i + 1).ToString());
            if((readyInput == true))
            {
				if (playerReadyBasic [i] == false && playerData.players[i].weaponType.value != WeaponTypeValue.Null) {
					playerReadyBasic[i] = true;  
				} else if (playerReadyPowerUp1 [i] == false && playerData.players[i].powerUp1Type.value != PowerUpTypeValue.Null) {
					playerReadyPowerUp1[i] = true;  
				} else if (playerReadyPowerUp2 [i] == false && playerData.players[i].powerUp2Type.value != PowerUpTypeValue.Null && 
					(playerData.players[i].powerUp1Type.value != playerData.players[i].powerUp2Type.value) ) {
					playerReadyPowerUp2[i] = true;  
				}
            }

            //back button
            bool backInput = Input.GetButtonDown("PowerUpFirst" + (i + 1).ToString());
            if (backInput == true)
            {
				if (playerReadyPowerUp2 [i] == true) {
					playerReadyPowerUp2[i] = false; 
				} else if (playerReadyPowerUp1 [i] == true) {
					playerReadyPowerUp1[i] = false; 
				} else if (playerReadyBasic [i] == true) {
					playerReadyBasic[i] = false;  
				}
            }

            float horizontalInput = Input.GetAxis("Horizontal" + (i+1).ToString());
			float verticalInput = Input.GetAxis("Vertical" + (i+1).ToString());

			//Debug.Log("Player: " + (i + 1) + "Horizontal: " + horizontalInput);

            // BEGIN Debounce the input
            if (Mathf.Abs(horizontalInput) < 0.1f) { debounceHorizontal[i] = 0.0f; }
            else { debounceHorizontal[i] += Time.deltaTime; }
            if (debounceHorizontal[i] < repeat) { horizontalInput = 0; }
            else { debounceHorizontal[i] = 0; }

			if (Mathf.Abs(verticalInput) < 0.1f) { debounceVertical[i] = 0.0f; }
			else { debounceVertical[i] += Time.deltaTime; }
			if (debounceVertical[i] < repeat) { verticalInput = 0; }
			else { debounceVertical[i] = 0; }
            // END Debounce the input

            if (horizontalInput != 0)
            {
                if (horizontalInput > 0.5f)
                {
					if (playerReadyBasic [i] == false) {
						playerData.players [i].weaponType.Next ();
					}
                }
                else if (horizontalInput < -0.5f)
                {
					if (playerReadyBasic [i] == false) {
						playerData.players [i].weaponType.Previous ();
					}
                }
            } 
			if (verticalInput != 0) {
				if (verticalInput > 0.5f) {
					if (playerReadyPowerUp1 [i] == true) {
						playerData.players [i].powerUp2Type.Next ();
					} else if (playerReadyBasic [i] == true) {
						playerData.players [i].powerUp1Type.Next ();
					}
				} else if (verticalInput < -0.5f) {
					if (playerReadyPowerUp1 [i] == true) {
						playerData.players [i].powerUp2Type.Previous ();
					} else if (playerReadyBasic [i] == true) {
						playerData.players [i].powerUp1Type.Previous ();
					}
				}
			}
        }
        //SetUIControllersAnimator();
        //Input.ResetInputAxes();
    }

    private void SetUIControllersAnimator()
    {
        for (int i = 0; i < teamOfEachController.Length; i++)
        {
            controllers[i].SetInteger("Team", teamOfEachController[i]);
			if (playerReadyBasic[i] == true)
            {
                numberOfControllers[i].color = readyColor;
            }
            else
            {
                numberOfControllers[i].color = unReadyColor;
            }
        }
    }

    private void CheckIfAllReadyOrGoBack()
    {
        bool isAllReady = true;
        bool isAllUnReady = true;
        int teamsCounter = 0;
        
        if (!UnityEngine.Debug.isDebugBuild) 
        {
            // Check all 4 players if ready
			for (int i = 0; i < playerReadyBasic.Length; i++)
            {
				isAllReady = isAllReady && playerReadyBasic[i] && playerReadyPowerUp1[i] && playerReadyPowerUp2[i];
				isAllUnReady = isAllUnReady && !playerReadyBasic[i] && !playerReadyPowerUp1[i] && !playerReadyPowerUp1[i];
                teamsCounter += teamOfEachController[i];
            }
        }
        else
        {
            // To allow playing in debug mode with connected controllers
            for (int i = 0; i < UnityEngine.Input.GetJoystickNames().Length; i++)
            {
				isAllReady = isAllReady && playerReadyBasic[i] && playerReadyPowerUp1[i] && playerReadyPowerUp1[i];
				isAllUnReady = isAllUnReady && !playerReadyBasic[i] && !playerReadyPowerUp1[i] && !playerReadyPowerUp1[i];
                teamsCounter = 0;
            }
        }

        if (isAllReady == true)
        {
            if(teamsCounter == 0) //means teams is balance
            {
                //callOtherScene = true;
                //instructionsGO.SetActive(false);
                //loadingGO.SetActive(true);
                //StartCoroutine(GoNextScene());
                SceneManager.LoadScene("Gameplay");
            }
            return;
        }

    }

    private IEnumerator GoNextScene()
    {
        //blue team is -1
        //red team is 1
        //none team is 0

        PlayerPrefs.SetString("FromScene", "SelectTeam"); // To tell if we came from select team scene

        for (int i = 0; i < teamOfEachController.Length; i++)
        {
            PlayerPrefs.SetInt("Controller" + i, teamOfEachController[i]);
        }
        
        SceneManager.LoadSceneAsync("newMergeScene4");
        yield return null;
    }

    private void GoPrevScene()
    {
        SceneManager.LoadScene("SelectLevel");
    }

}
