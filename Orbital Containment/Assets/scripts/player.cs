using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {

    //public fields
    public float rotationalSpeed = 1.0f;
    public float fireRate = 3.0f;
    public GameObject bullet;

    //private fields
    private float fireRateCounter = 0.0f;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        var angle = Mathf.Atan2(cursorPosition.y, cursorPosition.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            Quaternion.AngleAxis(angle, Vector3.forward), 
            Time.deltaTime * rotationalSpeed);

        if (Input.GetMouseButtonDown(0) && fireRateCounter <= 0)
        {
            Instantiate(bullet, this.transform.up * 4, this.transform.rotation);

            fireRateCounter = 1 / fireRate;
        }
        if(fireRateCounter > 0)
        {
            fireRateCounter -= Time.deltaTime;
        }
    }
}
