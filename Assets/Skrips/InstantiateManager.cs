using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System;


public class InstantiateManager : MonoBehaviour
{
    //Public
    public GameObject starPrefab;
    public GameObject planetPrefab;
    public List<GameObject> planets = new();
    public List<Action> planetType = new(); //0=rock, 1=ice, 2=gas
    public int planetAmount;
    public bool generated;
    public bool minOneEach;
    public float starScaleMin;
    public float starScaleMax;
    public float planetScaleMin;
    public float planetScaleMax;
    public float rockScale = 1f;
    public float gasScale = 2f;
    public float iceScale = 1.5f;
    public float iceDistance;
    public int[] planetWeights;
    
    //Private
    System.Random rng = new System.Random();
    private float scale;
    private int i = 0;
    private int n = 0;
    private float radius = 10f; // radius of circle in units
    private int numPoints = 360;  // adjust based on desired tradeoff of performance vs. quality
    private int rockIndex;
    private int iceIndex;
    private int gasIndex;
    private int iceRatio;

    // Start is called before the first frame update
    void Start()
    {
        iceRatio = planetWeights[1];
        Vector3[] points = new Vector3[numPoints];
        RotateAround rotate = planetPrefab.GetComponent<RotateAround>() as RotateAround;
        PlanetVariables type = planetPrefab.GetComponent<PlanetVariables>() as PlanetVariables;
        LineRenderer lineRenderer = planetPrefab.GetComponent<LineRenderer>() as LineRenderer;
        lineRenderer.positionCount = numPoints;

        planetType.Add(() => { scale = rockScale; type.planetType = "Rock";});
        planetType.Add(() => { scale = iceScale; type.planetType = "Ice";});
        planetType.Add(() => { scale = gasScale; type.planetType = "Gas";});
        //Planetscale ökar exponentielt då den aldrig nollställs

        if (minOneEach)
        {
            rockIndex = rng.Next(planetAmount);
            while (rockIndex == iceIndex) iceIndex = rng.Next(planetAmount);
            while (rockIndex == gasIndex || iceIndex == gasIndex) gasIndex = rng.Next(planetAmount);
        }

        if (!generated)
        {
            //Add a sun
            scale = (float)(rng.NextDouble()*(starScaleMax-starScaleMin))+starScaleMin;
            starPrefab.transform.localScale = new Vector3(scale,scale,scale);
            starPrefab.name = "Star " + i.ToString();
            starPrefab = Instantiate(starPrefab, new Vector3(0f,0f,0f), Quaternion.Euler(0f, 0f, 0f));

            for(i = 0; i<planetAmount;i++)
            {
                //Add a new planet
                planets.Add(planetPrefab);

                //Set rotation and radius
                rotate.degreesPerSecond = 30f;
                //radius = (float) Math.Pow((i+1)*8,1.4);
                radius = (float)(starPrefab.transform.localScale.x * 2 * i);

                //Set planet type; Only ice if far away from sun;
                if (radius < iceDistance) planetWeights[1] = 0;
                else planetWeights[1] = iceRatio;

                //planetWeights[0]
                //Ändra Weights baserat på avstånd från solen

                if (i != rockIndex && i != iceIndex && i != gasIndex) planetType[WeightedRNG.Execute(planetWeights)]();
                else if (i == rockIndex) planetType[0]();
                else if (i == iceIndex) planetType[1]();
                else planetType[2]();

                //Set scale
                scale = (float)(((rng.NextDouble()*(planetScaleMax-planetScaleMin))+planetScaleMin)*scale);

                //Add points in orbit
                for (n = 0; n < numPoints; n++) {
                    var angle = (Mathf.PI * 2f) * ((float)n / numPoints);
                    points[n] = new Vector3(Mathf.Sin(angle) * radius, 0f, Mathf.Cos(angle) * radius);
                }
                lineRenderer.SetPositions(points);

                //Apply Planet Scale and Instantiate
                planets[i].name = "Planet " + i.ToString();
                planets[i].transform.localScale = new Vector3(scale,scale,scale);
                planets[i] = Instantiate(planetPrefab, points[rng.Next(numPoints)], Quaternion.Euler(0f, 0f, 0f));
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
