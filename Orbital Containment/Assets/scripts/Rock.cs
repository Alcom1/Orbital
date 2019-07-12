using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private enum RockState
    {
        Start,
        Grow,
        Alive,
        Dying,
        Dead
    }

    public GameObject Visual;
    public SpriteRenderer Shadow;
    public bool IsDead { get { return this.rockState == RockState.Dead; } }
    public bool IsGrown { get { return this.transform.localScale.x == this.maxScale; } }
    public bool IsColliding { get { return isColliding; } }
    protected bool isColliding;

    private RockState rockState = RockState.Start;
    private readonly float shakeAngle = 60;
    private readonly float shakeIntensity = 0.08f;
    private readonly float shadowFrequency = 3;
    private readonly float shrinkRate = 2.0f;
    private readonly float growthRate = 1.0f;
    private float maxScale = 1.0f;
    private Vector3 previousPosition;

    // Use this for initialization
    void Start()
    {
        UpdateStarting();
    }
	
	// Update is called once per frame
	void Update()
    {
        UpdateRock();
    }

    //Base update logic for rocks
    protected virtual void UpdateRock()
    {
        switch (rockState)
        {
            case RockState.Grow:
                UpdateGrowing();
                break;
            case RockState.Alive:
                UpdateAlive();
                break;
            case RockState.Dying:
                UpdateDying();
                break;
            case RockState.Dead:
                UpdateDead();
                break;
        }
    }

    //Update for the starting state
    protected virtual void UpdateStarting()
    {
        this.ResetShadow();
        this.maxScale = this.transform.localScale.x;
        this.transform.localScale = Vector3.zero;
        this.rockState = RockState.Grow;
    }

    //Update for the growing state
    protected virtual void UpdateGrowing()
    {
        float currentScale = this.transform.localScale.x;
        if (currentScale < maxScale)
        {
            currentScale += maxScale * growthRate * Time.deltaTime;
            if(currentScale > maxScale)
            {
                currentScale = maxScale;
                this.rockState = RockState.Alive;
            }
            this.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
        }
    }

    //Update for the alive state
    protected virtual void UpdateAlive()
    {
        this.UpdateShadow();
    }

    //Update for the dying state
    protected virtual void UpdateDying()
    {
        var newScale = this.transform.localScale.x - this.shrinkRate * Time.deltaTime;
        if(newScale > 0)
        {
            this.transform.localScale = new Vector3(newScale, newScale, newScale);
        }
        else
        {
            this.transform.localScale = Vector3.zero;
            this.rockState = RockState.Dead;
        }
    }

    //Update for the dead state
    protected virtual void UpdateDead()
    {

    }

    //Kill this rock
    public void Kill()
    {
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        this.ResetShadow();
        this.rockState = RockState.Dying;
    }

    //Starts or resets shadow effect
    protected void ResetShadow()
    {
        //Set initial shadow position
        previousPosition = this.transform.position;
    }

    //Update shadow effect
    protected void UpdateShadow()
    {
        //Gradually decrease shadow opacity
        Shadow.color = new Color(1, 1, 1, Shadow.color.a - shadowFrequency * Time.deltaTime);

        //reset shadow once opacity reaches 0.
        if (Shadow.color.a <= 0)
        {
            Shadow.color = new Color(1, 1, 1, 1);
            previousPosition = this.transform.position;
        }

        //Force shadow to be stationary
        Shadow.transform.position = previousPosition;
    }

    //While Colliding
    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Boundary"))
        {
            isColliding = true;
            
            //Shake object while it is colliding.
            var shakeDirection = new Vector2(this.transform.position.x, this.transform.position.y).normalized;
            var shakeDirectionRandom = Quaternion.AngleAxis(Random.Range(-shakeAngle, shakeAngle), Vector3.back) * shakeDirection;
                shakeDirectionRandom *= Random.value * shakeIntensity;
                shakeDirectionRandom.z = Visual.transform.localPosition.z;

            Visual.transform.localPosition = shakeDirectionRandom;
        }
    }

    //On Exit Colliding
    void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;

        Visual.transform.localPosition = Vector3.zero;
    }
}
