﻿using UnityEngine;
using System.Collections;

public class CarDestroyerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CarScript>() != null)
            Destroy(other.gameObject);
    }
}
