using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCustomizationData : MonoBehaviour {

	public PlayerCustomizationStruct[] players = new PlayerCustomizationStruct[4];

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GetNicknamesFromNicknameManager() {
		NicknameSettingManager nickMgr = GameObject.FindObjectOfType<NicknameSettingManager> ();
		int curIndex = 0;
		foreach (string nickname in nickMgr.nicknames) {
			players [curIndex].playerEntity.nickname = nickname;
            players[curIndex].playerEntity.score = 0;
            // Initialise the data
            players [curIndex].weaponType.value = WeaponTypeValue.LaserGun1;
			players [curIndex].powerUp1Type.value = PowerUpTypeValue.Shield;
			players [curIndex].powerUp2Type.value = PowerUpTypeValue.EMP;
			curIndex++;
		}
	}
}
