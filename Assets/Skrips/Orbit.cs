using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    //Variables
    private float orbitSpeed;
    private int n = 0;
    private int numPoints = 360;
    private float radius = 1f;


    public Orbit()
    {
        Vector3[] points = new Vector3[numPoints];
    }

    public void SetOrbit(Vector3[] points)
    {
        for (n = 0; n < numPoints; n++) {
            var angle = (Mathf.PI * 2f) * ((float)n / numPoints);
            points[n] = new Vector3(Mathf.Sin(angle) * radius, 0f, Mathf.Cos(angle) * radius);
        }
        
    }
}
