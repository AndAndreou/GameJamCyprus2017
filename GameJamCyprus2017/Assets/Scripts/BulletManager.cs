using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {
	public void OnCollisionEnter(Collision other) {

        Destroy (gameObject);

        //TODO: damege if is player

	}
}
