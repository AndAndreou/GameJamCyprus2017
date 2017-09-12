using UnityEngine;
using UnityEngine.UI;

public class ButtonImageManager : MonoBehaviour {

	public Sprite buttonFocused;
	public Sprite buttonUnFocused;

	private Image buttonImage;

	private void Start()
	{

		buttonImage = this.GetComponent<Image> ();
		UnFocus ();
	}

	public void Focus() {
		buttonImage.sprite = buttonFocused;
	}

	public void UnFocus() {
		if (buttonImage != null && buttonUnFocused != null) {
			buttonImage.sprite = buttonUnFocused;
		}
	}
}
