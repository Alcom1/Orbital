using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBorderGenerator : MonoBehaviour {

    public int pointCount = 50;
    public float radius = 4.2f;

	// Use this for initialization
	void Start () {
        EdgeCollider2D edgeCollider = this.gameObject.AddComponent<EdgeCollider2D>();

        List<Vector2> points = new List<Vector2>();
        
        for(int i = 0; i <= pointCount; i++)
        {
            float angle = Mathf.PI * 2 * i / pointCount;
            points.Add(new Vector2(
                radius * Mathf.Sin(angle),
                radius * Mathf.Cos(angle)));
        }

        edgeCollider.points = points.ToArray();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
