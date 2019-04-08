﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float laserForce = 10.0f;
    private LineRenderer lineRenderer;

	// Use this for LASER initialization
	void Start ()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;
    }

    // Update is called once per LASER frame
    void Update ()
    {
        CheckFireLaser();

        if (lineRenderer.enabled)
        {
            ResolveFireLaser();
        }
    }

    private void CheckFireLaser()
    {
        if (Input.GetMouseButton(0))
        {
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private void ResolveFireLaser()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up);

        hit.rigidbody.AddForce(-transform.up * laserForce, ForceMode2D.Force);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, hit.point);
    }
}
