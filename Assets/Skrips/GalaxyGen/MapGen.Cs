using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Logic mockups. Does nothing with the generated stars

public class Galaxy : MonoBehaviour
{
    public List<StarNode> stars = new List<StarNode>();
    public int size;
    public GalaxyGenerator(int amount){
        for(int i = 0; i < amount; i++){
            stars.Add(CreateStar())
        }

        //creats hyperlane connections
        foreach(StarNode hyperRoot in stars){
            foreach(StarNode hyperTarget in stars){
                // if the target star is the current star or already has a connection to the current star
                if(hyperTarget == hyperRoot || hyperTarget.Contains(hyperRoot)) break; 
                //If the stars are close enough together 
                double laneDistance = Vector3.Distance(hyperRoot, hyperTarget);
                if(laneDistance < 5){
                    hyperRoot.hyperlaneNeighbors.Add(hyperTarget);
                    hyperTarget.hyperlaneNeighbors.Add(hyperRoot);
                }
            }
        }
        //checks so all stars have friends
        foreach(StarNode lonleyStar in stars){
            if(star.hyperlaneNeighbors.Count == 0){
                StarNode tempStarPointer = star[0]; //remembers the closest star so far and the distance
                int relationshipDistance = Vector3.Distance(lonleyStar.location, star[0].location);
                foreach(StarNode potentialFriend in stars){
                    int tempDistance = Vector3.Distance(lonleyStar.location, potentialFriendn.location);
                    if(relationshipDistance > tempDistance){ //if we find a closer star we make thet the new potential friend
                        tempStarPointer = potentialFriend;
                        relationshipDistance = tempDistance;
                    }
                }
            }
        }
    }
}

public class StarNode : MonoBehaviour
{
    static private Random randomGen = new Random();
    public string name;
    public var starmap; //placeholder for ANANAS
    public Vector3 location = Vector3;
    public double hyperStreamHeight = 1;
    public List<StarNode> hyperlaneNeighbors = new List<StarNode>();

    //TODO: distance
    static public StarNode CreateStar(){
        StarNode newStar;
        
        newStar.location.x = StarNode.randomGen.NextDouble()*100;
        newStar.location.y = StarNode.randomGen.NextDouble()*100;
        newStar.location.z = 0;

        return newStar;
    }
}

public class SubStream : MonoBehaviour
{
    (StarNode, double)[] stars = new (StarNode, double)[2];
    double distance;
    double distanceModifer = 0;
    public SubStream(StarNode root, StarNode target)
    {
        distance = Vector3.Distance(root, target)
        star[0,0] = root;
        star[1,0] = target;
        //Pre-calculating hyperstream height so we don't need to do that every time some ship needs to go somewhere.
        double difference = root.hyperStreamHeight - target.hyperStreamHeight;
        star[0,1] = difference;
        star[1,1] = !difference;
    }
}