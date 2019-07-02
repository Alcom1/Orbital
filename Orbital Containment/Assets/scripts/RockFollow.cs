using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFollow : Rock {
    
    private GameObject playerCenter;
    private Rigidbody2D rigidBody;
    private float velocity = 5.0f;

    // Use this for initialization
    void Start ()
    {
        playerCenter = GameObject.FindGameObjectWithTag("PlayerCenter");
        rigidBody = this.transform.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        var faceDirection = playerCenter.transform.position - this.transform.position;
        var faceDirectionNormalized = new Vector2(faceDirection.x, faceDirection.y).normalized;
        var angle = Mathf.Atan2(faceDirection.y, faceDirection.x) * Mathf.Rad2Deg - 90f;
        this.transform.rotation = Quaternion.Euler(0, 0, angle);

        this.rigidBody.AddForce(faceDirection.normalized * this.velocity);
        this.rigidBody.velocity = faceDirectionNormalized * Vector2.Dot(this.rigidBody.velocity, faceDirectionNormalized);
    }
}
