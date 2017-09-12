[System.Serializable]
public class PlayerCustomizationStruct {

	public PlayerType playerType;
	public WeaponType weaponType;
	public PowerUpType powerUp1Type;
	public PowerUpType powerUp2Type;
	public PlayerEntityStruct playerEntity;

	public PlayerCustomizationStruct() {
		playerType = new PlayerType ();
		weaponType = new WeaponType ();
		powerUp1Type = new PowerUpType ();
		powerUp2Type = new PowerUpType ();
		playerEntity = new PlayerEntityStruct ();

		playerType.value = PlayerTypeValue.Null;
		weaponType.value = WeaponTypeValue.Null;
		powerUp1Type.value = PowerUpTypeValue.Null;
		powerUp2Type.value = PowerUpTypeValue.Null;
	}
}
