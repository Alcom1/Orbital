using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float velocity = 15f;
    public float lifeRadius = 4.45f;

	// Use this for initialization
	void Start () {
        this.GetComponent<Rigidbody2D>().velocity = this.transform.up * velocity * -1;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(this.transform.position.magnitude);
		if(this.transform.position.magnitude > lifeRadius)
        {
            Destroy(this.gameObject);
        }
	}
}
