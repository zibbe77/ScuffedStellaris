using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet
{
    public GameObject planetObj;
    public Vector3 position;
    public float speed;
    public float scale;

    public Planet(GameObject planetPrefab, Vector3 position, float speed, float scale)
    {
        planetObj = planetPrefab;
        this.position = position;
        this.speed = speed;
        this.scale = scale;
        ApplyScale();
    }

    public void ApplyScale()
    {
        planetObj.transform.localScale = new Vector3(scale,scale,scale);
    }
}
