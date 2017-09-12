using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollector : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Object: " + other.name + " exited the GB and got destroyed.");
        Destroy(other.gameObject);
        
    }
}
