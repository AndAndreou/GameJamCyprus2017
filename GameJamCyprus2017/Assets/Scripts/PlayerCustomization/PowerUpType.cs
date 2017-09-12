public enum PowerUpTypeValue
{
	Null = -1,
	Shield = 0,
	EMP = 1,
	ProximityMine = 2,
	Hack = 3
}

[System.Serializable]
public class PowerUpType
{
	public PowerUpTypeValue value;

	public void Next ()
	{

		if (value == PowerUpTypeValue.Null) {
			value = PowerUpTypeValue.Shield;
		} else if (value == PowerUpTypeValue.Shield) {
			value = PowerUpTypeValue.EMP;
		} else if (value == PowerUpTypeValue.EMP) {
			value = PowerUpTypeValue.ProximityMine;
		} else if (value == PowerUpTypeValue.ProximityMine) {
			value = PowerUpTypeValue.Hack;
		} else if (value == PowerUpTypeValue.Hack) {
			value = PowerUpTypeValue.Shield;
		}		
		return;
	}

	public void Previous ()
	{
		if (value == PowerUpTypeValue.Null) {
			value = PowerUpTypeValue.Hack;
		} else if (value == PowerUpTypeValue.Shield) {
			value = PowerUpTypeValue.Hack;
		} else if (value == PowerUpTypeValue.EMP) {
			value = PowerUpTypeValue.Shield;
		} else if (value == PowerUpTypeValue.ProximityMine) {
			value = PowerUpTypeValue.EMP;
		} else if (value == PowerUpTypeValue.Hack) {
			value = PowerUpTypeValue.ProximityMine;
		}
		return;
	}

}