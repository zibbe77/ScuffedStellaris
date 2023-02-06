using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateManager : MonoBehaviour
{

    public GameObject starPrefab;
    public GameObject planetPrefab;
    public List<GameObject> planets = new();
    public int planetAmount;
    public bool generated;
    int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        RotateAround rotate = planetPrefab.GetComponent(typeof(RotateAround)) as RotateAround;
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
                planets[i] = Instantiate(planetPrefab, new Vector3((i*2f)+2f,0f,0f), Quaternion.Euler(0f, 0f, 0f));
                planets[i].name = "Planet " + i.ToString();
                planets[i].transform.localScale = new Vector3(i+1,i+1,i+1);


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
