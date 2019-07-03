﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public GameObject Visual;
    public bool IsColliding { get { return isColliding; } }

    protected bool isColliding;

    private float shakeAngle = 60;
    private float shakeIntensity = 0.08f;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //While Colliding
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Boundary"))
        {
            isColliding = true;
            
            var shakeDirection = new Vector2(this.transform.position.x, this.transform.position.y).normalized;
            var shakeDirectionRandom = Quaternion.AngleAxis(Random.Range(-shakeAngle, shakeAngle), Vector3.back) * shakeDirection;
                shakeDirectionRandom *= Random.value * shakeIntensity;
                shakeDirectionRandom.z = Visual.transform.localPosition.z;

            Visual.transform.localPosition = shakeDirectionRandom;
        }
    }

    //On Exit Colliding
    private void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;

        Visual.transform.localPosition = Vector3.zero;
    }
}
