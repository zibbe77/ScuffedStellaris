using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Logic mockups. Does nothing with the generated stars

public class Galaxy
{
    public List<StarNode> stars = new List<StarNode>();
    public List<SubStream> subStreams = new List<SubStream>();
    public int size;
    public Galaxy(int amount)
    {
        size = amount;
        GalaxyGenerator(size);
    }
    void GalaxyGenerator(int amount){
        for(int i = 0; i < amount; i++){
            stars.Add(StarNode.CreateStar());
            stars[i].name = i.ToString();
        }

        //creats hyperlane connections
        foreach(StarNode hyperRoot in stars){
            foreach(StarNode hyperTarget in stars){
                // if the target star is the current star or already has a connection to the current star
                if(hyperTarget == hyperRoot) continue;
                bool containedCheck = false;
                foreach(SubStream stream in hyperTarget.subStreams)
                {
                    if(stream.Contains(hyperRoot))
                    {
                        containedCheck = true;
                        break;
                    }
                }
                if(containedCheck) continue;
                //If the stars are close enough together 
                double laneDistance = Vector3.Distance(hyperRoot.location, hyperTarget.location);
                if(laneDistance < 5){
                    hyperRoot.subStreams.Add(new SubStream(hyperRoot, hyperTarget));
                    hyperTarget.subStreams.Add(hyperRoot.subStreams.Last());
                    // hyperRoot.hyperlaneNeighbors.Add(hyperTarget);
                    // hyperTarget.hyperlaneNeighbors.Add(hyperRoot);
                }
            }
        }
        //checks so all stars have friends
        foreach(StarNode lonleyStar in stars){
            if(lonleyStar.subStreams.Count == 0){
                StarNode tempStarPointer = lonleyStar; //remembers the closest star so far and the distance
                double relationshipDistance = 200;
                foreach(StarNode potentialFriend in stars){
                    double tempDistance = Vector3.Distance(lonleyStar.location, potentialFriend.location);
                    //if we find a closer star we make thet the new potential friend
                    //And that the closer star isn't the lonley star
                    if(relationshipDistance > tempDistance && potentialFriend != lonleyStar){ 
                        tempStarPointer = potentialFriend;
                        relationshipDistance = tempDistance;
                    }
                }
                lonleyStar.subStreams.Add(new SubStream(lonleyStar, tempStarPointer));
            }
        }
        foreach(StarNode star in stars)
        {
            foreach(SubStream link in star.subStreams)
            {
                subStreams.Add(link);
            }
        }
    }
}

public class StarNode
{
    static private Random randomGen = new Random();
    public string name; 
    public Vector3 location = new Vector3();
    public double hyperStreamHeight = 1;
    public List<StarNode> hyperlaneNeighbors = new List<StarNode>();
    public List<SubStream> subStreams = new List<SubStream>();
    static public StarNode CreateStar(){
        StarNode newStar = new();

        newStar.location.X = (float) StarNode.randomGen.NextDouble()*100;
        newStar.location.Y = (float) StarNode.randomGen.NextDouble()*100;
        newStar.location.Z = 0;

        return newStar;
    }
}

public class SubStream
{
    (StarNode root, double modifier)[] stars = new (StarNode, double)[2];
    double distance;
    double distanceModifier = 0;
    public SubStream(StarNode root, StarNode target)
    {
        if(root == target) throw new ArgumentException("Root StarNode and Target StarNode cannot be the same StarNode");
        distance = Vector3.Distance(root.location, target.location);
        stars[0].root = root;
        stars[1].root = target;
    }
    public bool Contains(StarNode target)
    {
        return (stars[0].root == target || stars[1].root == target) ? true : false;
    }
    public double Distance(StarNode root, bool withModifier = true)
    {
        double temp = distance; 
        int target = 0;
        if(!stars[target].root.Equals(root)) target++;
        if(withModifier)
        {
           temp = temp * distanceModifier;
           temp = temp * stars[target].modifier;
        }
        return temp;
    }
}