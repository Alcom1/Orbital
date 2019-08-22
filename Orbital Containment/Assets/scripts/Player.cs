using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //public fields
    public float rotationalSpeed = 1.0f;
    public float breakTime = 1.0f;
    private float rotationalSpeedMax = 0.0f;

	// Use this for awesome initialization
	void Start ()
    {
        rotationalSpeedMax = rotationalSpeed;
    }

    // Update is called once per awesome frame
    void Update()
    {
        Move();
        CheckBrakes();
    }

    private void Move()
    {
        var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var angle = Mathf.Atan2(cursorPosition.y, cursorPosition.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.AngleAxis(angle, Vector3.forward),
            Time.deltaTime * rotationalSpeed);
    }

    private void CheckBrakes()
    {
        if (!Input.GetMouseButton(1))
        {
            rotationalSpeed = rotationalSpeedMax;
        }
        else
        {
            rotationalSpeed -= rotationalSpeedMax * Time.deltaTime / breakTime;
        }
    }
}
