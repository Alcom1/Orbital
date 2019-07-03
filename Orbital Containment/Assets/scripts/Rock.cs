using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public GameObject Visual;
    public SpriteRenderer Shadow;
    public bool IsColliding { get { return isColliding; } }

    protected bool isColliding;

    private float shakeAngle = 60;
    private float shakeIntensity = 0.08f;
    private float shadowFrequency = 3;
    private Vector3 previousPosition;

    // Use this for initialization
    void Start ()
    {
        this.StartShadow();
    }
	
	// Update is called once per frame
	void Update ()
    {
        this.UpdateShadow();
    }

    //Start shadow effect
    protected void StartShadow()
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
