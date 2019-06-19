using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {

    public GameObject Visual;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Boundary"))
        {
            Visual.transform.localPosition = new Vector3(
                Random.value * 0.1f,
                Random.value * 0.1f,
                Visual.transform.localPosition.z);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Visual.transform.localPosition = Vector3.zero;
    }
}
