using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class InstantiateManager : MonoBehaviour
{

    public GameObject starPrefab;
    public GameObject planetPrefab;
    public List<GameObject> planets = new();
    public int planetAmount;
    public bool generated;
    int i = 0;
    int n = 0;
    float radius = 10f; // radius of circle in units
    int num_points = 360;  // adjust based on desired tradeoff of performance vs. quality
    


    // Start is called before the first frame update
    void Start()
    {
        Vector3[] points = new Vector3[num_points];
        RotateAround rotate = planetPrefab.GetComponent<RotateAround>() as RotateAround;
        LineRenderer lineRenderer = planetPrefab.GetComponent<LineRenderer>() as LineRenderer;

        lineRenderer.positionCount = num_points;

        if (!generated)
        {
            //Add a sun
            GameObject instantiatedObject = Instantiate(starPrefab, new Vector3(0f,0f,0f), Quaternion.Euler(0f, 0f, 0f));
            instantiatedObject.name = "Star " + i.ToString();

            for(i = 0; i<planetAmount;i++)
            {
                //Add a new planet
                planets.Add(planetPrefab);
                rotate.degreesPerSecond = (i+1)/0.05f;
                radius = ((float)Math.Pow(i+1,2));
                for (n = 0; n < num_points; n++) {
                    var angle = (Mathf.PI * 2f) * ((float)n / num_points);
                    points[n] = new Vector3(Mathf.Sin(angle) * radius, 0f, Mathf.Cos(angle) * radius);
                }
                lineRenderer.SetPositions(points);
                planets[i].name = "Planet " + i.ToString();
                planets[i].transform.localScale = new Vector3(i+1,i+1,i+1);
                planets[i] = Instantiate(planetPrefab, new Vector3(radius,0f,0f), Quaternion.Euler(0f, 0f, 0f));
            }
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
