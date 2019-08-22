using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    public GameObject ParticleEmitter;
    public GameObject LaserBase;
    public bool IsActive = false;

    private ParticleSystem laserParticles;
    private LineRenderer lineRenderer;
    private readonly float laserForce = 2000.0f;
    private readonly float laserShrinkRate = 1.0f;
    private readonly float laserExtension = 0.2f;

    // Use this for LASER initialization
    void Start ()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;

        LaserBase.SetActive(false);

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

    //Check inputs if a LASER is being fired
    private void CheckFireLaser()
    {
        if (Input.GetMouseButton(0) && IsActive)
        {
            lineRenderer.enabled = true;
            LaserBase.SetActive(true);
            if (laserParticles.isStopped) { laserParticles.Play(); }
        }
        else
        {
            lineRenderer.enabled = false;
            LaserBase.SetActive(false);
            if (laserParticles.isPlaying) { laserParticles.Stop(); }
        }
    }

    //Resolve a LASER being fired
    private void ResolveFireLaser()
    {
        //Cursor position
        var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        SetLaserBaseFaceDirection(cursorPosition);

        //Cast a ray to see if an object was hit by the laser
        RaycastHit2D hit = Physics2D.Raycast(transform.position, cursorPosition - transform.position);

        //Push any object with a rigidbody or trigger other functionality based on tags
        if (hit.rigidbody != null)
        {
            hit.rigidbody.AddForceAtPosition(-transform.up * laserForce * Time.deltaTime, hit.point, ForceMode2D.Force);
        }
        //Hitting a play object
        if(hit.transform.tag == "Play")
        {
            hit.transform.GetComponent<Play>().ApplyForce();
        }
        //Hitting a special rock object
        if (hit.transform.tag == "Rock")
        {
            var rockGrow = hit.transform.GetComponent<RockGrow>();
            if(rockGrow)
            {
                rockGrow.AddToScale(-this.laserShrinkRate * Time.deltaTime);
            }
        }

        //Set line renderer positions
        Vector3 hitPoint = new Vector3(hit.point.x, hit.point.y, transform.position.z);                 //hitpoint has a z-position for proper particle postioning
        Vector3 laserExtensionVector = (hitPoint - transform.position).normalized * laserExtension;
        lineRenderer.SetPosition(0, transform.position);                //LASER starts at this's position
        lineRenderer.SetPosition(1, hitPoint + laserExtensionVector);   //LASER ends past the hitpoint

        //Set particle emitter transforms
        ParticleEmitter.transform.position = hitPoint;
        ParticleEmitter.transform.LookAt(hitPoint + (Vector3)hit.normal, Vector3.back);
    }

    //Sets the laser base to face a direction.
    private void SetLaserBaseFaceDirection(Vector3 facePosition)
    {
        var faceDirection = facePosition - this.transform.position;
        var angle = Mathf.Atan2(faceDirection.y, faceDirection.x) * Mathf.Rad2Deg + 90f;
        LaserBase.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
