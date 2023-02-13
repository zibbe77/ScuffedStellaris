using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    //[SerializeField] private GameObject target;
    public float degreesPerSecond = 45f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(new Vector3(0,0,0), new Vector3(0,1,0), degreesPerSecond * Time.deltaTime);
    }
}
