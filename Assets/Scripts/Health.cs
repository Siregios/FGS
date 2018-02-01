using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public float maxHealth;
    public float health;
    public float stun = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (stun > 0)
            stun -= Time.deltaTime;
        if (stun <= 0)
        {
            stun = 0;
            health = maxHealth;
        }
	}
}
