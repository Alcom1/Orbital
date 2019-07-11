using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject ParticleEmitter;
    private ParticleSystem laserParticles;
    private LineRenderer lineRenderer;
    private readonly float laserForce = 30.0f;
    private readonly float laserExtension = 0.2f;

    // Use this for LASER initialization
    void Start ()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;

        laserParticles = ParticleEmitter.GetComponent<ParticleSystem>();
    }

    // Update is called once per LASER frame
    void FixedUpdate ()
    {
        CheckFireLaser();

        if (lineRenderer.enabled)
        {
            ResolveFireLaser();
        }
    }

    //Check inputs if a LASER is being fired
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

    //Resolve a LASER being fired
    private void ResolveFireLaser()
    {
        //Cursor position
        var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Cast a ray to see if an object was hit by the laser
        RaycastHit2D hit = Physics2D.Raycast(transform.position, cursorPosition - transform.position);

        //Push any object with a rigidbody or trigger other functionality based on tags
        if (hit.rigidbody != null)
        {
            hit.rigidbody.AddForce(-transform.up * laserForce, ForceMode2D.Force);                              
        }
        if(hit.transform.tag == "Play")
        {
            hit.transform.GetComponent<Play>().ApplyForce();
        }

        //Set line renderer positions
        Vector3 hitPoint = new Vector3(hit.point.x, hit.point.y, transform.position.z);                         //hitpoint has a z-position for proper particle postioning
        lineRenderer.SetPosition(0, transform.position);                                                        //LASER starts at this's position
        lineRenderer.SetPosition(1, hitPoint + (hitPoint - transform.position).normalized * laserExtension);    //LASER ends past the hitpoint

        //Set particle emitter transforms
        ParticleEmitter.transform.position = hitPoint;
        ParticleEmitter.transform.LookAt(hitPoint + (Vector3)hit.normal, Vector3.back);
    }
}
