﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTravel : Rock
{
    private Rigidbody2D rigidBody;
    private float force = 1.0f;

    // Use this for initialization
    void Start ()
    {
        this.StartShadow();

        rigidBody = this.transform.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        this.UpdateShadow();

        var faceDirection = this.transform.position;
        var faceDirectionNormalized = new Vector2(faceDirection.x, faceDirection.y).normalized;
        var angle = Mathf.Atan2(faceDirection.y, faceDirection.x) * Mathf.Rad2Deg - 90f;
        this.transform.rotation = Quaternion.Euler(0, 0, angle);

        this.rigidBody.AddForce(faceDirectionNormalized * this.force);
    }
}
