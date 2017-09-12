using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeManager : MonoBehaviour {
	public float flyTimeInSeconds;
	public float bombTimeInSeconds;

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		Invoke ("Fall", flyTimeInSeconds);	
		Invoke ("Explode", flyTimeInSeconds + bombTimeInSeconds);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void Explode () {
		// TODO: Show particle effect
		// TODO: Destroy surrounding players
		Destroy (gameObject);
	}

	private void Fall () {
		rb.constraints = RigidbodyConstraints.None;
	}
}
