using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFollow : Rock
{
    private GameObject playerCenter;
    private Rigidbody2D rigidBody;
    private readonly float force = 200.0f;

    // Use this for initialization
    void Start()
    {
        UpdateStarting();

        playerCenter = GameObject.FindGameObjectWithTag("PlayerCenter");
        rigidBody = this.transform.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update()
    {
        UpdateRock();
    }

    //Update for the growing state
    protected override void UpdateGrowing()
    {
        base.UpdateGrowing();
        CalculateFaceDirection();
    }

    //Update for the alive state
    protected override void UpdateAlive()
    {
        this.UpdateShadow();

        var faceDirectionNormalized = CalculateFaceDirection();
        this.rigidBody.AddForce(faceDirectionNormalized * this.force * Time.deltaTime);
        this.rigidBody.velocity = faceDirectionNormalized * Vector2.Dot(this.rigidBody.velocity, faceDirectionNormalized);
    }

    //Calculate a normalized facing direction and also set rock to face that direction.
    private Vector2 CalculateFaceDirection()
    {
        var faceDirection = playerCenter.transform.position - this.transform.position;
        var faceDirectionNormalized = new Vector2(faceDirection.x, faceDirection.y).normalized;
        var angle = Mathf.Atan2(faceDirection.y, faceDirection.x) * Mathf.Rad2Deg - 90f;
        this.transform.rotation = Quaternion.Euler(0, 0, angle);
        return faceDirectionNormalized;
    }
}
