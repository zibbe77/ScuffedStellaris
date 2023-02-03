using System.Collections;
using System.Collections.Generic;
using
using UnityEngine;

public class Galaxy : MonoBehaviour
{
    public List<StarNode>

    
}

public class StarNode : MonoBehaviour
{
    public string name;
    public int starmap; //placeholder for ANANAS
    public double[] location = new double[3];
    public List<StarNode> hyperlaneNeighbors = new List<StarNode>();
}