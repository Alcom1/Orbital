using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float laserForce = 10.0f;
    public GameObject ParticleEmitter;
    private ParticleSystem laserParticles;
    private LineRenderer lineRenderer;

	// Use this for LASER initialization
	void Start ()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;

        laserParticles = ParticleEmitter.GetComponent<ParticleSystem>();
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
            if (laserParticles.isStopped) { laserParticles.Play(); }
        }
        else
        {
            lineRenderer.enabled = false;
            if (laserParticles.isPlaying) { laserParticles.Stop(); }
        }
    }

    private void ResolveFireLaser()
    {
        var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, cursorPosition - transform.position);

        if(hit.rigidbody != null)
        {
            hit.rigidbody.AddForce(-transform.up * laserForce, ForceMode2D.Force);
        }
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, hit.point);

        ParticleEmitter.transform.position = hit.point;
        ParticleEmitter.transform.LookAt(hit.point + hit.normal, Vector3.back);
    }
}
