public enum WeaponTypeValue
{
	Null = -1,
	LaserGun1 = 0,
	// Small
	PulseGun2 = 1,
	// Medium
	BeamGun3 = 2,
	// Large
	PlasmaGun4 = 3
	// Bombs
}

[System.Serializable]
public class WeaponType
{
	public WeaponTypeValue value;

	public void Next ()
	{
		if (value == WeaponTypeValue.Null) {
			value = WeaponTypeValue.LaserGun1;
		} else if (value == WeaponTypeValue.LaserGun1) {
			value = WeaponTypeValue.PulseGun2;
		} else if (value == WeaponTypeValue.PulseGun2) {
			value = WeaponTypeValue.BeamGun3;
		} else if (value == WeaponTypeValue.BeamGun3) {
			value = WeaponTypeValue.PlasmaGun4;
		} else if (value == WeaponTypeValue.PlasmaGun4) {
			value = WeaponTypeValue.LaserGun1;
		}
		return;
	}

	public void Previous ()
	{
		if (value == WeaponTypeValue.Null) {
			value = WeaponTypeValue.PlasmaGun4;
		} else if (value == WeaponTypeValue.LaserGun1) {
			value = WeaponTypeValue.PlasmaGun4;
		} else if (value == WeaponTypeValue.PulseGun2) {
			value = WeaponTypeValue.LaserGun1;
		} else if (value == WeaponTypeValue.BeamGun3) {
			value = WeaponTypeValue.PulseGun2;
		} else if (value == WeaponTypeValue.PlasmaGun4) {
			value = WeaponTypeValue.BeamGun3;
		}
		return;
	}
}
