using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarRenderer : MonoBehaviour
{
    public float value = 0;
    public float valueMax = 100;
    public float radius = 4.8f;

    private LineRenderer lineRenderer;
    private float stepLength = Mathf.PI / 120;  //Angular distance each step in the bar.

    // Use this for initialization
    void Start ()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = true;
    }

    //Update the bar to be at a new value
    public void UpdateBar(float newValue)
    {
        //Limit the value to be <= the max value
        value = Mathf.Min(newValue, valueMax);

        var max = Mathf.PI * value / valueMax;  //Maximum angle of the bar
        var counter = 0;                        //Current step in the bar

        //Setup points for the Line Renderer
        lineRenderer.positionCount = Mathf.CeilToInt(2 * max / stepLength) + 1; //Number of points
        for (float f = -max; f < max; f += stepLength)
        {
            lineRenderer.SetPosition(counter, GetPointPosition(f)); //Setup point
            counter++;
        }
        lineRenderer.SetPosition(counter,GetPointPosition(max));    //Last point
    }

    //Get the position of the line for an angle
    private Vector3 GetPointPosition(float angle)
    {
        return new Vector3(
            Mathf.Sin(angle) * radius,
            Mathf.Cos(angle) * radius,
            0);
    }
}
