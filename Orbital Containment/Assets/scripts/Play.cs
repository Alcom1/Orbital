using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play : MonoBehaviour
{
    public GameObject Visual;

    public bool IsPressed
    {
        get
        {
            return Visual.transform.localScale.x <= 0;
        }
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        IncrementVisualScale(1.5f * Time.deltaTime);
    }

    //
    public void ApplyForce()
    {
        IncrementVisualScale(-3.0f * Time.deltaTime);
    }

    private void IncrementVisualScale(float increment)
    {
        var newScale = Mathf.Clamp01(Visual.transform.localScale.x + increment);
        Visual.transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}
