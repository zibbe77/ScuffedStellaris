using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeightedRNG : MonoBehaviour
{
    static System.Random random = new System.Random();
    static private int[] ratios;
    static int totalRatio;
    static int iteration; // so you know what to do next
    static int x;
    static int test = 0;

    //Takes 
    public static int Execute(int[] input)
    {
        ratios = input;
        totalRatio = 0;
        iteration = 0;
        x = 0;
        test++;
        Console.WriteLine(test);

        foreach(int r in ratios) totalRatio += r;

        x = random.Next(0, totalRatio);

        foreach(int r in ratios)
        {
            if((x -= r) < 0)
                break;
            iteration++;
        }
        return iteration;
    }
}
