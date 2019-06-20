using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {

    public GameObject Visual;
    public bool IsColliding { get { return isColliding; } }

    private bool isColliding;

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

            Visual.transform.localPosition = new Vector3(
                Random.value * 0.1f,
                Random.value * 0.1f,
                Visual.transform.localPosition.z);
        }
    }

    //On Exit Colliding
    private void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;

        Visual.transform.localPosition = Vector3.zero;
    }
}
