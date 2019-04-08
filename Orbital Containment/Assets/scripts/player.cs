using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //public fields
    public float rotationalSpeed = 1.0f;

	// Use this for awesome initialization
	void Start ()
    {
		//Here be dragons!
	}

    // Update is called once per awesome frame
    void Update()
    {
        var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        var angle = Mathf.Atan2(cursorPosition.y, cursorPosition.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            Quaternion.AngleAxis(angle, Vector3.forward), 
            Time.deltaTime * rotationalSpeed);
    }
}
