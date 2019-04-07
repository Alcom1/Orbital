using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    private LineRenderer lineRenderer;

	// Use this for LASER initialization
	void Start ()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
    }

    // Update is called once per LASER frame
    void Update ()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, hit.point);
    }
}
